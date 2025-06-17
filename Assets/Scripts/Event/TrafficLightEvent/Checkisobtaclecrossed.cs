using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkisobstaclecrossed : MonoBehaviour
{   
    public List<Light> trafficlight;
    public GameObject trafficLightEventTrigger;
    public GameObject obstacle_1;
    public GameObject obstacle_2;
    public GameObject TLEvent_Dot;
    private Coroutine eventStart;
    // Start is called before the first frame update
    void OnTriggerExit(Collider collider){

        if(collider.gameObject == obstacle_1){
        obstacle_1.SetActive(false);
        obstacle_2.SetActive(false);
        TLEvent_Dot.SetActive(false);
        
         if(eventStart != null)
        StopCoroutine(eventStart);
    
        eventStart = StartCoroutine(TLEvent_Start());
        }
    }
    private IEnumerator TLEvent_Start(){
        StartCoroutine(TLEvent_LightOn(1));
        yield return StartCoroutine(TLEvent_LightOff(0));
        Destroy(trafficLightEventTrigger.gameObject);
        }

    private IEnumerator TLEvent_LightOn(int num){
        if(trafficlight[num] != null){
        trafficlight[num].gameObject.SetActive(true);
        yield return null;
        }
        
    } 

    private IEnumerator TLEvent_LightOff(int num){
        if(trafficlight[num] != null){
        trafficlight[num].gameObject.SetActive(false);
        yield return null;
        }
    }
}