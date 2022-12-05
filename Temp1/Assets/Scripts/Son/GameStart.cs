using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    [NonSerialized]
    public GameObject creater;
    [NonSerialized]
    public GameObject player;
    //public GameObject[] room;
    //BoxCollider col;
    //Vector3 roomPosition;
    [SerializeField]
    bool isFirstRoomSpawn = false;
    private void Start()
    {
        player = MainManager.instance.Player.gameObject;
        creater = FindObjectOfType<DungeonCreator>().gameObject;
        creater.SetActive(true);
        creater.GetComponent<DungeonCreator>().CreateDungeon();
        player.SetActive(true);
        if (isFirstRoomSpawn)
        {

            Room r = creater.transform.GetChild(1).GetComponent<Room>();
            r.StartSpawn();
            //Debug.Log($"ù ��° ���ϵ�� {creater.transform.GetChild(1).name}");
            //Room[] r = creater.transform.GetComponentsInChildren<Room>();
            //foreach (Room room in r)
            //{
            //    room.StartSpawn();
            //}
        }
    }
    /// <summary>
    /// ���� ũ������Ʈ�� ���� �� ������ �ȿ��� �����ϸ� ����(�ݸ����� �Ȼ���)�� ����.
    /// </summary>
    public void DungeonCreate()
    {
        creater.SetActive(true);
        player.SetActive(true);
        //creater.GetComponent<DungeonCreator>().CreateDungeon();
        //player.transform.position = creater.GetComponent<DungeonCreator>().roomCollider[0].center;
    }
    public void DungeonSet()
    {
        //roomPosition = Vector3.zero;
        //room = new GameObject[creater.transform.childCount];
        //Debug.Log(room.Length);
        //for (int i = 0; i < room.Length; i++)
        //{
        //    room[i] = creater.transform.GetChild(i).gameObject;
        //    col = room[i].AddComponent<BoxCollider>();
        //    col.isTrigger = true;
        //    roomPosition += (i == 1) ? col.center : Vector3.zero;

        //}
        player.transform.position = creater.GetComponent<DungeonCreator>().roomCollider[0].center;
        //Debug.Log($"���� �÷��̾� ��ġ {player.transform.position}");
    }
}
