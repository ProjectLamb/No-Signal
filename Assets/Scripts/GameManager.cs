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
            if (instance == null)
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
    public GameObject deerEvent;
    public bool IsDeerClear = false;
    public bool IsCargateClear = false;
    public bool IsTrafficClear = false;
    public bool IsTutorial = true;
    public bool IsTutorialFirst = true;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        IsTutorial = true;
        CarController.IsGameOver = false;
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

    void Update()
    {
        if (IsDeerClear)
        {
            deerEvent.SetActive(false);   
        }
    }
}
