using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MeleeAttack : MonoBehaviour, ICharacter
{
    public EnemyData enemyData;
    Enemy_Orc orc;
    Enemy_Skelleton skeleton;
    Enemy_Shell shell;
    Player player;
    GameObject parent;

    public float damage;

    private void Awake()
    {
        parent = transform.root.gameObject;
        //Debug.Log(parent.name);
        switch (parent.name)
        {
            case "Orc":
                orc = GetComponentInParent<Enemy_Orc>();
                //Debug.Log($"{parent.name} 콤포넌트 가져오기 성공");
                break;
            case "Shell":
                shell = GetComponentInParent<Enemy_Shell>();
                //Debug.Log($"{parent.name} 콤포넌트 가져오기 성공");
                break;
            case "Skeleton":
                skeleton = GetComponentInParent<Enemy_Skelleton>();
                //Debug.Log($"{parent.name} 콤포넌트 가져오기 성공");
                break;
        }
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        int damage = 0;
        if (other.gameObject.CompareTag("Player"))
        {
            switch (parent.name)
            {
                case "Orc":
                    damage = orc.enemyData.EDamage;
                    break;
                case "Shell":
                    damage = shell.enemyData.EDamage;
                    break;
                case "Skeleton":
                    damage = skeleton.enemyData.EDamage;
                    break;
            }
            Debug.Log($"{parent.name} 의 데미지 :{damage}");
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
