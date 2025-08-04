using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class DeerEvent : MonoBehaviour
{
    Animator anim;
    public Transform rushTr;
    public GameObject deerDot;
    public float deerSpeed = 2f;
    public float rotationSpeed = 1f; // 회전 속도
    public static bool IsEventStart = false;
    public Vector3 relativeSpawnPos = new Vector3(-50, 0, 100); //new location of deer

    private EventInstance deerFootsteps;

    void Awake()
    {
        anim = GetComponent<Animator>();
        deerFootsteps = AudioManager.Instance.CreateInstance(FMODEvents.instance.deerFootsteps);
    }

    void Update()
    {
        if (IsEventStart)
        {
            IsEventStart = false;

            anim.SetBool("IsRun", true);
            deerFootsteps.start();
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
        if (col.gameObject.CompareTag("Car"))
        {
            deerFootsteps.stop(STOP_MODE.IMMEDIATE);
            Destroy(deerDot.gameObject);
            Destroy(this.gameObject);
        }
    }
}
