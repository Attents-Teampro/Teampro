using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Flying : MonoBehaviour
{
    // ���ͳ� ������

    public float speed = 2f;            // �� �Ʒ� �����̴� �ӵ�
    public float height = 0.5f;         // �����϶� �����̴� 
    public float startingY = 3.0f;

    void Update()
    {
        var pos = transform.position;
        var newY = startingY + height * Mathf.Cos(Time.time * speed); // Time.deltaTime ���� �ϸ� �ȿ�����... Time.time ������
        transform.position = new Vector3(pos.x, newY, pos.z);
    }
}
