//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using static UnityEngine.GraphicsBuffer;

public class DangerLine : MonoBehaviour
{
    public float speed = 1f;
    public float destoyTime;
    Rigidbody rb;
    float scaleZ;
    public Enemy_Bat parentBat;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 1f);
        rb.velocity = transform.forward * speed;
        //scaleZ = transform.localScale.z;
    }
    void Update()
    {
        this.transform.localScale += new Vector3(0.0f, 0.0f, 5.0f * Time.deltaTime);
    }

}
