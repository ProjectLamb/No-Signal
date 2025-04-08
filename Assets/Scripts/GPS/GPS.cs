using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour
{
    public Transform playerBody;
    public GameObject car;
    public GameObject minimapMark;
    public CarRide carRideSystem;

    void Update()
    {
        if(carRideSystem.IsRide)
        {
            minimapMark.transform.SetParent(car.transform);
            minimapMark.transform.position = new Vector3(car.transform.position.x, car.transform.position.y + 20f, car.transform.position.z);
        }
        else if(!carRideSystem.IsRide)
        {
            minimapMark.transform.SetParent(playerBody);
            minimapMark.transform.position = new Vector3(playerBody.position.x, playerBody.position.y + 20f, playerBody.position.z);
        }
    }
}
