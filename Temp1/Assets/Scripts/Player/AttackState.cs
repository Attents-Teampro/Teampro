using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    if (animator != null)
    //    {
    //        //Debug.Log("ÀÌÆåÆ® ÄÑÁü");
    //        MainManager.instance.Player.WeaponEffectSwitch(true);   // ¹«±â ÀÌÆÑÆ® ÄÑ±â
    //    }

    //}

    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion
    }

    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    Debug.Log("ÀÌÆåÆ® ²¨Áü");
    //    MainManager.instance.Player.WeaponEffectSwitch(false);  // ¹«±â ÀÌÆÑÆ® ²ô±â

    //    animator.ResetTrigger("doSwing");        // ¾îÅÃ Æ®¸®°Åµµ ÀÏ´Ü ÃÊ±âÈ­
    //}

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("ÀÌÆåÆ® ÄÑÁü");
        MainManager.instance.Player.WeaponEffectSwitch(true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("ÀÌÆåÆ® ²¨Áü");
        MainManager.instance.Player.WeaponEffectSwitch(false);
    }

}
