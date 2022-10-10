using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal_Right : MonoBehaviour
{
    Player player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("teleported");

            
            //player.transform.position = new Vector3((float)-4.5, 0, 0);
            
            
        }

    }
}
