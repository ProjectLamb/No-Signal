using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using TMPro;

public class TitleButton : MonoBehaviour
{
    public Image fadeImage;
    public TextMeshProUGUI startGameText;
    public GameObject ContinueText;

    private EventInstance title;

    void Awake()
    {
        title = AudioManager.Instance.CreateInstance(FMODEvents.instance.title);
    }

    void Start()
    {

        Cursor.visible = true;
        if (!SaveLoadManager.Instance.IsTrafficClear && !SaveLoadManager.Instance.IsCargateClear && !SaveLoadManager.Instance.IsDeerClear && !SaveLoadManager.Instance.IsChaseEvent)
        {
            startGameText.text = "Start Game";
            ContinueText.SetActive(false);
        }
        else
        {
            startGameText.text = "New Game";
            ContinueText.SetActive(true);
        }
        title.start();
        StartCoroutine("CoFadeOut");
    }
    public void NewGame()
    {
        SaveLoadManager.Instance.CleanUp();
        GameManager.Instance.IsTutorialFirst = true;
        GameManager.Instance.IsTutorial = true;

        FadeIn();
    }

    public void Continue()
    {
        GameManager.Instance.IsTutorialFirst = false;
        FadeIn();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void FadeIn()
    {
        title.stop(STOP_MODE.ALLOWFADEOUT);
        fadeImage.gameObject.SetActive(true);
        StartCoroutine("CoFadeIn");
    }

    IEnumerator CoFadeIn()
    {
        Color fadeCol = fadeImage.color;
        while (fadeCol.a < 1f)
        {
            fadeCol.a += 0.05f;
            fadeImage.color = fadeCol;
            yield return new WaitForSeconds(0.01f);
        }
        StopCoroutine("CoFadeOut");
        SceneManager.LoadScene("kabocha");
    }

    IEnumerator CoFadeOut()
    {
        yield return new WaitForSeconds(0.1f);
        Color fadeCol = fadeImage.color;
        while (fadeCol.a > 0f)
        {
            fadeCol.a -= 0.02f;
            fadeImage.color = fadeCol;
            yield return new WaitForSeconds(0.02f);
        }
        fadeImage.gameObject.SetActive(false);
    }
}
