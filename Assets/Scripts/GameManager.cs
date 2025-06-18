using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    public GameObject car;

    public Transform deerSpawn;
    public Transform cargateSpawn;
    public Transform traffitSpawn;
    public Transform creatureSpawn;
    public static bool IsDeerClear = false;
    public static bool IsCargateClear = false;
    public static bool IsTrafficClear = false;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        if (IsDeerClear)
        {
            car.transform.position = traffitSpawn.position;
            car.transform.rotation = traffitSpawn.rotation;
        }
        if (IsCargateClear)
        {
            car.transform.position = deerSpawn.position;
            car.transform.rotation = deerSpawn.rotation;
        }
        if (IsTrafficClear)
        {
            car.transform.position = creatureSpawn.position;
            car.transform.rotation = creatureSpawn.rotation;
        }
    }
}
