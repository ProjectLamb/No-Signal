using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;

public class Checkiscarcrash : MonoBehaviour
{
    // Start is called before the first frame update
    private EventInstance carCrash;
    public Image soundFill;
    // private EventInstance humanFall;
    // public GameObject CrossCheckTrigger;
    public GameObject Car;
    public List<Light> light;
    public static bool isCrashed = false;
    private Coroutine eventEnd;
    // Update is called once per frame
    void Awake()
    {
        carCrash = AudioManager.instance.CreateInstance(FMODEvents.instance.carCrash);
    //     humanFall = AudioManager.instance.CreateInstance(FMODEvents.instance.humanFall);
    }

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject == Car){
        AudioManager.instance.PlayOneShot(FMODEvents.instance.carCrash, this.transform.position);
        //사운드 재생

        // AudioManager.instance.PlayOneShot(FMODEvents.instance.humanFall, this.transform.position);
        //철푸덕 소리

        //차량 흔들림
        
        //차량 속도 50퍼 감속
        soundFill.fillAmount += 0.5f;
        //부딫힐 경우 사운드 게이지 50퍼 증가
        isCrashed = true;
            if(eventEnd != null)
        StopCoroutine(eventEnd);
        eventEnd = StartCoroutine(TLEvent_End());

        };
        // CrossCheckTrigger.gameObject.SetActive(false);
        //2초 후 SetActiveOff
        
    }
    
    private IEnumerator TLEvent_End(){
        
        yield return StartCoroutine(TLEvent_LightOn(1)); // Green
        yield return StartCoroutine(TLEvent_LightOff(0)); // Red
        
        yield return StartCoroutine(TLEvent_ObstacleActiveOff());
    }

    
    private IEnumerator TLEvent_LightOn(int num){
        if(light[num] != null){
        light[num].gameObject.SetActive(true);
        yield return null;
        }
        
    } 

    private IEnumerator TLEvent_LightOff(int num){
        if(light[num] != null){
        light[num].gameObject.SetActive(false);
        yield return null;
        }
    }

    private IEnumerator TLEvent_ObstacleActiveOff(){
        this.gameObject.SetActive(false); 
        yield return null;
    }
    
    
    
}
