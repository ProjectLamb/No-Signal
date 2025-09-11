using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{

    public GameObject pausePanel;
    public GameObject buttons;
    private float gameScale = 1;
    void Update()
    {
        if (GameManager.Instance.IsTutorial || GameManager.Instance.IsDeathEvent) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseOn();
        }

        Time.timeScale = gameScale;
    }
    public void PauseOn()
    {
        GameManager.Instance.IsGamePaused = true;
        AudioManager.Instance.PauseOn();
        pausePanel.SetActive(true);
        buttons.SetActive(true);
        gameScale = 0;
    }

    public void PauseOff()
    {
        GameManager.Instance.IsGamePaused = false;
        AudioManager.Instance.PauseOff();
        pausePanel.SetActive(false);
        buttons.SetActive(false);
        gameScale = 1;
    }

    public void ExitGame()
    {
        GameManager.Instance.IsGamePaused = false;
        gameScale = 1;
        SceneManager.LoadScene("Title");
    }

}
