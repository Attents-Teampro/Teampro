using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MeleeAttack : MonoBehaviour, ICharacter
{
    public EnemyData enemyData;
    Enemy_Orc orc;
    Enemy_Skelleton skelleton;
    Player player;
    

    private void Awake()
    {
        orc = GetComponentInParent<Enemy_Orc>();
        skelleton = GetComponentInParent<Enemy_Skelleton>();
        player = FindObjectOfType<Player>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (orc != null)
            {
                Debug.Log($"{orc.name}Attack : {enemyData.EDamage}");
                Attack(other.gameObject, orc.enemyData.EDamage);

            }
            else if (skelleton != null)
            {
                Debug.Log($"{orc.name}Attack : {enemyData.EDamage}");
                Attack(other.gameObject, skelleton.enemyData.EDamage);
            }
        }
    }

    public void Attack(GameObject target, int damage)
    {
        Debug.Log($"{transform.root.name}가 {target.name}을 공격. {damage}만큼의 피해를 입혔습니다.\n" +
            $"현재{target.name}의 HP는 {target.GetComponent<Player>().pHP}");
    }

    public void Attacked(int damage)
    {
        player.pHP -= damage;
    }

    public void Die()
    {
    }
}