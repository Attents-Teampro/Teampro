using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLight : MonoBehaviour
{
    [SerializeField]
    GameObject lightObj;
    // Start is called before the first frame update
    void Start()
    {
        GameObject light = Instantiate(lightObj);
        light.transform.position += transform.position;
    }

}
