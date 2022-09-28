using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range };  // �ٰŸ� ���Ÿ�
    public Type type;
    public int damage;
    public float rate;  // ���� �ӵ�
    public BoxCollider meleeArea;   // ���� ����

    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
    }
        IEnumerator Swing()
        {
            yield return new WaitForSeconds(0.1f);  // 0.1�� ���
            meleeArea.enabled = true;

            yield return new WaitForSeconds(0.3f);
            meleeArea.enabled = false;
            

        }
}
