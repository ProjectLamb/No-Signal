using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniCam : MonoBehaviour
{

    private Vector2 lastMousePos;
    private float mouseX;
    private float mouseY;
    private float rotX = 0f;
    private float rotY;

    public float sensitivity;
    public Transform playerTr;

    void Start()
    {
        lastMousePos = Mouse.current.position.ReadValue();
    }
    void Update()
    {
        MiniCamFollow();
    }
    
    void MiniCamFollow()
    {
        //카메라 회전
        Vector2 delta = Mouse.current.delta.ReadValue() * sensitivity;

        mouseX = delta.x;
        mouseY = delta.y;

        rotY += mouseX;

        transform.localRotation = Quaternion.Euler(90f,rotY + 180f,0f);

        //카메라 플레이어 추적
        transform.position = new Vector3(playerTr.position.x , 30f, playerTr.position.z);
    }
}
