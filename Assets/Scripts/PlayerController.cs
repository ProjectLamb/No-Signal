using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    private bool IsMove;
    private Vector3 moveDirection;
    private Rigidbody myRigid;
    private float moveSpeed = 4f;


    void Awake()
    {
        myRigid = GetComponent<Rigidbody>();
        if(myRigid != null) Debug.Log("성공!");
    }

    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        if (input != null)
        {
            moveDirection = new Vector3(input.x, 0f, input.y);
        }
    }

    void Move()
    {
        if(moveDirection != Vector3.zero)
        {
           myRigid.velocity = moveDirection * moveSpeed + Vector3.up * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        Move();
    }
}
