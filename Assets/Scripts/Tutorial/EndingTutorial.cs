using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;

public class EndingTutorial : MonoBehaviour
{
    public Image memo1;
    public GameObject endingPanel;
    public GameObject GPSLine;

    private EventInstance tutorialPage;
    private int page = 0;
    private bool IsFade = false;
    private bool IsCanSkip = false;
    void Start()
    {
        GameManager.Instance.IsTutorial = true;
        StartCoroutine(CoFadeIn(memo1));
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
                AudioManager.Instance.PlayOneShot(FMODEvents.instance.tutorialPage, this.transform.position);
                StartCoroutine("WaitSkip");
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
        if (page < 1) IsCanSkip = false;
        IsFade = false;
        if (page == 1)
        {
            GameManager.Instance.IsTutorial = false;
            StartCoroutine("EndingNaviStart");
        }
    }

    IEnumerator WaitSkip()
    {
        yield return new WaitForSeconds(2.0f);
        IsCanSkip = true;
    }

    IEnumerator EndingNaviStart()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.instance.naviStart, this.transform.position);
        yield return new WaitForSeconds(3.5f);
        AudioManager.Instance.PlayOneShot(FMODEvents.instance.naviBeep, this.transform.position);
        GPSLine.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        AudioManager.Instance.PlayOneShot(FMODEvents.instance.carLight, this.transform.position);
        endingPanel.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Credit");
    }
}
