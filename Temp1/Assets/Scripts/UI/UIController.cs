using UnityEngine;
using UnityEngine.UI;
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

    //private void Start()
    //{
    //    LoadingSceneController.LoadScene("Title_Scene"); //�����Ҵ� �ε� �����ְ� Ÿ��Ʋȭ�� �����ֱ�
    //}



    private void Awake()
    {
        //var obj = FindObjectsOfType<UIController>();
        //if (obj.Length == 1)
        //{
            DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //    Debug.Log("�ߺ�����");
        //}
        
    }

    public void GameStart()
    {
        //SceneManager.LoadScene($"{sceneName}");
        LoadingSceneController.LoadScene($"{sceneName}"); //�ε����� ���� sceneName ���� �ε�
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
