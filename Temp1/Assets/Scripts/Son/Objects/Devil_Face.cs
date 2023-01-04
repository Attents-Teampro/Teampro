using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil_Face : MonoBehaviour
{
    // 살짝만 움직이게 하기

    float speed = 0.2f;
    Vector3 endPosition = new Vector3(20.73f, -5.93f, 25.69f);

    private void Update()
    {
        if (transform.position != endPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
        }
    }

}