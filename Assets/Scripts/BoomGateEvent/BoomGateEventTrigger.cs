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
    
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.gameObject.name);
        if(collider.gameObject.name == "Car_Nosignal_01")
        //리팩터링 필요
        {   
            Rigidbody carRb = collider.gameObject.GetComponent<Rigidbody>();
                if (carRb != null)
            {   
                StartCoroutine(SmoothStop(carRb));           
            }
        };
        //EventTrigger collider를 지나간 gameObject의 name을 콘솔에 출력
    }
    IEnumerator SmoothStop(Rigidbody carRb)
    {
        float duration = 2.0f;
        float elapsed = 0f;
        Vector3 initialVelocity = carRb.velocity;
        Quaternion initialRotation = CameraFollow.carTarget.rotation;

        while (elapsed < duration)
        {   carRb.velocity = Vector3.Lerp(initialVelocity, Vector3.zero, elapsed / duration);
            CameraFollow.carTarget.rotation = Quaternion.Slerp(initialRotation, Quaternion.Euler(0,180,0), elapsed / duration);
            //근데 이거 적용이 안됌..
            //속도 점차 줄여서 0으로
            elapsed += Time.deltaTime;
             
            yield return null;
        }
            isBoomEvent = true;
            Debug.Log("이벤트 켜짐");
            PlayableDirector.gameObject.SetActive(true);
            PlayableDirector.Play();
            carRb.isKinematic = true; 
            //물리적 움직임 차단
                
            
                
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
       
        // if(Input.GetKeyDown(KeyCode.P))
    //     {
    //     PlayableDirector.gameObject.SetActive(true);
    //     PlayableDirector.Play();
    // }
    }
}
