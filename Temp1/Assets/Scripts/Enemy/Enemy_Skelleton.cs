using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Skelleton : EnemyBase, ICharacter
{
    
    public int currentHP;
    public int maxHP;

    protected override void Awake()
    {
        base.Awake();
        meshs = GetComponentsInChildren<SkinnedMeshRenderer>();
        maxHP = enemyData.EHP;
        currentHP = maxHP;  //시작시 최대HP값을 enemyData 에서 가져옴
        enemyType = EnemyType.Skeleton;
    }
    protected override void Start()
    {
        base.Start();
        MeleeAttackTrigger(false);
    }

    protected override void FixedUpdate()
    {
        base .FixedUpdate();
    }
    protected override void SearchPlayer()
    {
        base.SearchPlayer();
    }
    protected override void MoveToTarget()
    {
        base.MoveToTarget();
    }
    /// <summary>
    /// 공격을 하기 위해 이동을 멈춤
    /// </summary>
    /// <param name="isStop">bool 값</param>
    protected override void StopNavMesh(bool isStop)
    {
        base.StopNavMesh(isStop);
    }
    protected override void Targeting()
    {
        base.Targeting();
        //https://ssabi.tistory.com/29
        //https://www.youtube.com/watch?v=voEFSbIPYjw
        //SphereCastAll(생성위치, 반지름,구가 생겨야 할 방향(벡터), 최대 길이, 체크할 레이어 마스크(체크할 레이어의 물체가 아니면 무시)
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, enemyData.TargetRadius,
                transform.forward, enemyData.TargetRange, LayerMask.GetMask("Player"));

        // 레이캐스트에 Player 오브젝트가 판별되면 어택
        if (rayHits.Length > 0 && !isAttack && !isGetHit)
        {
            StartCoroutine(enemyAttack());
        }
    }

    IEnumerator enemyAttack()
    {
        transform.LookAt(target.transform.position);
        isChase = false;
        isAttack = true;
        anim.SetBool("isWalk", false);
        anim.SetTrigger("doAttack");
        meleeAttack.SetActive(true);
        yield return new WaitForSeconds(1f);
        StartCoroutine(WaitForAttack());
    }

    IEnumerator OnDead()
    {
        capsuleCollider.enabled = false;

        anim.SetTrigger("doDie");
        isDead = true;
        yield return new WaitForSeconds(1.5f);

        //10.11 추가
        //메인 매니저에게 죽은 몬스터 수를 갱신
        mainManager.numOfDieEnemy++;
        if (mainManager.numOfDieEnemy == mainManager.numOfStageEnemy)
        {
            mainManager.StageClear();
        }
        //by 손동욱
        Instantiate(dropItems[0], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator OnGetHit()
    {
        anim.SetBool("isWalk", false);
        isGetHit = true;
        anim.SetTrigger("doGetHit");
        MeleeAttackTrigger(false);

        HitColor(true);

        yield return new WaitForSeconds(0.1f);

        HitColor(false);

        if (currentHP <= 0)
        {
            Die();
        }
        yield return new WaitForSeconds(0.8f);

        isGetHit = false;
    }
    /// <summary>
    /// 피격시 매시 컬러 변경
    /// </summary>
    /// <param name="isHit"> On / Off </param>
    void HitColor(bool isHit)
    {
        if (isHit)
        {
            foreach (SkinnedMeshRenderer mesh in meshs)
                mesh.material.color = Color.red;
        }
        else
        {
            foreach (SkinnedMeshRenderer mesh in meshs)
                mesh.material.color = Color.white;
        }
    }
    public void Die()
    {
        
        StartCoroutine(OnDead());
    }
    public void Attacked(int damage)
    {
        currentHP -= damage;
        StartCoroutine(OnGetHit());
    }

    public void Attack(GameObject target, int damage)
    {
    }
}
