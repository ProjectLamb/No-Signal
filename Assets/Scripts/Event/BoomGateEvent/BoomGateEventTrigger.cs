using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoomGateEventTrigger : MonoBehaviour
{
    public EventMove EventMove;
    public GameObject target;
    public static bool isBoomEvent = false;
    // private Transform carCamTr;


    void Start()
    {
        GameObject carObj = GameObject.Find("Car");
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Car")
        //리팩터링 필요
        {
            GameManager.Instance.IsCargateClear = true;
            GameManager.Instance.IsCargateEvent = true;
            Rigidbody carRb = collider.gameObject.GetComponent<Rigidbody>();
            Transform carTr = collider.gameObject.GetComponent<Transform>();
            if (carRb != null)
            {
                StartCoroutine(HandleEventSequence(carRb, carTr));
            }

        }

    }
    IEnumerator HandleEventSequence(Rigidbody carRb, Transform carTr)
    {
        yield return StartCoroutine(SmoothStop(carRb, carTr));
        EventMove.MoveToTarget();
    }
    IEnumerator SmoothStop(Rigidbody carRb, Transform carTr)
    {
        Collider carCollider = carRb.GetComponent<Collider>();
        if (carCollider != null) carCollider.enabled = false; // 충돌 비활성화

        float duration = 3.0f;
        float elapsed = 0f;

        Vector3 initialVelocity = carRb.velocity;
        Vector3 initialCarPosition = carTr.position;
        Vector3 targetPosition = target.transform.position;
        Quaternion initialCarRotation = carTr.rotation;

        Vector3 carTrToGate = (targetPosition - carTr.position).normalized;
        Quaternion targetCarRotation = Quaternion.LookRotation(carTrToGate, Vector3.up);

        CameraFollow.isEvent = true;
        isBoomEvent = true;

        while (elapsed < duration){
            
            float t = elapsed / duration;
            carTr.rotation = Quaternion.Slerp(initialCarRotation, targetCarRotation, t);
            Vector3 nextPosXZ = Vector3.Lerp(initialCarPosition, targetPosition, t/3.0f);

            carTr.position = new Vector3(nextPosXZ.x, carTr.position.y, nextPosXZ.z);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
            
            if (carCollider != null) carCollider.enabled = true; // 충돌 복원
            
            carRb.isKinematic = true; 
                 
            Destroy(this.gameObject); 

    }
}