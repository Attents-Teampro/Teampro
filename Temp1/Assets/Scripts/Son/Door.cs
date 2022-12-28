using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.UIElements.ToolbarMenu;

public class Door : MonoBehaviour
{
    //�ݴ��� �� ��ũ��Ʈ
    public Door door;

    public bool isSpawn = false;
    bool isRelationDoorSpawn = false;
    bool isBoss = false;
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
        checkRange = 5f;
    }

    private void Start()
    {
        if(room == null)
        {
            Destroy(gameObject);
        }
        //SetPosition();
        SetPositionver2();

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
    }
    private void DoorControl(bool t)
    {
        if(mr!= null && col != null)
        {
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
        
    }
   
    public bool CheckRoom()
    {
        bool result = false;

        Vector3 box = new Vector3(0, checkRange, 0);
        Collider[] colliders = Physics.OverlapSphere(transform.position - new Vector3(0, transform.localScale.y*0.5f, 0),0.1f,  LayerMask.GetMask("floor"));

        //Collider[] colliders = Physics.OverlapBox(transform.position-new Vector3(0,transform.localScale.y,0), box, transform.rotation, LayerMask.GetMask("floor"));

        if (colliders.Length > 0)
        {
            //������ �� ����
            thisRoom = colliders[0].gameObject;
            //�濡 �� ����
            room = thisRoom.GetComponent<Room>();
            room.door.Add(this);
            result = true;
        }
        if (!result)
        {
           //Debug.Log($"���� {transform.localPosition}\n��� : {transform.position + (transform.localPosition * 1.5f) - new Vector3(0, transform.localScale.y * 0.5f, 0)}");
            Collider[] colliders1 = Physics.OverlapSphere(transform.position - new Vector3(0, transform.localScale.y * 0.5f, 0)+
                (transform.localPosition * 1.5f), 0.1f, LayerMask.GetMask("floor"));
            //Collider[] colliders1 = Physics.OverlapBox(transform.position - new Vector3(0, transform.localScale.y, 0) + transform.localPosition*3, box, transform.rotation, LayerMask.GetMask("floor"));

            if (colliders1.Length > 0)
            {
                //������ �� ����
                thisRoom = colliders1[0].gameObject;
                //�濡 �� ����
                room = thisRoom.GetComponent<Room>();
                room.door.Add(this);
                result = true;
            }
        }
        return result;
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"��. {gameObject.name}");
        //Debug.Log($"���� : \n" +
        //    $"�±� : {other.gameObject.CompareTag("Player")} \n" +
        //    $"isSpawn : {isSpawn}\n" +
        //    $"room.isClear : {room.isClear} ");

        //�÷��̾� ������ ����
        if (other.gameObject.CompareTag("Player"))
        {
            //�ش� ���� ���� ����� ����� �� ����, ����� ���� Ŭ����� ���� ���� �� ����
            //=>�÷��̾ �濡 ���� ���� �� ����
            if (!isSpawn && !room.isClear)
            {
                //���� ����� ����Ǿ��ٰ� bool ������ �ʱ�ȭ
                SetInfo();
                //�� �ݱ�
                IsOpen = false;
                //Room ��ũ��Ʈ�� �÷��̾� �� ������ �� ���� �Լ� ȣ��
                room.PlayerInThisRoom();
                if (isBoss)
                {
                    room.StartSpawnBoss();
                }
            }
            //�ش� ���� ����� ���� Ŭ����Ǿ���, ����� �ݴ��� ���� �������� �ʾҰ�, ����� �ݴ��� ���� Ŭ������� �ʾ����� ���� 
            else if (room.isClear && !isRelationDoorSpawn && !door.room.isClear)
            {
                
                //���� �÷��̾ �� ��° ������ üũ�ϴ� ���� ���� �� ���
                int roomIndex = ++other.gameObject.GetComponent<Player>().countCurrentRoom;
                //door.room.spawners = MainManager.instance.spawnManager.spawners[roomIndex];
                door.room.indexPlayerIn = roomIndex;
                //�ݴ��� ���� ����� �� ���� ����
                door.isBoss= door.room.StartSpawn();
                //�ش� ���� �ݴ��� ���� �����ߴ� ���� �ִٰ� bool ���� �ʱ�ȭ
                isRelationDoorSpawn = true;
            }
        }
        
    }

    public void SetInfo()
    {
        //�ش� ���� ������� �����ߴ��� ����
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
    public void SetPositionver2()
    {
        BoxCollider p = transform.parent.parent.gameObject.GetComponent<BoxCollider>();
        float colliderX = p.bounds.size.x * 0.5f;
        float colliderZ = p.bounds.size.z * 0.5f;
        float minValue = float.MaxValue;
        //�� ������
        Vector2 thisPositionVertual = new Vector2(transform.position.x, transform.position.z) ;
        //���� ������ ������(���� ���⿡ �°�, ���� Ƣ����ų� ������ ������ �ʴ� ������ ��ġ)
        Vector2 destination = Vector2.zero;
        //Debug.Log($"���� �������� {thisPositionVertual}�̴�.");

        Vector2[] arr = new Vector2[4];
        arr[0] = new Vector2(0, colliderZ) + new Vector2(p.center.x, p.center.z);
        arr[1] = new Vector2(0, -colliderZ) + new Vector2(p.center.x, p.center.z);
        arr[2] = new Vector2(colliderX, 0) + new Vector2(p.center.x, p.center.z);
        arr[3] = new Vector2(-colliderX, 0) + new Vector2(p.center.x, p.center.z);
        int count = 0;
        foreach (var i in arr)
        {
            
            Vector2 dis = thisPositionVertual - i;
            float distence = dis.sqrMagnitude;
            if(distence < minValue)
            {
                minValue = distence;
                destination = i;
                count++;
            }
        }
        transform.position = new Vector3(destination.x, transform.position.y, destination.y - p.center.y);



    }
    public void SetPosition(Collider parant =null)
    {
        //Ȥ�� ���� ������Ʈ�� �ݸ����� �����ȵǾ������� ����
        if (parant == null)
        {
            parant = transform.parent.parent.gameObject.GetComponent<BoxCollider>();
        }
        //���� ������Ʈ �߽��� ���ϱ�
        Vector3 parantPosition = parant.bounds.center;
        //���� ������Ʈ�� � �࿡ ������ �پ��ִ��� ���ϱ�

        //��� �࿡ �پ��ִ��� Ȯ��
        //x�� = true, z�� = false
        bool isAxis = false;

        //���� ��� �������� üũ
        int isSign = 0;

        //x�� üũ
        if(parantPosition.x < room.roomPosition.x + room.boxCol.bounds.size.x && parantPosition.x > room.roomPosition.x - room.boxCol.bounds.size.x)
        {
            //x�� �ȿ� ���� ���
            isAxis = true;
            //�濡�� ���� ��ġ�� z�� �������� +���� -���� ����
            if(parantPosition.z < room.roomPosition.z)
            {
                isSign = -1;
            }
            else
            {
                isSign = 1;
            }
        }
        //z�� üũ
        else if(parantPosition.z < room.roomPosition.z + room.boxCol.bounds.size.z && parantPosition.z > room.roomPosition.z - room.boxCol.bounds.size.z)
        {
            //z�� �ȿ� ���� ���
            isAxis = false;
            if (parantPosition.x < room.roomPosition.x)
            {
                isSign = -1;
            }
            else
            {
                isSign = 1;
            }
        }
        //����(�밢�� ������ ��
        else
        {
            Debug.Log("���� �߻�. ���� ��ġ�� ���� x��� z�� ��𿡵� �پ����� ����.");
        }


        float parantX = parant.bounds.extents.x;
        float parantZ = parant.bounds.extents.z;
        float thisPositionX = transform.position.x;
        float thisPositionZ = transform.position.z; 
        //z�� �������� ���� �����Ǿ� ���� ��
        if (isAxis)
        {
            thisPositionZ = parant.bounds.size.z * isSign;
        }
        else 
        {
            thisPositionX = parant.bounds.size.x * isSign;
        }

        transform.position = new Vector3(thisPositionX, transform.position.y, thisPositionZ) + transform.parent.position;

        Debug.Log($"{transform.parent.parent.name}�� �� {transform.name}�� ��ġ�� {transform.position}�̴�.");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position - new Vector3(0, transform.localScale.y * 0.5f, 0), 0.1f);
    }


}
