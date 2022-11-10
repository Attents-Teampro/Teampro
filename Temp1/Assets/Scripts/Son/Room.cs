using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Spawner[] spawners;
    public List<Door> door = new List<Door>(3);
    public Vector3 roomPosition;

    public List<Door> getDoor => door;

    public void StartSpawn()
    {
        foreach(var i in spawners)
        {
            i.StartSpawn();
        }
        
        MainManager.instance.onClearthisRoom += OpenAllDoor;
    }

    void OpenAllDoor()
    {
        foreach(var i in door)
        {
            i.Clear();
        }
        MainManager.instance.onClearthisRoom -= OpenAllDoor;
    }

}
