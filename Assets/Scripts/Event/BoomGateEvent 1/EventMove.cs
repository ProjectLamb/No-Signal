using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using System.Collections.Generic;

public class EventMove : MonoBehaviour
{   
    public Animator animator;
    public Transform target;       // BoomGate의 Transform
    public Transform boomgatebar;
    public GameObject light;
    public GameObject BGEventWave;
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
        animator = GetComponent<Animator>();
        Vector3 destination = target.position;
        Vector3 startPos = transform.position;

        
        // 1. 이동
        animator.SetInteger("Movement", 0);
        yield return StartCoroutine(BGEvent_Lighton()); // 이벤트 진입 시 헤드라이트 on
        yield return StartCoroutine(BGEvent_Move(destination)); // 차단바까지 이동
        animator.SetInteger("Movement", 1); // 차단바 주섬주섬 애니메이션
        yield return new WaitForSecondsRealtime(3f); // 3초 기다림
        

        // 2. 회전 및 헤드라이트 off
        StartCoroutine(BGEvent_WaveActive());
        yield return StartCoroutine(BGEvent_Lightoff());
        yield return new WaitForSecondsRealtime(1.0f);

        animator.SetInteger("Movement", 2);
        yield return StartCoroutine(BGEvent_Rotate());
        yield return new WaitForSecondsRealtime(2.0f);
        // 3. 헤드라이트 점등
        yield return StartCoroutine(BGEvent_Lightblink(3));
        yield return StartCoroutine(BGEvent_WaveInactive());
        yield return new WaitForSecondsRealtime(1.5f);
        yield return StartCoroutine(BGEvent_Rotate());
        animator.SetInteger("Movement", 3);
        yield return new WaitForSecondsRealtime(1.0f);
        yield return StartCoroutine(BGEvent_BoomGateOpen());

        // 4. 복귀
        animator.SetInteger("Movement", 4);
        yield return StartCoroutine(BGEvent_Rotate());
        animator.SetInteger("Movement", 5);
        yield return StartCoroutine(BGEvent_Return(startPos));
    }
    private IEnumerator BGEvent_Move(Vector3 destination){
        while (Vector3.Distance(transform.position, destination) > 0.45f)
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

    private IEnumerator BGEvent_Lighton()
    {   
        light.SetActive(true);
        yield return null;
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

    private IEnumerator BGEvent_BoomGateOpen()
    {   
       
        Quaternion startRot = boomgatebar.transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, 0, -90);
        float elapsed = 0f;
        float turnDuration = 8.0f;

        while (elapsed < turnDuration)
        {
            boomgatebar.transform.rotation = Quaternion.Slerp(startRot, endRot, elapsed / turnDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        boomgatebar.transform.rotation = endRot;

        yield return new WaitForSecondsRealtime(1.0f);
    }
    private IEnumerator BGEvent_WaveActive()
    {   
        BGEventWave.gameObject.SetActive(true);
        BGEventDotFollowTarget.isWaveActived = true;
        yield return null;
    }
    private IEnumerator BGEvent_WaveInactive()
    {
        BGEventWave.gameObject.SetActive(false);
        BGEventDotFollowTarget.isWaveActived = false;
        yield return null;
    }
}
