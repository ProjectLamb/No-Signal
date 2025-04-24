using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BoomGateEventTrigger : MonoBehaviour
{
    public PlayableDirector PlayableDirector;
    // public Camera Camera;
    // private Transform CarCamTr;
    public static bool isBoomEvent = false;
    private Transform carCamTr;

    void Start()
    {
        GameObject carObj = GameObject.Find("Car_Nosignal_01");
        if (carObj != null)
        {
            carCamTr = carObj.transform.Find("CarCam");
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.gameObject.name);
        if(collider.gameObject.name == "Car_Nosignal_01")
        //리팩터링 필요
        {   

            Rigidbody carRb = collider.gameObject.GetComponent<Rigidbody>();
            Transform carTr = collider.gameObject.GetComponent<Transform>();
        Camera carCam = carCamTr != null ? carCamTr.GetComponent<Camera>() : null;
                if (carRb != null)
            {   
                StartCoroutine(SmoothStop(carRb, carTr, carCam));           
            }
        };
        //EventTrigger collider를 지나간 gameObject의 name을 콘솔에 출력
    }
    IEnumerator SmoothStop(Rigidbody carRb, Transform carTr, Camera carCam)
    {
        float duration = 1.0f;
        float elapsed = 0f;
        Vector3 initialVelocity = carRb.velocity;

        Quaternion initialCarRotation = carTr.transform.rotation;
        Quaternion initialCamRotation = carCam.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 180, 0); 
        CameraFollow.isEvent = true;
        isBoomEvent = true;
        carRb.angularVelocity = Vector3.zero;
        

        while (elapsed < duration){
            carTr.transform.rotation = Quaternion.Slerp(initialCarRotation, targetRotation, elapsed / duration);
            carCam.transform.rotation = Quaternion.Slerp(initialCamRotation, targetRotation, elapsed / duration);

            //속도 점차 줄여서 0으로
            elapsed += Time.deltaTime;
            
            yield return null;
        }
            
            Debug.Log("이벤트 켜짐");
            PlayableDirector.gameObject.SetActive(true);
            PlayableDirector.Play();
            carRb.isKinematic = true; 
            //물리적 움직임 차단
                
            
                
    }
}

