using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCar : MonoBehaviour
{
    private float speed = 5f;
    private float intensity = 0.003f;
    private float litRange = 0f;
    private float passedTime = 0f;
    
    public Transform modelTr;
    public Light leftHeadlight;
    public Light rightHeadlight;

    void Start()
    {
        StartCoroutine("LightOnOff");
    }
    void Update()
    {
        Vibrate();
    }

    void Vibrate()
    {
        modelTr.localPosition = intensity * new Vector3(
            Mathf.PerlinNoise(speed * Time.time, 1),
            Mathf.PerlinNoise(speed * Time.time, 2),
            Mathf.PerlinNoise(speed * Time.time, 3));
    }

    IEnumerator LightOnOff()
    {
        litRange = Random.Range(5, 20);
        passedTime = 0;
        while (true)
        {
            passedTime += Time.deltaTime;
            if (passedTime > litRange)
            {
                LightOff();
                yield return new WaitForSeconds(0.1f);
                LightOn();
                yield return new WaitForSeconds(0.1f);
                LightOff();
                yield return new WaitForSeconds(0.1f);
                LightOn();
                yield return new WaitForSeconds(0.1f);
                litRange = Random.Range(5, 20);
                passedTime = 0;
            }
            yield return new WaitForSeconds(0.001f);
        }
    }

    void LightOn()
    {
        leftHeadlight.enabled = true;
        rightHeadlight.enabled = true;
    }

    void LightOff()
    {
        leftHeadlight.enabled = false;
        rightHeadlight.enabled = false;
    }
}
