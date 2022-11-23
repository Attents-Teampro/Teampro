using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class EnemyBase : MonoBehaviour
{
    public enum EnemyType
    {
        Orc = 0,
        Skeleton,
        Mage,
        Shell,
        Boss,
        Bat,
        Dragon,
    }

    public EnemyType enemyType;

    [Header("-------[ 컴포넌트 ]")]
    public EnemyData enemyData;         //기본데이터
    public Transform target;            //플레이어 타겟
    public GameObject meleeAttack;      //밀리 어택용 컬리젼 박스 : 밀리어택은 하위 클래스에서 처리
    public NavMeshAgent nav;            //네비 매쉬를 사용
    public CapsuleCollider capsuleCollider;     //피격에 사용되는 기본 컬리젼
    public Animator anim;
    public MainManager mainManager;     //몬스터가 죽으면 현재 몬스터의 숫자를 계산하는 클래스
    public SkinnedMeshRenderer[] meshs; //피격시 몬스트의 색을 바꾸기 위한 콤포넌트

    [Header("-------[ 몬스터 상태 체커 ]")]
    public bool isChase;                //추적상태         
    public bool isAttack;               //공격상태
    public bool isDead;                 //죽은상태
    public bool isGetHit = false;       //피격상태
    public bool isPlaeyerFind = false;  //플레이어 발견 상태

    public float sightRange = 5f;       //플레이어 발견 거리
    public float attackInterval = 2f;

    [Header("-------[ 드롭 아이템 ]")]
    public GameObject[] dropItems;

    protected virtual void Awake()
    {
        //컴포넌트 생성
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        //meshs = GetComponentsInChildren<SkinnedMeshRenderer>();

        if (mainManager == null)
        {
            mainManager = FindObjectOfType<MainManager>();
        }
    }

    protected virtual void Start()
    {
        //10.11 수정. 이름으로 찾는 방식이 합칠 때마다 에러가 뜰 위험이 있어서 클래스를 찾는 방식으로 변경했습니다.
        //target = GameObject.Find("Player").GetComponent<Transform>();
        target = FindObjectOfType<Player>().transform;
        //by 손동욱

    }

    protected virtual void Update()
    {
        //몬스터 기본 행동 패턴
        SearchPlayer();
        if (!isAttack && !isDead && isPlaeyerFind)
        {
            MoveToTarget(); // 타겟을 향이 이동하는 메소드
            Targeting();
        }
    }
    /// <summary>
    /// 레이어 마스트를 통해 타겟을 찾고 타겟을 찾으면 isPlayerFind 를 반환
    /// </summary>
    protected virtual void SearchPlayer()
    {
        isPlaeyerFind = false;

        // 레이어 마스크를 통해 오브젝트(플레이어)를 감지하는 물리 구체
        Collider[] collider =
            Physics.OverlapSphere(transform.position, sightRange,
            LayerMask.GetMask("Player"));

        if (collider.Length > 0)
        {
            isPlaeyerFind = true;
        }
    }

    protected virtual void MoveToTarget()
    {
        isChase = true;
        float stopPoint = nav.stoppingDistance;

        if (!isGetHit && !isAttack)
        {
            if (Vector3.Distance(transform.position, target.position) < stopPoint)
            {
                StopNavMesh(true);
            }
            else
            {
                StopNavMesh(false);
            }
        }
    }
    /// <summary>
    /// 이동을 즉시 멈추기 위해 네비매시 이동을 멈춤
    /// </summary>
    /// <param name="isStop">네비매시 이동 ON/OFF</param>
    protected virtual void StopNavMesh(bool isStop)
    {
        if (isStop)
        {
            anim.SetBool("isWalk", false);          //애니메이션을 멈춤
            //포지션 값을 몬스터의 기준으로 변경
            nav.SetDestination(transform.position); //목표를 자신으로 설정
            nav.acceleration = float.MaxValue;      //목표까지의 가속을 최대치로 바꿈
            transform.position = transform.position;//나의 위치를 현재 위치로 바꿈
            nav.velocity = Vector3.zero;            //물리 힘을 제로로 만듬
            //네비매쉬 이동 관련값을 false로 변경
            nav.isStopped = true;                   //네비매쉬 멈춤
            nav.updatePosition = false;             //위치 업데이트 멈춤
            nav.updateRotation = false;             //방향 업데이트 멈춤
        }
        else
        {
            nav.isStopped = false;
            nav.updatePosition = true;
            nav.updateRotation = true;
            nav.SetDestination(target.position);
            anim.SetBool("isWalk", true);
        }
    }
    protected virtual void Targeting()
    {
        
    }
    /// <summary>
    /// 근접 몬스터 MeleeAttack Collision 켜고/끄기
    /// </summary>
    /// <param name="isAttack"></param>
    protected virtual void MeleeAttackTrigger(bool isAttack)
    {
        if (isAttack)
        {
            meleeAttack.SetActive(true);
        }
        else
        {
            meleeAttack.SetActive(false);
        }
    }

    /// <summary>
    /// 몬스터 공격 대기 시간
    /// 몬스터 공격 대기 시간
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator WaitForAttack()
    {
        Debug.Log("공격대기");
        MeleeAttackTrigger(false);              //meleeAttack Collision 끔
        isAttack = true;
        isChase = true;
        yield return new WaitForSeconds(attackInterval);
        isAttack = false;
    }
    /// <summary>
    /// SearchPlayer() 의 범위를 에디터에서만 표시
    /// </summary>
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Handles.DrawWireDisc(transform.position, transform.up, sightRange);
#endif
    }
}
