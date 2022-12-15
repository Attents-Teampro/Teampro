using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossColliderAttack : MonoBehaviour
{
    Collider c;
    Enemy_Boss boss;
    bool isAttack = false;
    [SerializeField]
    ParticleSystem ps;
    private void Awake()
    {
        c = GetComponent<Collider>();
        boss = GetComponentInParent<Enemy_Boss>();
    }
    private void OnEnable()
    {
        isAttack = false;
        c.isTrigger= true;
    }
    private void OnDisable()
    {
        //isAttack = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isAttack)
        {
            AttackSuccess(other.GetComponent<ICharacter>(), other.transform.position);
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        
        if (other.CompareTag("Player") && !isAttack)
        {
            AttackSuccess(other.GetComponent<ICharacter>(), other.transform.position);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        
        
        //c.isTrigger = true;
        if (other.gameObject.CompareTag("Player")&& !isAttack)
        {
            AttackSuccess(other.gameObject.GetComponent<ICharacter>(), other.transform.position);
        }
    }
    void AttackSuccess(ICharacter ic, Vector3  point)
    {
        //실제 공격에 적용되었을 때 공격 콜리더에 부딫히게 트리거 on/off설정
        c.isTrigger = false;
        //공격 적용 시 다시 적용되지 않게 true변경
        isAttack = true;
        ic.Attacked(boss.eDamage);
        Vector3 contactPoint = (boss.transform.position - point)*0.7f;
        GameObject g =Instantiate(ps.gameObject, boss.transform.position - contactPoint + Vector3.up, Quaternion.Euler(0, 0, 0));
        g.GetComponent<ParticleSystem>().Play();
        Destroy(g, 4.0f);
        //ps.Play();
    }

}
