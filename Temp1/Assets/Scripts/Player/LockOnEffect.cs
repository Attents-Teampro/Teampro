using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnEffect : MonoBehaviour
{
    //IPlayer TargetHealth;

    ///// <summary>
    ///// ���� ��� ����
    ///// </summary>
    ///// <param name = "newParent" > ���ο� ���� ���(������ Enemy�̾�� �Ѵ�.). null�Ƹ� ����</param>
    //public void SetLockOnTarget(Transform newParent)
    //{
    //    if (TargetHealth != null)    // ���� ����� �־�����
    //    {
    //        TargetHealth.onDie -= ReleaseTarget;    // ����Ǿ��ִ� ��������Ʈ ��� ����
    //    }

    //    if (newParent != null)
    //    {
    //        TargetHealth = newParent.gameObject.GetComponent<IPlayer>();    // ���Ӱ� ��� ����
    //        TargetHealth.onDie += () => ReleaseTarget();                    // ���� �� ����Ʈ �����ϵ��� �Լ� ���
    //    }

    //    transform.SetParent(newParent);                 // �θ� ����
    //    transform.localPosition = Vector3.zero;         // �θ� ��ġ�� �̵�
    //    this.gameObject.SetActive(newParent != null);   // newParent�� ������ true, null�̸� false
    //}

    //void ReleaseTarget()
    //{
    //    SetLockOnTarget(null);
    //}
}
