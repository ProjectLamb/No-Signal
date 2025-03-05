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
    private Animator playerAnim;
    private Vector2 input2;


    void Awake()
    {
        myRigid = GetComponent<Rigidbody>();
        playerAnim = GetComponentInChildren<Animator>();
    }

    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        if (input != Vector2.zero)
        {
            input2 = input;
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }

    void Move()
    {
        if (moveDirection != Vector3.zero)
        {
            myRigid.velocity = moveDirection * moveSpeed + Vector3.up * myRigid.velocity.y;
            playerAnim.SetBool("IsWalk", true);
        }
        else playerAnim.SetBool("IsWalk", false);
    }

    void Look()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0f;
        right.y = 0f;

        // 입력 벡터를 카메라 기준으로 변환
        moveDirection = (forward * input2.y + right * input2.x).normalized;
    }

    void FixedUpdate()
    {
        Move();
        Look();
    }
}
