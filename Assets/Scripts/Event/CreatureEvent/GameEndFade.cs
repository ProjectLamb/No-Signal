using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMODUnity;

public class GameEndFade : MonoBehaviour
{
    public Image fadeImage;
    public GameObject car;
    public GameObject endingTutorial;
    public Transform startTr;
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
        yield return new WaitForSeconds(1.0f);
        AudioManager.Instance.PlayOneShot(FMODEvents.instance.reality,car.transform.position);
        car.transform.position = startTr.position;
        car.transform.rotation = startTr.rotation;

        yield return new WaitForSeconds(1.68f);
        fadeImage.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.5f);
        endingTutorial.SetActive(true);
    }
}
