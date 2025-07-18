using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeFov : MonoBehaviour
{

    public CinemachineVirtualCamera vCam1;
    public CinemachineVirtualCamera vCam2;
    public Camera cam;

    public void Start()
    {
        vCam2.m_Lens.FieldOfView = 22f;
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
        if (vCam2.m_Lens.FieldOfView > 10f)
        {
            vCam2.m_Lens.FieldOfView -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
