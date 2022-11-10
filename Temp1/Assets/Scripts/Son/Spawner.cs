using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //������ ���� ������, ���Ͱ� �����Ǵ� ��ġ ����� ���� �� ������Ʈ
    public GameObject enemyObject;//, Wall;

    //������ ���� ���� ������ �Ŵ��� ��ũ��Ʈ
    //������ �� ���� �Ŵ������� ���� �ѱ��. ���� �Ŵ����� �Ѱ��� ������ ���ؼ� �� ���� ���� ���Ѵ�.
    //public MainManager mainManager;

    //���� floor���� Ground ������Ʈ ������ �˴ϴ�.
    //public BoxCollider floor;

    //������ ���� ��
    public int numOfEnemy = 5; 

    //floor������Ʈ�� ��ġ���� ���� �� �����Լ��� ���� ����
    Vector3 sizeOfGround;

    //���� ���� ��ġ�� �ùٸ� ���� false�� �Ǵ� bool ����
    bool resetPosition = true;

    //���� �������� Ȯ���ϰ�, ���� ���͸� ��ġ �������ִ� bool ����
    public bool isBoss = false;

    //������ ��ġ�� ����
    public Vector3 bossPosition;

    //�Ž������� ����
    MeshRenderer mr;
    private void Awake()
    {
        //mr = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        //11.10 ����. ������ start�� �������ڸ��� ������ �ƴ϶�, Ư�� �Լ� ȣ�� �� ����
        //StartCoroutine(SpawnerStart());
    }

    public void StartSpawn()
    {
        StartCoroutine(SpawnerStart());
    }
    IEnumerator SpawnerStart()
    {
        yield return new WaitForSeconds(Time.deltaTime * 5);
        //���� �Ŵ������� ������ ������ ���� �ѱ��.
        
        //mainManager = FindObjectOfType<MainManager>();
        MainManager.instance.numOfStageEnemy += numOfEnemy;

        //���� ���͸� �����ϴ� ��� ������ ��ġ bossPosition�� ���� ���ӿ�����Ʈ ���� �� Ŭ���� ����
        if (isBoss)
        {
            Instantiate(enemyObject, bossPosition, Quaternion.identity);
            //return;
            yield break;
        }
        //11.10 �ּ� by �յ���
        //���� �ٲ� �ּ� ó�� 
        ////���ϵ� 0��aa���� ������� ������ ���� �� �Ʒ� ��. ������ �¾ƾ� �Ʒ� �������� ���� ����
        //Vector3[] WallPosition;
        //WallPosition = new Vector3[Wall.transform.childCount];
        ////Wall ������Ʈ�� ���ϵ� ������Ʈ �¿����Ʒ� ����

        Debug.Log("�޽������� ã��");
        mr = GetComponent<MeshRenderer>();
        Vector3 floor = mr.bounds.size * 0.5f;

        
        //���� ����. 
        for (int i = 0; i < numOfEnemy; i++)
        {
            //���� �ʱ�ȭ. �� �ʿ���
            resetPosition = true;

            //���Ͱ� �� �ȿ� �����Ǵ��� if�� ����ϰ�, �� ���� �ƴϸ� ��� ���ư��� while�� 
            //(�� �ȿ� �����Ǹ� resetPosition = false) 
            while (resetPosition)
            {
                //�ٴ� ������Ʈ floor�� �ݸ��� ����� ���ؼ�(Vector3������ ����) Random.value�� ���ϴ� ��.
                //�� ������ �ٴ� �ݸ��� ũ�⿡ �°� ��ġ���� ���.. �׷��� �ݸ��� ũ�Ⱑ �� Ŀ�� �׷��� �ڲ� ���� �Ѿ
                //�׷��� �ٷ� �Ʒ��� if������ �� �ȿ� �ִ��� �ǵ��ϰ� �������.
                //�Ƹ� �ݸ��� ũ�Ⱑ �� ū�� �ݸ��� /2�� �ϸ� �ذ� ������ �𸣰��� �ϴ� if������ �ذ��ϰ� ���߿� Ȯ���� ����
                sizeOfGround = new Vector3(Random.Range(-floor.x, floor.x), floor.y, Random.Range(-floor.z, floor.z));
                if (mr.bounds.max.x < sizeOfGround.x ||   //������ ��
                    -mr.bounds.max.x > sizeOfGround.x ||   //���� ��
                    mr.bounds.max.z < sizeOfGround.z ||   //�� ��
                    -mr.bounds.max.x > sizeOfGround.z)     //�Ʒ� ��
                {
                    //�󸶳� while���� �ݺ��Ǿ����� �� �� ����. ���� �������� ���̴� �� �ĺ������� ���� ����
                    Debug.Log("Enemy ������Ʈ ��ġ ��Ż. ������");
                }
                else
                {
                    //���� ������Ʈ ����.
                    Instantiate(enemyObject, sizeOfGround + GetComponent<Room>().roomPosition, Quaternion.identity);
                    //while�� ����. for���� ��� ����Ǳ� ������ �ʱ⿡ ������ numOfEnemy ���� �°� ���� ������ ��
                    resetPosition = false;
                }
            }
        }
    }
}
