using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour
{
    public GameObject car;
    public GameObject minimapMark;

    void Update()
    {
        GPSFollow();
    }

    void GPSFollow()
    {
        minimapMark.transform.SetParent(car.transform);
        minimapMark.transform.position = new Vector3(car.transform.position.x, car.transform.position.y + 20f, car.transform.position.z);
        minimapMark.transform.rotation = car.transform.rotation;
    }
}
