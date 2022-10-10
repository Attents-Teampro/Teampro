using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainManager : MonoBehaviour
{
    
    public static MainManager instance;
    
    //포탈 오브젝트. 포탈을 담은 빈 오브젝트를 비활성화 상태로 두고, 필요할 때만 활성화
    public GameObject portalObject;

    //각각 스테이지에 있는 몬스터, 죽은 몬스터 수를 세는 변수다.
    //스테이지 몬스터는 스포너에게, 죽은 몬스터 수는 몬스터 스크립트에서 얻는다.
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

    //몬스터가 모두 죽으면 실행되는 함수. 포탈을 활성화하고, 변수를 초기화한다.
    public void StageClear()
    {
        portalObject.SetActive(true);
        numOfDieEnemy = 0;
        numOfStageEnemy = 0;
    }
}