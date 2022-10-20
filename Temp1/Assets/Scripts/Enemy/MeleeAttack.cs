using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MeleeAttack : MonoBehaviour, ICharacter
{
    public EnemyData enemyData;
    Enemy_Orc orc;
    Enemy_Skelleton skelleton;

    private void Awake()
    {
        //enemyData = GetComponent<EnemyData>();
        orc = GetComponentInParent<Enemy_Orc>();
        skelleton = GetComponentInParent<Enemy_Skelleton>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {   
            if(orc != null)
            {
            Debug.Log($"{orc.name}Attack");
            }
            else if(skelleton != null)
            {
                Debug.Log($"{skelleton.name}Attack");
            }
            //Attack(enemy.target.gameObject,enemy.maxDamage);
            Attacked(enemyData.EDamage);
        }
    }

    public void Attack(GameObject target, int damage)
    {
    }

    public void Attacked(int damage)
    {
        
    }

    public void Die()
    {
    }
}
