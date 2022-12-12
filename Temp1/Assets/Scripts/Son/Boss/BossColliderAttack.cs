using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossColliderAttack : MonoBehaviour
{
    Collider c;
    Enemy_Boss boss;
    bool isAttack = false;
    private void Awake()
    {
        c = GetComponent<Collider>();
        boss = GetComponentInParent<Enemy_Boss>();
    }
    private void OnEnable()
    {
        isAttack = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isAttack)
        {
            //실제 공격에 적용되었을 때 공격 콜리더에 부딫히게 트리거 on/off설정
            c.isTrigger = false;
            //공격 적용 시 다시 적용되지 않게 true변경
            isAttack = true;
            ICharacter i = other.GetComponent<ICharacter>();
            i.Attacked(boss.eDamage);
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isAttack)
        {
            //실제 공격에 적용되었을 때 공격 콜리더에 부딫히게 트리거 on/off설정
            c.isTrigger = false;
            //공격 적용 시 다시 적용되지 않게 true변경
            isAttack = true;
            ICharacter i = other.GetComponent<ICharacter>();
            i.Attacked(boss.eDamage);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        c.isTrigger = true;
    }


}
