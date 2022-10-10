using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainManager : MonoBehaviour
{
    public static MainManager instance;
    //¹Ì´Ï¸ÊÀÇ °¡·Î, ¼¼·Î Ä­ ¼ö
    public int vertical = 3, horizontal = 3;
    public Scene[,] sceness;
    public Scene[] scenes;
    public Spawner spawner;



    int playerHP;
    Vector2 mapStart, mapEnd;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        //if(spawner == null)
        //{
        //    spawner = FindObjectOfType<Spawner>();
        //}
        instance = this;
        scenes = new Scene[vertical * horizontal - 1];
        for (int i = 0; i < vertical * horizontal - 1; i++)
        {
            scenes[i] = SceneManager.GetSceneByBuildIndex(i);
            
            Debug.Log($"{scenes[i].name} = {SceneManager.GetSceneByBuildIndex(i).name}");
        }
        DontDestroyOnLoad(gameObject);

    }
}