using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range };  // �ٰŸ� ���Ÿ�
    public Type type;

    //10.11 ���� �̸� ���� by �յ���
    public int iDamage = 100;

    public float rate;  // ���� �ӵ�
    public BoxCollider meleeArea;   // ���� ����
    public Transform arrowPos;
    public GameObject arrow;
    

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

            yield return new WaitForSeconds(0.3f);
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
}
