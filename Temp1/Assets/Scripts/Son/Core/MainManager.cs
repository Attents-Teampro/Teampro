using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : Singleton<MainManager>
{
    Player player;

    [SerializeField]
    int healAmount_StageClear= 20;

    public Player Player => player;

    public static MainManager instance;

    //포탈 오브젝트. 포탈을 담은 빈 오브젝트를 비활성화 상태로 두고, 필요할 때만 활성화
    //public GameObject portalObject;

    public Action onClearthisRoom, onClosethisRoom;

    //각각 스테이지에 있는 몬스터, 죽은 몬스터 수를 세는 변수다.
    //스테이지 몬스터는 스포너에게, 죽은 몬스터 수는 몬스터 스크립트에서 얻는다.
    public int numOfStageEnemy = 0;
    
    /// <summary>
    /// 스테이지(룸)에서 죽은 몬스터 카운터
    /// 방이 클리어되면 초기화된다.
    /// </summary>
    public int numOfDieEnemy = 0;

    /// <summary>
    /// 전체 죽인 몬스터 개수
    /// 방이 클리어 되어도 초기화하지 않는다.
    /// </summary>
    public int numOfTotalKillEnemy = 0;

    /// <summary>
    /// 획득한 총 골드
    /// </summary>
    public int gold = 0;

    /// <summary>
    /// 클리어 시 나올 UI윈도우
    /// </summary>
    public GameObject clearUIWindos;
    GameStart gameStart;

    /// <summary>
    /// 스폰매니저 저장 변수
    /// </summary>
    [NonSerialized]
    public SpawnManager spawnManager;

    
    /// <summary>
    /// 스테이지가 모두 클리어되었을 때(보스가 죽고 클리어 UI가 나왔을 때 true가 되는 변수
    /// </summary>
    public bool isClear = false;
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
    public GameObject failUIWindow;
    protected override void Start()
    {
        base.Start();
        GameStartFunc();
    }
    /// <summary>
    /// 몬스터가 모두 죽으면 실행되는 함수. 포탈을 활성화하고, 변수를 초기화한다.
    /// </summary>
    /// <param name="isBoss">ture면 보스가 죽었을 때 실행되는 클리어 함수</param>
    public void StageClear(bool isBoss = false)
    {
        onClearthisRoom?.Invoke();
        
        
        //portalObject.SetActive(true);
        numOfDieEnemy = 0;
        numOfStageEnemy = 0;

        //스테이지 클리어 시 회복 기능
        //if(player != null && !isBoss)
        //{
        //    ICharacter ic = player.GetComponent<ICharacter>();
        //    ic.Attacked(-healAmount_StageClear);
        //}
        
        //보스가 처치되면
        if (isBoss)
        {
            Timer timer = FindObjectOfType<Timer>();
            if (timer != null)
            {
                timer.SetTimerOff();
            }
            //클리어 bool 변수 초기화
            isClear = true;

            if(clearUIWindos!= null)
            {
                //클리어UI 창 활성화
                clearUIWindos.SetActive(true);
            }
            else
            {
                GameObject canvas = GameObject.Find("Canvas");
                if(canvas != null ) 
                {
                    clearUIWindos = canvas.transform.GetChild(canvas.transform.childCount - 2).gameObject;
                    clearUIWindos.SetActive(true);
                }
            }
            
            
        }
    }

    /// <summary>
    /// 게임 시작 시 실행되는 함수
    /// </summary>
    public void GameStartFunc()
    {
        if (gameStart.player == null) 
        {
            gameStart.player = Player.gameObject; 
        }
        if(gameStart.creater == null)
        {
            DungeonCreator d = FindObjectOfType<DungeonCreator>();
            if(d != null)
            {
                gameStart.creater = d.gameObject;
            }
            
        }
        gameStart.DungeonCreate();   
    }

    protected override void Initialize()
    {
        base.Initialize();
        player = FindObjectOfType<Player>();
        gameStart = GetComponent<GameStart>();
        spawnManager = GetComponent<SpawnManager>();
        if (clearUIWindos == null)
        {
            
        }
    }
    /// <summary>
    /// 플레이어 사망시 UI 활성
    /// </summary>
    public void StageFailed()
    {

        if (failUIWindow != null)
        {
            failUIWindow.SetActive(true);
        }
        else
        {
            GameObject canvas = GameObject.Find("Canvas");
            failUIWindow = canvas.transform.GetChild(canvas.transform.childCount - 1).gameObject;
            failUIWindow.SetActive(true);
        }

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
    public void Initialize_()
    {
        player = FindObjectOfType<Player>();
        gameStart = GetComponent<GameStart>();
        spawnManager = GetComponent<SpawnManager>();
    }

}