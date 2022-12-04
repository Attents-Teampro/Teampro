using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingLamp : MonoBehaviour
{
    // 인터넷 참고함

    public float speed = 2f;            // 위 아래 움직이는 속도
    public float height = 0.5f;         // 움직일때 움직이는 
    public float startingY = 3.0f;

    public Transform player;

    void Update()
    {
        var pos = transform.position;
        var newY = startingY + height * Mathf.Cos(Time.time * speed); // Time.deltaTime 으로 하면 안움직임... Time.time 설정함
        transform.position = new Vector3(pos.x, newY, pos.z);

        transform.LookAt(player);
    }
}
