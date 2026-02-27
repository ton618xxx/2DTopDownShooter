using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public float moveSpeed;

    public float rangeToChasePlayer;
    private Vector3 moveDirection;



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


    }
}
