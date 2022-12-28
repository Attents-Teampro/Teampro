using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSet : MonoBehaviour
{
    Player p;
    Vector3 offset;
    Quaternion rotation;
    Camera c;
    private void Start()
    {
        p = FindObjectOfType<Player>();
        c = p.gameObject.GetComponentInChildren<Camera>();
        
    }

    public void SetPositionToPlayer()
    {
        offset = c.transform.position;
        rotation = c.transform.rotation;
        transform.position = offset;
        transform.rotation = rotation;
        transform.parent = p.gameObject.transform;
    }
}
