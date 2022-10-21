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
    public Transform target;            //�÷��̾� Ÿ��
    public float moveSpeed = default;             //�̵� �ӵ�
    public float attackSpeed = 3f;
    //public float rotSpeed = 1.0f;
    public float targetRadiusSleeping = 15f; //������ �ڴ� ���¿��� �÷��̾ �����ϴ� ���� ������ �Ÿ�
    public float targetRadius = 2.5f;   //sphere�� ������ => ������ �÷��̾ �ν��ϴ� ���� ��Ÿ��� �̿�
    public float targetRange = 0f; //sphere�� ��Ƴ��� �Ÿ�(0���� �ؼ� ���ҽ� �Ƴ�, ���� ���������� ���� ���ϴ°� ���� �� ����
    public float delayStart = 3f;
    public int eHP = 100;
    public int damage = 100;
    public float godTime = 0.1f; //�ǰ� �� ������ �ð�

    //GameObject player;
    //public GameObject projectile;       //���Ÿ��� �߻�ü
    //public BoxCollider meleeAttack;     //�и� ���ÿ� �ø��� �ڽ�
    //public NavMeshAgent nav;            //�׺� �Ž��� ���
    //public CapsuleCollider capsuleCollider;     //�ǰݿ� ���Ǵ� �⺻ �ø���
    Rigidbody rb;
    Animator anim;
    RaycastHit[] rayHits;
    Vector3 targetDirection;
    bool isSleeping = false; //���� ���� �� ���� üũ
    bool isActive = false; //��Ʈ��(��ȿ) �������� üũ
    bool isChase = false;  //�̵� üũ
    bool isAttack = false; //���� ������(�÷��̾ ��Ÿ� �ȿ� �־�߸�) üũ
    bool isDead = false; //HP�� 0���� üũ
    bool isBattle = false; //����(���� ������) ������ üũ
    bool isAttacked = false;
    bool isGod = false; //���� �� �ǰ� �� �ǰݹ��� üũ
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

        //���� �ð���(deley����) ���� �Ŀ� ������ ��Ƽ�� �ǵ���
        StartCoroutine(DelayStart());

    }
    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(delayStart);
        isSleeping = true;
        //Debug.Log("���� ����");
    }
    private void Update()
    {

        if (!isAttack && isActive && !isAttacked) // idle ����
        {
            transform.LookAt(target);
            rayHits = Physics.SphereCastAll(transform.position, targetRadius,
                transform.forward, targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length != 0 && !isAttacked) // �� ����, ����
            {
                eDamage = damage;//Ư���ϰ� ������ �߰��� ������ public���� ������ damage�� eDmage�� ����
                isAttack = true;
                attackTimer = 0;
                StartCoroutine(AniAttack());
            }
            else // ���� �i�Ƽ� �̵�
            {
                MoveToTarget();  // Ÿ���� ���ؼ� �̵��ϴ� �޼ҵ�
            }
        }
        else if (isSleeping)
        {
            SleepBoss();
        }
        if (isAttack && isActive) // ���� ��
        {
            transform.LookAt(target);
            attackTimer += Time.deltaTime;
            if (attackSpeed < attackTimer)
            { 
                isAttack = false;
                Debug.Log("isAttack�� ���� �����κ���");
            }

        }

    }
    private void MoveToTarget()
    {
        targetDirection = (target.position - transform.position).normalized;


        //�������� �༭ �̵��ϴ� velocity, MovePosition, AddForce�� �÷��̾� ������ٵ�� �浹�ؼ� ��� �̻��ϰ� ��������
        //�׷��� ��ġ(position) �̵� ��� ��� by �յ��� 10.19
        transform.position += targetDirection * Time.deltaTime * moveSpeed;
        //rb.MovePosition(transform.position + targetDirection * Time.deltaTime * moveSpeed); //������ٵ� ����Ͽ� Ÿ������ �̵�
        //rb.velocity += targetDirection * Time.deltaTime * moveSpeed;
        anim.SetBool("isWalk", true);


    }
    //���ݹ���*100�ȿ� ���� ���� + ���� �����̽ð��� ������ ������ �ڴ� ����
    IEnumerator AniAttack()
    {
        int randomType = 0;
        //+randomType ������ �����ϰ� ����, ���� �������� ������ �� �ֵ��� ���� ���� by �յ��� 10.19
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
                aniType = "isFireball";//�߻�ü ���� ������
                break;
            default:
                Debug.Log($"�ε�������, �Է� �ε��� : {randomType}");
                break;
        }
        anim.SetTrigger(name: aniType.ToString());
        yield return new WaitForSeconds(attackSpeed*0.5f);
        if (!isAttacked && isAttack)
        {
            Attack(rayHits[0].transform.gameObject, eDamage); //���� ������ ����
            Debug.Log("����");
        }
        anim.SetBool("isWalk", false);

        //isAttack = true;
        //Debug.Log("isAttack�� Ʈ�� �����κ���");

        //anim.SetBool(`", true);
        //���������� ���� isBasicAttack, isTailAttack, ���̾�ִϸ��̼�(�߻�ü�������), ���ý�Ÿ���� �ִϸ��̼� ���� Ʈ���� ���� ����
        //����� �׳� basicattack����
        //=>anim.SetBool("isBasicAttack", true);
        //anim.SetBool(name: aniType.ToString(), value: true);
        //anim.SetBool(name: aniType.ToString(), value: true);
        //anim.SetBool(name: aniType.ToString(), value: false);
        //anim.SetBool("isAttack", false);
        //������ �÷��̾�� ������Ǵ� ������
        //���� : �ִϸ��̼� ���۰� ���ÿ� �������� ���� ���ڿ����������� �����̸� �������� �־���//���� �ڵ�

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
            StartCoroutine(CanHit()); //isGod ��� �ڷ�ƾ
            StartCoroutine(DelayAttacked(0.87f)); //isAttacked ��Ʈ�� �� �ڷ�ƾ
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
        //�÷��̾� �ٶ󺸱�
        transform.LookAt(target); //���߿� ������ �ϸ� �ε巴�� ȸ���� �� by �յ��� 10.18
        //��ȿ�ϱ�
        anim.SetBool("isActive", true);
        //��ȿ�����°� ��ٸ���(�� Waitforseconds��ŭ ��ٸ���), �Ϸ�Ǹ� MoveToTarget ���� �������ֱ�(isActive)
        //��ȿ �ִϸ��̼��� ������ �ð�, �� �Ŀ� ������ ������
        
        yield return new WaitForSeconds(3f);
        isActive = true;
    }
    //isAttacked ��Ʈ�� �� �ڷ�ƾ
    IEnumerator DelayAttacked(float time=1f)
    {
        isActive = true; //���� ���� ���¿��� ���� ���� �ø� ���� true
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
        //���� �� �̵��� �����ϰ� ���ִ� false ����
    }
    //�ǰ� �� ���� �ð� ��� �ڷ�ƾ
    IEnumerator CanHit()
    {
        isGod = true;
        yield return new WaitForSeconds(godTime);
        isGod = false;
    }

}
