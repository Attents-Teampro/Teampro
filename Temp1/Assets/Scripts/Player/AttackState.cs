using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (animator != null)
        {
            Debug.Log("이펙트 켜짐");
            MainManager.instance.Player.WeaponEffectSwitch(true);   // 무기 이팩트 켜기
        }

    }

    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion
    }

    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    Debug.Log("이펙트 꺼짐");
    //    MainManager.instance.Player.WeaponEffectSwitch(false);  // 무기 이팩트 끄기

    //    animator.ResetTrigger("doSwing");        // 어택 트리거도 일단 초기화
    //}

    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    Debug.Log("이펙트 켜짐");
    //    MainManager.instance.Player.WeaponEffectSwitch(true);
    //}

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    Debug.Log("이펙트 꺼짐");
    //    MainManager.instance.Player.WeaponEffectSwitch(false);

    //    animator.ResetTrigger("doSwing");        // 어택 트리거도 일단 초기화
    //}

}
