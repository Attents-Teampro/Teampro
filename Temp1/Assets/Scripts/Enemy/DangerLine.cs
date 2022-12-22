using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DangerLine : MonoBehaviour
{
    public float speed = 1f;
    public float destoyTime;
    Rigidbody rb;
    TrailRenderer tr;
    float scaleZ;
    //GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        //tr = GetComponent<TrailRenderer>();
        rb= GetComponent<Rigidbody>();
        Destroy(gameObject, 1f);
        rb.velocity = transform.forward * speed;
        //rb.velocity = transform.right * speed;
        scaleZ = transform.localScale.z;
        Enemy_Bat bat = transform.root.GetComponent<Enemy_Bat>();
        //bat.onBatDie += SelfDestroy;
    }
    void SelfDestroy()
    {
        Destroy(this.gameObject);
    }
    void Update()
    {
        this.transform.localScale += new Vector3(0.0f, 0.0f, 5.0f * Time.deltaTime);
    }

}
