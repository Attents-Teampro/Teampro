using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil_Face : MonoBehaviour
{
    public Transform player;

    private void Start()
    {

    }

    void Update()
    {
        transform.LookAt(player);
    }

    
}