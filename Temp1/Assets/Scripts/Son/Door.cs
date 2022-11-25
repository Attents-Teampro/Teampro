using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Door : MonoBehaviour
{
    //반대편 문 스크립트
    public Door door;

    public bool isSpawn = false;
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
        //    //Debug.Log($"방 찾기 성공\n해당 문 {transform.parent.parent.name}자식 {name}의 방은 {thisRoom}입니다.");
        //}
        //else
        //{
        //    //Debug.Log($"방 찾기 실패\n해당 문 {transform.parent.parent.name}자식 {name}의 방은 찾지못했습니다.");
        //    Destroy(gameObject);
        //}
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
        //Debug.Log("문 열기 실행 1");
    }
    private void DoorControl(bool t)
    {
        //Debug.Log("문 열기 실행 2");
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

        //Debug.Log("탐지");
        if (colliders.Length > 0)
        {
            //Debug.Log("실행");
            //변수에 방 대입
            thisRoom = colliders[0].gameObject;
            //방에 문 대입
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

    ////방이 이미 콜리더 안에 있기 때문에 stay사용
    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Room"))
    //    {

    //    }
    //}

}
