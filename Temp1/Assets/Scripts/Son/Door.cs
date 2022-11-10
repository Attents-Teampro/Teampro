using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    //�ݴ��� �� ��ũ��Ʈ
    public Door door;

    //���� ���ȴ��� üũ
    bool isOpen = false;

    //�ش� ���� ����� �� ���� ����
    GameObject thisRoom;

    //������Ʈ ����
    BoxCollider col;
    MeshRenderer mr;
    private void Awake()
    {
        col = GetComponent<BoxCollider>();
        mr = GetComponent<MeshRenderer>();
    }

    public void Clear()
    {
        //�� �̹��� ���ֱ�
        mr.enabled = false;

        //���� Ŭ���������� ���� ����
        //1. �� üũ ���� ����
        isOpen = true;
        //2. Ʈ���� Ȱ��ȭ
        col.isTrigger = true;
        //3. ��� ����� ������ Ȱ��ȭ
        //door

    }

    public void CheckRoom()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (isOpen)
        {
            Clear();
        }
        
    }





}
