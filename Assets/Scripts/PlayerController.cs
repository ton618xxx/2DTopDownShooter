using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    public float moveSpeed;
    private Vector2 moveInput;
   

    public Rigidbody2D theRB;

    public Transform gunArm;

    private Camera theCam; 

    void Start()
    {
        theCam = Camera.main; 
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        //transform.position += new Vector3(moveInput.x * Time.deltaTime * moveSpeed, moveInput.y * Time.deltaTime * moveSpeed, 0f);

        theRB.linearVelocity = moveInput * moveSpeed;


        Vector3 mousePos =  Input.mousePosition;
        Vector3 screenPoint = theCam.WorldToScreenPoint(transform.localPosition);

        if(mousePos.x < screenPoint.x)
        {
            transform.localScale = new Vector3(-1f,1f,1f);
            gunArm.localScale = new Vector3(-1f, -1f, 1f); 
        }else
        {
            transform.localScale = Vector3.one;
            gunArm.localScale = Vector3.one; 
        }

        //rotate gun arm
        Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        gunArm.rotation = Quaternion.Euler(0,0,angle);  
    }


}
