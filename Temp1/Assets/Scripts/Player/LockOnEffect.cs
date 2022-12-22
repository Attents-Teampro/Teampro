using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnEffect : MonoBehaviour
{
    IPlayer targetHealth;



    /// <summary>
    /// ���� ��� ����
    /// </summary>
    /// <param name = "newParent" > ���ο� ���� ���(������ Enemy�̾�� �Ѵ�.). null�Ƹ� ����</param>
    public void SetLockOnTarget(Transform newParent)
    {
        if (targetHealth != null)    // ���� ����� �־�����
        {
            targetHealth.onDie -= ReleaseTarget;    // ����Ǿ��ִ� ��������Ʈ ��� ����
            Debug.Log("��");
        }

        if (newParent != null)
        {
            targetHealth = newParent.gameObject.GetComponent<IPlayer>();    // ���Ӱ� ��� ����
            targetHealth.onDie += ReleaseTarget;                            // ���� �� ����Ʈ �����ϵ��� �Լ� ���
            Debug.Log("�Ʒ�");
        }

        transform.SetParent(newParent);                 // �θ� ����
        transform.localPosition = Vector3.zero;         // �θ� ��ġ�� �̵�
        this.gameObject.SetActive(newParent != null);   // newParent�� ������ true, null�̸� false
    }

    void ReleaseTarget()
    {
        SetLockOnTarget(null);
    }
}
