using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingFade : MonoBehaviour
{

    public Image endingFadeImage;
    public GameObject endingTutorial;
    private bool IsEndingStart = true;

    void Start()
    {
        StartCoroutine("CoFadeIn");
    }

    IEnumerator CoFadeIn()
    {
        // Color fadeCol = endingFadeImage.color;
        // while (fadeCol.a < 1f)
        // {
        //     fadeCol.a += 0.02f;
        //     endingFadeImage.color = fadeCol;
        //     yield return new WaitForSeconds(0.02f);
        // }
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Credit");
    }
}
