using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (animator != null)
        {
            Debug.Log("����Ʈ ����");
            MainManager.instance.Player.WeaponEffectSwitch(true);   // ���� ����Ʈ �ѱ�
        }

    }

    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion
    }

    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    Debug.Log("����Ʈ ����");
    //    MainManager.instance.Player.WeaponEffectSwitch(false);  // ���� ����Ʈ ����

    //    animator.ResetTrigger("doSwing");        // ���� Ʈ���ŵ� �ϴ� �ʱ�ȭ
    //}

    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    Debug.Log("����Ʈ ����");
    //    MainManager.instance.Player.WeaponEffectSwitch(true);
    //}

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    Debug.Log("����Ʈ ����");
    //    MainManager.instance.Player.WeaponEffectSwitch(false);

    //    animator.ResetTrigger("doSwing");        // ���� Ʈ���ŵ� �ϴ� �ʱ�ȭ
    //}

}
