using System;
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

    public float shakeAmount = 0f; // 흔들림 강도
    public float shakeSpeed = 0.001f;
    public static float carSpeed;
    public static Action stopCam;

    private float currentYaw = 0f;
    private Vector3 originalPos;

    public static bool IsEvent = false; // 이벤트 제어용 변수 추가
    public static Quaternion fixedRotation = Quaternion.identity;
    private Camera carCam;
    private CinemachineBrain cinemachineBrain;



    void Awake()
    {
        carCam = GetComponent<Camera>();
        cinemachineBrain = GetComponent<CinemachineBrain>();

        stopCam = () =>
        {
            StopCam();
        };
    }
    void Start()
    {
        FollowTarget();
        originalPos = carCam.transform.localPosition;
    }

    void Update()
    {
        if (GameManager.Instance.IsGamePaused) return;
        if (GameManager.Instance.IsTutorial || GameManager.Instance.IsDeathEvent) return;

        if (!CarGateEventTrigger.isCargateEvent && !IsEvent)
        {
            FollowTarget();
            CamVibrate();
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

    void CamVibrate()
    {
        // 기존보다 훨씬 작은 값으로 시작
        float shakeAmount = Mathf.Lerp(0.005f, 0.01f, carSpeed / 50f);

        // 간단한 Perlin Noise 기반 흔들림
        float offsetX = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * shakeAmount;
        float offsetY = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * shakeAmount;
        carCam.transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);
    }

    public void ChangeFOV()
    {
        cinemachineBrain.enabled = false;
        carCam.fieldOfView = 22f;
    }
    public void TurnFOV2()
    {
        cinemachineBrain.enabled = false;
        carCam.fieldOfView = 10f;
    }

    public void StopCam()
    {
        float deltaX = Input.GetAxis("Mouse X") * mouseSensitivity;
        currentYaw = Mathf.Clamp(currentYaw + deltaX, -maxYawAngle, maxYawAngle + 40); // ���� + ����

        Quaternion baseRot = carTarget.rotation * Quaternion.Euler(rotationOffset);
        Quaternion yawRot = Quaternion.Euler(0f, 0f, 0f);

        transform.rotation = baseRot * yawRot;
    }
}
