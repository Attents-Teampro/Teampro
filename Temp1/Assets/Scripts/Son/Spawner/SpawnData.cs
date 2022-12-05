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
        //생성할 몬스터 프리팹, 몬스터가 생성되는 위치 계산을 위한 벽 오브젝트
        public GameObject enemyObject;
        //생성할 몬스터 수
        public int numOfEnemy;
        //오브젝트인지 아닌지
        public bool isObject;
    }
    [Header("해당 방에 스폰될 몬스터 총 정보.")]
    public enemySpawnInfo[] SpawnArr;

    
    
}
 