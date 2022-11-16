using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Start : Test_Base
{
    public Room room;

    //���� ������ ����
    protected override void Test1(InputAction.CallbackContext _)
    {
        MainManager.instance.GameStartFunc();
        //Debug.Log("���۽�");
    }

    //room�� ����� �� ���� ����
    protected override void Test2(InputAction.CallbackContext _)
    {
        room.StartSpawn();
        //MainManager.instance.SetGame();
    }
    //room�� ����� ���� ��� óġ
    protected override void Test3(InputAction.CallbackContext obj)
    {
        foreach (var i in room.spawners)
        {
            i.KillAllMonsters();
        }
    }
}
