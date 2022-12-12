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
            //���� ���ݿ� ����Ǿ��� �� ���� �ݸ����� �΋H���� Ʈ���� on/off����
            c.isTrigger = false;
            //���� ���� �� �ٽ� ������� �ʰ� true����
            isAttack = true;
            ICharacter i = other.GetComponent<ICharacter>();
            i.Attacked(boss.eDamage);
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isAttack)
        {
            //���� ���ݿ� ����Ǿ��� �� ���� �ݸ����� �΋H���� Ʈ���� on/off����
            c.isTrigger = false;
            //���� ���� �� �ٽ� ������� �ʰ� true����
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
