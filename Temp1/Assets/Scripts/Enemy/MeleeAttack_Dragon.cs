using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MeleeAttack_Dragon : MonoBehaviour, ICharacter
{
    public EnemyData enemyData;
    Enemy_Orc orc;
    Enemy_Skelleton skeleton;
    Enemy_Shell shell;
    Enemy_Dragon dragon;
    Player player;

    EnemyBase parent;

    [SerializeField] int damage = 0;

    private void Start()
    {
        parent = transform.root.GetComponent<EnemyBase>();
        
        switch(parent.enemyType)
        {
            case EnemyBase.EnemyType.Orc:
                orc = GetComponentInParent<Enemy_Orc>();
                damage = orc.enemyData.EDamage;
                break;
            case EnemyBase.EnemyType.Shell:
                shell = GetComponentInParent<Enemy_Shell>();
                damage = shell.enemyData.EDamage;
                break;
            case EnemyBase.EnemyType.Skeleton:
                skeleton = GetComponentInParent<Enemy_Skelleton>();
                damage = skeleton.enemyData.EDamage;
                break;
            case EnemyBase.EnemyType.Dragon:
                dragon = GetComponentInParent<Enemy_Dragon>();
                damage = dragon.enemyData.EDamage;
                break;
        }
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(damage);
            Attack(other.gameObject, damage);
        }
    }

    public void Attack(GameObject target, int damage)
    {
        //Debug.Log($"{transform.root.name}가 {target.name}을 공격. {damage}만큼의 피해를 입혔습니다.\n" +
        //    $"현재{target.name}의 HP는 {target.GetComponent<Player>().pHP}");
        ICharacter targetIC = target.GetComponent<ICharacter>();
        targetIC.Attacked(damage);
    }

    public void Attacked(int damage)
    {
        //player.pHP -= damage;
    }

    public void Die()
    {
    }
}
