using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool closeWhenEntered, openWhenEnemiesCleared; 

    public GameObject[] doors;

    public List <GameObject> enemies = new List<GameObject> ();


    private bool roomCleared;

    void Start()
    {
        
    }

    void Update()
    {
        if(enemies.Count > 0 && openWhenEnemiesCleared)
        {
            for(int i = 0;  i < enemies.Count; i++)
            {

                if(enemies[i] == null || !enemies[i].activeInHierarchy)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }
        }


        if(!roomCleared && openWhenEnemiesCleared && enemies.Count == 0)
        {
            roomCleared = true;

            foreach (GameObject door in doors)
            {
                door.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            CameraController.instance.ChangeTarget(transform);

            // Закрываем двери только если комната еще не была зачищена
            if (closeWhenEntered && !roomCleared)
            {
                foreach(GameObject door in doors)
                {
                    door.SetActive(true);
                }
            }

            // Если комната уже зачищена, убедимся, что двери открыты
            if (roomCleared)
            {
                foreach (GameObject door in doors)
                {
                    door.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

    }
}
