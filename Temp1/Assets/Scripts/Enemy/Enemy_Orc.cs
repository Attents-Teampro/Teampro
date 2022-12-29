using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy_Orc : EnemyBase, ICharacter
{
    [Header("-------[ Enemy Unique Status]")]
    public int currentHP;           //현재 HP 값
    public int maxHP;

    [Header("-------[ Audio Clip ]")]
    public AudioClip attackSfx;
    public AudioClip getHitSfx;
    public AudioClip dieSfx;
    AudioSource audioSource;

    EnemyHealth health;
    [Header("-------[FX]")]
    public GameObject hitEffect;
    protected override void Awake()
    {
        base.Awake();
        meshs = GetComponentsInChildren<SkinnedMeshRenderer>();
        health = GetComponentInChildren<EnemyHealth>();
        audioSource = GetComponent<AudioSource>();
        currentHP = enemyData.EHP;
        maxHP = enemyData.EHP;
        enemyType = EnemyType.Orc;

    }
    protected override void Start()
    {
        base.Start();
        MeleeAttackTrigger(false);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
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
        MeleeAttackTrigger(true);
        transform.LookAt(target.transform.position);
        isChase = false;
        isAttack = true;
        anim.SetBool("isWalk", false);
        anim.SetTrigger("doAttack");
        audioSource.PlayOneShot(attackSfx);
        yield return new WaitForSeconds(1f);   //AudioPlay 를 위한 지연 시간
        StartCoroutine(WaitForAttack());
    }

    IEnumerator OnDead()
    {
        isDead = true;
        capsuleCollider.enabled = false;
        anim.SetTrigger("doDie");
        audioSource.PlayOneShot(dieSfx);
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
        hitEffect.GetComponent<ParticleSystem>().Play();
        isGetHit = true;
        anim.SetTrigger("doGetHit");
        //MeleeAttackOff();   //meleeAttack Collision 끔
        audioSource.PlayOneShot(getHitSfx);
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
        onDead?.Invoke(this);
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

