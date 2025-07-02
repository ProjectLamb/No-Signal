using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowEvent : MonoBehaviour
{
    public Transform carLandSpot;
    public Transform finalDestination;
    public GameObject crowDot;
    public float followSpeed = 5f; // 이동 속도
    public float flyAwaySpeed = 1f;
    public float rotationSpeed = 1f; // 회전 속도
    private float passedTime = 0f;
    private int EventPsv = 0;
    public static bool IsEventStart = false;
    private bool IsStayCar = false;
    private bool IsFlyAway = false;
    private bool RanEvent = false;
    private bool IsPsvCheck = false;


    private Animator anim;


    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        if (GameManager.Instance.IsTutorial || GameManager.Instance.IsChaseEvent || GameManager.Instance.IsCargateEvent) return;
        if (CarController.IsChaseEventStart) return;
        if (IsStayCar) StayOnCar();

        if (IsEventStart)
        {
            // 여기가 까마귀 이벤트의 시작
            IsEventStart = false;
            crowDot.SetActive(true);
            anim.SetBool("flying", true);
            this.transform.position = carLandSpot.position + new Vector3(50f, 30f, 50f);
            StartCoroutine("FollowTarget");
        }

        if (IsFlyAway) FlyToTheDest();
    }

    void Update()
    {
        if (GameManager.Instance.IsTutorial || GameManager.Instance.IsChaseEvent || GameManager.Instance.IsCargateEvent) return;
        if (CarController.IsChaseEventStart) return;
        RandomEvent();
    }

    void RandomEvent()
    {

        passedTime += Time.deltaTime;

        if ((int)passedTime != 0 && (int)passedTime % 10 == 0 && !IsPsvCheck)
        {
            IsPsvCheck = true;
            EventPsv = (int)passedTime;
            int ran = Random.Range(0, 100);
            if (ran <= EventPsv && !RanEvent)
            {
                RanEvent = true;
                passedTime = 0f;
                IsEventStart = true;
            }
        }
        else if ((int)passedTime % 10 != 0) IsPsvCheck = false;
    }

    IEnumerator FollowTarget()
    {
        while (true)
        {
            // 현재 위치에서 대상 위치로 이동
            transform.position = Vector3.MoveTowards(transform.position, carLandSpot.position, followSpeed * Time.deltaTime);

            // 대상 방향으로 회전
            Vector3 direction = (carLandSpot.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

            float distance = Vector3.Distance(transform.position, carLandSpot.position);
            if (distance < 0.3f)
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

    IEnumerator WorAndCaw()
    {
        while (CarController.lightOffTime <= 3f)
        {
            EventManager.Instance.SetEvent(0);
            EventManager.Instance.PlayEvent();
            yield return new WaitForSeconds(2f);
        }
    }

    public void StayOnCar()
    {
        StopCoroutine("FollowTarget");
        transform.position = carLandSpot.position;
        this.transform.rotation = carLandSpot.rotation * Quaternion.Euler(0, 180f, 0);
    }

    public void FlyAway()
    {
        anim.SetBool("landing", true);
        IsFlyAway = true;
    }

    private void FlyToTheDest()
    {
        if (CarController.lightOffTime <= 3f) return;
        if (CarController.IsChaseEventStart) return;
        anim.SetBool("flying", true);
        float ranDestX = Random.Range(30, 50);
        float ranDestZ = Random.Range(30, 50);
        finalDestination.position = this.transform.position + new Vector3(ranDestX, 20f, ranDestZ);
        IsStayCar = false;
        this.transform.position = Vector3.Lerp(this.transform.position, finalDestination.position, flyAwaySpeed * Time.deltaTime);
        // 대상 방향으로 회전
        Vector3 direction = (finalDestination.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        float distance = Vector3.Distance(transform.position, finalDestination.position);
        if (distance < 2f)
        {
            anim.SetBool("flying", false);
            anim.SetBool("landing", false);
            transform.position = finalDestination.position;

            crowDot.SetActive(false);
            IsFlyAway = false;
        }
    }

    public void Cawcaw()
    {
        CarController.IsCrowCaw = true;
        StartCoroutine("WorAndCaw");
    }
}
