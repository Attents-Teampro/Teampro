using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break_Ghost : MonoBehaviour
{
    // 벌룬 에셋에서 변형된 스크립트

    public bool Is_Breaked = false;
    public GameObject ghost_normal;
    public GameObject ghost_Parts;
    public Animator ghost;
    int counter;

    public Transform player;
    

    void Start()
    {
        ghost_normal.SetActive(true);
        ghost_Parts.SetActive(false);
    }

    
    void Update()
    {
        transform.LookAt(player);
        if(Is_Breaked == true)
        {
            ghost_Parts.SetActive(true);
            ghost_normal.SetActive(false);
        }
        
    }

    public void break_Ghost()
    {
        Is_Breaked = true;
    }
    public void play_anim()
    {
        counter += 1;
        if(counter == 2)
        {
            counter = 0;
            ghost.Play("idle");
        }
        else
        {
            ghost.Play("attack");
        }
    }
}
