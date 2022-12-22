using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Vector3 offset;
    [SerializeField]
    Transform player;

    public bool isCinema = false;

    void Start()
    {
        offset = transform.position - player.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCinema)
        {

        }
        else
        {
            transform.position = player.position + offset;
        }
        
    }
}
