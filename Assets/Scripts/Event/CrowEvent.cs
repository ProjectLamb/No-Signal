using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowEvent : MonoBehaviour
{
    public Transform target;        // 쫓을 대상 (차)
    public Transform finalDestination;
    public float speed = 5f;       // 이동 속도
    public float rotationSpeed = 1f; // 회전 속도
    public static bool IsEventStart = false;

    private bool IsStayCar = false;

    private Animator anim;
    Vector3 targetPosition;


    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (IsEventStart)
        {
            // 여기가 까마귀 이벤트의 시작
            IsEventStart = false;
            anim.SetBool("flying", true);
            StartCoroutine("FollowTarget");
        }

        if (IsStayCar) StayOnCar();
    }

    IEnumerator FollowTarget()
    {
        while (true)
        {
            // 충돌 하기 전, 디스턴스 비교해서 일정 디스턴스 아래면 stayoncar 실행하게끔 수정하기

            targetPosition = target.position;
            // 현재 위치에서 대상 위치로 이동
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

            // 대상 방향으로 회전
            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

            float distance = Vector3.Distance(transform.position, target.position);
            if (distance < 0.05f)
            {
                IsStayCar = true;
                anim.SetBool("landing", true);
                anim.SetBool("flying", false);
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        anim.SetBool("landing", false);

        EventManager.Instance.SetEvent(0);
        EventManager.Instance.PlayEvent();
    }

    public void StayOnCar()
    {
        StopCoroutine("FollowTarget");
        transform.position = target.position;
        this.transform.rotation = target.rotation * Quaternion.Euler(0, 180f, 0);
    }

    public void FlyAway()
    {
        IsStayCar = false;
        anim.SetBool("landing", true);
        StartCoroutine("FlyToTheDest");
    }

    IEnumerator FlyToTheDest()
    {
        anim.SetBool("flying", true);
        while (true)
        {

            targetPosition = finalDestination.position;
            // 현재 위치에서 대상 위치로 이동
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.02f * Time.deltaTime);

            // 대상 방향으로 회전
            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

            float distance = Vector3.Distance(transform.position, target.position);
            if (distance < 0.01f)
            {
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        anim.SetBool("flying", false);
        anim.SetBool("landing", false);

        transform.position = finalDestination.position;

        StopCoroutine("FlyToTheDest");
        
    }
}
