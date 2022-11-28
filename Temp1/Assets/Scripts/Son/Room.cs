using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //던전크리에이터에서 생성된 순서 인덱스
    public int index;
    //플레이어가 진입한 순서 인덱스(스폰이 된 시점에서 계산)
    public int indexPlayerIn = 0;
    public Spawner spawners = null;
    public List<Door> door = new List<Door>(4);
    public Vector3 roomPosition;
    public bool isClear = false;
    public BoxCollider boxCol;
    public int isBossIndex=0;

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
        //일반 방 일 때
        if (isBossIndex!=indexPlayerIn && MainManager.instance.spawnManager.spawners.Length-1 >= indexPlayerIn)
        {
            spawners = MainManager.instance.spawnManager.spawners[indexPlayerIn];
            spawners.StartSpawn(gameObject);
        }
        //보스 방 일 때(던전크리에이터에서 마지막 방 생성 시 spanwer을 설정해주므로)
        else if(isBossIndex==indexPlayerIn)
        {
            spawners.bossPosition = roomPosition;
            spawners.StartSpawn(gameObject);
        }
        else
        {
            Debug.Log($"에러. {gameObject.name}의 스포너 인덱스 초과 상태. ");
        }
        
        //for(int i = 0; i<spawners.Length; i++)
        //{
        //    spawners[i].StartSpawn(gameObject);
        //}

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
        spawners.AddMainToSpawnNum();
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
