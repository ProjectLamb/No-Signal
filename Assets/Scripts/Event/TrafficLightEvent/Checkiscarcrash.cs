using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class Checkiscarcrash : MonoBehaviour
{
    // Start is called before the first frame update
    private EventInstance carCrash;
    private EventInstance humanFall;
    public GameObject CrossCheckTrigger;
    public GameObject Car;

    public static bool isCrashed = false;
    // Update is called once per frame
    void Awake()
    {
        carCrash = AudioManager.instance.CreateInstance(FMODEvents.instance.carCrash);
        humanFall = AudioManager.instance.CreateInstance(FMODEvents.instance.humanFall);
    }

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject == Car){
        AudioManager.instance.PlayOneShot(FMODEvents.instance.carCrash, this.transform.position);
        //사운드 재생

        AudioManager.instance.PlayOneShot(FMODEvents.instance.humanFall, this.transform.position);
        //철푸덕 소리

        //차량 흔들림
        
        //차량 속도 50퍼 감속
        isCrashed = true;
        CrossCheckTrigger.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
    }

    
    
    
}
