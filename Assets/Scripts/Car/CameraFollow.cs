using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public static Transform carTarget;

    public float mouseSensitivity = 2f;
    private float mouseX = 0f;
    private float mouseY = 0f;
    private float rotY;

    public static bool isEvent = false; // 이벤트 제어용 변수 추가
    public static Quaternion fixedRotation = Quaternion.identity;

    void Start()
    {
        carTarget = GameObject.Find("CarTr").transform;
    }

    void LateUpdate()
    {
        if (!BoomGateEventTrigger.isBoomEvent && !isEvent)
        {
            FollowTarget();
        }

    }

    void FollowTarget()
    {
        HandlePosition();
        //HandleRotation();
        CarCamRotate();
    }

    void HandlePosition()
    {
        transform.position = carTarget.TransformPoint(positionOffset);
    }

    void HandleRotation()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;

        Quaternion baseRot = carTarget.rotation * Quaternion.Euler(rotationOffset);
        Quaternion mouseRot = Quaternion.Euler(0f, mouseX, 0f);

        transform.rotation = baseRot * mouseRot;
    }

    void CarCamRotate()
    {
        Vector2 delta = Mouse.current.delta.ReadValue() * mouseSensitivity;

        mouseX = delta.x;
        mouseY = delta.y;

        rotY += mouseX;
        rotY = Mathf.Clamp(rotY, -30f, 30f);

        transform.localRotation = Quaternion.Euler(0f, rotY, 0f);

        carTarget.Rotate(Vector3.up * mouseY);
    }
}
