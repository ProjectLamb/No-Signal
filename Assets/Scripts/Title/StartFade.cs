using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;

public class StartFade : MonoBehaviour
{

    public Image startFadeImage;
    public GameObject tutorial;
    public GameObject GPSLine;

    void Awake()
    {
    }
    void Start()
    {
        StartCoroutine("WaitTime");
    }

    IEnumerator CoFadeOut()
    {
        Color fadeCol = startFadeImage.color;
        while (fadeCol.a > 0f)
        {
            fadeCol.a -= 0.02f;
            startFadeImage.color = fadeCol;
            yield return new WaitForSeconds(0.02f);
        }
        startFadeImage.gameObject.SetActive(false);
        tutorial.SetActive(GameManager.Instance.IsTutorialFirst);
        if (!GameManager.Instance.IsTutorial && !GameManager.Instance.IsTutorialFirst)
        {
            StartCoroutine("NaviStart");
        }
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(1.5f);
        AudioManager.Instance.PlayOneShot(FMODEvents.instance.introNoise, this.transform.position);
        yield return new WaitForSeconds(2.5f);
        StartCoroutine("CoFadeOut");
    }

    IEnumerator NaviStart()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.instance.naviStart, this.transform.position);
        yield return new WaitForSeconds(3.5f);
        AudioManager.Instance.PlayOneShot(FMODEvents.instance.naviBeep, this.transform.position);
        GPSLine.SetActive(true);
    }
}
