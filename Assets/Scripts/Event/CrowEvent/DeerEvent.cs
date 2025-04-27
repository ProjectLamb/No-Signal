using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerEvent : MonoBehaviour
{
    Animator anim;
    public Transform rushTr;
    private float deerSpeed = 1f;
    public float rotationSpeed = 1f; // 회전 속도
    public static bool IsEventStart = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (IsEventStart)
        {
            IsEventStart = false;
            anim.SetBool("IsRun", true);
            StartCoroutine("RushToCar");
        }
    }
    IEnumerator RushToCar()
    {
        while (true)
        {
            // 현재 위치에서 대상 위치로 이동
            transform.position = Vector3.MoveTowards(transform.position, rushTr.position, deerSpeed);

            // 대상 방향으로 회전
            Vector3 direction = (rushTr.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

            float distance = Vector3.Distance(transform.position, rushTr.position);
            if (distance < 0.1f)
            {
                anim.SetBool("IsRun", false);
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Car")
        {
            Destroy(this.gameObject);
        }
    }
}
