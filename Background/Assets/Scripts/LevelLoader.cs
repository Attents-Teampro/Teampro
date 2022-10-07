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
        // �� �� ���� buildIndex ���ڰ� �ִ�. ù��°�� 0, �� ������ 1. +1�̸��� �� ���� ������ �Ѿ�ٴ� �ǹ�
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
