using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Door : MonoBehaviour
{
    //�ݴ��� �� ��ũ��Ʈ
    public Door door;

    
    //���� ���ȴ��� üũ
    bool isOpen = false;
    bool IsOpen
    {
        get => isOpen;
        set 
        {
            if (isOpen != value)
            {
                isOpen = value;
                DoorControl(isOpen);
            }
        }
    }

    //�ش� ���� ����� �� ���� ����
    GameObject thisRoom = null;

    //������Ʈ ����
    BoxCollider col;
    MeshRenderer mr;
    public float checkRange = 5f;

    private void Awake()
    {
        col = GetComponent<BoxCollider>();
        mr = GetComponent<MeshRenderer>();
        checkRange = transform.localScale.y * 0.5f + 0.2f;
    }

    private void Start()
    {
        if (CheckRoom())
        {
            //Debug.Log($"�� ã�� ����\n�ش� �� {transform.parent.parent.name}�ڽ� {name}�� ���� {thisRoom}�Դϴ�.");
        }
        else
        {
            //Debug.Log($"�� ã�� ����\n�ش� �� {transform.parent.parent.name}�ڽ� {name}�� ���� ã�����߽��ϴ�.");
            Destroy(gameObject);
        }
    }

    public void Clear()
    {
        //���� Ŭ���������� ���� ����
        //�� üũ ���� ����
        isOpen = true;

    }
    private void DoorControl(bool t)
    {
        if (t)
        {
            DoorOpen();
        }
        else
        {
            DoorClose();
        }
    }

    private void DoorClose()
    {
        mr.enabled = true;
        col.isTrigger = false;
    }

    private void DoorOpen()
    {
        mr.enabled = false;
        col.isTrigger = true;
    }
    public bool CheckRoom()
    {
        bool result = false;

        Collider[] colliders = Physics.OverlapSphere(transform.position, checkRange, LayerMask.GetMask("floor"));
        //Debug.Log("Ž��");
        if (colliders.Length > 0)
        {
            //Debug.Log("����");
            //������ �� ����
            thisRoom = colliders[0].gameObject;
            //�濡 �� ����
            thisRoom.GetComponent<Room>().door.Add(this);
            
            //thisRoom.GetComponent<Room>().StartSpawn();
            result = true;
        }

        return result;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isOpen)
        {
            //door.CheckRoom();
            door.thisRoom.GetComponent<Room>().StartSpawn();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isOpen = false;
    }
    ////���� �̹� �ݸ��� �ȿ� �ֱ� ������ stay���
    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Room"))
    //    {

    //    }
    //}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(checkRange, checkRange, checkRange));
    }



}
