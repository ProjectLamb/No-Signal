using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    public Image fadeImage;

    public void LoadGame()
    {
        SceneManager.LoadScene("kabocha");
    }

    public void FadeIn()
    {
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
}
