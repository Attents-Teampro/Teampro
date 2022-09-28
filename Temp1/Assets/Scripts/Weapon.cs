using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range };  // 근거리 원거리
    public Type type;
    public int damage;
    public float rate;  // 공격 속도
    public BoxCollider meleeArea;   // 공격 범위

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
            yield return new WaitForSeconds(0.1f);  // 0.1초 대기
            meleeArea.enabled = true;

            yield return new WaitForSeconds(0.3f);
            meleeArea.enabled = false;
            

        }
}
