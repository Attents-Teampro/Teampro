using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Projectile_Bat : MonoBehaviour, ICharacter
{
    public EnemyData enemyData;
    public float projectileSpeed = 5f;
    //Vector3 targetPoint;

    Rigidbody rb;
    GameObject player;
   
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 2f);
        rb.velocity = transform.forward * projectileSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.forward*projectileSpeed*Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Attacked");
            Attack(other.gameObject, enemyData.EDamage);
            Destroy(this.gameObject);
        }
        else if (!other.CompareTag("Enemy"))
        {
        }
    }

    public void Die()
    {
    }
    public void Attacked(int damage)
    {        
    }

    public void Attack(GameObject target, int damage)
    {
        ICharacter iPlayer = target.GetComponent<ICharacter>();
        Debug.Log($"{transform.root.name}가 {target.name}을 공격. {damage}만큼의 피해를 입혔습니다.\n" +
           $"현재{target.name}의 HP는 {target.GetComponent<Player>().pHP}");
        iPlayer.Attacked(damage);
    }
}
