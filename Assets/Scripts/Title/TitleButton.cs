using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class TitleButton : MonoBehaviour
{
    public Image fadeImage;
    private EventInstance title;

    void Awake()
    {
        title = AudioManager.instance.CreateInstance(FMODEvents.instance.title);
    }

    void Start()
    {
        title.start();
        StartCoroutine("CoFadeOut");
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("kabocha");
    }

    public void FadeIn()
    {
        title.stop(STOP_MODE.ALLOWFADEOUT);
        fadeImage.gameObject.SetActive(true);
        StartCoroutine("CoFadeIn");
    }

    IEnumerator CoFadeIn()
    {
        float fadeAlp = 0;
        Color fadeCol = fadeImage.color;
        while (fadeCol.a < 1f)
        {
            fadeCol.a += 0.05f;
            fadeImage.color = fadeCol;
            yield return new WaitForSeconds(0.01f);
        }
        LoadGame();
    }

    IEnumerator CoFadeOut()
    {
        yield return new WaitForSeconds(0.1f);
        float fadeAlp = 1;
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
