using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Boss : MonoBehaviour
{
    //public int curHealth;
    //public int maxHealth;
    public Transform target;            //�÷��̾� Ÿ��
    public float moveSpeed = default;             //�̵� �ӵ�
    //public float rotSpeed = 1.0f;
    public float targetRadiusSleeping = 15f; //������ �ڴ� ���¿��� �÷��̾ �����ϴ� ���� ������ �Ÿ�
    public float targetRadius = 2.5f;   //sphere�� ������ => ������ �÷��̾ �ν��ϴ� ���� ��Ÿ��� �̿�
    public float targetRange = 0f; //sphere�� ��Ƴ��� �Ÿ�(0���� �ؼ� ���ҽ� �Ƴ�, ���� ���������� ���� ���ϴ°� ���� �� ����
    public float delayStart = 3f;

    Vector3 targetDirection;
    //GameObject player;

    //public GameObject projectile;       //���Ÿ��� �߻�ü
    //public BoxCollider meleeAttack;     //�и� ���ÿ� �ø��� �ڽ�
    //public NavMeshAgent nav;            //�׺� �Ž��� ���
    Rigidbody rb;
    //public CapsuleCollider capsuleCollider;     //�ǰݿ� ���Ǵ� �⺻ �ø���
    Animator anim;

    bool isSleeping = false; //���� ���� �� ���� üũ
    bool isActive=false; //��Ʈ��(��ȿ) �������� üũ
    bool isChase=false;  //�̵� üũ
    bool isAttack=false; //���� ������(�÷��̾ ��Ÿ� �ȿ� �־�߸�) üũ
    bool isDead=false; //HP�� 0���� üũ
    bool isBattle = false; //����(���� ������) ������ üũ
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        target = target.GetComponent<Transform>();

        //���� �ð���(deley����) ���� �Ŀ� ������ ��Ƽ�� �ǵ���
        StartCoroutine(DelayStart());
        
    }
    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(delayStart);
        isSleeping = true;
        Debug.Log("���� ����");
    }
    

    private void Update()
    {


        if (!isAttack&&isActive)
        {
            Debug.Log("�̵� ����");
            MoveToTarget();          // Ÿ���� ���� �̵��ϴ� �޼ҵ�
            
        }else if (isSleeping)
        {
            SleepBoss();
        }

    }

    private void MoveToTarget()
    {
        //�̵� �ڵ�
        //isChase = true;                                                                     // �̵������� �˸��� bool ��
        targetDirection = (target.position - transform.position).normalized;                //Ÿ�� ��ġ�� ����

        rb.MovePosition(transform.position + targetDirection * Time.deltaTime * moveSpeed); //������ٵ� ����Ͽ� Ÿ������ �̵�
        transform.LookAt(target);                                                           //Lookat�� ����Ͽ� Ÿ�� �ٶ󺸱�

        anim.SetBool("isWalk", true);                                                       // walk �ִϸ��̼� Ȱ��ȭ

        //�� ���� �Լ�
        FindPlayer();//���� ���� �ȿ� �÷��̾ ������ Attack �Լ� ����
    }
    //���ݹ���*100�ȿ� ���� ���� + ���� �����̽ð��� ������ ������ �ڴ� ����

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
        yield return new WaitForSeconds(2.5f);//��ȿ �ִϸ��̼��� ������ �ð�, �� �Ŀ� ������ ������
        isActive = true;
    }

    //���� ���� �ȿ� �÷��̾ ������ Attack �Լ� ����
    void FindPlayer()
    {
        
        
        if (!isDead)
        {
            
            

        }
        else
        {
            //die �ִϸ��̼� + ������Ʈ �ı� + ���� ������Ʈ ���� / Victory UI active
        }

        //https://ssabi.tistory.com/29
        //https://www.youtube.com/watch?v=voEFSbIPYjw
        //SphereCastAll(������ġ, ������,���� ���ܾ� �� ����(����), �ִ� ����, üũ�� ���̾� ����ũ(üũ�� ���̾��� ��ü�� �ƴϸ� ����)
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
        //���������� ���� isBasicAttack, isTailAttack, ���̾�ִϸ��̼�(�߻�ü�������), ���ý�Ÿ���� �ִϸ��̼� ���� Ʈ���� ���� ����
        //����� �׳� basicattack����
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
                aniType = "isFireball";//�߻�ü ���� ������
                break;
            default:
                Debug.Log($"�ε�������, �Է� �ε��� : {randomType}");
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
