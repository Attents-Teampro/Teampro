using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    //Canvas canvas; 삭제 예정
    // resultPanel

    //일시정지
    //bool isPause= false;

    private void Awake()
    {
        //resultPanel = GetComponentInChildren<Canvas>();

        //resultPanel.gameObject.SetActive(false);
    }

    //private void Start()
    //{
    //    isPause = false;   
    //}

    public void GameStart()
    {
        SceneManager.LoadScene($"{sceneName}");
        Debug.Log($"{sceneName} will be load");
    }

    public void GameEnd()
    {
        Application.Quit();
        Debug.Log("App Quite");
    }

    public void NextStage()
    {
        SceneManager.LoadScene($"{nextStage}");
        Debug.Log("test goNext");
    }

    public void GotoTitle()
    {
        SceneManager.LoadScene("Title_Scene");
    }



    public void ShowResult()
    {
        //if (resultPanel != null)
        //{
        //    resultPanel.gameObject.SetActive(true);
        //}
    }

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
