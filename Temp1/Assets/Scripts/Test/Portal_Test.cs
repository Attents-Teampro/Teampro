using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    // Portal_Up = 1
    // Portal_Down = 2
    // Portal_Left = 3
    // Portal_Right = 4



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
            Debug.Log("teleported");
        }
        
    }

}
