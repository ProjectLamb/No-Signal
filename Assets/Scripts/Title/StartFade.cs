using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;

public class StartFade : MonoBehaviour
{

    public Image startFadeImage;
    public GameObject tutorial;

    private EventInstance introNoise;


    void Awake()
    {
        introNoise = AudioManager.instance.CreateInstance(FMODEvents.instance.introNoise);
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
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.introNoise,this.transform.position);
        yield return new WaitForSeconds(2.5f);
        StartCoroutine("CoFadeOut");
    }
}
