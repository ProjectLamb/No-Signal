using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMOD.Studio;

public class PlayerController : MonoBehaviour
{
    private Vector3 moveDirection;
    private Rigidbody myRigid;

    private float moveSpeed = 4f;
    private bool IsMove;

    private Animator playerAnim;
    private Vector2 input2;

    private EventInstance playerFootsteps;


    void Awake()
    {
        myRigid = GetComponent<Rigidbody>();
        playerAnim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        playerFootsteps = AudioManager.instance.CreateInstance(FMODEvents.instance.playerFootSteps);
    }

    void FixedUpdate()
    {
        Move();
        Look();
        UpdateSound();
    }

    void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        input2 = input;

        if (input2 == Vector2.zero)
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
        else
        {
            playerAnim.SetBool("IsWalk", false);
        }
    }

    void Look()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0f;
        right.y = 0f;


        // 입력 벡터를 카메라 기준으로 변환
        moveDirection = (forward * input2.y + right * input2.x).normalized;

        if (input2 == Vector2.zero)
        {
            moveDirection = Vector3.zero;
        }
    }

    private void UpdateSound()
    {
        if (input2 != Vector2.zero)
        {
            PLAYBACK_STATE playbackState;
            playerFootsteps.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerFootsteps.start();
            }
        }
        else
        {
            playerFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }

}
