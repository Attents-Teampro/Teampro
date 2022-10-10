using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal_Down : MonoBehaviour
{
    Player player;

    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
            Debug.Log("teleported");
            //player.transform.position = new Vector3(0, 0, (float)1.6);
        }

    }
}
