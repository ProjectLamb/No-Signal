using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;

public class EventMove : MonoBehaviour
{   
    private NavMeshAgent agent;
    private EventInstance playerFootSteps;
    private EventInstance boomGateBarSound;
    private EventInstance carLight;
    private EventInstance fuseOff;
    public Animator animator;
    public Transform target;       // BoomGate의 Transform
    public Transform boomgatebar;
    public GameObject light;
    public Rigidbody CarRb;
    public float speed = 2f;       // 이동 속도
    private Coroutine moveRoutine;

    public List<EventDot> BGEventWave;
    public void MoveToTarget()
    {   
        if (this.gameObject != null)
        {
            this.gameObject.SetActive(true);
            if (moveRoutine != null) StopCoroutine(moveRoutine);
            moveRoutine = StartCoroutine(MoveRoutine());
        }
        else 
        {
            CameraFollow.isEvent = false;
            BoomGateEventTrigger.isBoomEvent = false;
            CarRb.isKinematic = false;
            CarRb.velocity = Vector3.zero;
            // 차단바 이벤트 진입시 player가 null인 에러가 떠도 차 움직이게 하는 예외처리
        }
    }

   void Awake()
    {  
        agent = GetComponent<NavMeshAgent>();
        playerFootSteps = AudioManager.Instance.CreateInstance(FMODEvents.instance.playerFootSteps);
        boomGateBarSound = AudioManager.Instance.CreateInstance(FMODEvents.instance.boomGateBarSound);
    }

    private IEnumerator MoveRoutine()
    {   
        Vector3 destination = target.position;
        Vector3 startPos = transform.position;

        animator = GetComponent<Animator>();
        agent.enabled = true;
        agent.SetDestination(destination);
        Vector3 dir = (destination - transform.position).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5f);
        

        // 1. 이동 및 헤드라이트 on
        yield return StartCoroutine(BGEvent_Lighton()); //이벤트 진입 시 헤드라이트 on
        animator.SetInteger("Movement", 0);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(playerFootSteps, transform, GetComponent<Rigidbody>());
        playerFootSteps.start();
        agent.stoppingDistance = 0.5f;
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }
        agent.isStopped = true;
        // yield return StartCoroutine(BGEvent_Lighton());
        // yield return StartCoroutine(BGEvent_Move(destination)); // 차단바까지 이동
        if (playerFootSteps.isValid())
        {
        playerFootSteps.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        animator.SetInteger("Movement", 1); // 차단바 주섬주섬 애니메이션
        yield return new WaitForSecondsRealtime(3f); // 3초 기다림


        // 2. 파동 시작
        StartCoroutine(BGEvent_WaveActive());
        yield return new WaitForSecondsRealtime(4.0f);
        // 3. 회전 및 헤드라이트 점등
        
        yield return StartCoroutine(BGEvent_Lightblink(3));
        yield return new WaitForSecondsRealtime(0.5f);
        animator.SetInteger("Movement", 2);
        yield return StartCoroutine(BGEvent_Rotate());
        yield return StartCoroutine(BGEvent_Lightoff());
        yield return StartCoroutine(BGEvent_WaveInactive());
        
        yield return new WaitForSecondsRealtime(3.5f);
        // 여기 퓨즈나가는 소리 추가
        yield return StartCoroutine(BGEvent_Rotate());
        animator.SetInteger("Movement", 3);
        yield return new WaitForSecondsRealtime(1.0f);
        yield return StartCoroutine(BGEvent_BoomGateOpen());

        // 4. 복귀
        animator.SetInteger("Movement", 4);
        yield return StartCoroutine(BGEvent_Rotate());
        animator.SetInteger("Movement", 5);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(playerFootSteps, transform, GetComponent<Rigidbody>());
        playerFootSteps.start();
        yield return StartCoroutine(BGEvent_Return(startPos));
        if (playerFootSteps.isValid())
        {
        playerFootSteps.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        yield return StartCoroutine(PlayerActiveOff());
        yield return new WaitForSecondsRealtime(1.0f);
        
        // playerFootSteps.release();
    }   

   

    private IEnumerator PlayerActiveOff(){
        GameManager.Instance.IsCargateEvent = false;
        CameraFollow.isEvent = false;
        BoomGateEventTrigger.isBoomEvent = false;
        this.gameObject.SetActive(false);
        CarRb.isKinematic = false;
        CarRb.velocity = Vector3.zero;
        
         //BoomGateEventTrigger off
        yield return null;
    }

    // private IEnumerator BGEvent_Move(Vector3 destination){
    //     while (Vector3.Distance(transform.position, destination) > 0.45f)
    //     {
    //         Vector3 dir = (destination - transform.position).normalized;
    //         transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5f);
    //         transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    //         yield return null;
    //     }
    //     }

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
        agent.isStopped = false;
        agent.SetDestination(startPos);

        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        agent.isStopped = true;
        // while (Vector3.Distance(transform.position, startPos) > 0.1f)
        // {
        // transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
        // yield return null;
        // }
        }

    private IEnumerator BGEvent_Lighton()
    {   
        light.SetActive(true);
        AudioManager.Instance.PlayOneShot(FMODEvents.instance.carLight, this.transform.position);
        yield return null;
    }
    
    private IEnumerator BGEvent_Lightoff()
    {   
        light.SetActive(false);
        yield return null;

    }

    private IEnumerator BGEvent_Lightblink(int num)
    {   
        AudioManager.Instance.PlayOneShot(FMODEvents.instance.fuseOff, this.transform.position);
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
        AudioManager.Instance.PlayOneShot(FMODEvents.instance.boomGateBarSound, target.transform.position);
        Quaternion startRot = boomgatebar.transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, -90, 0);
        float elapsed = 0f;
        float turnDuration = 5.0f;

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
        BGEventDotFollowTarget.isWaveActived = true;
        for (int i = 0; i < BGEventWave.Count; i++)
    {   
        BGEventWave[i].gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        // 시간 랜덤화(0.2~0.8) 필요
    }
        
        yield return null;
    }
    private IEnumerator BGEvent_WaveInactive()
    {   
        BGEventDotFollowTarget.isWaveActived = false;
        foreach (var dot in BGEventWave)
        {
            dot.gameObject.SetActive(false);
        }
        
        yield return null;
    }
}