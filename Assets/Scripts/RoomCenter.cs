using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{

    public bool openWhenEnemiesCleared; 

    public List<GameObject> enemies = new List<GameObject>();

    public Room theRoom; 
    void Start()
    {
        if(openWhenEnemiesCleared)
        {
            theRoom.closeWhenEntered = true; 
        }
    }

    void Update()
    {
        if (enemies.Count > 0 && openWhenEnemiesCleared && !theRoom.roomCleared)
        {
            for (int i = 0; i < enemies.Count; i++)
            {

                if (enemies[i] == null || !enemies[i].activeInHierarchy)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }
        }


        if (openWhenEnemiesCleared && !theRoom.roomCleared && enemies.Count == 0)
        {
            theRoom.OpenDoors();
        }
    }
}
