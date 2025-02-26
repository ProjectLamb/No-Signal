using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{

    private Vector2 lastMousePos;

    public float mouseX;
    public float mouseY;
    public float rotX = 0f;
    public float rotY;
    public float sensitivity = 400f;

    public Transform playerTr;

    void Start()
    {
        lastMousePos = Mouse.current.position.ReadValue();
    }
    void Update()
    {
        transform.position = playerTr.position;
        FirstPersonLook();
    }
    
    void FirstPersonLook()
    {
        Vector2 delta = Mouse.current.delta.ReadValue() * sensitivity;

        mouseX = delta.x;
        mouseY = delta.y;

        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotX,0f,0f);

        playerTr.Rotate(Vector3.up * mouseX);
    }
}
