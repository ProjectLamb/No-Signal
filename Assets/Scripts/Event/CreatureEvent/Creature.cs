using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMOD.Studio;
using FMODUnity;

public class Creature : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private StudioEventEmitter stepEmitter;
    private Rigidbody rb;
    private Animator anim;

    public Transform targetTr;
    public Transform oakTree;
    public Transform gameOverTr;
    public Transform attachTr;
    public Collider col;

    public float rotSpeed = 3f;
    public static bool IsDie = false;
    private bool IsChase = false;
    private bool IsReveal = false;
    private bool IsAttachCar = false;
    public static bool IsEnding = false;
    public static bool IsGameOver = false;
    //private bool  
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.instance.creatureHowl, this.transform.position);
        stepEmitter = AudioManager.Instance.InitializeEventEmitter(FMODEvents.instance.creatureStep, this.gameObject);
        IsReveal = true;
        stepEmitter.Play();
        anim.SetBool("IsRun", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDie)
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.instance.creatureDeath, this.transform.position);
        }
        if (IsGameOver)
        {
            navMeshAgent.enabled = false;
        }
        if (!IsGameOver && IsChase)
        {
            IsReveal = false;
            if (navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.destination = targetTr.position;
                anim.SetBool("IsRun", true);
            }
            else
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(this.transform.position, out hit, 50.0f, NavMesh.AllAreas))
                    navMeshAgent.Warp(hit.position);
            }
        }
        LookTarget();

        if (IsReveal)
        {
            anim.SetBool("IsRun", true);
            Vector3 revealPos = new Vector3(targetTr.position.x - 21f, targetTr.position.y - 1.7f, targetTr.position.z + 2f);
            this.transform.position = Vector3.MoveTowards(transform.position, revealPos, 20f * Time.deltaTime);

            if (transform.position == revealPos)
            {
                anim.SetBool("IsRun", false);
                IsReveal = false;
            }
        }

        if (IsAttachCar)
        {
            this.transform.position = attachTr.position;
        }
    }

    void LookTarget()
    {
        Quaternion creatureRot = Quaternion.LookRotation(targetTr.position - transform.position);
        creatureRot.eulerAngles = new Vector3(0, creatureRot.eulerAngles.y, 0);
        this.transform.rotation = creatureRot;
    }

    public void ChaseStart()
    {
        StartCoroutine("WaitChase");
        IsChase = true;
        anim.SetBool("IsRun", true);
        stepEmitter.Play();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Oak")
        {
            //stepEmitter.Stop();
            anim.SetTrigger("DoDie");
            IsChase = false;
        }

        if (collision.gameObject.tag == "Car" && GameManager.Instance.IsJunctionEvent)
        {
            IsAttachCar = true;
            stepEmitter.Stop();
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Car")
        {
            if (!IsGameOver)
            {
                IsGameOver = true;
                EventManager.Instance.SetEvent(4);
                EventManager.Instance.PlayEvent();
                stepEmitter.Stop();
            }
        }
    }

    IEnumerator WaitChase()
    {
        yield return new WaitForSeconds(2f);
        stepEmitter.Stop();
    }

    public void JumpToCar()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.instance.creatureHowl, targetTr.position);
        LookTarget();
        anim.SetTrigger("DoAttack");
    }

    public void SetRushPosition()
    {
        rb.isKinematic = false;
        col.isTrigger = true;
        this.transform.position = new Vector3(targetTr.position.x - 4f, targetTr.position.y - 5f, targetTr.position.z - 32f);
    }
}
