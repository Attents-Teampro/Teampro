using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, ICharacter
{
    public GameObject hitEffect;
    public enum Type { Melee, Range };  // �ٰŸ� ���Ÿ�
    public Type type;

    //10.11 ���� �̸� ���� by �յ���
    public int iDamage = 100;

    public float rate;  // ���� �ӵ�
    public BoxCollider meleeArea;   // ���� ����
    public Transform arrowPos;
    public GameObject arrow;
    public ParticleSystem weaponPS;

    public AudioClip attackSfx;
    public AudioClip getHitSfx;
    AudioSource audioSource;

    // 11.11
    public EnemyData enemyData;
    Enemy_Orc orc;
    Enemy_Skelleton skelleton;
    Enemy_Shell shell;
    Player player;


    private void Awake()
    {
        meleeArea = GetComponent<BoxCollider>();
        //11.10 �߰� by �յ���
        //mellArea�� null�̸� ������ ��� ���ͼ� ����
        if(meleeArea == null)
        {
            Debug.Log("����, Weapon��ũ��Ʈ�� meleeArea������ null�Դϴ�.");
        }
        else
        {
            meleeArea.enabled = false;
        }

        // 11.11
        shell = GetComponentInParent<Enemy_Shell>();
        orc = GetComponentInParent<Enemy_Orc>();
        skelleton = GetComponentInParent<Enemy_Skelleton>();
        player = FindObjectOfType<Player>();
        audioSource = GetComponent<AudioSource>();

    }

    //private void Start()
    //{
    //    weaponPS
    //}

    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
            
        }
        else if (type == Type.Range)
        {
            StartCoroutine("Shot");
            
        }
    }

        IEnumerator Swing()
        {
            yield return new WaitForSeconds(0.1f);  // 0.1�� ���
            meleeArea.enabled = true;
            audioSource.PlayOneShot(attackSfx);

        yield return new WaitForSeconds(0.2f);
            meleeArea.enabled = false;
        }

    IEnumerator Shot()
    {
        // �Ѿ� �߻�
        GameObject intantArrow = Instantiate(arrow, arrowPos.position, arrowPos.rotation);
        Rigidbody arrowRigid = intantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = arrowPos.forward * 50;
        //Debug.Log(arrowPos.forward);
        //Time.timeScale = 0;
        yield return null;

    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    //if (collision.gameObject.tag == "Floor")
    //    //{
    //    //    anim.SetBool("isJump", false);
    //    //    isJump = false;
    //    //}
    //    //10.11 �ӽ� �߰�. ���� ���� ��ǿ� �����ϼž� ���� ������ ����� �� �����ϴ�.
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        Attack(collision.gameObject,Player.instance.pDamage);
    //        Debug.Log($"{collision.gameObject.name}");
    //    }
    //    //by �յ���
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Attack(other.gameObject, Player.instance.pDamage);
            Debug.Log($"{other.gameObject.name}");
            Vector3 impactPoint = transform.position + transform.up;
            Vector3 effectPoint = other.ClosestPoint(impactPoint);
            Instantiate(hitEffect, effectPoint, Quaternion.identity);   // ����Ʈ�� ���� �ε�ģ ������ ����
        }
    }

    public void Die()
    {
        
    }

    public void Attacked(int d)
    {
        //Debug.Log("������ ����");
        //pHP -= d;
        //Debug.Log("������ ����");
        ////UI�� �÷��̾� pHP ���� ���� -������ 11.04
        //Health.instance.SetCurrentHealth(pHP);
        //Debug.Log("UI ������ ����");
    }

    public void Attack(GameObject target, int d)
    {
        ICharacter ic = target.GetComponent<ICharacter>();
        ic.Attacked(d);
        Debug.Log($"������ ����\n{target.name}�� {d}��ŭ ����");
        //Debug.Log($"{gameObject.name}�� {target.name}�� ����. {d}��ŭ�� ���ظ� �������ϴ�.\n����{target.name}�� HP�� {target.GetComponent<Enemy>().curHealth}");
    }
}
