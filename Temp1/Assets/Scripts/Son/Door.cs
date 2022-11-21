using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Door : MonoBehaviour
{
    //�ݴ��� �� ��ũ��Ʈ
    public Door door;

    public bool isSpawn = false;
    //�ش� ���� ����� �� ���� ����
    GameObject thisRoom = null;

    Room room;

    //������Ʈ ����
    BoxCollider col;
    MeshRenderer mr;
    public float checkRange = 5f;

    //���� ���ȴ��� üũ
    bool isOpen = false;
    public bool IsOpen
    {
        get => isOpen;
        set 
        {
            
            isOpen = value;
            DoorControl(isOpen);
        }
    }
    

    

    private void Awake()
    {
        col = GetComponent<BoxCollider>();
        mr = GetComponent<MeshRenderer>();
        //sphereCol = GetComponent<SphereCollider>();
        checkRange = transform.localScale.y * 0.5f +5f;
    }

    private void Start()
    {
        if(room == null)
        {
            Destroy(gameObject);
        }
        //if (CheckRoom())
        //{
        //    //Debug.Log($"�� ã�� ����\n�ش� �� {transform.parent.parent.name}�ڽ� {name}�� ���� {thisRoom}�Դϴ�.");
        //}
        //else
        //{
        //    //Debug.Log($"�� ã�� ����\n�ش� �� {transform.parent.parent.name}�ڽ� {name}�� ���� ã�����߽��ϴ�.");
        //    Destroy(gameObject);
        //}
    }

    /// <summary>
    /// Ŭ����, Ŭ��� ���
    /// </summary>
    /// <param name="x">true�� Open, false�� Close</param>
    public void ClearAndClose(bool x)
    {
        //���� Ŭ���������� ���� ����
        //�� üũ ���� ����
        IsOpen = x;
        door.IsOpen = x;
        //Debug.Log("�� ���� ���� 1");
    }
    private void DoorControl(bool t)
    {
        //Debug.Log("�� ���� ���� 2");
        //Open
        if (t)
        {
            mr.enabled = false;
            col.isTrigger = true;
            SetColliderSize(true);
        }
        //Close
        else
        {
            mr.enabled = true;
            col.isTrigger = false;
            SetColliderSize(false);
        }
    }
   
    public bool CheckRoom()
    {
        bool result = false;

        Vector3 box = new Vector3(0.1f, checkRange, 0);
        Collider[] colliders = Physics.OverlapBox(transform.position, box, transform.rotation, LayerMask.GetMask("floor"));
        //Physics.OverlapSphere(transform.position, checkRange, LayerMask.GetMask("floor"));

        //Debug.Log("Ž��");
        if (colliders.Length > 0)
        {
            //Debug.Log("����");
            //������ �� ����
            thisRoom = colliders[0].gameObject;
            //�濡 �� ����
            room = thisRoom.GetComponent<Room>();
            room.door.Add(this);
            //Debug.Log(room.door);

            //thisRoom.GetComponent<Room>().StartSpawn();
            result = true;
        }

        return result;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isSpawn && !room.isClear)
        {
            SetInfo();
            IsOpen = false;
            room.PlayerInThisRoom();
            //room.StartSpawn();
        }
        else if (room.isClear)
        {
            door.room.StartSpawn();
        }
    }

    public void SetInfo()
    {
        isSpawn = true;
    }
    public void SetColliderSize(bool isFront)
    {
        float colX = 0;
        if (isFront)
        {
            colX = -18;
        }
        col.center = new Vector3(colX, col.center.y, col.center.z);


    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player") && !room.isClear)
    //    {
    //        room.StartSpawn();
    //    }
    //}


    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, checkRange);
    }

    ////���� �̹� �ݸ��� �ȿ� �ֱ� ������ stay���
    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Room"))
    //    {

    //    }
    //}

}
