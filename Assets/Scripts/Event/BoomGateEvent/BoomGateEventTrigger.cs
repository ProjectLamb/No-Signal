using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;


public class BoomGateEventTrigger : MonoBehaviour
{
    public EventMove EventMove;
    public GameObject target;
    public static bool isBoomEvent = false;
    public static bool isFinished = false;
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
            SaveLoadManager.Instance.IsCargateClear = true;
            SaveLoadManager.Instance.SaveGameData();
            GameManager.Instance.IsCargateEvent = true;
            Rigidbody carRb = collider.gameObject.GetComponent<Rigidbody>();
            Transform carTr = collider.gameObject.GetComponent<Transform>();
            if (carRb != null)
            {
                StartCoroutine(HandleEventSequence(carRb, carTr));
            }

        }

    }
    List<Collider> disabledColliders = new List<Collider>();

    void DisableAllColliders(GameObject car)
    {
        Collider[] colliders = car.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            if (col.enabled)
            {
                col.enabled = false;
                disabledColliders.Add(col);
            }
        }
    }

    void EnableAllColliders()
    {
        foreach (Collider col in disabledColliders)
        {
            if (col != null)
                col.enabled = true;
        }
        disabledColliders.Clear();
    }

    IEnumerator HandleEventSequence(Rigidbody carRb, Transform carTr)
    {
        yield return StartCoroutine(SmoothStop(carRb, carTr));
        EventMove.MoveToTarget();
    }
    IEnumerator SmoothStop(Rigidbody carRb, Transform carTr)
    {
        // Collider carCollider = carRb.GetComponent<Collider>();
        DisableAllColliders(carTr.gameObject);

        // if (carCollider != null) carCollider.enabled = false; // 충돌 비활성화

        Vector3 initialVelocity = carRb.velocity;
        Vector3 initialCarPosition = carTr.position;
        Vector3 targetPosition = target.transform.position;
        Quaternion initialCarRotation = carTr.rotation;

        Vector3 carTrToGate = (targetPosition - carTr.position).normalized;
        Quaternion targetCarRotation = Quaternion.LookRotation(carTrToGate, Vector3.up);
        Vector3 targetRotation = targetCarRotation.eulerAngles;

        CameraFollow.isEvent = true;
        isBoomEvent = true;

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(carTr.DORotate(targetRotation, 3.0f));
        mySequence.Join(carTr.DOMove(targetPosition, 3.0f));
        mySequence.OnComplete(() => isFinished = true);
        mySequence.Play();

        yield return new WaitUntil(() => isFinished);

        EnableAllColliders(); // 이벤트 끝나고 Collider 복구
        // if (carCollider != null) carCollider.enabled = true; // 충돌 복원
        carRb.isKinematic = true;
        carRb.useGravity = false;

        Destroy(this.gameObject);



    }
}