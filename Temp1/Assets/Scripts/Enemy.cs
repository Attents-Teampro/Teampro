using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class Enemy : MonoBehaviour, ICharacter
{
    public enum Type { Orc, Skelleton, Mage, Shell, Boss }
    public Type enemyType;              //Attack 메서드에서 공격 타입을 설정하기 위해
    public int curHealth;               //현재 체력
    public int maxHealth;               //최대 체력
    public int minDamage;               //최소 공격 데미지
    public int maxDamage;               //최대 공격 데미지
    public Transform target;            //플레이어 타겟
    public float moveSpeed = default;   //이동 속도
    public float rotSpeed = 1.0f;

    Vector3 targetDirection;

    public GameObject projectile;       //원거리용 발사체
    public GameObject meleeAttack;     //밀리 어택용 컬리젼 박스 : 밀리어택은 하위 클래스에서 처리
    public NavMeshAgent nav;            //네비 매쉬를 사용
    public Rigidbody rb;
    public CapsuleCollider capsuleCollider;     //피격에 사용되는 기본 컬리젼
    public Animator anim;

    public bool isChase;                //타겟을 향해 이동중
    public bool isAttack;
    public bool isDead;
    public bool isGetHit = false;

    ICharacter playerCharacter;

    private Transform mageBulletPosition;   //마법사 발사체(projectile) 생성 위치
    private Transform mageStaff;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        //projectile = gameObject.GetComponent<GameObject>();

        if (enemyType == Type.Mage)
        {
            mageStaff = transform.GetChild(2);
            mageBulletPosition = mageStaff.GetChild(0);
        }
    }

    private void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
        playerCharacter = target.GetComponent<ICharacter>();      //플레이어 캐릭터의 인터페이스 참조
        if(meleeAttack != null)
        {
            meleeAttack.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isAttack)
        {
            MoveToTarget();                     // 타겟을 향이 이동하는 메소드
            Targeting();
        }
    }
        
    /// <summary>
    /// 타겟을 향해 이동 : 추 후 타겟 거리를 보고 이동 targetDistance 변수로 처리 예정
    /// </summary>
    private void MoveToTarget()
    {
        isChase = true;                                                                     // 이동중임을 알리는 bool 값

        if (!isGetHit)
        {
            targetDirection = (target.position - transform.position).normalized;                //타겟 위치의 방향

            rb.MovePosition(transform.position + targetDirection * Time.deltaTime * moveSpeed); //리지드바디를 사용하여 타겟으로 이동
            transform.LookAt(target);                                                           //Lookat을 사용하여 타겟 바라보기

            anim.SetBool("isWalk", true);                                                       // walk 애니메이션 활성화}
        }
    }

    void Targeting()
    {
        float targetRadius = default;   //sphere의 반지름
        float targetRange = default;    //최대 길이
        
        if (!isDead)
        {
            switch (enemyType)
            {
                case Type.Orc:
                    targetRadius = 1.5f;
                    targetRange = 1f;
                    break;

                case Type.Skelleton:
                    targetRadius = 1.5f;
                    targetRange = 1f;
                    break;

                case Type.Mage:
                    targetRadius = 0.5f;
                    targetRange = 10f;
                    break;

                case Type.Shell:
                    targetRadius = 0.5f;
                    targetRange = 7f;
                    break;
            }
        }

        //https://ssabi.tistory.com/29
        //https://www.youtube.com/watch?v=voEFSbIPYjw
        //SphereCastAll(생성위치, 반지름,구가 생겨야 할 방향(벡터), 최대 길이, 체크할 레이어 마스크(체크할 레이어의 물체가 아니면 무시)
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius,
                transform.forward, targetRange, LayerMask.GetMask("Player"));

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
       

        switch (enemyType)
        {
            case Type.Orc:
                anim.SetTrigger("doAttack");
                meleeAttack.SetActive(true);
                yield return new WaitForSeconds(1f);
                isAttack = false;
                meleeAttack.SetActive(false);
                break;

            case Type.Skelleton:
                anim.SetTrigger("doAttack");
                meleeAttack.SetActive(true);
                yield return new WaitForSeconds(0.6f);
                isAttack = false;
                meleeAttack.SetActive(false);
                break;

            case Type.Mage:
                anim.SetTrigger("doAttack");
                yield return new WaitForSeconds(0.4f);
                MageAttack();
                yield return new WaitForSeconds(1.1f);
                isAttack = false;
                break;

            case Type.Shell:
                anim.SetTrigger("doAttack");
                meleeAttack.SetActive(true);
                transform.position = target.position;
                yield return new WaitForSeconds(0.5f);
                isAttack = false;
                meleeAttack.SetActive(false);
                break;
        }

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

        Destroy(gameObject);
    }

    
    /// <summary>
    /// Enemy 피격 함수 : 피격관련 모션 처리
    /// </summary>
    /// <returns></returns>
    IEnumerator OnGetHit()
    {
        curHealth -= 50; // 테스트용 데미지 값
        anim.SetBool("isWalk", false);
        isGetHit = true;
        anim.SetTrigger("doGetHit");
        if (curHealth < 0)
        {
            Die();
        }
        yield return new WaitForSeconds(0.8f);
        isGetHit = false;
    }
    
    
    /// <summary>
    /// 마법사용 원거리 발사체 생성 함수
    /// </summary>
    /// <returns></returns>
    void MageAttack()
    {
        //yield return new WaitForSeconds(0.2f);
        Instantiate(projectile, mageBulletPosition.position, Quaternion.identity);
        //yield return new WaitForSeconds(1.1f);
        //isAttack = false;
    }

    public void Die()
    {
        StartCoroutine(OnDead());
    }
    public void Attacked(int damage)
    {
        curHealth -= damage;
        StartCoroutine(OnGetHit());
    }

    public void Attack(GameObject target, int damage)
    {
        StartCoroutine(enemyAttack());
       // playerCharacter.Attacked(maxDamage);
    }
}
