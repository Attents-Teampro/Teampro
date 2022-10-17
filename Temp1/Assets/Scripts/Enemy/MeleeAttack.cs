using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MeleeAttack : MonoBehaviour, ICharacter
{
    EnemyData enemyData;

    private void Awake()
    {
        enemyData = GetComponent<EnemyData>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Attack");
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
