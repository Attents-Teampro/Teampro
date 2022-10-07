using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {

            LoadNextRoom();

        }
    }

    public void LoadNextRoom()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1)); 
        // 각 씬 마다 buildIndex 숫자가 있다. 첫번째께 0, 그 다음께 1. +1이면은 그 다음 씬으로 넘어간다는 의미
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
