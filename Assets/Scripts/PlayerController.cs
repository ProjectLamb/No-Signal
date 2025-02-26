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


    void Awake()
    {
        myRigid = GetComponent<Rigidbody>();
        playerAnim = GetComponentInChildren<Animator>();
    }

    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        // if (input != null)
        // {
        //     moveDirection = new Vector3(input.x, 0f, input.y);
        // }

        if (input != Vector2.zero)
        {
            //카메라의 전방과 우측 벡터 가져오기 (y축 이동 제거)
            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;
            forward.y = 0f;
            right.y = 0f;

            // 입력 벡터를 카메라 기준으로 변환
            moveDirection = (forward * input.y + right * input.x).normalized;
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
            // myRigid.velocity = moveDirection * moveSpeed + Vector3.up * Time.deltaTime;
            myRigid.velocity = moveDirection * moveSpeed + Vector3.up * myRigid.velocity.y;
            playerAnim.SetBool("IsWalk", true);
        }
        else playerAnim.SetBool("IsWalk", false);
    }

    void FixedUpdate()
    {
        Move();
    }
}
