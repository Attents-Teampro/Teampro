using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Boss : MonoBehaviour, ICharacter
{
    public MainManager mainManager;

    //public int curHealth;
    //public int maxHealth;
    public Transform target;            //플레이어 타겟
    public float moveSpeed = default;             //이동 속도
    public float attackSpeed = 3f;
    //public float rotSpeed = 1.0f;
    public float targetRadiusSleeping = 15f; //보스가 자는 상태에서 플레이어를 감지하는 원의 반지름 거리
    public float targetRadius = 2.5f;   //sphere의 반지름 => 보스가 플레이어를 인식하는 공격 사거리로 이용
    public float targetRange = 0f; //sphere을 쏘아내는 거리(0으로 해서 리소스 아낌, 굳이 일직선으로 설정 안하는게 좋을 것 같음
    public float delayStart = 3f;
    public int eHP = 100;
    public int damage = 100;
    public float godTime = 0.1f; //피격 시 무적인 시간

    //GameObject player;
    //public GameObject projectile;       //원거리용 발사체
    //public BoxCollider meleeAttack;     //밀리 어택용 컬리젼 박스
    //public NavMeshAgent nav;            //네비 매쉬를 사용
    //public CapsuleCollider capsuleCollider;     //피격에 사용되는 기본 컬리젼
    Rigidbody rb;
    Animator anim;
    RaycastHit[] rayHits;
    Vector3 targetDirection;
    bool isSleeping = false; //보스 전투 전 상태 체크
    bool isActive = false; //인트로(포효) 끝났는지 체크
    bool isChase = false;  //이동 체크
    bool isAttack = false; //공격 중인지(플레이어가 사거리 안에 있어야만) 체크
    bool isDead = false; //HP가 0인지 체크
    bool isBattle = false; //전투(공격 중인지) 중인지 체크
    bool isAttacked = false;
    bool isGod = false; //전투 중 피격 시 피격무적 체크
    int eDamage = 0;
    float attackTimer = 0;

    private void Awake()
    {
        mainManager = FindObjectOfType<MainManager>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        if (target == null)
        {
            target = FindObjectOfType<Player>().transform;
        }
        target = target.GetComponent<Transform>();

        //일정 시간이(deley변수) 지난 후에 보스가 액티브 되도록
        StartCoroutine(DelayStart());

    }
    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(delayStart);
        isSleeping = true;
        //Debug.Log("보스 시작");
    }
    private void Update()
    {

        if (!isAttack && isActive && !isAttacked) // idle 상태
        {
            transform.LookAt(target);
            rayHits = Physics.SphereCastAll(transform.position, targetRadius,
                transform.forward, targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length != 0 && !isAttacked) // 적 감지, 공격
            {
                eDamage = damage;//특별하게 데미지 추가가 없으면 public으로 설정한 damage를 eDmage에 대입
                isAttack = true;
                attackTimer = 0;
                StartCoroutine(AniAttack());
            }
            else // 적을 i아서 이동
            {
                MoveToTarget();  // 타겟을 향해서 이동하는 메소드
            }
        }
        else if (isSleeping)
        {
            SleepBoss();
        }
        if (isAttack && isActive) // 공격 중
        {
            transform.LookAt(target);
            attackTimer += Time.deltaTime;
            if (attackSpeed < attackTimer)
            { 
                isAttack = false;
                Debug.Log("isAttack은 폴스 값으로변경");
            }

        }

    }
    private void MoveToTarget()
    {
        targetDirection = (target.position - transform.position).normalized;


        //물리힘을 줘서 이동하는 velocity, MovePosition, AddForce는 플레이어 리지드바디와 충돌해서 계속 이상하게 움직여짐
        //그래서 위치(position) 이동 방식 사용 by 손동욱 10.19
        transform.position += targetDirection * Time.deltaTime * moveSpeed;
        //rb.MovePosition(transform.position + targetDirection * Time.deltaTime * moveSpeed); //리지드바디를 사용하여 타겟으로 이동
        //rb.velocity += targetDirection * Time.deltaTime * moveSpeed;
        anim.SetBool("isWalk", true);


    }
    //공격범위*100안에 들어온 상태 + 시작 딜레이시간이 지나기 전에는 자는 상태
    IEnumerator AniAttack()
    {
        int randomType = 0;
        //+randomType 변수를 랜덤하게 지정, 랜덤 패턴으로 공격할 수 있도록 변경 예정 by 손동욱 10.19
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
        anim.SetTrigger(name: aniType.ToString());
        yield return new WaitForSeconds(attackSpeed*0.5f);
        if (!isAttacked && isAttack)
        {
            Attack(rayHits[0].transform.gameObject, eDamage); //실제 데미지 적용
            Debug.Log("공격");
        }
        anim.SetBool("isWalk", false);

        //isAttack = true;
        //Debug.Log("isAttack은 트루 값으로변경");

        //anim.SetBool(`", true);
        //랜덤변수에 따라서 isBasicAttack, isTailAttack, 파이어볼애니메이션(발사체현재없음), 슈팅스타어택 애니메이션 등의 트리거 조절 예정
        //현재는 그냥 basicattack으로
        //=>anim.SetBool("isBasicAttack", true);
        //anim.SetBool(name: aniType.ToString(), value: true);
        //anim.SetBool(name: aniType.ToString(), value: true);
        //anim.SetBool(name: aniType.ToString(), value: false);
        //anim.SetBool("isAttack", false);
        //공격이 플레이어에게 실적용되는 딜레이
        //이유 : 애니메이션 시작과 동시에 데미지가 들어가면 부자연스러움으로 딜레이를 수동으로 주었음//이전 코드

    }
    public void Die()
    {
        isActive = false;
        anim.SetTrigger("doDie");
        mainManager.numOfDieEnemy++;
        if (mainManager.numOfDieEnemy == mainManager.numOfStageEnemy)
        {
            mainManager.StageClear();
        }
        Destroy(gameObject, 1.7f * 2f);
    }
    public void Attacked(int d)
    {
        if (isGod)
        {
        }
        else
        {
            StartCoroutine(CanHit()); //isGod 계산 코루틴
            StartCoroutine(DelayAttacked(0.87f)); //isAttacked 컨트롤 용 코루틴
            eHP -= d;
            if (eHP <= 0)
            {
                Die();
            }
            else
            {
                anim.SetTrigger("isGetHit");
            }
        }
    }
    public void Attack(GameObject target, int d)
    {
        ICharacter ic = target.GetComponent<ICharacter>();
        ic.Attacked(d);
    }
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
        //플레이어 바라보기
        transform.LookAt(target); //나중에 램프로 하면 부드럽게 회전할 듯 by 손동욱 10.18
        //포효하기
        anim.SetBool("isActive", true);
        //포효끝나는거 기다리고(밑 Waitforseconds만큼 기다리기), 완료되면 MoveToTarget 조건 성립해주기(isActive)
        //포효 애니메이션이 끝나는 시간, 이 후에 보스가 움직임
        
        yield return new WaitForSeconds(3f);
        isActive = true;
    }
    //isAttacked 컨트롤 용 코루틴
    IEnumerator DelayAttacked(float time=1f)
    {
        isActive = true; //보스 슬립 상태에서 공격 받을 시를 위한 true
        isAttacked = true;
        yield return new WaitForSeconds(time);
        if(time > godTime)
        {
            if (isGod)
            {
                yield break;
            }
        }
        isAttacked = false;
        isAttack = false;
        //공격 및 이동이 가능하게 해주는 false 대입
    }
    //피격 시 무적 시간 계산 코루틴
    IEnumerator CanHit()
    {
        isGod = true;
        yield return new WaitForSeconds(godTime);
        isGod = false;
    }
}
