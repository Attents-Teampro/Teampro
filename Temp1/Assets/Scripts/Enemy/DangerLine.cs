using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DangerLine : MonoBehaviour
{
    public float speed = 10f;
    public float destoyTime;
    //public Vector3 targetPoint;
    Rigidbody rb;
    TrailRenderer tr;
    //GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        //player = FindObjectOfType<Player>().gameObject;//수정코드
        tr = GetComponent<TrailRenderer>();
        rb= GetComponent<Rigidbody>();
      //  targetPoint = player.transform.position + new Vector3(0, 1f, 0);
        Destroy(gameObject, 1f);
        rb.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        //DangerlineShot();
        //transform.position = Vector3.Lerp(transform.position,targetPoint, speed * Time.deltaTime);
    }

    void DangerlineShot()
    {
        //Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 30f, LayerMask.GetMask("Player"));
        //targetPoint = hit.point;
        //Debug.Log("데인저 라인 플레이어 발견" + targetPoint);
        //if (targetPoint != null)
        //{
        //    transform.position = Vector3.Lerp(transform.position, targetPoint, speed * Time.deltaTime);
        //}
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Destroy(this.gameObject);
    //    }
    //    else if (!other.CompareTag("Enemy"))
    //    {
    //    }
    //}
}