using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniCam : MonoBehaviour
{

    private Vector2 lastMousePos;
    private float mouseX;
    private float mouseY;
    private float rotY;

    public float sensitivity;
    public Transform target;

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

        //카메라 플레이어 추적
        transform.position = new Vector3(target.position.x , target.position.y + 250f, target.position.z);
    }
}
