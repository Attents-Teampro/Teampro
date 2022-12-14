using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    /// <summary>
    /// 게임 스타트 버튼 누를시 넘어갈 스테이지 scene
    /// </summary>
    public string sceneName;


    /// <summary>
    /// 다음 스테이지(로비)로 씬으로 넘어가는 버튼 
    /// </summary>
    public string nextStage;

    //private void Start()
    //{
    //    LoadingSceneController.LoadScene("Title_Scene"); //시작할대 로딩 보여주고 타이틀화면 보여주기
    //}

    public void GameStart()
    {
        //SceneManager.LoadScene($"{sceneName}");
        LoadingSceneController.LoadScene($"{sceneName}"); //로딩씬을 통해 sceneName 씬을 로드
        Debug.Log($"{sceneName} will be load");
    }

    public void GameEnd()
    {
        Application.Quit();
        Debug.Log("App Quite");
    }

    public void NextStage()
    {
        //SceneManager.LoadScene($"{nextStage}");
        LoadingSceneController.LoadScene($"{nextStage}");
        Debug.Log("test goNext");
    }

    public void GotoTitle()
    {
        //SceneManager.LoadScene("Title_Scene");
        LoadingSceneController.LoadScene("Title_Scene");
    }






    //public void ShowResult()
    //{

    //}

    //public void GamePause()
    //{
    //    if(/*일시정지 버튼을 눌렀을때*/)
    //        if (isPause)
    //        {
    //            Time.timeScale = 0;
    //            isPause = true;

    //        }

    //        else
    //        {
    //            Time.timeScale = 1;
    //            isPause = false;

    //        }

    //}
}
