using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingLamp : MonoBehaviour
{
    // ���ͳ� ������

    public float speed = 2f;            // �� �Ʒ� �����̴� �ӵ�
    public float height = 0.5f;         // �����϶� �����̴� 
    public float startingY = 3.0f;

    public Transform player;

    GameObject lightObject;
    private void Start()
    {
        lightObject = transform.GetChild(1).gameObject;
        lightObject.SetActive(false);

    }

    void Update()
    {
        var pos = transform.position;
        var newY = startingY + height * Mathf.Cos(Time.time * speed); // Time.deltaTime ���� �ϸ� �ȿ�����... Time.time ������
        transform.position = new Vector3(pos.x, newY, pos.z);

        transform.LookAt(player);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lightObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lightObject.SetActive(false);
        }
    }
}
