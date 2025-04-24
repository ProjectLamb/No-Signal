using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using System.Collections.Generic;

public class EventMove : MonoBehaviour
{
    public Transform target;       // BoomGate의 Transform
    public GameObject light;
    public float speed = 4f;       // 이동 속도
    private Coroutine moveRoutine;

    public void MoveToTarget()
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveRoutine());
    }


    private IEnumerator MoveRoutine()
    {

        Vector3 destination = target.position;
        Vector3 startPos = transform.position;

        // 1. 이동
        yield return StartCoroutine(BGEvent_Move(destination));
        yield return new WaitForSecondsRealtime(3f); // 3초 기다림

        // 2. 회전
        yield return StartCoroutine(BGEvent_Lightoff());
        yield return new WaitForSecondsRealtime(1f);
        yield return StartCoroutine(BGEvent_Rotate());
        yield return StartCoroutine(BGEvent_Lightblink(3));
        
        // 3. 복귀
        yield return StartCoroutine(BGEvent_Return(startPos));
    }
    private IEnumerator BGEvent_Move(Vector3 destination){
        while (Vector3.Distance(transform.position, destination) > 0.15f)
        {
            Vector3 dir = (destination - transform.position).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5f);
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            yield return null;
        }
        }
    private IEnumerator BGEvent_Rotate(){
        Quaternion startRot = transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, 180, 0);
        float elapsed = 0f;
        float turnDuration = 1.0f;

        while (elapsed < turnDuration)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, elapsed / turnDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endRot;
        }
    private IEnumerator BGEvent_Return(Vector3 startPos){
        while (Vector3.Distance(transform.position, startPos) > 0.1f)
        {
        transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
        yield return null;
        }
        }

    private IEnumerator BGEvent_Lightoff()
    {
        light.SetActive(false);
        yield return null;
    }

    private IEnumerator BGEvent_Lightblink(int num)
    {   
        int i = 0;

        while (i < num)
        {
        i++;
        light.SetActive(false);
        yield return new WaitForSecondsRealtime(0.1f);
        light.SetActive(true);
        yield return new WaitForSecondsRealtime(0.1f);
        }
        //num 만큼 반복
    }
}
