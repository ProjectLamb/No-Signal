using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{

    public Transform deerSpawn;
    public Transform cargateSpawn;
    public Transform traffitSpawn;
    public Transform creatureSpawn;
    public GameObject deerEvent;
    public GameObject car;
    // Start is called before the first frame update
    void Start()
    {
        CarController.IsGameOver = false;
        if (GameManager.Instance.IsDeerClear)
        {
            car.transform.position = traffitSpawn.position;
            car.transform.rotation = traffitSpawn.rotation;
        }
        if (GameManager.Instance.IsCargateClear)
        {
            car.transform.position = deerSpawn.position;
            car.transform.rotation = deerSpawn.rotation;
        }
        if (GameManager.Instance.IsTrafficClear)
        {
            car.transform.position = creatureSpawn.position;
            car.transform.rotation = creatureSpawn.rotation;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsDeerClear)
        {
            deerEvent.SetActive(false);
        }

        if (GameManager.Instance.IsTrafficClear)
        {
            deerEvent.SetActive(false);
        }
    }
}
