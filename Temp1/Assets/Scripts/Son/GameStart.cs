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
    /// ���� ũ������Ʈ�� ���� �� ������ �ȿ��� �����ϸ� ����(�ݸ����� �Ȼ���)�� ����.
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
        Debug.Log($"�÷��̾� ��ġ ����\n�÷��̾� ��ġ({player.transform.position})�� ��1��({room[1].name})�� ��ġ{roomPosition}���� �̵�");
        player.transform.position = roomPosition;
        //Debug.Log($"���� �÷��̾� ��ġ {player.transform.position}");
    }
}
