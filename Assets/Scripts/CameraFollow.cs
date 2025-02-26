using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public float moveSmoothness;
    public float rotSmoothness;

    public Vector3 moveOffset;
    public Vector3 rotOffset;
    public Vector2 mousePos;
    private Vector2 lastMousePos;

    public float mouseX;
    public float mouseY;
    public float rotX = 0f;
    public float rotY;
    public float sensitivity = 400f;

    public Transform target;


    void Start()
    {
        lastMousePos = Mouse.current.position.ReadValue();
    }
    void Update()
    {
        //HandleMove();
        //HandleRot();

        transform.position = target.position;
        FirstPersonLook();
    }

    void HandleMove()
    {
        Vector3 targetPos = new Vector3();
        targetPos = target.transform.TransformPoint(moveOffset);

        transform.position = Vector3.Lerp(transform.position, targetPos, moveSmoothness * Time.deltaTime);
    }

    void HandleRot()
    {
        var dir = target.transform.position - transform.position;
        var rot = new Quaternion();

        rot = Quaternion.LookRotation(dir + rotOffset, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotSmoothness * Time.deltaTime);
    }
    
    void FirstPersonLook()
    {
        Vector2 delta = Mouse.current.delta.ReadValue() * sensitivity;

        mouseX = delta.x;
        mouseY = delta.y;

        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotX,0f,0f);

        target.Rotate(Vector3.up * mouseX);
    }
}
