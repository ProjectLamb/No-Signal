using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class Tutorial : MonoBehaviour
{
    public Image memo1;
    public Image memo2;

    private EventInstance tutorialPage;
    private int page = 0;
    private bool IsFade = false;
    private bool IsCanSkip = false;
    public static bool IsTutorial = false;
    void Start()
    {
        IsTutorial = true;
        StartCoroutine(CoFadeIn(memo1));
        StartCoroutine(CoFadeIn(memo2));
        StartCoroutine("WaitSkip");
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsFade && IsCanSkip)
        {
            if (page == 0)
            {
                page++;
                StartCoroutine(CoFadeOut(memo1));
                AudioManager.instance.PlayOneShot(FMODEvents.instance.tutorialPage, this.transform.position);
                StartCoroutine("WaitSkip");
            }

            else if (page == 1)
            {
                page++;
                StartCoroutine(CoFadeOut(memo2));
                AudioManager.instance.PlayOneShot(FMODEvents.instance.tutorialPage, this.transform.position);
            }
        }
    }

    IEnumerator CoFadeIn(Image fadeImage)
    {
        fadeImage.gameObject.SetActive(true);
        IsFade = true;
        Color fadeCol = fadeImage.color;
        while (fadeCol.a < 1f)
        {
            fadeCol.a += 0.05f;
            fadeImage.color = fadeCol;
            yield return new WaitForSeconds(0.01f);
        }
        IsFade = false;
    }

    IEnumerator CoFadeOut(Image fadeImage)
    {
        IsFade = true;
        Color fadeCol = fadeImage.color;
        while (fadeCol.a > 0f)
        {
            fadeCol.a -= 0.05f;
            fadeImage.color = fadeCol;
            yield return new WaitForSeconds(0.01f);
        }
        fadeImage.gameObject.SetActive(false);
        if (page == 1) IsCanSkip = false;
        if (page == 2) IsTutorial = false;
        IsFade = false;
        GameManager.IsTutorialFirst = false;
    }

    IEnumerator WaitSkip()
    {
        yield return new WaitForSeconds(3.0f);
        IsCanSkip = true;
    }
}
