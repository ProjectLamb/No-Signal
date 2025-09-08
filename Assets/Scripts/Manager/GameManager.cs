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
                Debug.LogError("No GameManagerInstance");
            }
            return instance;
        }
    }

    public bool IsCargateEvent = false;

    public bool IsTutorial = true;
    public bool IsTutorialFirst = true;
    public bool IsJunctionEvent = false;
    public bool IsRushToTree = false;
    public bool IsDeathEvent = false;
    public bool IsEnding = false;
    public bool IsGamePaused = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        CarController.IsGameOver = false;
        IsTutorial = true;
    }

}
