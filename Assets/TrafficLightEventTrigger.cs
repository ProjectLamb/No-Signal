using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrafficLightEventTrigger : MonoBehaviour
{
    public List<Light> light;
    public static bool isTLEvent = false;
    private Coroutine eventStart;

    void Start()
    {
        GameObject carObj = GameObject.Find("Car");
    }
    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.name == "Car")
        //리팩터링 필요
        {   
            if(eventStart != null)
        StopCoroutine(eventStart);
    
        eventStart = StartCoroutine(TLEvent_Start());

        };
    }
   
    private IEnumerator TLEvent_Start(){
    
        yield return StartCoroutine(TLEvent_LightOn(0));
        yield return new WaitForSeconds(3);
        yield return StartCoroutine(TLEvent_LightOff(0));
        yield return StartCoroutine(TLEvent_LightOn(1));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(TLEvent_LightOff(1));
        yield return StartCoroutine(TLEvent_LightOn(2));
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
        
}

