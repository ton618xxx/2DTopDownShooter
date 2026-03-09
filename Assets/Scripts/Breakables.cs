using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Breakables : MonoBehaviour
{
    public GameObject[] brokenPieces;
    public int maxPieces = 5; 

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (PlayerController.instance.dashCounter > 0)
            {
                Destroy(gameObject);

                int piecesToDrop = Random.Range(1, maxPieces);

                for(int i = 0; i < piecesToDrop; i++)
                {
                    int randomPiece = UnityEngine.Random.Range(0, brokenPieces.Length);

                    Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
                }
            }


        }
    }
}
