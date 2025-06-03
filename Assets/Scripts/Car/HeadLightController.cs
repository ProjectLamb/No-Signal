using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadLightController : MonoBehaviour
{
    public Image lightFill;
    private bool IsCanUseLight = true;

    void Update()
    {
        if (CarController.IsHeadlightsOn && IsCanUseLight)
        {
            lightFill.fillAmount -= 0.0333f * Time.deltaTime;
        }
        else if (!CarController.IsHeadlightsOn && IsCanUseLight)
        {
            lightFill.fillAmount += 0.02f * Time.deltaTime;
        }

        if (lightFill.fillAmount == 0)
        {
            StartCoroutine("lightCool");
        }
    }

    IEnumerator lightCool()
    {
        IsCanUseLight = false;
        yield return new WaitForSeconds(5f);
        IsCanUseLight = true;
    }
}
