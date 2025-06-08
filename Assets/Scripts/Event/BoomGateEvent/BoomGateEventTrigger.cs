using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BoomGateEventTrigger : MonoBehaviour
{
    public PlayableDirector PlayableDirector;
    public EventMove EventMove;
    public static bool isBoomEvent = false;
    private Transform carCamTr;

    void Start()
    {
        GameObject carObj = GameObject.Find("Car");
        if (carObj != null)
        {
            carCamTr = carObj.transform.Find("CarCam");
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.name == "Car")
        //리팩터링 필요
        {   

            Rigidbody carRb = collider.gameObject.GetComponent<Rigidbody>();
            Transform carTr = collider.gameObject.GetComponent<Transform>();
        Camera carCam = carCamTr != null ? carCamTr.GetComponent<Camera>() : null;
                if (carRb != null)
            {   
               StartCoroutine(HandleEventSequence(carRb, carTr, carCam));     
            }
            
            
        };
        //EventTrigger collider를 지나간 gameObject의 name을 콘솔에 출력
   
    } IEnumerator HandleEventSequence(Rigidbody carRb, Transform carTr, Camera carCam){
       yield return StartCoroutine(SmoothStop(carRb, carTr, carCam));
       EventMove.MoveToTarget();
    }
    IEnumerator SmoothStop(Rigidbody carRb, Transform carTr, Camera carCam)
    {
        float duration = 2.0f;
        float elapsed = 0f;

        Vector3 initialVelocity = carRb.velocity;
        Quaternion initialCarRotation = carTr.transform.rotation;
        Quaternion initialCamRotation = carCam.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(10, 300, 10); 
        CameraFollow.isEvent = true;
        isBoomEvent = true;
        // carRb.angularVelocity = Vector3.zero;
        

        while (elapsed < duration){
            carRb.velocity = Vector3.Lerp(initialVelocity, Vector3.zero, elapsed / duration);
            carTr.transform.rotation = Quaternion.Slerp(initialCarRotation, targetRotation, elapsed / duration);
            carCam.transform.rotation = Quaternion.Slerp(initialCamRotation, targetRotation, elapsed / duration);

            //속도 점차 줄여서 0으로
            elapsed += Time.deltaTime;
            
            yield return null;
        }
            // carRb.velocity = Vector3.zero;
            PlayableDirector.gameObject.SetActive(true);
            PlayableDirector.Play();
            carRb.isKinematic = true;          
    }
}

