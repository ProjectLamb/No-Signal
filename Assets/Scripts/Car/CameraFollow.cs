using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public Transform carTarget;

    public float mouseSensitivity = 0.5f;
    public float maxYawAngle = 20f;

    private float currentYaw = 0f;

    public static bool isEvent = false; // 이벤트 제어용 변수 추가
    public static Quaternion fixedRotation = Quaternion.identity;
    private Camera carCam;
    private CinemachineBrain cinemachineBrain;

    void Awake()
    {
        carCam = GetComponent<Camera>();
        cinemachineBrain = GetComponent<CinemachineBrain>();
    }
    void Start()
    {
        FollowTarget();
    }

    void Update()
    {
        if (GameManager.Instance.IsTutorial || GameManager.Instance.IsDeathEvent) return;

        if (!BoomGateEventTrigger.isBoomEvent && !isEvent)
        {
            FollowTarget();
        }
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
        currentYaw = Mathf.Clamp(currentYaw + deltaX, -maxYawAngle, maxYawAngle + 40); // ���� + ����

        Quaternion baseRot = carTarget.rotation * Quaternion.Euler(rotationOffset);
        Quaternion yawRot = Quaternion.Euler(0f, currentYaw, 0f);

        transform.rotation = baseRot * yawRot;
    }

    public void TurnOffBrain()
    {
        cinemachineBrain.enabled = false;
        carCam.fieldOfView = 30f;
    }
}
