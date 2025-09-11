using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeFov : MonoBehaviour
{
    public CinemachineVirtualCamera vCam1;
    public CinemachineVirtualCamera vCam2;
    public Camera cam;
    private float focalLength;

    void Start()
    {
        if (CarController.IsAFCGameOver)
        {
            vCam1.m_Lens.FarClipPlane = 30f;
            vCam2.m_Lens.FarClipPlane = 30f;
        }
    }

    public void ChangeFOV()
    {
        StartCoroutine("FOVDown");
        vCam2.m_Lens.FieldOfView = 22f;
    }

    public void ChangeFOV2()
    {
        StartCoroutine("FOVDown2");
    }

    IEnumerator FOVDown()
    {
        cam.fieldOfView = 22f;
        if (vCam1.m_Lens.FieldOfView > 22f)
        {
            vCam1.m_Lens.FieldOfView -= 0.5f;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator FOVDown2()
    {
        focalLength = FOVToFocalLength(vCam2);
        while (focalLength < 4f)
        {
            focalLength += 0.3f;
            vCam2.m_Lens.FieldOfView = FocalLengthToFOV(vCam2, focalLength);
            yield return new WaitForSeconds(0.01f);
        }
    }

    // FOV(수직 기준) -> Focal Length(mm)
    private float FOVToFocalLength(CinemachineVirtualCamera vcam)
    {
        return vcam.m_Lens.SensorSize.y * 0.5f / Mathf.Tan(Mathf.Deg2Rad * vcam.m_Lens.FieldOfView * 0.5f);
    }

    // Focal Length(mm) -> FOV(수직 기준)
    private float FocalLengthToFOV(CinemachineVirtualCamera vcam, float focalLength)
    {
        if (focalLength < 0.001f)
            return 180f;
        return Mathf.Rad2Deg * 2.0f * Mathf.Atan(vcam.m_Lens.SensorSize.y * 0.5f / focalLength);
    }

}
