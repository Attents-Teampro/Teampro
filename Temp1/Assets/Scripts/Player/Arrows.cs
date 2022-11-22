using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrows : MonoBehaviour, ICharacter
{

    public float arrowSpeed = 10.0f;
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rigid.velocity = transform.forward * arrowSpeed;
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
