using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverFade : MonoBehaviour
{
    public Image fadeImage;
    public void Start()
    {
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
        SceneManager.LoadScene("Death");
    }
}
