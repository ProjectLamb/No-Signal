using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowEvent : MonoBehaviour
{
    public Transform carLandSpot;
    public Transform finalDestination;
    public float speed = 5f;       // 이동 속도
    public float rotationSpeed = 1f; // 회전 속도
    public static bool IsEventStart = false;
    private bool IsStayCar = false;
    private bool IsFlyAway = false;

    private Animator anim;


    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        if (IsStayCar) StayOnCar();

        if (IsEventStart)
        {
            // 여기가 까마귀 이벤트의 시작
            IsEventStart = false;
            anim.SetBool("flying", true);
            StartCoroutine("FollowTarget");
        }

        if(IsFlyAway) FlyToTheDest();
    }

    IEnumerator FollowTarget()
    {
        while (true)
        {
            // 현재 위치에서 대상 위치로 이동
            transform.position = Vector3.MoveTowards(transform.position, carLandSpot.position, speed);

            // 대상 방향으로 회전
            Vector3 direction = (carLandSpot.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

            float distance = Vector3.Distance(transform.position, carLandSpot.position);
            if (distance < 0.1f)
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
        transform.position = carLandSpot.position;
        this.transform.rotation = carLandSpot.rotation * Quaternion.Euler(0, 180f, 0);
    }

    public void FlyAway()
    {
        IsStayCar = false;
        anim.SetBool("landing", true);
        IsFlyAway = true;
    }

    private void FlyToTheDest()
    {
        anim.SetBool("flying", true);
        this.transform.position = Vector3.Lerp(this.transform.position, finalDestination.position, speed * Time.deltaTime * 0.3f);
        // 대상 방향으로 회전
        Vector3 direction = (finalDestination.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        float distance = Vector3.Distance(transform.position, finalDestination.position);
        if (distance < 0.1f)
        {
            anim.SetBool("flying", false);
            anim.SetBool("landing", false);
            transform.position = finalDestination.position;

            IsFlyAway = false;
        }
    }
}
