using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int index;
    public Spawner[] spawners;
    public List<Door> door = new List<Door>(4);
    public Vector3 roomPosition;
    public bool isClear = false;
    public BoxCollider boxCol;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider>();
    }
    private void Start()
    {
        if (index == 0)
        {
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
        foreach (var i in door)
        {
            i.ClearAndClose(true);
        }
        isClear = true;

        
        MainManager.instance.onClearthisRoom -= OpenAllDoor;
    }

    void CloseAllDoor()
    {
        foreach (var i in door)
        {
            i.ClearAndClose(false);
        }
    }
    
}
