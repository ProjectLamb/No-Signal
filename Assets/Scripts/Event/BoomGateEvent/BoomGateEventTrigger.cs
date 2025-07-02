using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BoomGateEventTrigger : MonoBehaviour
{
    public PlayableDirector PlayableDirector;
    public EventMove EventMove;
    public GameObject target;
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
        if (collider.gameObject.name == "Car")
        //리팩터링 필요
        {
            GameManager.Instance.IsCargateClear = true;
            GameManager.Instance.IsCargateEvent = true;
            Rigidbody carRb = collider.gameObject.GetComponent<Rigidbody>();
            Transform carTr = collider.gameObject.GetComponent<Transform>();
            Camera carCam = carCamTr != null ? carCamTr.GetComponent<Camera>() : null;
            if (carRb != null)
            {
                StartCoroutine(HandleEventSequence(carRb, carTr, carCam));
            }


        }
        ;
        //EventTrigger collider를 지나간 gameObject의 name을 콘솔에 출력

    }
    IEnumerator HandleEventSequence(Rigidbody carRb, Transform carTr, Camera carCam)
    {
        yield return StartCoroutine(SmoothStop(carRb, carTr, carCam));
        EventMove.MoveToTarget();
    }
    IEnumerator SmoothStop(Rigidbody carRb, Transform carTr, Camera carCam)
    {
        Collider carCollider = carRb.GetComponent<Collider>();
        if (carCollider != null) carCollider.enabled = false; // 충돌 비활성화

        float duration = 3.0f;
        float elapsed = 0f;

        Vector3 initialVelocity = carRb.velocity;
        Vector3 initialCarPosition = carTr.position;
        Vector3 targetPosition = target.transform.position;
        Quaternion initialCarRotation = carTr.rotation;
        Quaternion initialCamRotation = carCam.transform.rotation;

        Vector3 dirToGate = (targetPosition - carTr.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(dirToGate, Vector3.up);
        //Quaternion.Euler(8, 300, 2); 
        CameraFollow.isEvent = true;
        isBoomEvent = true;
        // carRb.angularVelocity = Vector3.zero;


        while (elapsed < duration)
        {
            carRb.velocity = Vector3.Lerp(initialVelocity, Vector3.zero, elapsed / duration);
            carTr.rotation = Quaternion.Slerp(initialCarRotation, targetRotation, elapsed / duration);
            carCam.transform.rotation = Quaternion.Slerp(initialCamRotation, targetRotation, elapsed / duration);
            Vector3 nextPosXZ = Vector3.Lerp(initialCarPosition, targetPosition, elapsed / (duration * 3));
            carTr.position = new Vector3(nextPosXZ.x, carTr.position.y, nextPosXZ.z);
            //속도 점차 줄여서 0으로
            elapsed += Time.deltaTime;
            yield return null;
        }
        if (carCollider != null) carCollider.enabled = true; // 충돌 복원

        PlayableDirector.gameObject.SetActive(true);
        PlayableDirector.Play();
        carRb.isKinematic = true;
        Destroy(this.gameObject);
    }
}