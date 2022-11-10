using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    //반대편 문 스크립트
    public Door door;

    //문이 열렸는지 체크
    bool isOpen = false;

    //해당 문과 연결된 방 저장 변수
    GameObject thisRoom;

    //컴포넌트 변수
    BoxCollider col;
    MeshRenderer mr;
    private void Awake()
    {
        col = GetComponent<BoxCollider>();
        mr = GetComponent<MeshRenderer>();
    }

    public void Clear()
    {
        //문 이미지 없애기
        mr.enabled = false;

        //방을 클리어했으니 문을 열기
        //1. 문 체크 변수 변경
        isOpen = true;
        //2. 트리거 활성화
        col.isTrigger = true;
        //3. 방과 연결된 스포너 활성화
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
