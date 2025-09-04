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
    public static bool IsAttachCar = false;
    private bool IsGameOver = false;

    //public static bool IsJunction = false;
    public static bool IsEnding = false;
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
        // if (IsJunction)
        // {
        //     IsJunction = false;
        //     float ranDestX = UnityEngine.Random.Range(20, 30);
        //     float ranDestY = UnityEngine.Random.Range(3, 5);

        //     Vector3 offset = targetTr.forward * ranDestX + targetTr.up * ranDestY;
        //     this.transform.position = targetTr.position + offset;
        //     navMeshAgent.enabled = false;
        //     NavCheck();
        //     navMeshAgent.enabled = true;
        // }
        if (IsDie)
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.instance.creatureDeath, this.transform.position);
        }
        if (IsGameOver)
        {
            navMeshAgent.enabled = false;
        }
        if (!IsGameOver && IsChase && !IsAttachCar)
        {
            IsReveal = false;
            NavCheck();
            distCheck();
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

        if (IsAttachCar && !GameManager.Instance.IsEnding)
        {
            this.transform.position = attachTr.position;
            anim.SetBool("IsRun", false);
        }
    }

    void NavCheck()
    {
        if (navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.destination = targetTr.position;
            anim.SetBool("IsRun", true);
        }
        else
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(this.transform.position, out hit, 50f, NavMesh.AllAreas))
                navMeshAgent.Warp(hit.position);
        }
    }
    void LookTarget()
    {
        Quaternion creatureRot = Quaternion.LookRotation(targetTr.position - transform.position);
        creatureRot.eulerAngles = new Vector3(0, creatureRot.eulerAngles.y, 0);
        this.transform.rotation = creatureRot;
    }

    void distCheck()
    {
        float distance = 0f;
        distance = Vector3.Distance(targetTr.position, transform.position);
        if (distance > 30f)
        {
            navMeshAgent.speed = 30f;
        }
        else navMeshAgent.speed = 10f;
    }
    public void ChaseStart()
    {
        StartCoroutine("WaitChase");
        IsChase = true;
        anim.SetBool("IsRun", true);
        stepEmitter.Play();
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Oak"))
        {
            //stepEmitter.Stop();
            anim.SetTrigger("DoDie");
            IsChase = false;
        }

        if (col.gameObject.CompareTag("Car"))
        {
            // if (GameManager.Instance.IsJunctionEvent)
            // {
            //     IsAttachCar = true;
            //     AudioManager.Instance.PlayOneShot(FMODEvents.instance.carCol, this.transform.position);
            //     stepEmitter.Stop();
            // }
            //if (!IsGameOver && !GameManager.Instance.IsJunctionEvent)
            if(!IsGameOver && !GameManager.Instance.IsRushToTree)
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.instance.carCol, this.transform.position);
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
        AudioManager.Instance.PlayOneShot(FMODEvents.instance.creatureGameOver, targetTr.position);
        LookTarget();
        anim.SetTrigger("DoAttack");
        rb.isKinematic = true;
    }

    public void SetRushPosition()
    {
        rb.isKinematic = false;
        col.isTrigger = true;
        Vector3 offset = targetTr.forward * 35f + targetTr.up * -6f;
        this.transform.position = targetTr.position + offset;
    }

    public void Die()
    {
        anim.SetTrigger("DoDie");
        IsChase = false;
    }
}
