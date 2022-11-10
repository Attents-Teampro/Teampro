using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Start : Test_Base
{
    protected override void Test1(InputAction.CallbackContext _)
    {
        MainManager.instance.StartGame();
        Debug.Log("Ω√¿€Ω∫");
    }
    protected override void Test2(InputAction.CallbackContext _)
    {
        MainManager.instance.SetGame();
    }
    protected override void Test3(InputAction.CallbackContext _)
    {

    }
}
