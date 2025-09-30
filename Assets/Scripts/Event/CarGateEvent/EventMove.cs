using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Cinemachine;

public class EventMove : MonoBehaviour
{   
    private NavMeshAgent agent;
    private EventInstance playerFootSteps;
    private EventInstance carGateBarSound;
    private EventInstance carLight;
    private EventInstance fuseOff;
    public Animator animator;
    public Transform target;       // carGate의 Transform
    public Transform carGatebar;
    public GameObject light;
    public Rigidbody CarRb;
    public float speed = 2f;       // 이동 속도
    private Coroutine moveRoutine;
    public List<EventDot> CGEventWave;
    public CinemachineBrain cinemachineBrain;

    public Collider carCollider;

    [Header("Animator Controllers")]
    public RuntimeAnimatorController carGatePlayer;
    public RuntimeAnimatorController carGatePlayer1;
    public RuntimeAnimatorController carGatePlayer2;
    public RuntimeAnimatorController carGatePlayer3;
    
    [Header("Scripts")]
    public CarController CarController;

    // 애니메이터 교체 전용 코루틴
    private IEnumerator ChangeAnimatorController(RuntimeAnimatorController newController, string startStateName = null, float waitAfterChange = 0f)
    {
        if (animator == null) animator = GetComponent<Animator>();

        if (newController != null)
        {
            animator.runtimeAnimatorController = newController;
            Debug.Log($"Animator Controller 변경됨 → {newController.name} ✅");
            // 원하는 State로 강제 시작
            if (!string.IsNullOrEmpty(startStateName))
            {
                animator.Play(startStateName, 0, 0f); 
                Debug.Log($"Animator State 강제 시작 → {startStateName}");
            }
        }
        
        else
        {
            Debug.LogWarning("넘겨받은 Animator Controller가 null 입니다 ⚠️");
        }

        // 교체 후 대기 (옵션)
        if (waitAfterChange > 0f)
        {
            yield return new WaitForSecondsRealtime(waitAfterChange);
        }
    }

