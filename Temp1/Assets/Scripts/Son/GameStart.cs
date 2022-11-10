using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public GameObject creater, player;
    public GameObject[] room;
    BoxCollider col;
    Vector3 roomPosition;
    private void Start()
    {
        creater.GetComponent<DungeonCreator>().CreateDungeon();
        creater.SetActive(false);
        player.SetActive(false);
    }
    /// <summary>
    /// 던전 크리에이트와 샛을 한 프레임 안에서 실행하면 에러(콜리더가 안생김)가 있음.
    /// </summary>
    public void DungeonCreate()
    {
        creater.SetActive(true);
        player.SetActive(true);
        creater.GetComponent<DungeonCreator>().CreateDungeon();
    }
    public void DungeonSet()
    {
        roomPosition = Vector3.zero;
        room = new GameObject[creater.transform.childCount];
        Debug.Log(room.Length);
        for (int i = 0; i < room.Length; i++)
        {
            room[i] = creater.transform.GetChild(i).gameObject;
            col = room[i].AddComponent<BoxCollider>();
            col.isTrigger = true;
            roomPosition += (i == 1) ? col.center : Vector3.zero;

        }
        Debug.Log($"플레이어 위치 조정\n플레이어 위치({player.transform.position})을 룸1번({room[1].name})의 위치{roomPosition}으로 이동");
        player.transform.position = roomPosition;
        //Debug.Log($"현재 플레이어 위치 {player.transform.position}");
    }
}
