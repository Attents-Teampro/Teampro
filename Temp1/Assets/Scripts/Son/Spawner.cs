using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    
    ////, Wall;
    ////생성할 몬스터 프리팹, 몬스터가 생성되는 위치 계산을 위한 벽 오브젝트
    //public GameObject enemyObject;
    ////생성할 몬스터 수
    //public int numOfEnemy;
    //생성된 몬스터 수를 저장할 매니저 스크립트
    //시작할 때 메인 매니저에게 값을 넘긴다. 메인 매니저는 넘겨진 값들을 합해서 총 몬스터 수를 구한다.
    //public MainManager mainManager;

    //변수 floor에는 Ground 오브젝트 넣으면 됩니다.
    //public BoxCollider floor;


    //floor오브젝트의 위치값을 저장 및 랜덤함수를 돌릴 변수
    Vector3 sizeOfGround;

    //몬스터 생성 위치가 올바를 때만 false가 되는 bool 변수
    bool resetPosition = true;

    //보스 몬스터인지 확인하고, 보스 몬스터면 위치 지정해주는 bool 변수
    public bool isBoss = false;

    //보스면 위치할 벡터
    public Vector3 bossPosition;

    //매쉬랜더러 변수
    MeshRenderer mr;
    //Room 스크립트 변수
    Room room;
    [Header("해당 방에 스폰될 몬스터 총 정보.")]
    public SpawnData spawnData;
    int totalSpawnMonster = 0;
    //테스트용 변수
    GameObject[] mon;

    //[SerializeField]
    //bool isObject = false;
    public void StartSpawn(GameObject obj)
    {
        
        mr = obj.GetComponent<MeshRenderer>();
        room = obj.GetComponent<Room>();
        if(spawnData != null && !isBoss)
        {
            for(int i = 0; i< spawnData.SpawnArr.Length;i++)
            {
                SpawnerStart(i);
            }
        }else if (isBoss)
        {
            Instantiate(spawnData.SpawnArr[0].enemyObject, bossPosition, Quaternion.identity);
            //return;
            return;
        }
        
        
    }
    public void AddMainToSpawnNum()
    {
        if (!isBoss)
        {
            for (int i = 0; i < spawnData.SpawnArr.Length; i++)
            {
                int numOfEnemy = spawnData.SpawnArr[i].numOfEnemy;
                if (!spawnData.SpawnArr[i].isObject)
                {
                    MainManager.instance.numOfStageEnemy += numOfEnemy;
                }
            }
        }
        else
        {
            MainManager.instance.numOfStageEnemy += spawnData.SpawnArr[0].numOfEnemy;
        }
        
    }
    void SpawnerStart(int index)
    {
        GameObject enemyObject = spawnData.SpawnArr[index].enemyObject;
        int numOfEnemy = spawnData.SpawnArr[index].numOfEnemy;

        //메인 매니저에게 생성될 몬스터의 수를 넘긴다.

        //mainManager = FindObjectOfType<MainManager>();
        //MainManager.instance.numOfStageEnemy += numOfEnemy;
        //Debug.Log($"값 {numOfEnemy}를 메인에 넘겨줌");

        ////보스 몬스터를 스폰하는 경우 지정된 위치 bossPosition에 보스 게임오브젝트 생성 후 클래스 종료
        //if (isBoss)
        //{
        //    Instantiate(enemyObject, bossPosition, Quaternion.identity);
        //    //return;
        //    return;
        //}
        //11.10 주석 by 손동욱
        //벽이 바뀌어서 주석 처리 
        ////차일드 0번aa부터 순서대로 오른쪽 왼쪽 위 아래 벽. 순서가 맞아야 아래 이프문이 정상 실행
        //Vector3[] WallPosition;
        //WallPosition = new Vector3[Wall.transform.childCount];
        ////Wall 오브젝트의 차일드 오브젝트 좌우위아래 대입

        //Debug.Log("메쉬랜더러 찾음");
        //mr = GetComponent<MeshRenderer>();
        Vector3 floor = mr.bounds.size * 0.5f * 0.9f;

        //테스트 변수 관련 코드
        mon = new GameObject[numOfEnemy];
        //몬스터 생성. 
        for (int i = 0; i < numOfEnemy; i++)
        {
            //변수 초기화. 꼭 필요함
            resetPosition = true;

            //몬스터가 벽 안에 생성되는지 if로 계산하고, 벽 안이 아니면 계속 돌아가는 while문 
            //(벽 안에 생성되면 resetPosition = false) 
            while (resetPosition)
            {
                //바닥 오브젝트 floor의 콜리더 사이즈를 구해서(Vector3값으로 받음) Random.value를 곱하는 식.
                //이 식으로 바닥 콜리더 크기에 맞게 위치값을 계산.. 그런데 콜리더 크기가 더 커서 그런지 자꾸 벽을 넘어감
                //그래서 바로 아래에 if문으로 벽 안에 있는지 판독하게 만들었음.
                //아마 콜리더 크기가 더 큰게 콜리더 /2를 하면 해결 될지도 모르겠음 일단 if문으로 해결하고 나중에 확인할 예정
                sizeOfGround = new Vector3(Random.Range(-floor.x, floor.x), floor.y, Random.Range(-floor.z, floor.z));
                if (mr.bounds.max.x < sizeOfGround.x ||   //오른쪽 벽
                    -mr.bounds.max.x > sizeOfGround.x ||   //왼쪽 벽
                    mr.bounds.max.z < sizeOfGround.z ||   //윗 벽
                    -mr.bounds.max.x > sizeOfGround.z)     //아래 벽
                {
                    //얼마나 while문이 반복되었는지 알 수 있음. 추후 오차값을 줄이는 데 식별용으로 쓰일 예정
                    Debug.Log("Enemy 오브젝트 위치 이탈. 재조정");
                }
                else
                {
                    //몬스터 오브젝트 생성.
                    mon[i] = Instantiate(enemyObject, sizeOfGround + room.roomPosition, Quaternion.identity);
                    //while문 종료. for문은 계속 실행되기 때문에 초기에 설정한 numOfEnemy 수에 맞게 몬스터 생성이 됨
                    resetPosition = false;
                }
            }
        }
        totalSpawnMonster = mon.Length;
    }

    public void KillAllMonsters()
    {
        foreach(var i in mon)
        {
            ICharacter ic = i.GetComponent<ICharacter>();
            ic.Die();
            Debug.Log(i.name);
            Debug.Log("죽음");

        }
    }
}
