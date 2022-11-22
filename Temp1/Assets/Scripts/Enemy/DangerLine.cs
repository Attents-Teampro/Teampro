using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DangerLine : MonoBehaviour
{
    private float speed = 10f;
    Vector3 targetPoint;

    Rigidbody rb;
    GameObject player;
   
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //10.11 수정. 
        //기존의 Player를 오브젝트 이름으로 찾는 방식이 
        //업데이트마다 플레이어 오브젝트의 이름이 변경될 시 에러가 뜰 위험이 있고 지금도 뜨고 있어서
        //클래스를 찾는 방식으로 변경했습니다.
        //기존 코드 player = GameObject.Find("Player").GetComponent<Transform>();
        player = FindObjectOfType<Player>().gameObject;//수정코드
        //by 손동욱
        
        targetPoint = player.transform.position + new Vector3(0, 1f, 0);
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,targetPoint, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
        else if (!other.CompareTag("Enemy"))
        {
        }
    }
}
