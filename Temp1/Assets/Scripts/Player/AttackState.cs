using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    MainManager.instance.Player.WeaponEffectSwitch(true);   // ���� ����Ʈ �ѱ�
    //    Debug.Log("����Ʈ ����");
    //}

    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    MainManager.instance.Player.WeaponEffectSwitch(false);  // ���� ����Ʈ ����

    //    //animator.SetInteger("ComboState", 0);
    //    animator.ResetTrigger("doSwing");        // ���� Ʈ���ŵ� �ϴ� �ʱ�ȭ
    //}

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("����Ʈ ����");
        //Player tempPlayer;
        //tempPlayer = MainManager.instance.Player;
        //tempPlayer.WeaponEffectSwitch(true);
        MainManager.instance.Player.WeaponEffectSwitch(true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("����Ʈ ����");
        MainManager.instance.Player.WeaponEffectSwitch(false);

        animator.SetInteger("ComboState", 0);
        animator.ResetTrigger("doSwing");        // ���� Ʈ���ŵ� �ϴ� �ʱ�ȭ
    }

}
