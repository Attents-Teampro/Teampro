using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Arrows : MonoBehaviour, ICharacter
{

    public float arrowSpeed = 1.0f;

    //Vector3 targetPoint;
    EnemyBase enemy;
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rigid.velocity = transform.forward * arrowSpeed;
        //enemy = FindObjectOfType<EnemyBase>();
        //targetPoint = enemy.transform.position + new Vector3(0, 1f, 0);
    }

    private void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, targetPoint, arrowSpeed * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Attack(other.gameObject, Player.instance.pDamage);
            Destroy(this.gameObject);
        }
        Destroy(this.gameObject, 1.0f);
    }


    public void Die()
    {
        
    }

    public void Attacked(int damage)
    {
        
    }

    public void Attack(GameObject target, int damage)
    {
        ICharacter ic = target.GetComponent<ICharacter>();
        ic.Attacked(damage);
        Debug.Log($"데미지 입힘\n{target.name}에 {damage}만큼 입힘");
    }
}
