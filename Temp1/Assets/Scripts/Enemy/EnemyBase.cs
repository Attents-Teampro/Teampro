using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class EnemyBase : MonoBehaviour
{
    public EnemyData enemyData;
    public Transform target;            //플레이어 타겟

    public GameObject meleeAttack;     //밀리 어택용 컬리젼 박스 : 밀리어택은 하위 클래스에서 처리
    public NavMeshAgent nav;            //네비 매쉬를 사용
    public Rigidbody rb;
    public CapsuleCollider capsuleCollider;     //피격에 사용되는 기본 컬리젼
    public Animator anim;
    public MainManager mainManager;     //몬스터가 죽으면 현재 몬스터의 숫자를 계산하는 클래스
    public float sightRange = 100f;

    public bool isChase;                //타겟을 향해 이동중
    public bool isAttack;
    public bool isDead;
    public bool isGetHit = false;
    public bool isPlaeyerFind = false;

   
    protected virtual void Awake()
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

    protected virtual void Start()
    {
        //10.11 수정. 이름으로 찾는 방식이 합칠 때마다 에러가 뜰 위험이 있어서 클래스를 찾는 방식으로 변경했습니다.
        //target = GameObject.Find("Player").GetComponent<Transform>();
        target = FindObjectOfType<Player>().transform;
        //by 손동욱
    }

    protected virtual void Update()
    {
        SearchPlayer();
        if (!isAttack && !isDead && isPlaeyerFind)
        {
            MoveToTarget();                     // 타겟을 향이 이동하는 메소드
            Targeting();
        }
    }
    protected virtual void SearchPlayer()
    {
        isPlaeyerFind = false;        

        // 레이어 마스크를 통해 오브젝트를 감지하는 물리 구체
        Collider[] collider =
            Physics.OverlapSphere(transform.position, sightRange,
            LayerMask.GetMask("Player"));

        if (collider.Length > 0)
        {
            //Vector3 playerPos = collider[0].transform.position;
            //Vector3 toPlayerDir = playerPos - transform.position;
            //float angle = Vector3.Angle(transform.forward, toPlayerDir);
            //if (sightHalfAngle > angle)
            //{
            //    Ray ray = new(transform.position + transform.up * 0.5f, toPlayerDir);
            //    if (Physics.Raycast(ray, out RaycastHit hit, sightRange))
            //    {
            //        if (hit.collider.CompareTag("Player"))
            //        {
            //            chaseTarget = collider[0].transform;
            //            result = true;
            //        }
            //    }
            //}
            //result = true;
            isPlaeyerFind = true;
        }
        //return result;
    }

    protected virtual void MoveToTarget()
    {
        isChase = true;                                                                     // 이동중임을 알리는 bool 값

        if (!isGetHit && !isAttack)
        {
            //targetDirection = (target.position - transform.position).normalized;                //타겟 위치의 방향

            //rb.MovePosition(transform.position + targetDirection * Time.deltaTime * enemyData.MoveSpeed); //리지드바디를 사용하여 타겟으로 이동
            //transform.LookAt(target);                                                           //Lookat을 사용하여 타겟 바라보기

            //anim.SetBool("isWalk", true);                                                       // walk 애니메이션 활성화}
        }
    }
    protected virtual void Targeting()
    {
        //https://ssabi.tistory.com/29
        //https://www.youtube.com/watch?v=voEFSbIPYjw
        //SphereCastAll(생성위치, 반지름,구가 생겨야 할 방향(벡터), 최대 길이, 체크할 레이어 마스크(체크할 레이어의 물체가 아니면 무시)
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, enemyData.TargetRadius,
                transform.forward, enemyData.TargetRange, LayerMask.GetMask("Player"));

        // 레이캐스트에 Player 오브젝트가 판별되면 어택
        //if (rayHits.Length > 0)
        //{
        //    StartCoroutine(enemyAttack());
        //}
    }

    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, transform.up, sightRange);
    }
}
