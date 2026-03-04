using System.Collections;
using System.Collections.Generic;   
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public float moveSpeed;

    public float rangeToChasePlayer;
    private Vector3 moveDirection;

    public Animator anim;

    public int health = 150;

    public GameObject[] deathSplatters;

    public GameObject hitEffect; 




    void Start()
    {

    }

    void Update()
    {
        if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer)
        {
            moveDirection = PlayerController.instance.transform.position - transform.position;
        }else
        {
            moveDirection = Vector3.zero; 
        }

        moveDirection.Normalize();

        theRB.linearVelocity = moveDirection * moveSpeed;

        if (moveDirection != Vector3.zero) //animation setup 
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }



    }

    public void DamageEnemy(int damage)
    {
        health -= damage;

        Instantiate(hitEffect, transform.position, transform.rotation); 

        if(health <= 0)
        {
            Destroy(gameObject);

            int selectedSplatter = Random.Range(0, deathSplatters.Length);

            int rotation = Random.Range(0, 4); 

            Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, rotation * 90f)); 

            //Instantiate(deathSplatters, transform.position, transform.rotation); 
        }
    }
}
