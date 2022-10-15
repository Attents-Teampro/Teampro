using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enem_Orc : MonoBehaviour, ICharacter
{
    public EnemyData enemyData;
    public Transform target;            //플레이어 타겟
    int currentHP;

    Vector3 targetDirection;

    public GameObject meleeAttack;     //밀리 어택용 컬리젼 박스 : 밀리어택은 하위 클래스에서 처리
    public NavMeshAgent nav;            //네비 매쉬를 사용
    public Rigidbody rb;
    public CapsuleCollider capsuleCollider;     //피격에 사용되는 기본 컬리젼
    public Animator anim;
    public MainManager mainManager;     //몬스터가 죽으면 현재 몬스터의 숫자를 계산하는 클래스

    public bool isChase;                //타겟을 향해 이동중
    public bool isAttack;
    public bool isDead;
    public bool isGetHit = false;

    ICharacter playerCharacter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>(); 
        
        if (mainManager == null)
        {
            mainManager = FindObjectOfType<MainManager>();
        }
    }

    private void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
        //target = FindObjectOfType<Player>().transform;
        playerCharacter = target.GetComponent<ICharacter>();
    }

    private void Update()
    {
        if (!isAttack)
        {
            MoveToTarget();                     // 타겟을 향이 이동하는 메소드
            Targeting();
        }
    }
    private void MoveToTarget()
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

    void Targeting()
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

    IEnumerator enemyAttack()
    {
        isChase = false;
        isAttack = true;
        anim.SetBool("isWalk", false);
        anim.SetTrigger("doAttack");
        meleeAttack.SetActive(true);
        yield return new WaitForSeconds(1f);
        
        isAttack = false;
        meleeAttack.SetActive(false);
        isChase = true;
        yield return new WaitForSeconds(1f);
    }

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

    IEnumerator OnGetHit()
    {
        currentHP = enemyData.EHP;
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

