using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public Transform carTarget;

    public float mouseSensitivity = 2f;
    public float maxYawAngle = 10f;

    private float currentYaw = 0f; // 현재 회전 각도

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
        float deltaX = Input.GetAxis("Mouse X") * mouseSensitivity;
        currentYaw = Mathf.Clamp(currentYaw + deltaX, -maxYawAngle, maxYawAngle+40); // 누적 + 제한

        Quaternion baseRot = carTarget.rotation * Quaternion.Euler(rotationOffset);
        Quaternion yawRot = Quaternion.Euler(0f, currentYaw, 0f);

        transform.rotation = baseRot * yawRot;
    }
}
