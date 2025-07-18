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
        vCam2.m_Lens.FieldOfView = 30f;
    }
    public void ChangeFOV()
    {
        StartCoroutine("FOVDown");
    }

    IEnumerator FOVDown()
    {
        if (vCam1.m_Lens.FieldOfView > 30f)
        {
            vCam1.m_Lens.FieldOfView -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
