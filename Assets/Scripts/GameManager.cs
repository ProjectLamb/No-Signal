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

    public bool IsDeerClear = false;
    public bool IsCargateClear = false;
    public bool IsCargateEvent = false;
    public bool IsTrafficClear = false;
    public bool IsTutorial = true;
    public bool IsTutorialFirst = true;
    public bool IsChaseEvent = false;

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
        IsTutorial = true;
    }

}
