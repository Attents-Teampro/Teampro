using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { Orc, Skelleton, Mage, Shell, Boss }
    public Type enemyType;              //Attack 메서드에서 공격 타입을 설정하기 위해
    public int curHealth;
    public int maxHealth;
    public Transform target;            //플레이어 타겟

    public GameObject projectile;       //원거리용 발사체
    public BoxCollider meleeAttack;     //밀리 어택용 컬리젼 박스
    public NavMeshAgent nav;            //네비 매쉬를 사용
    public Rigidbody rigid;
    public BoxCollider boxCollider;     //피격에 사용되는 기본 컬리젼
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
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        //meleeAttack.enabled = false;        //밀리 어택 컬리젼을 꺼 둠
        //StartCoroutine(Action());           //코루틴 테스트용 임시 코드
    }

    private void Update()
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
    //IEnumerator Action()
    //{
    //    meleeAttack.enabled = true;         //밀리어택의 컬리젼을 켜 줌
    //    anim.SetBool("isAttack", true);
    //    int actionNum = Random.Range(0, 4);
    //    switch (actionNum)
    //    {
    //        case 0:
    //            anim.SetBool("isAttack", true);
    //            break;
    //    }
    //}
}
