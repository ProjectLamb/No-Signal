using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class TLRadioSound : MonoBehaviour
{   
    private EventInstance radio_TL;
    public GameObject Car;
    
    // Start is called before the first frame update
    void Awake()
    {
        radio_TL = AudioManager.Instance.CreateInstance(FMODEvents.instance.radio_TL);
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider collider){
        if(collider.gameObject == Car){
        AudioManager.Instance.PlayOneShot(FMODEvents.instance.radio_TL, this.transform.position);

        this.gameObject.SetActive(false);
    }
    }
}
