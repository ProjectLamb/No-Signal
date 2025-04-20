using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarCam : MonoBehaviour
{

    private Vector2 lastMousePos;
    private float mouseX;
    private float mouseY;
    private float rotY;

    public float sensitivity;
    public Transform car;



    void Start()
    {
        lastMousePos = Mouse.current.position.ReadValue();
    }
    // Update is called once per frame
    void Update()
    {   
        
        CarLook();
    }

    void CarLook()
    {
        this.transform.position = this.car.position;
        Vector2 delta = Mouse.current.delta.ReadValue() * sensitivity;

        mouseX = delta.x;
        mouseY = delta.y;

        rotY += mouseX;
        rotY = Mathf.Clamp(rotY, -45f, 45f);

        transform.localRotation = Quaternion.Euler(0f, rotY, 0f);

        car.Rotate(Vector3.up * mouseY);
    }
}
