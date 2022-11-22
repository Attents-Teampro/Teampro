using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    /// <summary>
    /// ���� ��ŸƮ ��ư ������ �Ѿ �������� scene
    /// </summary>
    public string sceneName;

    /// <summary>
    /// ���� ��������(�κ�)�� ������ �Ѿ�� ��ư 
    /// </summary>
    public string nextStage;

    //Canvas canvas; ���� ����
    // resultPanel

    //�Ͻ�����
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
    //    if(/*�Ͻ����� ��ư�� ��������*/)
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
