using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VolFx;
public class ChaseEvent : MonoBehaviour
{
    public Volume vhsVolume;
    private VhsVol vvs;
    void VhsEffect()
    {
        if (vhsVolume.profile.TryGet(out vvs))
        {
            if (vvs._weight.value < 0.5f)
            {
                vvs._weight.value += 0.02f;
            }
        }
        return;
    }
}
