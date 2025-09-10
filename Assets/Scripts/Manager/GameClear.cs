using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{

    void Start()
    {
        Cursor.visible = true;
    }
    public void BackToTitle()
    {
        CarController.IsGameOver = false;
        SceneManager.LoadScene("Title");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
