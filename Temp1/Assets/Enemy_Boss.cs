using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Boss : MonoBehaviour
{
    //public int curHealth;
    //public int maxHealth;
    public Transform target;            //플레이어 타겟
    public float moveSpeed = default;             //이동 속도
    //public float rotSpeed = 1.0f;
    public float targetRadiusSleeping = 15f; //보스가 자는 상태에서 플레이어를 감지하는 원의 반지름 거리
    public float targetRadius = 2.5f;   //sphere의 반지름 => 보스가 플레이어를 인식하는 공격 사거리로 이용
    public float targetRange = 0f; //sphere을 쏘아내는 거리(0으로 해서 리소스 아낌, 굳이 일직선으로 설정 안하는게 좋을 것 같음
    public float delayStart = 3f;

    Vector3 targetDirection;
    //GameObject player;

    //public GameObject projectile;       //원거리용 발사체
    //public BoxCollider meleeAttack;     //밀리 어택용 컬리젼 박스
    //public NavMeshAgent nav;            //네비 매쉬를 사용
    Rigidbody rb;
    //public CapsuleCollider capsuleCollider;     //피격에 사용되는 기본 컬리젼
    Animator anim;

    bool isSleeping = false; //보스 전투 전 상태 체크
    bool isActive=false; //인트로(포효) 끝났는지 체크
    bool isChase=false;  //이동 체크
    bool isAttack=false; //공격 중인지(플레이어가 사거리 안에 있어야만) 체크
    bool isDead=false; //HP가 0인지 체크
    bool isBattle = false; //전투(공격 중인지) 중인지 체크
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        target = target.GetComponent<Transform>();

        //일정 시간이(deley변수) 지난 후에 보스가 액티브 되도록
        StartCoroutine(DelayStart());
        
    }
    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(delayStart);
        isSleeping = true;
        Debug.Log("보스 시작");
    }
    

    private void Update()
    {


        if (!isAttack&&isActive)
        {
            Debug.Log("이동 시작");
            MoveToTarget();          // 타겟을 향이 이동하는 메소드
            
        }else if (isSleeping)
        {
            SleepBoss();
        }

    }

    private void MoveToTarget()
    {
        //이동 코드
        //isChase = true;                                                                     // 이동중임을 알리는 bool 값
        targetDirection = (target.position - transform.position).normalized;                //타겟 위치의 방향

        rb.MovePosition(transform.position + targetDirection * Time.deltaTime * moveSpeed); //리지드바디를 사용하여 타겟으로 이동
        transform.LookAt(target);                                                           //Lookat을 사용하여 타겟 바라보기

        anim.SetBool("isWalk", true);                                                       // walk 애니메이션 활성화

        //적 감지 함수
        FindPlayer();//공격 범위 안에 플레이어가 들어오면 Attack 함수 실행
    }
    //공격범위*100안에 들어온 상태 + 시작 딜레이시간이 지나기 전에는 자는 상태

    void SleepBoss()
    {
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadiusSleeping,
        transform.forward, targetRange, LayerMask.GetMask("Player"));
        
        if (rayHits.Length > 0)
        {
            //Debug.Log($"{rayHits[0].transform.gameObject.name}");
            isSleeping = false;
            StartCoroutine(SleepAwake());
        }
    }
    IEnumerator SleepAwake()
    {
        anim.SetBool("isActive", true);
        yield return new WaitForSeconds(2.5f);//포효 애니메이션이 끝나는 시간, 이 후에 보스가 움직임
        isActive = true;
    }

    //공격 범위 안에 플레이어가 들어오면 Attack 함수 실행
    void FindPlayer()
    {
        
        
        if (!isDead)
        {
            
            

        }
        else
        {
            //die 애니메이션 + 오브젝트 파괴 + 보상 오브젝트 생성 / Victory UI active
        }

        //https://ssabi.tistory.com/29
        //https://www.youtube.com/watch?v=voEFSbIPYjw
        //SphereCastAll(생성위치, 반지름,구가 생겨야 할 방향(벡터), 최대 길이, 체크할 레이어 마스크(체크할 레이어의 물체가 아니면 무시)
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius,
                transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttack = true;
        isChase = false;
        anim.SetBool("isWalk", false);
        anim.SetBool("isAttack", true);
        //랜덤변수에 따라서 isBasicAttack, isTailAttack, 파이어볼애니메이션(발사체현재없음), 슈팅스타어택 애니메이션 등의 트리거 조절 예정
        //현재는 그냥 basicattack으로
        int randomType = 0;
        //=>anim.SetBool("isBasicAttack", true);

        
        string aniType = "";
        switch (randomType)
        {
            case 0:
                aniType = "isBasicAttack";
                break;
            case 1:
                aniType = "isTailAttack";
                break;
            case 2:
                aniType = "isFireball";//발사체 현재 미지정
                break;
            default:
                Debug.Log($"인덱스에러, 입력 인덱스 : {randomType}");
                break;
        }
        anim.SetBool(name: aniType.ToString(), value: true);
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius,
                transform.forward, targetRange, LayerMask.GetMask("Player"));
        while (rayHits.Length > 0)
        {
            yield return new WaitForSeconds(2f);
            anim.SetBool(name: aniType.ToString(), value: true);

            rayHits = Physics.SphereCastAll(transform.position, targetRadius,
                transform.forward, targetRange, LayerMask.GetMask("Player"));



        }
        yield return new WaitForSeconds(0.5f);
        anim.SetBool(name: aniType.ToString(), value: false);
        yield return new WaitForSeconds(0.7f);
        isAttack = false;
        anim.SetBool("isAttack", false);

    }

}
