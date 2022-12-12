using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    
    ////, Wall;
    ////������ ���� ������, ���Ͱ� �����Ǵ� ��ġ ����� ���� �� ������Ʈ
    //public GameObject enemyObject;
    ////������ ���� ��
    //public int numOfEnemy;
    //������ ���� ���� ������ �Ŵ��� ��ũ��Ʈ
    //������ �� ���� �Ŵ������� ���� �ѱ��. ���� �Ŵ����� �Ѱ��� ������ ���ؼ� �� ���� ���� ���Ѵ�.
    //public MainManager mainManager;

    //���� floor���� Ground ������Ʈ ������ �˴ϴ�.
    //public BoxCollider floor;


    //floor������Ʈ�� ��ġ���� ���� �� �����Լ��� ���� ����
    Vector3 sizeOfGround;

    //���� ���� ��ġ�� �ùٸ� ���� false�� �Ǵ� bool ����
    bool resetPosition = true;

    //���� �������� Ȯ���ϰ�, ���� ���͸� ��ġ �������ִ� bool ����
    public bool isBoss = false;

    //������ ��ġ�� ����
    public Vector3 bossPosition;

    //�Ž������� ����
    MeshRenderer mr;
    //Room ��ũ��Ʈ ����
    Room room;
    [Header("�ش� �濡 ������ ���� �� ����.")]
    public SpawnData spawnData;
    int totalSpawnMonster = 0;
    //�׽�Ʈ�� ����
    GameObject[] mon;

    //[SerializeField]
    //bool isObject = false;
    public void StartSpawn(GameObject obj)
    {
        
        mr = obj.GetComponent<MeshRenderer>();
        room = obj.GetComponent<Room>();
        if(spawnData != null && !isBoss)
        {
            for(int i = 0; i< spawnData.SpawnArr.Length;i++)
            {
                SpawnerStart(i);
            }
        }else if (isBoss)
        {
            Instantiate(spawnData.SpawnArr[0].enemyObject, bossPosition, Quaternion.identity);
            //return;
            return;
        }
        
        
    }
    public void AddMainToSpawnNum()
    {
        if (!isBoss)
        {
            for (int i = 0; i < spawnData.SpawnArr.Length; i++)
            {
                int numOfEnemy = spawnData.SpawnArr[i].numOfEnemy;
                if (!spawnData.SpawnArr[i].isObject)
                {
                    MainManager.instance.numOfStageEnemy += numOfEnemy;
                }
            }
        }
        else
        {
            MainManager.instance.numOfStageEnemy += spawnData.SpawnArr[0].numOfEnemy;
        }
        
    }
    void SpawnerStart(int index)
    {
        GameObject enemyObject = spawnData.SpawnArr[index].enemyObject;
        int numOfEnemy = spawnData.SpawnArr[index].numOfEnemy;

        //���� �Ŵ������� ������ ������ ���� �ѱ��.

        //mainManager = FindObjectOfType<MainManager>();
        //MainManager.instance.numOfStageEnemy += numOfEnemy;
        //Debug.Log($"�� {numOfEnemy}�� ���ο� �Ѱ���");

        ////���� ���͸� �����ϴ� ��� ������ ��ġ bossPosition�� ���� ���ӿ�����Ʈ ���� �� Ŭ���� ����
        //if (isBoss)
        //{
        //    Instantiate(enemyObject, bossPosition, Quaternion.identity);
        //    //return;
        //    return;
        //}
        //11.10 �ּ� by �յ���
        //���� �ٲ� �ּ� ó�� 
        ////���ϵ� 0��aa���� ������� ������ ���� �� �Ʒ� ��. ������ �¾ƾ� �Ʒ� �������� ���� ����
        //Vector3[] WallPosition;
        //WallPosition = new Vector3[Wall.transform.childCount];
        ////Wall ������Ʈ�� ���ϵ� ������Ʈ �¿����Ʒ� ����

        //Debug.Log("�޽������� ã��");
        //mr = GetComponent<MeshRenderer>();
        Vector3 floor = mr.bounds.size * 0.5f * 0.9f;

        //�׽�Ʈ ���� ���� �ڵ�
        mon = new GameObject[numOfEnemy];
        //���� ����. 
        for (int i = 0; i < numOfEnemy; i++)
        {
            //���� �ʱ�ȭ. �� �ʿ���
            resetPosition = true;

            //���Ͱ� �� �ȿ� �����Ǵ��� if�� ����ϰ�, �� ���� �ƴϸ� ��� ���ư��� while�� 
            //(�� �ȿ� �����Ǹ� resetPosition = false) 
            while (resetPosition)
            {
                //�ٴ� ������Ʈ floor�� �ݸ��� ����� ���ؼ�(Vector3������ ����) Random.value�� ���ϴ� ��.
                //�� ������ �ٴ� �ݸ��� ũ�⿡ �°� ��ġ���� ���.. �׷��� �ݸ��� ũ�Ⱑ �� Ŀ�� �׷��� �ڲ� ���� �Ѿ
                //�׷��� �ٷ� �Ʒ��� if������ �� �ȿ� �ִ��� �ǵ��ϰ� �������.
                //�Ƹ� �ݸ��� ũ�Ⱑ �� ū�� �ݸ��� /2�� �ϸ� �ذ� ������ �𸣰��� �ϴ� if������ �ذ��ϰ� ���߿� Ȯ���� ����
                sizeOfGround = new Vector3(Random.Range(-floor.x, floor.x), floor.y, Random.Range(-floor.z, floor.z));
                if (mr.bounds.max.x < sizeOfGround.x ||   //������ ��
                    -mr.bounds.max.x > sizeOfGround.x ||   //���� ��
                    mr.bounds.max.z < sizeOfGround.z ||   //�� ��
                    -mr.bounds.max.x > sizeOfGround.z)     //�Ʒ� ��
                {
                    //�󸶳� while���� �ݺ��Ǿ����� �� �� ����. ���� �������� ���̴� �� �ĺ������� ���� ����
                    Debug.Log("Enemy ������Ʈ ��ġ ��Ż. ������");
                }
                else
                {
                    //���� ������Ʈ ����.
                    mon[i] = Instantiate(enemyObject, sizeOfGround + room.roomPosition, Quaternion.identity);
                    //while�� ����. for���� ��� ����Ǳ� ������ �ʱ⿡ ������ numOfEnemy ���� �°� ���� ������ ��
                    resetPosition = false;
                }
            }
        }
        totalSpawnMonster = mon.Length;
    }

    public void KillAllMonsters()
    {
        foreach(var i in mon)
        {
            ICharacter ic = i.GetComponent<ICharacter>();
            ic.Die();
            Debug.Log(i.name);
            Debug.Log("����");

        }
    }
}
