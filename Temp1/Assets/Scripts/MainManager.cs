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
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //���Ͱ� ��� ������ ����Ǵ� �Լ�. ��Ż�� Ȱ��ȭ�ϰ�, ������ �ʱ�ȭ�Ѵ�.
    public void StageClear()
    {
        portalObject.SetActive(true);
        numOfDieEnemy = 0;
        numOfStageEnemy = 0;
    }
}