using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data_Room", menuName = "Scriptable Object/Spawn Data", order = 2)]
public class SpawnData : ScriptableObject
{
    [Serializable]
    public struct enemySpawnInfo
    {
        //������ ���� ������, ���Ͱ� �����Ǵ� ��ġ ����� ���� �� ������Ʈ
        public GameObject enemyObject;
        //������ ���� ��
        public int numOfEnemy;
        //������Ʈ���� �ƴ���
        public bool isObject;
    }
    [Header("�ش� �濡 ������ ���� �� ����.")]
    public enemySpawnInfo[] SpawnArr;

    
    
}
 