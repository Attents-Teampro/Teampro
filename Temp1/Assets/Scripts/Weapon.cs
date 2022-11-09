using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, ICharacter
{
    public enum Type { Melee, Range };  // 근거리 원거리
    public Type type;

    //10.11 변수 이름 수정 by 손동욱
    public int iDamage = 100;

    public float rate;  // 공격 속도
    public BoxCollider meleeArea;   // 공격 범위
    public Transform arrowPos;
    public GameObject arrow;

    

    private void Awake()
    {
        meleeArea = GetComponent<BoxCollider>();
        meleeArea.enabled = false;
    }

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
            yield return new WaitForSeconds(0.1f);  // 0.1초 대기
            meleeArea.enabled = true;

            yield return new WaitForSeconds(0.3f);
            meleeArea.enabled = false;
        }

    IEnumerator Shot()
    {
        // 총알 발사
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
    //    //10.11 임시 추가. 추후 공격 모션에 적용하셔야 몬스터 공격이 실행될 것 같습니다.
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        Attack(collision.gameObject,Player.instance.pDamage);
    //        Debug.Log($"{collision.gameObject.name}");
    //    }
    //    //by 손동욱
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Attack(other.gameObject, Player.instance.pDamage);
            Debug.Log($"{other.gameObject.name}");
        }
    }

    public void Die()
    {
        
    }

    public void Attacked(int d)
    {
        //Debug.Log("데미지 입음");
        //pHP -= d;
        //Debug.Log("데미지 적용");
        ////UI에 플레이어 pHP 값을 전달 -양해인 11.04
        //Health.instance.SetCurrentHealth(pHP);
        //Debug.Log("UI 데미지 적용");
    }

    public void Attack(GameObject target, int d)
    {
        ICharacter ic = target.GetComponent<ICharacter>();
        ic.Attacked(d);
        //Debug.Log($"{gameObject.name}가 {target.name}을 공격. {d}만큼의 피해를 입혔습니다.\n현재{target.name}의 HP는 {target.GetComponent<Enemy>().curHealth}");
    }
}
