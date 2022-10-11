using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainManager : MonoBehaviour
{
    
    public static MainManager instance;
    
    //��Ż ������Ʈ. ��Ż�� ���� �� ������Ʈ�� ��Ȱ��ȭ ���·� �ΰ�, �ʿ��� ���� Ȱ��ȭ
    public GameObject portalObject;

    //���� ���������� �ִ� ����, ���� ���� ���� ���� ������.
    //�������� ���ʹ� �����ʿ���, ���� ���� ���� ���� ��ũ��Ʈ���� ��´�.
    public int numOfStageEnemy = 0, numOfDieEnemy = 0;

    private void Awake()
    {
        //Ȱ��ȭ �� �� �̹� ���� �ų��� Ŭ������ ���� �� �� ������Ʈ�� �����ϴ� �ڵ�
        if (instance != null)
        {
            //���� �� ��Ż ������Ʈ�� ��ü �������� ���ʹ� ���� �����´�.
            instance.portalObject = this.portalObject;
            Debug.Log($"{numOfStageEnemy}, {instance.numOfStageEnemy}���� �������� ���� ��");
            instance.numOfStageEnemy = this.numOfStageEnemy;
            Debug.Log($"{numOfStageEnemy}, {instance.numOfStageEnemy}���� �������� ���� ��");
            Destroy(gameObject);
            return;
        }
        numOfDieEnemy = 0;
        numOfStageEnemy = 0;
        instance = this;

        //�� �̵��� �������� �ʰ� ���ִ� �Լ�
        DontDestroyOnLoad(gameObject);
    }

    //���Ͱ� ��� ������ ����Ǵ� �Լ�. ��Ż�� Ȱ��ȭ�ϰ�, ������ �ʱ�ȭ�Ѵ�.
    public void StageClear()
    {
        portalObject.SetActive(true);
        numOfDieEnemy = 0;
        numOfStageEnemy = 0;
    }

    /*
    //�� �̵� �� ��Ż ������Ʈ�� �ʱ�ȭ �Ǿ ���Ͱ� �� �׾ ��Ż ������Ʈ�� Ȱ��ȭ���� �ʴ� ������ ����
    //�׷��� FixedUpdate�� 
    //�ӽ÷� ��Ż ������Ʈ�� �� ���� �̸� �������� ������ ��Ż ������Ʈ�� �ҷ����ִ� �Լ�
    //���� ���۾����� ��Ż�� ���� ���ε� 
    //���Ŀ� �ڵ����� �ʿ� ���� ��Ż�� ������ �ȴٸ� �����Ǿ�� �� �κ� by 10.11 �յ���
    void FixedUpdate()
    {
        if (portalObject==null)
        {
            portalObject = GameObject.Find("Portal");
        }
    }
    */

    
}