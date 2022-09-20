using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public enum Type { Orc, Skelleton, Mage, Shell, Boss }
    public Type enemyType;              //Attack 메서드에서 공격 타입을 설정하기 위해
    public int curHealth;
    public int maxHealth;
    public Transform target;            //플레이어 타겟
    public float moveSpeed = default;             //이동 속도
    public float rotSpeed = 1.0f;

    Vector3 targetDirection;

    public GameObject projectile;       //원거리용 발사체
    public BoxCollider meleeAttack;     //밀리 어택용 컬리젼 박스
    public NavMeshAgent nav;            //네비 매쉬를 사용
    public Rigidbody rb;
    public CapsuleCollider capsuleCollider;     //피격에 사용되는 기본 컬리젼
    public Animator anim;

    public bool isChase;                //타겟을 향해 이동중
    public bool isAttack;
    public bool isDead;

    bool isAttackTest = false;
    bool isWalkTest = false;
    bool isGetHitTest = false;
    bool isDieTest = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        meleeAttack = GetComponent<BoxCollider>();
        target = target.GetComponent<Transform>();
    }

    private void Start()
    {
        //meleeAttack.enabled = false;        //밀리 어택 컬리젼을 꺼 둠
        //StartCoroutine(Action());           //코루틴 테스트용 임시 코드

    }

    private void Update()
    {
        AnimationTest();                     // 애니메이션 테스트 메소드 1,2,3,4
        MoveToTarget();                     // 타겟을 향이 이동하는 메소드
    }

    private void MoveToTarget()
    {
        targetDirection = (target.position - transform.position).normalized;                //타겟 위치의 방향
        rb.MovePosition(transform.position + targetDirection * Time.deltaTime * moveSpeed); //리지드바디를 사용하여 타겟으로 이동
        transform.LookAt(target);                                                           //Lookat을 사용하여 타겟 바라보기
        isChase = true;                                                                     // 이동중임을 알리는 bool 값
        anim.SetBool("isWalk", true);                                                       // walk 애니메이션 활성화
    }

    private void AnimationTest()
    {
        if (Input.GetKeyDown("1") && !isAttackTest)
            StartCoroutine(Action1());

        if (Input.GetKeyDown("2") && !isWalkTest)
            StartCoroutine(Action2());

        if (Input.GetKeyDown("3") && !isGetHitTest)
            StartCoroutine(Action3());

        if (Input.GetKeyDown("4") && !isDieTest)
            StartCoroutine(Action4());
    }

    IEnumerator Action1()
    {
        anim.SetBool("isAttack", true);
        isAttackTest = true;
        yield return new WaitForSeconds(0.8f);

        anim.SetBool("isAttack", false);
        isAttackTest = false;
        StopCoroutine(Action1());
    }
    IEnumerator Action2()
    {
        anim.SetBool("isWalk", true);
        isWalkTest = true;
        yield return new WaitForSeconds(0.8f);

        anim.SetBool("isWalk", false);
        isWalkTest = false;
        StopCoroutine(Action2());
    }
    IEnumerator Action3()
    {
        isGetHitTest = true;
        anim.SetBool("isGetHit", true);
        yield return new WaitForSeconds(0.8f);

        anim.SetBool("isGetHit", false);
        isGetHitTest = false;
        StopCoroutine(Action3());
    }
    IEnumerator Action4()
    {
        anim.SetTrigger("doDie");
        isDieTest = true;
        yield return new WaitForSeconds(0.8f);

        isDieTest = false;
        StopCoroutine(Action4());
    }
    
}
