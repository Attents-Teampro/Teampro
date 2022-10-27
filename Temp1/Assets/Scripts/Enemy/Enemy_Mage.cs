//gitTest
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class Enemy_Mage : EnemyBase, ICharacter
{
    public GameObject projectile;
    int currentHP;
    Vector3 targetDirection;

    ICharacter playerCharacter;

    private Transform mageBulletPosition;   //마법사 발사체(projectile) 생성 위치
    private Transform mageStaff;

    protected override void Start()
    {
        base.Start();
        playerCharacter = target.GetComponent<ICharacter>();
        currentHP = enemyData.EHP;
        mageStaff = transform.GetChild(2);
        mageBulletPosition = mageStaff.GetChild(0);
    }

    protected override void Update()
    {
        base.Update();
        //if (!isAttack && !isDead)
        //{
        //    MoveToTarget();                     // 타겟을 향이 이동하는 메소드
        //    Targeting();
        //}
    }

    protected override void SearchPlayer()
    {
        base.SearchPlayer();
    }

    /// <summary>
    /// 타겟을 향해 이동 : 추 후 타겟 거리를 보고 이동 targetDistance 변수로 처리 예정
    /// </summary>
    protected override void MoveToTarget()
    {
        isChase = true;                                                                     // 이동중임을 알리는 bool 값

        if (!isGetHit && !isAttack)
        {
            targetDirection = (target.position - transform.position).normalized;                //타겟 위치의 방향

            rb.MovePosition(transform.position + targetDirection * Time.deltaTime * enemyData.MoveSpeed); //리지드바디를 사용하여 타겟으로 이동
            transform.LookAt(target);                                                           //Lookat을 사용하여 타겟 바라보기

            anim.SetBool("isWalk", true);                                                       // walk 애니메이션 활성화}
        }
    }

    protected override void Targeting()
    {
        
        //https://ssabi.tistory.com/29
        //https://www.youtube.com/watch?v=voEFSbIPYjw
        //SphereCastAll(생성위치, 반지름,구가 생겨야 할 방향(벡터), 최대 길이, 체크할 레이어 마스크(체크할 레이어의 물체가 아니면 무시)
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, enemyData.TargetRadius,
                transform.forward, enemyData.TargetRange, LayerMask.GetMask("Player"));

        // 레이캐스트에 Player 오브젝트가 판별되면 어택
        if (rayHits.Length > 0)
        {
            StartCoroutine(enemyAttack());
        }
    }

    /// <summary>
    /// Enemy 어택 함수 : 어택 관련 모션 처리
    /// </summary>
    /// <returns></returns>
    IEnumerator enemyAttack()
    {
        isChase = false;
        isAttack = true;
        anim.SetBool("isWalk", false);
        
        anim.SetTrigger("doAttack");
        yield return new WaitForSeconds(0.4f);
        Instantiate(projectile, mageBulletPosition.position, Quaternion.identity);
        yield return new WaitForSeconds(1.1f);
        isAttack = false;
        
        isChase = true;

        yield return new WaitForSeconds(1f);
    }
    /// <summary>
    /// Enemy 죽는 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator OnDead()
    {
        anim.SetTrigger("doDie");
        yield return new WaitForSeconds(1.5f);

        //10.11 추가
        //메인 매니저에게 죽은 몬스터 수를 갱신
        mainManager.numOfDieEnemy++;
        if (mainManager.numOfDieEnemy == mainManager.numOfStageEnemy)
        {
            mainManager.StageClear();
        }
        //by 손동욱

        Destroy(gameObject);
    }


    /// <summary>
    /// Enemy 피격 함수 : 피격관련 모션 처리
    /// </summary>
    /// <returns></returns>
    IEnumerator OnGetHit()
    {
        //currentHP = enemyData.EHP;
        currentHP -= 50; // 테스트용 데미지 값
        anim.SetBool("isWalk", false);
        isGetHit = true;
        anim.SetTrigger("doGetHit");
        if (currentHP < 0)
        {
            Die();
        }
        yield return new WaitForSeconds(0.8f);
        isGetHit = false;
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
        //StartCoroutine(enemyAttack());
        // playerCharacter.Attacked(maxDamage);
    }
}
