using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeFov : MonoBehaviour
{

    private CinemachineVirtualCamera vCam;
    public Camera cam;

    public void Awake()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void Start()
    {
        if (this.gameObject.name == "DeathCam")
        {
            ChangeFOV();
            cam.fieldOfView = 30f;
        }
        else
        {
            vCam.m_Lens.FieldOfView = 30f;
            cam.fieldOfView = 30f;
        }
    }
    public void ChangeFOV()
    {
        StartCoroutine("FOVDown");
    }

    IEnumerator FOVDown()
    {
        if (vCam.m_Lens.FieldOfView > 30f)
        {
            vCam.m_Lens.FieldOfView -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
