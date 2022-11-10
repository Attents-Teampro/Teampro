using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Door : MonoBehaviour
{
    //반대편 문 스크립트
    public Door door;

    
    //문이 열렸는지 체크
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

    //해당 문과 연결된 방 저장 변수
    GameObject thisRoom = null;

    //컴포넌트 변수
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
            //Debug.Log($"방 찾기 성공\n해당 문 {transform.parent.parent.name}자식 {name}의 방은 {thisRoom}입니다.");
        }
        else
        {
            //Debug.Log($"방 찾기 실패\n해당 문 {transform.parent.parent.name}자식 {name}의 방은 찾지못했습니다.");
            Destroy(gameObject);
        }
    }

    public void Clear()
    {
        //방을 클리어했으니 문을 열기
        //문 체크 변수 변경
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
        //Debug.Log("탐지");
        if (colliders.Length > 0)
        {
            //Debug.Log("실행");
            //변수에 방 대입
            thisRoom = colliders[0].gameObject;
            //방에 문 대입
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
    ////방이 이미 콜리더 안에 있기 때문에 stay사용
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
