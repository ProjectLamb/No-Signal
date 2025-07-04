using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{

    public CinemachineVirtualCamera vcam;
    public float intensity = 0.5f;
    private CinemachineBasicMultiChannelPerlin camNoise;

    private bool IsShake = false;
    private bool IsCamOn = false;


    void Awake()
    {
        //vcam = GetComponent<CinemachineVirtualCamera>();
        camNoise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Start()
    {
        camNoise.m_AmplitudeGain = 0f;
    }
    // Update is called once per frame
    void Update()
    {
        if (IsShake && IsCamOn) ShakeCam();
        if (!IsShake && IsCamOn) ResetCam();
    }

    void ShakeCam()
    {
        camNoise.m_AmplitudeGain = intensity;
    }

    void ResetCam()
    {
        camNoise.m_AmplitudeGain = 0f;
    }

    public void CamOn()
    {
        IsCamOn = true;
    }

    public void CamOff()
    {
        IsCamOn = false;
    }

    public void ShakeOn()
    {
        IsShake = true;
    }

    public void ShakeOff()
    {
        IsShake = false;
    }
}
