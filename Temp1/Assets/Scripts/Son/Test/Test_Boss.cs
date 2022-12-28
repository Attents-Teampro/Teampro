using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Boss : Test_Base
{
    Player p;
    Room r;

    protected override void Test1(InputAction.CallbackContext _)
    {
        r = FindObjectOfType<Room>();
        p = FindObjectOfType<Player>();
        p.countCurrentRoom = r.isBossIndex - 1;
        Debug.Log("보스 세팅완");
    }
    protected override void Test2(InputAction.CallbackContext _)
    {
        
    }
}
