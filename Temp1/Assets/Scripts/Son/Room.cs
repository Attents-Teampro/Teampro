using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int index;
    public Spawner[] spawners;
    public List<Door> door = new List<Door>(3);
    public Vector3 roomPosition;
    public bool isClear = false;

    
    private void Start()
    {
        //Debug.Log("ㅇㅇ1");
        if (index == 0)
        {
            //Debug.Log("ㅇㅇ2");
            OpenAllDoor();
            foreach(Door d in door)
            {
                d.SetInfo();
            }
        }
    }
    public void StartSpawn()
    {
        
        for(int i = 0; i<spawners.Length; i++)
        {
            spawners[i].StartSpawn(gameObject);
        }

        //foreach (var i in spawners)
        //{
        //    Debug.Log($"{count++}번 째 스포너 {i}");
        //    i.StartSpawn(gameObject);
        //}

        
        //Debug.Log($"연결. {name}은 함수 연결 완료");
    }
    public void PlayerInThisRoom()
    {
        MainManager.instance.onClearthisRoom += OpenAllDoor;
    }
    void OpenAllDoor()
    {
        //Debug.Log($"실행. {name}은 함수 실행 완료");
        foreach (var i in door)
        {
            //Debug.Log($"{transform.name}의 문 열기 실행 0");
            i.ClearAndClose(true);
        }
        isClear = true;

        if (index != 1)
        {
            MainManager.instance.onClearthisRoom -= OpenAllDoor;
        }
    }

    void CloseAllDoor()
    {
        foreach (var i in door)
        {
            i.ClearAndClose(false);
        }
    }
    
}
