using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Projectile : MonoBehaviour, ICharacter
{
    public int attackDamage = 50;
    private float projectileSpeed = 10f;
    Vector3 targetPoint;

    ICharacter iPlayer;

    Rigidbody rb;
    GameObject player;
   
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //10.11 수정. 
        //기존의 Player를 오브젝트 이름으로 찾는 방식이 
        //업데이트마다 플레이어 오브젝트의 이름이 변경될 시 에러가 뜰 위험이 있고 지금도 뜨고 있어서
        //클래스를 찾는 방식으로 변경했습니다.
        //기존 코드 player = GameObject.Find("Player").GetComponent<Transform>();
        player = FindObjectOfType<Player>().gameObject;//수정코드
        //by 손동욱

        iPlayer = player.GetComponent<ICharacter>();
        targetPoint = player.transform.position + new Vector3(0, 1f, 0);
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,targetPoint, projectileSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("Player Attacked");
            Attack(other.gameObject, attackDamage);
        }
    }

    public void Die()
    {
    }
    public void Attacked(int damage)
    {
        //iPlayer.pHP -= damage;
    }

    public void Attack(GameObject target, int damage)
    {
        Debug.Log($"{transform.root.name}가 {target.name}을 공격. {damage}만큼의 피해를 입혔습니다.\n" +
           $"현재{target.name}의 HP는 {target.GetComponent<Player>().pHP}");
        Attacked(damage);
    }
}