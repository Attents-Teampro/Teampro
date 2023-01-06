using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{
    Player player;

    float speed;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //speed = player.moveSpeed;
            //player.moveSpeed = 2.5f;
            player.moveSpeed = 2.5f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //player.moveSpeed = speed;
            player.moveSpeed = 5.0f;
        }
    }

}
