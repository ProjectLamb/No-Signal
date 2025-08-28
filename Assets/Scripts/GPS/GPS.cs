using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour
{
    public GameObject car;
    public GameObject minimapMark;

    public Transform target;              // 타겟 오브젝트
    public Camera minimapCam;

    private bool IsIndicate = false;

    void Update()
    {
        GPSFollow();
        // if (IsIndicate)
        // {
        //     SetIndicator();
        // }
    }

    void SetIndicator()
    {
        Vector3 viewportPos = minimapCam.WorldToViewportPoint(target.position);
        // 타겟이 미니맵 카메라 안에 있음
        bool Isinview = viewportPos.z > 0 &&
                        viewportPos.x >= 0 && viewportPos.x <= 1 &&
                        viewportPos.y >= 0 && viewportPos.y <= 1;
        if (Isinview)
        {
            CarController.IsEndingStart = true;
        }
    }
    void GPSFollow()
    {
        minimapMark.transform.SetParent(car.transform);
        minimapMark.transform.position = new Vector3(car.transform.position.x, car.transform.position.y + 20f, car.transform.position.z);
        minimapMark.transform.rotation = car.transform.rotation;
    }
}
