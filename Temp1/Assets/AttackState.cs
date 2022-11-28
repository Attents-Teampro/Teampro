using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    // �� ���¸ӽſ� ������ ��(Entry���� ��) ����
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    GameManager.Inst.Player.WeaponEffectSwitch(true);   // ���� ����Ʈ �ѱ�
    //    Debug.Log("����Ʈ ����");
    //}

    //// OnStateMachineExit is called when exiting a state machine via its Exit Node
    //// �� ���¸ӽ��� Exit�� �� �� ����
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    GameManager.Inst.Player.WeaponEffectSwitch(false);  // ���� ����Ʈ ����

    //    animator.ResetTrigger("doSwing");        // ���� Ʈ���ŵ� �ϴ� �ʱ�ȭ
    //}

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("����Ʈ ����");
        //Player tempPlayer;
        //tempPlayer = MainManager.instance.Player;
        //tempPlayer.WeaponEffectSwitch(true);
        //GameManager.Inst.Player.WeaponEffectSwitch(true);   // ���� ����Ʈ �ѱ�
        MainManager.instance.Player.WeaponEffectSwitch(true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("����Ʈ ����");
        //GameManager.Inst.Player.WeaponEffectSwitch(false);  // ���� ����Ʈ ����
        MainManager.instance.Player.WeaponEffectSwitch(false);

        animator.ResetTrigger("doSwing");        // ���� Ʈ���ŵ� �ϴ� �ʱ�ȭ
    }
}
