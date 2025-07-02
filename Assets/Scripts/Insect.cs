using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class Insect : MonoBehaviour
{
    private StudioEventEmitter insectEmitter;
    // Start is called before the first frame update
    void Start()
    {
        insectEmitter = AudioManager.Instance.InitializeEventEmitter(FMODEvents.instance.insect, this.gameObject);
        insectEmitter.Play();
    }
}
