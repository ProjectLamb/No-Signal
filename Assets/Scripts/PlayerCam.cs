using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
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
        FirstPersonLook();
    }
    
    void FirstPersonLook()
    {
        transform.position = new Vector3(playerTr.position.x, playerTr.position.y + 1.3f, playerTr.position.z);
        Vector2 delta = Mouse.current.delta.ReadValue() * sensitivity;

        mouseX = delta.x;
        mouseY = delta.y;

        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotX,0f,0f);

        playerTr.Rotate(Vector3.up * mouseX);
    }
}
