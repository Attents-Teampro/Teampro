using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    MainManager.instance.Player.WeaponEffectSwitch(true);   // 무기 이팩트 켜기
    //    Debug.Log("이펙트 켜짐");
    //}

    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    MainManager.instance.Player.WeaponEffectSwitch(false);  // 무기 이팩트 끄기

    //    //animator.SetInteger("ComboState", 0);
    //    animator.ResetTrigger("doSwing");        // 어택 트리거도 일단 초기화
    //}

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("이펙트 켜짐");
        //Player tempPlayer;
        //tempPlayer = MainManager.instance.Player;
        //tempPlayer.WeaponEffectSwitch(true);
        MainManager.instance.Player.WeaponEffectSwitch(true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("이펙트 꺼짐");
        MainManager.instance.Player.WeaponEffectSwitch(false);

        animator.SetInteger("ComboState", 0);
        animator.ResetTrigger("doSwing");        // 어택 트리거도 일단 초기화
    }

}
