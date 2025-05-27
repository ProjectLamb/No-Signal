using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreenManager : MonoBehaviour
{
    private static GameScreenManager instance;
    public static GameScreenManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameScreenManager>();
            }
            return instance;
        }
    }
    public bool IsFullScreen;

    void Awake()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<GameScreenManager>(); ;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(instance.gameObject);
    }

    void Start()
    {
        if (GameScreenManager.Instance.IsFullScreen) SetFullScreen();
        else SetWindowScreen();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Return) && GameScreenManager.Instance.IsFullScreen)
        {
            GameScreenManager.Instance.IsFullScreen = false;
            SetWindowScreen();
        }
        else if (Input.GetKeyDown(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Return) && !GameScreenManager.Instance.IsFullScreen)
        {
            GameScreenManager.Instance.IsFullScreen = true;
            SetFullScreen();
        }
    }

    void SetWindowScreen()
    {
        Screen.SetResolution(1200, 740, false);
    }

    void SetFullScreen()
    {
        Screen.SetResolution(1920, 1080, true);
    }
}
