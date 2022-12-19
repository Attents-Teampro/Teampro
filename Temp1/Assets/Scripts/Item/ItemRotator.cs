using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotator : MonoBehaviour
{
    public float rotateSpeed = 5f;
    public float virticalSpeed;
    public float minHeight = 1;
    public float maxHeight;
    float timeElapsed;
    float halfDiff;
    Vector3 newPosition;
    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;
        newPosition.y = minHeight;
        transform.position = newPosition;

        timeElapsed = 0;
        halfDiff = 0.5f * (maxHeight - minHeight);

        //드랍할 아이템의 값을 몬스터에 적용하는것보다 아이템이 고유로 갖는게 좋을것 같음
        //coinValue = (int)GameObject.Find("Orc").GetComponent<EnemyBase>().enemyData.GoldValue;
        //coinValue = (int)transform.GetComponentInParent<EnemyBase>().enemyData.GoldValue;
        
        //Debug.Log(coinValue);
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime * virticalSpeed;
        newPosition.x = transform.position.x;
        newPosition.z = transform.position.z;
        newPosition.y = minHeight + (1 - Mathf.Cos(timeElapsed)) * halfDiff;

        transform.position = newPosition;

        transform.Rotate(0, Time.deltaTime * rotateSpeed, 0);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        MainManager.instance.gold += coinValue;
    //        Destroy(this.gameObject);
    //        Debug.Log($"현재 플레이어 골드 : {MainManager.instance.gold}");
    //    }
    //}
}
