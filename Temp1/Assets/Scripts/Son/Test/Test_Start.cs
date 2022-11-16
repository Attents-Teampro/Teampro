using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Start : Test_Base
{
    public Room room;

    //던전 절차적 생성
    protected override void Test1(InputAction.CallbackContext _)
    {
        MainManager.instance.GameStartFunc();
        //Debug.Log("시작스");
    }

    //room에 저장된 방 스폰 시작
    protected override void Test2(InputAction.CallbackContext _)
    {
        room.StartSpawn();
        //MainManager.instance.SetGame();
    }
    //room에 저장된 몬스터 모두 처치
    protected override void Test3(InputAction.CallbackContext obj)
    {
        foreach (var i in room.spawners)
        {
            i.KillAllMonsters();
        }
    }
}
