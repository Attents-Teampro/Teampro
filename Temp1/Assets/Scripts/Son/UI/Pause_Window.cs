using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause_Window : MonoBehaviour
{
    [SerializeField]
    string mainSceneName;
    public Pause pause;

    Button resumeBtn;
    Button mainBtn;
    Button exitBtn;

    [System.Obsolete]
    private void Awake()
    {
        resumeBtn = transform.FindChild("Resume").GetComponent<Button>();
        mainBtn = transform.FindChild("Main").GetComponent<Button>();
        exitBtn = transform.FindChild("Exit").GetComponent<Button>();
    }
    private void Start()
    {
        resumeBtn.onClick.AddListener(Resume);
        mainBtn.onClick.AddListener(MainManu);
        exitBtn.onClick.AddListener(ExitGame);
    }
    public void Resume()
    {
        if (pause.IsPaused)
        {
            pause.UI_Pause();
        }
        Debug.Log("�����");
    }
    public void MainManu()
    {
        Time.timeScale = 1.0f;
        if(mainSceneName != null)
        {
            SceneManager.LoadScene(mainSceneName);
        }
        else
        {
            Debug.Log("���� �� �̸��� ������ �����Դϴ�.");
        }
        Debug.Log("���θ޴��̵�");
    }
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("����");
    }
}
