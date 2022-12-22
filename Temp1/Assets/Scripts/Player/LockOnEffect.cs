using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnEffect : MonoBehaviour
{
    IPlayer targetHealth;



    /// <summary>
    /// 락온 대상 설정
    /// </summary>
    /// <param name = "newParent" > 새로운 락온 대상(무조건 Enemy이어야 한다.). null아면 끄기</param>
    public void SetLockOnTarget(Transform newParent)
    {
        if (targetHealth != null)    // 기존 대상이 있었으면
        {
            targetHealth.onDie -= ReleaseTarget;    // 연결되어있던 델리게이트 등록 해제
            Debug.Log("위");
        }

        if (newParent != null)
        {
            targetHealth = newParent.gameObject.GetComponent<IPlayer>();    // 새롭게 대상 설정
            targetHealth.onDie += ReleaseTarget;                            // 죽을 때 이펙트 제거하도록 함수 등록
            Debug.Log("아래");
        }

        transform.SetParent(newParent);                 // 부모 설정
        transform.localPosition = Vector3.zero;         // 부모 위치로 이동
        this.gameObject.SetActive(newParent != null);   // newParent가 있으면 true, null이면 false
    }

    void ReleaseTarget()
    {
        SetLockOnTarget(null);
    }
}
