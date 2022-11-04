using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Boss : MonoBehaviour, ICharacter
{
    public MainManager mainManager;
    public GameObject projectile_FireBall;
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
    public const float godTime = 0.1f; //피격 시 무적인 시간
    public float longAttackTurm = 3f;  //원거리 공격을 하는 간격. 걷는 모션 중에 타이머가 작동하여 해당 변수보다 타이머가 커지면 원거리 공격 실행
    public float frontOfMouse_X = 3f;
    public float frontOfMouse_Y = 1.7f;

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
    float longAttackTimer = 0;
    int randomType = 0; //공격 타입 index를 저장할 변수. 0=일반, 1=꼬리, 2=불구슬날리기... 더 늘 수 있음
    string aniType = "";//공격 타입 index에 맞는 string 값이 저장될 변수. 수동으로 입력한 애니메이션 각 공격 트리거의 트리거 변수값을 그대로 받는다.
    private void Awake()
    {
        mainManager = FindObjectOfType<MainManager>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        
        //일정 시간이(deley변수) 지난 후에 보스가 액티브 되도록
        StartCoroutine(DelayStart());
    }
    void Start()
    {
        if (target == null)
        {
            target = FindObjectOfType<Player>().transform;
        }
        target.position = new Vector3(target.position.x, 0, target.position.y);
    }
    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(delayStart);
        isSleeping = true;
        //Debug.Log("보스 시작");
    }
    private void Update()
    {
        //idle 상태
        if (!isAttack && isActive && !isAttacked) 
        {
            transform.LookAt(target);
            rayHits = Physics.SphereCastAll(transform.position, targetRadius,
                transform.forward, targetRange, LayerMask.GetMask("Player"));

            //적 감지, 근접 공격
            if (rayHits.Length != 0) 
            {
                //주석 이유 : 공격 타입 랜덤 지정에서 eDamage를 타입에 맞는 값으로 지정하기 때문에 중복을 방지하기 위해서 주석 처리
                //eDamage = damage;//특별하게 데미지 추가가 없으면 public으로 설정한 damage를 eDmage에 대입
                isAttack = true;
                attackTimer = 0;
                StartCoroutine(MeleeAniAttack());
            }
            //적이 근접하지 않았을 때(원거리 or 걷기)
            else
            {
                //원거리 공격 범위 내에 플레이어 찾기. 보스는 플레이어를 LoockAt하기 때문에 방향은 forward해도 플레이어 쪽에 레이를 쏜다.
                //또한, 보스가 한 번 액티브 되면 계속 쫒아도 괜찮은 것 같으니 distance를 25 정도로 높게 둔다.
                rayHits = Physics.BoxCastAll(transform.position + Vector3.up * 1.3f, transform.lossyScale / 2.0f,
                target.position - transform.position, transform.rotation, 100, LayerMask.GetMask("Player"));
                //원거리 공격 범위에 적이 있을 때
                if (rayHits.Length != 0)
                {
                    Debug.Log("인식");
                    //원거리 공격 타이머 재기
                    longAttackTimer += Time.deltaTime * 100;
                    if(longAttackTimer > longAttackTurm)
                    {
                        longAttackTimer = 0;
                        isAttack = true;
                        attackTimer = 0;
                        StartCoroutine(LongAniAttack(target.gameObject));
                    }
                    else
                    {
                        MoveToTarget();
                    }
                }
                else
                {
                    MoveToTarget();
                }
                //MoveToTarget();  // 타겟을 향해서 이동하는 메소드
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
        if (target!=null)
        {
            targetDirection = (target.position - transform.position).normalized;
            //물리힘을 줘서 이동하는 velocity, MovePosition, AddForce는 플레이어 리지드바디와 충돌해서 계속 이상하게 움직여짐
            //그래서 위치(position) 이동 방식 사용 by 손동욱 10.19
            transform.position += targetDirection * Time.deltaTime * moveSpeed;
            //rb.MovePosition(transform.position + targetDirection * Time.deltaTime * moveSpeed); //리지드바디를 사용하여 타겟으로 이동
            //rb.velocity += targetDirection * Time.deltaTime * moveSpeed;
            anim.SetBool("isWalk", true);
        }
        else
        {
            return;
        }
    }
    //근접 랜덤 공격
    IEnumerator MeleeAniAttack()
    {
        anim.SetBool("isWalk", false);
        randomType = Random.Range(0,2);//Random.Range는 Max 벨류가 Exclusive라 포함되지 않는(미만)이기 때문에 Max값을 +1 해야함
        //Debug.Log(randomType);
        //+randomType 변수를 랜덤하게 지정, 랜덤 패턴으로 공격할 수 있도록 변경 예정 by 손동욱 10.19
        //aniType = "";
        
        //근접 공격 랜덤 switch 문
        switch (randomType)
        {
            case 0:
                aniType = "isBasicAttack";
                eDamage = damage;
                break;
            case 1:
                aniType = "isTailAttack";
                eDamage = damage * 2;
                break;
                //주석 이유 : 원거리 공격은 따로 만들어야 플레이 적으로 괜찮을 것 같아서 변경
                /*
            case 2:
                aniType = "isFireball";//발사체 현재 미지정
                eDamage = damage * 3;
                break;
                */
            default:
                Debug.Log($"인덱스에러, 입력 인덱스 : {randomType}");
                aniType = "";
                break;
        }
        anim.SetTrigger(name: aniType.ToString());
        yield return null;
        //주석 이유 : 이전 코드
        //yield return new WaitForSeconds(attackSpeed*0.5f);
        //CalculateRealAttack()


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
    //원거리 랜덤 공격(현재 파이어볼 하나)
    IEnumerator LongAniAttack(GameObject obj)
    {
        Debug.Log(obj.name);
        anim.SetBool("isWalk", false);
        randomType = Random.Range(0, 1); // 현재 하나라 0까지만 받기로..
        switch (randomType)
        {
            case 0:
                aniType = "isFireballShoot";//발사체 현재 미지정
                eDamage = damage * 3;
                break;
            default:
                Debug.Log($"인덱스에러, 입력 인덱스 : {randomType}");
                aniType = "";
                break;
        }
        anim.SetTrigger(name: aniType.ToString());
        yield return null;
    }

    //파이어볼 생성, 애니메이터에서 이벤트 호출됨 
    public void CreateFireball()
    {
        Vector3 frontOfMouse = transform.forward * frontOfMouse_X + Vector3.up * frontOfMouse_Y;
        Debug.Log("생성");
        GameObject projectile = Instantiate(projectile_FireBall, transform.position + frontOfMouse, transform.rotation);
        projectile.GetComponent<FireBall>().target = target.gameObject;
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
        //현재 플레이어가 보스 공격에 죽으면 바로 프리팹이 사라져서 보스 몬스터의 move함수가 에러 생성(target이 null이 되었으니..) 
        //나중에 HP를 구하던가 일시정지를 하던가 해야함
    }
    void SleepBoss()
    {
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadiusSleeping*2,
        transform.forward, 0, LayerMask.GetMask("Player"));

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
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if(target == null)
        {
            target = FindObjectOfType<Player>().transform;
        }
        // 함수 파라미터 : 현재 위치, Ray의 방향, RaycastHit 결과, Raycast를 진행할 거리
        if (true == Physics.BoxCast(transform.position + Vector3.up * 1.3f, transform.lossyScale / 2.0f,
                target.position - transform.position, out RaycastHit hit, transform.rotation, 100, LayerMask.GetMask("Player")))
        {
            // Hit된 지점까지 ray를 그려준다.
            Gizmos.DrawRay(transform.position, transform.forward * hit.distance);

            Gizmos.DrawWireCube(transform.position + transform.forward * hit.distance, transform.lossyScale);
        }
        else
        {
            // Hit가 되지 않았으면 최대 검출 거리로 ray를 그려준다.
            Gizmos.DrawRay(transform.position, transform.forward * 100);
            Gizmos.DrawWireCube(transform.position + transform.forward * 100, transform.lossyScale);
        }
        //Gizmos.DrawRay(transform.position, transform.forward * hit.distance + Vector3.up);
    }
    //애니메이션이 끝나면 실행되는 코루틴 함수. 
    IEnumerator CalculateRealAttack()
    {
        //isAttack(공격속도를 타이머로 재서 true, false로 변하는 bool 변수)이 굳이 필요하진 않지만, 기왕 만들었으니 사용
        //!isAttacked(공격받는 중인지 true, false로 변하는 bool 변수)는 공격 받는 중에는 경직 및 GetHit 애니메이션이 실행되므로 실제 데미지를 입히는건 안맞는 듯
        if (!isAttacked && isAttack)
        {
            //공격을 했을 때 이미 rayHits가 플레이어를 잡았을거라 예상되는데 혹시 몰라서 다시 찾아놓기
            if (rayHits == null)
            {
                rayHits = Physics.SphereCastAll(transform.position, targetRadius,
                transform.forward, targetRange, LayerMask.GetMask("Player"));
            }
            Attack(rayHits[0].transform.gameObject, eDamage); //실제 데미지 적용
        }
        yield return null;
    }
    //GetHit 애니메이션에 추가할 예정이었던 함수.
    //GetHit애니메이션에 이벤트를 달아서 플레이어의 데미지를 보스에 적용할려고 했는데 
    //원래 맞는 애니메이션은 맞는 순간 데미지가 들어오는 거니까 굳이 안넣어도 될 것 같아서 수정
    /*
    IEnumerator CalculateRealGetHit()
    {
        yield return null;
    }*/
}
