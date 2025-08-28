using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class VideoManager : MonoBehaviour
{
    private EventInstance creditOST;
    // Start is called before the first frame update

    void Awake()
    {
        creditOST = AudioManager.Instance.CreateInstance(FMODEvents.instance.credit);
    }
    void Start()
    {
        creditOST.start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
