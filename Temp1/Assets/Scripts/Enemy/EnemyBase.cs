using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public bool isChase;                //타겟을 향해 이동중
    public bool isAttack;
    public bool isDead;
    public bool isGetHit = false;

   
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

}
