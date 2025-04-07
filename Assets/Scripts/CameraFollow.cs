using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public Transform carTarget;

    public float mouseSensitivity = 2f;
    private float mouseX = 0f;

    void LateUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        HandlePosition();
        HandleRotation();
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
}
