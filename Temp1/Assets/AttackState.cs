using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    // 이 상태머신에 들어왔을 때(Entry했을 때) 실행
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    GameManager.Inst.Player.WeaponEffectSwitch(true);   // 무기 이팩트 켜기
    //    Debug.Log("이펙트 켜짐");
    //}

    //// OnStateMachineExit is called when exiting a state machine via its Exit Node
    //// 이 상태머신이 Exit로 갈 때 실행
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    GameManager.Inst.Player.WeaponEffectSwitch(false);  // 무기 이팩트 끄기

    //    animator.ResetTrigger("doSwing");        // 어택 트리거도 일단 초기화
    //}

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("이펙트 켜짐");
        //Player tempPlayer;
        //tempPlayer = MainManager.instance.Player;
        //tempPlayer.WeaponEffectSwitch(true);
        //GameManager.Inst.Player.WeaponEffectSwitch(true);   // 무기 이팩트 켜기
        MainManager.instance.Player.WeaponEffectSwitch(true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("이펙트 꺼짐");
        //GameManager.Inst.Player.WeaponEffectSwitch(false);  // 무기 이팩트 끄기
        MainManager.instance.Player.WeaponEffectSwitch(false);

        animator.ResetTrigger("doSwing");        // 어택 트리거도 일단 초기화
    }
}
