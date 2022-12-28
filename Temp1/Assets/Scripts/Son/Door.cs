using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.UIElements.ToolbarMenu;

public class Door : MonoBehaviour
{
    //반대편 문 스크립트
    public Door door;

    public bool isSpawn = false;
    bool isRelationDoorSpawn = false;
    bool isBoss = false;
    //해당 문과 연결된 방 저장 변수
    GameObject thisRoom = null;

    Room room;
    //컴포넌트 변수
    BoxCollider col;
    MeshRenderer mr;
    public float checkRange = 5f;

    //문이 열렸는지 체크
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
    /// 클리어, 클로즈를 담당
    /// </summary>
    /// <param name="x">true면 Open, false면 Close</param>
    public void ClearAndClose(bool x)
    {
        //방을 클리어했으니 문을 열기
        //문 체크 변수 변경
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
            //변수에 방 대입
            thisRoom = colliders[0].gameObject;
            //방에 문 대입
            room = thisRoom.GetComponent<Room>();
            room.door.Add(this);
            result = true;
        }
        if (!result)
        {
           //Debug.Log($"실패 {transform.localPosition}\n결과 : {transform.position + (transform.localPosition * 1.5f) - new Vector3(0, transform.localScale.y * 0.5f, 0)}");
            Collider[] colliders1 = Physics.OverlapSphere(transform.position - new Vector3(0, transform.localScale.y * 0.5f, 0)+
                (transform.localPosition * 1.5f), 0.1f, LayerMask.GetMask("floor"));
            //Collider[] colliders1 = Physics.OverlapBox(transform.position - new Vector3(0, transform.localScale.y, 0) + transform.localPosition*3, box, transform.rotation, LayerMask.GetMask("floor"));

            if (colliders1.Length > 0)
            {
                //변수에 방 대입
                thisRoom = colliders1[0].gameObject;
                //방에 문 대입
                room = thisRoom.GetComponent<Room>();
                room.door.Add(this);
                result = true;
            }
        }
        return result;
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"인. {gameObject.name}");
        //Debug.Log($"정보 : \n" +
        //    $"태그 : {other.gameObject.CompareTag("Player")} \n" +
        //    $"isSpawn : {isSpawn}\n" +
        //    $"room.isClear : {room.isClear} ");

        //플레이어 들어오면 실행
        if (other.gameObject.CompareTag("Player"))
        {
            //해당 문의 스폰 기능이 실행된 적 없고, 연결된 방이 클리어된 적이 없을 때 실행
            //=>플레이어가 방에 진입 했을 때 실행
            if (!isSpawn && !room.isClear)
            {
                //스폰 기능이 실행되었다고 bool 변수들 초기화
                SetInfo();
                //문 닫기
                IsOpen = false;
                //Room 스크립트의 플레이어 방 들어왔을 때 실행 함수 호출
                room.PlayerInThisRoom();
                if (isBoss)
                {
                    room.StartSpawnBoss();
                }
            }
            //해당 문과 연결된 방이 클리어되었고, 연결된 반대편 문을 스폰하지 않았고, 연결된 반대편 문이 클리어되지 않았으면 실행 
            else if (room.isClear && !isRelationDoorSpawn && !door.room.isClear)
            {
                
                //현재 플레이어가 몇 번째 방인지 체크하는 변수 저장 및 계산
                int roomIndex = ++other.gameObject.GetComponent<Player>().countCurrentRoom;
                //door.room.spawners = MainManager.instance.spawnManager.spawners[roomIndex];
                door.room.indexPlayerIn = roomIndex;
                //반대편 문과 연결된 방 스폰 시작
                door.isBoss= door.room.StartSpawn();
                //해당 문은 반대편 문을 스폰했던 적이 있다고 bool 변수 초기화
                isRelationDoorSpawn = true;
            }
        }
        
    }

    public void SetInfo()
    {
        //해당 문이 스폰기능 실행했는지 여부
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
        //문 포지션
        Vector2 thisPositionVertual = new Vector2(transform.position.x, transform.position.z) ;
        //최종 목적지 포지션(복도 방향에 맞고, 문이 튀어나오거나 안으로 빠지지 않는 최적의 위치)
        Vector2 destination = Vector2.zero;
        //Debug.Log($"가상 포지션은 {thisPositionVertual}이다.");

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
        //혹시 복도 오브젝트의 콜리더가 지정안되어있으면 지정
        if (parant == null)
        {
            parant = transform.parent.parent.gameObject.GetComponent<BoxCollider>();
        }
        //복도 오브젝트 중심점 구하기
        Vector3 parantPosition = parant.bounds.center;
        //복도 오브젝트의 어떤 축에 복도가 붙어있는지 구하기

        //어디 축에 붙어있는지 확인
        //x축 = true, z축 = false
        bool isAxis = false;

        //축의 어디 방향인지 체크
        int isSign = 0;

        //x축 체크
        if(parantPosition.x < room.roomPosition.x + room.boxCol.bounds.size.x && parantPosition.x > room.roomPosition.x - room.boxCol.bounds.size.x)
        {
            //x축 안에 있을 경우
            isAxis = true;
            //방에서 복도 위치가 z축 기준으로 +인지 -인지 구분
            if(parantPosition.z < room.roomPosition.z)
            {
                isSign = -1;
            }
            else
            {
                isSign = 1;
            }
        }
        //z축 체크
        else if(parantPosition.z < room.roomPosition.z + room.boxCol.bounds.size.z && parantPosition.z > room.roomPosition.z - room.boxCol.bounds.size.z)
        {
            //z축 안에 있을 경우
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
        //에러(대각선 생성일 때
        else
        {
            Debug.Log("에러 발생. 복도 위치가 방의 x축과 z축 어디에도 붙어있지 않음.");
        }


        float parantX = parant.bounds.extents.x;
        float parantZ = parant.bounds.extents.z;
        float thisPositionX = transform.position.x;
        float thisPositionZ = transform.position.z; 
        //z축 방향으로 방이 나열되어 있을 때
        if (isAxis)
        {
            thisPositionZ = parant.bounds.size.z * isSign;
        }
        else 
        {
            thisPositionX = parant.bounds.size.x * isSign;
        }

        transform.position = new Vector3(thisPositionX, transform.position.y, thisPositionZ) + transform.parent.position;

        Debug.Log($"{transform.parent.parent.name}의 문 {transform.name}의 위치는 {transform.position}이다.");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position - new Vector3(0, transform.localScale.y * 0.5f, 0), 0.1f);
    }


}
