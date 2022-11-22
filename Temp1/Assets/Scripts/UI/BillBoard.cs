  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    Transform camera_EHP;

    private void Start()
    {
        camera_EHP = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + camera_EHP.rotation*Vector3.forward,camera_EHP.rotation*Vector3.up);
    }

    ///생기고 바로 사라지는 문제
}
