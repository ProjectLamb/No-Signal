using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BoomGateEventTrigger : MonoBehaviour
{
    public PlayableDirector PlayableDirector;

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.gameObject.name);
        if(collider.gameObject.name == "Car_Nosignal_01")
        {
             Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero; // 속도를 0으로
                rb.angularVelocity = Vector3.zero; // 회전 속도도 0으로
                rb.isKinematic = true; // 물리적 움직임 차단
                PlayableDirector.gameObject.SetActive(true);
        PlayableDirector.Play();
            }

            // CarController 같은 스크립트를 비활성화해서 입력 차단
            // CarController carControl = collider.gameObject.GetComponent<CarController>();
            // if (carControl != null)
            // {
            //     carControl.enabled = false;
            // }
        };
        //EventTrigger collider를 지나간 gameObject의 name을 콘솔에 출력
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
