using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal_Lobby : MonoBehaviour
{
    Player player;

    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            SceneManager.LoadScene("Test");
            Debug.Log("teleported");
            //player.transform.position = new Vector3(0, 0, (float)1.6);
        }

    }
}
