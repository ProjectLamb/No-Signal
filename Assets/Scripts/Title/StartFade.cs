using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartFade : MonoBehaviour
{

    public Image startFadeImage;

    void Start()
    {
        StartCoroutine("CoFadeOut");
    }

    IEnumerator CoFadeOut()
    {
        float fadeAlp = 1;
        Color fadeCol = startFadeImage.color;
        while (fadeCol.a > 0f)
        {
            fadeCol.a -= 0.05f;
            startFadeImage.color = fadeCol;
            yield return new WaitForSeconds(0.01f);
        }
        startFadeImage.gameObject.SetActive(false);
    }
}
