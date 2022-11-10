using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainManager : MonoBehaviour
{
    
    public static MainManager instance;

    //포탈 오브젝트. 포탈을 담은 빈 오브젝트를 비활성화 상태로 두고, 필요할 때만 활성화
    //public GameObject portalObject;

    public Action onClearthisRoom, onClosethisRoom;

    //각각 스테이지에 있는 몬스터, 죽은 몬스터 수를 세는 변수다.
    //스테이지 몬스터는 스포너에게, 죽은 몬스터 수는 몬스터 스크립트에서 얻는다.
    public int numOfStageEnemy = 0;
    
    public int numOfDieEnemy = 0;

    GameStart gameStart;
    public SpawnManager spawnManager;

    private void Awake()
    {
        //활성화 될 때 이미 메인 매너지 클래스가 있을 시 이 오브젝트를 삭제하는 코드
        if (instance != null)
        {
            //삭제 전 포탈 오브젝트와 전체 스테이지 에너미 수를 가져온다.
            //instance.portalObject = this.portalObject;
            Debug.Log($"{numOfStageEnemy}, {instance.numOfStageEnemy}현재 스테이지 몬스터 수");
            instance.numOfStageEnemy = this.numOfStageEnemy;
            Debug.Log($"{numOfStageEnemy}, {instance.numOfStageEnemy}현재 스테이지 몬스터 수");
            Destroy(gameObject);
            return;
        }
        numOfDieEnemy = 0;
        numOfStageEnemy = 0;
        instance = this;

        gameStart = GetComponent<GameStart>();
        spawnManager = GetComponent<SpawnManager>();
        //씬 이동시 삭제되지 않게 해주는 함수
        DontDestroyOnLoad(gameObject);
    }

    //몬스터가 모두 죽으면 실행되는 함수. 포탈을 활성화하고, 변수를 초기화한다.
    public void StageClear()
    {
        Debug.Log("스테이지 클리어");
        onClearthisRoom?.Invoke();

        //portalObject.SetActive(true);
        numOfDieEnemy = 0;
        numOfStageEnemy = 0;
    }

    /// <summary>
    /// 게임 시작 시 실행되는 함수
    /// </summary>
    public void StartGame()
    {
        gameStart.DungeonCreate();   
    }

    /*
    //씬 이동 시 포탈 오브젝트가 초기화 되어서 몬스터가 다 죽어도 포탈 오브젝트가 활성화되지 않는 에러가 있음
    //그래서 FixedUpdate로 
    //임시로 포탈 오브젝트를 각 씬에 미리 수동으로 만들어둔 포탈 오브젝트를 불러와주는 함수
    //현재 수작업으로 포탈을 설정 중인데 
    //추후에 자동으로 맵에 따라서 포탈이 나오게 된다면 수정되어야 할 부분 by 10.11 손동욱
    void FixedUpdate()
    {
        if (portalObject==null)
        {
            portalObject = GameObject.Find("Portal");
        }
    }
    */


}