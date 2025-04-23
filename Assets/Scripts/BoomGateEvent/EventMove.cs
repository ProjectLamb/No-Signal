using UnityEngine;
using UnityEngine.Playables;

public class EventMove : MonoBehaviour
{
    public Transform target;       // BoomGate의 Transform
    public PlayableDirector timeline;
    public float speed = 4f;       // 이동 속도
    private bool isMoving = false;
    private bool isTurning = false;
    private bool isBackMoving = false;
    private Vector3 startPos;
    private Vector3 destination;
    
    float duration = 1.0f;
    float elapsed = 0f;

    void Update()
    {
        if (isMoving && target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            // 일정 거리 도달하면 정지 (원하면 조건 추가)
            if (Vector3.Distance(transform.position, destination) < 0.3f)
            {
                isMoving = false;
                isTurning = true;

                // timeline.Resume(); 
                //도착 후 타임라인 재생
            }
        }
        else if (isTurning && target != null)
    {
        
        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration); // 0~1 사이 비율
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(0, 0, 0);

        transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

        if (elapsed >= duration)
        {
         isTurning = false;
         isBackMoving = true;
         }
        }
        else if(isBackMoving && target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);

            // 일정 거리 도달하면 정지 (원하면 조건 추가)
            if (Vector3.Distance(transform.position, startPos) < 0.3f)
            {
                isBackMoving = false;
                // timeline.Resume(); 
                //도착 후 타임라인 재생
            }
        }
    }
    public void MoveToTargetAndPauseTimeline()
    {
        startPos = transform.position;
        destination = target.position;
        isMoving = true;

        timeline.Pause();   // 이동하는 동안 타임라인 멈춤
    }
}
    // public void TriggerMove()
    // {   
    //     Debug.Log("시그널 들어가는중");
    //     isMoving = true;
    // }