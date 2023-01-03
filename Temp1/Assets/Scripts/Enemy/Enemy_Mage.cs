//gitTest
using System;
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
    public int currentHP;
    public int maxHP;

    private Transform mageBulletPosition;   //마법사 발사체(projectile) 생성 위치
    private Transform mageStaff;

    [Header("-------[FX]")]
    public GameObject hitEffect;

    public Action onMageDie;

    protected override void Awake()
    {
        base.Awake();
        meshs = GetComponentsInChildren<SkinnedMeshRenderer>();
        maxHP = enemyData.EHP;
        currentHP = maxHP;
    }
    protected override void Start()
    {
        base.Start();
        //mageStaff = transform.GetChild(0);
        mageBulletPosition = transform.GetChild(3);
    }

    

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
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
        base.MoveToTarget();
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
        
        yield return new WaitForSeconds(2f);
        isAttack = false;
        isChase = true;
        yield return new WaitForSeconds(attackInterval);
    }
    /// <summary>
    /// Enemy 죽는 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator OnDead()
    {
        isDead = true;
        capsuleCollider.enabled = false;
        onMageDie?.Invoke();
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
        Instantiate(dropItems[0], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }


    /// <summary>
    /// Enemy 피격 함수 : 피격관련 모션 처리
    /// </summary>
    /// <returns></returns>
    IEnumerator OnGetHit()
    {
        anim.SetBool("isWalk", false);
        hitEffect.GetComponent<ParticleSystem>().Play();
        isGetHit = true;
        anim.SetTrigger("doGetHit");

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
        if (target.GetComponent<Player>().nearest != this)
        {
            onDead?.Invoke(this);
        }
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
