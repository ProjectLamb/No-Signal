using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrafficLightEventTrigger : MonoBehaviour
{   
    public GameObject Car;
    public List<Light> light;
    public static bool isTLEvent = false;
    public GameObject obstacle;
    public GameObject EventDot;
    private Coroutine eventStart;
    

    void OnTriggerEnter(Collider collider)
    {   
        if(collider.gameObject == Car)
        //리팩터링 필요
        {   
            if(eventStart != null)
        StopCoroutine(eventStart);
        eventStart = StartCoroutine(TLEvent_Start());

        };
    }
   
    private IEnumerator TLEvent_Start(){
        
        yield return StartCoroutine(TLEvent_LightOn(0)); // Red
        yield return StartCoroutine(TLEvent_LightOff(1)); // Green
        yield return StartCoroutine(TLEvent_EventDotActiveOn());

        yield return StartCoroutine(TLEvent_ObstacleActiveOn());
        yield return StartCoroutine(TLEvent_LightOn(1)); // Red
        yield return StartCoroutine(TLEvent_LightOff(0)); // Green
        yield return StartCoroutine(TLEvent_EventDotActiveOff());
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

    private IEnumerator TLEvent_EventDotActiveOn(){
        EventDot.SetActive(true);
        yield return null;
    }

    private IEnumerator TLEvent_ObstacleActiveOn(){
        obstacle.SetActive(true);
        yield return new WaitForSecondsRealtime(10.0f);
        obstacle.SetActive(false);
    }

    private IEnumerator TLEvent_EventDotActiveOff(){
        Destroy(EventDot);
        yield return null;
    }
}

