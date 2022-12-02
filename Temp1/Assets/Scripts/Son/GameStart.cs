using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public DungeonCreator creater;
    public Player player;
    //public GameObject[] room;
    //BoxCollider col;
    //Vector3 roomPosition;
    [SerializeField]
    bool isFirstRoomSpawn = false;
    private void Start()
    {
        creater.gameObject.SetActive(true);
        creater.player = player.gameObject;
        creater.CreateDungeon();
        
        player.gameObject.SetActive(true);
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
        if(creater == null)
        {
            creater = FindObjectOfType<DungeonCreator>();
        }else if(player== null)
        {
            player = FindObjectOfType<Player>();
        }
        creater.gameObject.SetActive(true);
        player.gameObject.SetActive(true);
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
        player.transform.position = creater.roomCollider[0].center;
        //Debug.Log($"���� �÷��̾� ��ġ {player.transform.position}");
    }
}
