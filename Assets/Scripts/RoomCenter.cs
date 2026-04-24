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

        // Привязываем всех врагов этой секции к конкретной комнате, чтобы
        // они не реагировали на игрока, пока он не вошёл именно сюда.
        if (theRoom != null)
        {
            EnemyController[] centerEnemies = GetComponentsInChildren<EnemyController>(true);
            foreach (EnemyController enemy in centerEnemies)
            {
                if (enemy != null)
                {
                    enemy.homeRoom = theRoom;
                }
            }
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
