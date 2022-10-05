using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MeleeAttack : MonoBehaviour, ICharacter
{
    Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Attack");
            Attack(enemy.target.gameObject,enemy.maxDamage);
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