    void Awake()
    {  
        
        agent = GetComponent<NavMeshAgent>();
        playerFootSteps = AudioManager.Instance.CreateInstance(FMODEvents.instance.playerFootSteps);
        carGateBarSound = AudioManager.Instance.CreateInstance(FMODEvents.instance.carGateBarSound);
    }

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
            cinemachineBrain.enabled = false;
            CameraFollow.IsEvent = false;
            CarGateEventTrigger.isCargateEvent = false;
            CarRb.isKinematic = false;
            CarRb.velocity = Vector3.zero;
            // 차단바 이벤트 진입시 player가 null인 에러가 떠도 차 움직이게 하는 예외처리
        }
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
        yield return StartCoroutine(CGEvent_Lighton()); //이벤트 진입 시 헤드라이트 on
        yield return StartCoroutine(ChangeAnimatorController(carGatePlayer));
        animator.SetInteger("Movement", 0);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(playerFootSteps, transform, GetComponent<Rigidbody>());
        playerFootSteps.start();
        agent.stoppingDistance = 0.5f;
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }
        agent.isStopped = true;
        agent.updateRotation = false;
        if (playerFootSteps.isValid())
        {
        playerFootSteps.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        
        animator.SetInteger("Movement", 1); // 차단바 주섬주섬 애니메이션
        yield return new WaitForSecondsRealtime(3f); // 3초 기다림


        // 2. 파동 시작
        StartCoroutine(CGEvent_WaveActive());
        yield return new WaitForSecondsRealtime(4.0f);
        // 3. 회전 및 헤드라이트 점등
        
        yield return StartCoroutine(CGEvent_Lightblink(3));
        yield return new WaitForSecondsRealtime(0.5f);
        yield return StartCoroutine(CGEvent_Lightoff());
        animator.SetInteger("Movement", 2); // 도는 애니메이션
        yield return new WaitForSecondsRealtime(4.0f);

        yield return StartCoroutine(CGEvent_Rotate120());
        yield return StartCoroutine(CGEvent_WaveInactive());
        yield return StartCoroutine(ChangeAnimatorController(carGatePlayer1));
        animator.SetInteger("Movement", 3); // 도는 애니메이션
        yield return new WaitForSecondsRealtime(4.0f);

        
        // yield return new WaitForSecondsRealtime(3.5f);
        yield return StartCoroutine(CGEvent_Rotate300());
        yield return StartCoroutine(ChangeAnimatorController(carGatePlayer2));
        animator.SetInteger("Movement", 4); // 차단바 버튼 푸쉬 애니메이션
        yield return new WaitForSecondsRealtime(1.0f);
        yield return StartCoroutine(CGEvent_carGateOpen());

        
        // 4. 복귀
        animator.SetInteger("Movement", 5); // 도는 애니메이션
        yield return new WaitForSecondsRealtime(4.0f);

        yield return StartCoroutine(CGEvent_Rotate120());

        yield return StartCoroutine(ChangeAnimatorController(carGatePlayer3));
        animator.SetInteger("Movement", 6); // 복귀 애니메이션
        agent.updateRotation = true;
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(playerFootSteps, transform, GetComponent<Rigidbody>());
        playerFootSteps.start();
        yield return StartCoroutine(CGEvent_Return(startPos));
        if (playerFootSteps.isValid())
        {
        playerFootSteps.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        yield return StartCoroutine(CGEvent_Lighton());
        yield return StartCoroutine(PlayerActiveOff());
        yield return new WaitForSecondsRealtime(1.0f);
        
        // playerFootSteps.release();
    }   

   

    private IEnumerator PlayerActiveOff(){
        GameManager.Instance.IsCargateEvent = false;
        CameraFollow.IsEvent = false;
        CarGateEventTrigger.isCargateEvent = false;
        this.gameObject.SetActive(false);
        CarRb.isKinematic = false;
        CarRb.useGravity = true;
        CarRb.velocity = Vector3.zero;
        cinemachineBrain.enabled = false;
         //CarGateEventTrigger off
        yield return null;
    }

    // private IEnumerator CGEvent_Move(Vector3 destination){
    //     while (Vector3.Distance(transform.position, destination) > 0.45f)
    //     {
    //         Vector3 dir = (destination - transform.position).normalized;
    //         transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5f);
    //         transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    //         yield return null;
    //     }
    //     }

    private IEnumerator CGEvent_Rotate120(){
        
        // Quaternion startRot = transform.rotation;
        // Quaternion endRot = startRot * Quaternion.Euler(0, 90, 0);
        // float elapsed = 0f;
        // float turnDuration = 1.0f;

        // while (elapsed < turnDuration)
        // {
        //     transform.rotation = Quaternion.Slerp(startRot, endRot, elapsed / turnDuration);
        //     elapsed += Time.deltaTime;
        //     yield return null;
        // }
        // transform.rotation = endRot;
        transform.rotation = Quaternion.Euler(0,120,0);
        yield return null;
        }
    private IEnumerator CGEvent_Rotate300(){
        
        // Quaternion startRot = transform.rotation;
        // Quaternion endRot = startRot * Quaternion.Euler(0, 90, 0);
        // float elapsed = 0f;
        // float turnDuration = 1.0f;

        // while (elapsed < turnDuration)
        // {
        //     transform.rotation = Quaternion.Slerp(startRot, endRot, elapsed / turnDuration);
        //     elapsed += Time.deltaTime;
        //     yield return null;
        // }
        // transform.rotation = endRot;
        transform.rotation = Quaternion.Euler(0,300,0);
        yield return null;
        }
    private IEnumerator CGEvent_Return(Vector3 startPos){
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

    private IEnumerator CGEvent_Lighton()
    {   
        if(!CarController.IsHeadlightsOn)
        {
        CarController.IsHeadlightsOn = true;
        light.SetActive(true);
        AudioManager.Instance.PlayOneShot(FMODEvents.instance.carLight, this.transform.position);
        }
        yield return null;
    }
    
    private IEnumerator CGEvent_Lightoff()
    {   
        CarController.IsHeadlightsOn = false;
        light.SetActive(false);
        yield return null;

    }

    private IEnumerator CGEvent_Lightblink(int num)
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

    private IEnumerator CGEvent_carGateOpen()
    {   
        AudioManager.Instance.PlayOneShot(FMODEvents.instance.carGateBarSound, target.transform.position);
        Quaternion startRot = carGatebar.transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, -90, 0);
        float elapsed = 0f;
        float turnDuration = 5.0f;

        while (elapsed < turnDuration)
        {
            carGatebar.transform.rotation = Quaternion.Slerp(startRot, endRot, elapsed / turnDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        carGatebar.transform.rotation = endRot;

        yield return new WaitForSecondsRealtime(3.0f);
    }
    private IEnumerator CGEvent_WaveActive()
    {
        CGEventDotFollowTarget.isWaveActived = true;
        for (int i = 0; i < CGEventWave.Count; i++)
    {   
        CGEventWave[i].gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        // 시간 랜덤화(0.2~0.8) 필요
    }
        
        yield return null;
    }
    private IEnumerator CGEvent_WaveInactive()
    {   
        CGEventDotFollowTarget.isWaveActived = false;
        foreach (var dot in CGEventWave)
        {
            dot.gameObject.SetActive(false);
        }
        
        yield return null;
    }
}