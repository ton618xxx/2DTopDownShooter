using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool closeWhenEntered /* , openWhenEnemiesCleared */;

    public GameObject[] doors;

    //public List <GameObject> enemies = new List<GameObject> ();

    [HideInInspector]
    public bool roomCleared;

    [HideInInspector]
    public bool playerInRoom;

    private bool isClosingDoorsCoroutineRunning;

    void Start()
    {
        // Привязываем всех врагов-потомков к этой комнате, чтобы они
        // активировались только когда игрок находится именно в этой секции.
        EnemyController[] childEnemies = GetComponentsInChildren<EnemyController>(true);
        foreach (EnemyController enemy in childEnemies)
        {
            if (enemy != null && enemy.homeRoom == null)
            {
                enemy.homeRoom = this;
            }
        }
    }

    void Update()
    {

    }

    public void OpenDoors()
    {
        if (roomCleared)
        {
            return;
        }

        roomCleared = true;

        foreach (GameObject door in doors)
        {
            door.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            playerInRoom = true;

            CameraController.instance.ChangeTarget(transform);

            // Если комната уже зачищена, убеждаемся, что двери открыты, и выходим.
            if (roomCleared)
            {
                foreach (GameObject door in doors)
                {
                    door.SetActive(false);
                }
                return;
            }

            // Закрываем двери ТОЛЬКО после того, как игрок отошёл от них
            // на безопасное расстояние. Это нужно, чтобы дверь не появилась
            // прямо на игроке и не вытолкнула его обратно в коридор.
            if (closeWhenEntered && !isClosingDoorsCoroutineRunning)
            {
                StartCoroutine(CloseDoorsSafely());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerInRoom = false;
        }
    }

    private IEnumerator CloseDoorsSafely()
    {
        isClosingDoorsCoroutineRunning = true;

        const float maxWaitTime = 0.75f;
        float timer = 0f;

        // Ждём, пока игрок либо отойдёт от всех дверей, либо выйдет из комнаты,
        // либо сработает таймаут (страховка, чтобы двери не остались открытыми навсегда).
        while (playerInRoom && !roomCleared && IsPlayerNearAnyDoor() && timer < maxWaitTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (playerInRoom && !roomCleared)
        {
            foreach (GameObject door in doors)
            {
                door.SetActive(true);
            }
        }

        isClosingDoorsCoroutineRunning = false;
    }

    private bool IsPlayerNearAnyDoor()
    {
        if (PlayerController.instance == null)
        {
            return false;
        }

        Vector2 playerPos = PlayerController.instance.transform.position;
        // Запас с учётом радиуса коллайдера игрока (~0.38) + небольшой буфер.
        const float playerSafeRadius = 0.6f;

        foreach (GameObject door in doors)
        {
            if (door == null)
            {
                continue;
            }

            BoxCollider2D col = door.GetComponent<BoxCollider2D>();
            if (col == null)
            {
                continue;
            }

            Vector2 doorCenter = (Vector2)door.transform.position + col.offset;
            Vector2 doorHalfSize = col.size * 0.5f;

            if (playerPos.x >= doorCenter.x - doorHalfSize.x - playerSafeRadius &&
                playerPos.x <= doorCenter.x + doorHalfSize.x + playerSafeRadius &&
                playerPos.y >= doorCenter.y - doorHalfSize.y - playerSafeRadius &&
                playerPos.y <= doorCenter.y + doorHalfSize.y + playerSafeRadius)
            {
                return true;
            }
        }

        return false;
    }
}
