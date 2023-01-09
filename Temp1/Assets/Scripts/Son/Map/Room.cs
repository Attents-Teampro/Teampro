using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //����ũ�������Ϳ��� ������ ���� �ε���
    public int index;
    //�÷��̾ ������ ���� �ε���(������ �� �������� ���)
    public int indexPlayerIn = 0;
    public Spawner spawners = null;
    public List<Door> door = new List<Door>(4);
    public Vector3 roomPosition;
    public bool isClear = false;
    public BoxCollider boxCol;
    public int isBossIndex=0;
    public Action onBossSpawn;

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
        isBossIndex -= 3;
    }
    /// <summary>
    /// ���� ������ ���۽�Ű�� �Լ�
    /// </summary>
    /// <returns>true�� ����, false�� �Ϲ� ��</returns>
    public bool StartSpawn()
    {
        bool result = false;
        //�Ϲ� �� �� ��
        if (isBossIndex!=indexPlayerIn && MainManager.instance.spawnManager.spawners.Length-1 >= indexPlayerIn)
        {
            spawners = MainManager.instance.spawnManager.spawners[indexPlayerIn-1];
            spawners.StartSpawn(gameObject);
        }
        //���� �� �� ��(����ũ�������Ϳ��� ������ �� ���� �� spanwer�� �������ֹǷ�)
        else if(isBossIndex==indexPlayerIn)
        {
            result= true;
            //spawners.bossPosition = roomPosition;
            //spawners.StartSpawn(gameObject);
        }
        else
        {
            Debug.Log($"����. {gameObject.name}�� ������ �ε��� �ʰ� ����. ");
        }

        //for(int i = 0; i<spawners.Length; i++)
        //{
        //    spawners[i].StartSpawn(gameObject);
        //}

        //foreach (var i in spawners)
        //{
        //    Debug.Log($"{count++}�� ° ������ {i}");
        //    i.StartSpawn(gameObject);
        //}

        return result;
        //Debug.Log($"����. {name}�� �Լ� ���� �Ϸ�");
    }
    public void StartSpawnBoss()
    {
        onBossSpawn?.Invoke();
        spawners.bossPosition = roomPosition;
        spawners.StartSpawn(gameObject);
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
