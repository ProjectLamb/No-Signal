using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrafficLightEventTrigger : MonoBehaviour
{   
    public GameObject Car;
    public List<Light> light;
    public static bool isTLEvent = false;
    public GameObject obstacle;
    // public GameObject obstacle_2;
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
        
        yield return StartCoroutine(TLEvent_ObstacleActiveOn());
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

    private IEnumerator TLEvent_ObstacleActiveOn(){
        // float duration = 5.0f;
        // float elapsed = 0f;
        // float speed = 1f;
        obstacle.SetActive(true);
        yield return new WaitForSecondsRealtime(5.0f);
        // obstacle_2.SetActive(true);
        // {   
        //     if(obstacle.activeSelf != false)
        //     {
        //         while (elapsed <= duration)
        //     {   
        //         Debug.Log(elapsed);
        //         elapsed += Time.deltaTime;
        //         obstacle.transform.position = Vector3.MoveTowards(obstacle.transform.position, obstacle.transform.position + new Vector3(-4.27f,0,2),speed * Time.deltaTime);
        //     }
        //     }
        
        // }
    }
    
    // void ObstacleMove(){
    //      if(obstacle_1.activeSelf != false){
    //     obstacle_1.transform.Translate(Vector3.right * Time.deltaTime/2);
    //     }
    // }
    // void Update(){
    //     ObstacleMove();
    // }
}

