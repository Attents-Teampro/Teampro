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
        room = FindObjectOfType<DungeonCreator>().gameObject.transform.GetChild(1).GetComponent<Room>();
        room.StartSpawn();
        //Debug.Log("���۽�");
    }

    //room�� ����� �� ���� ����
    protected override void Test2(InputAction.CallbackContext _)
    {
        foreach (var i in room.spawners)
        {
            i.KillAllMonsters();
        }
    }
    //room�� ����� ���� ��� óġ
    protected override void Test3(InputAction.CallbackContext obj)
    {
        
    }
}
