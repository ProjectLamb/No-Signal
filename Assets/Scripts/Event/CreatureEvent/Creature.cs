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
    private StudioEventEmitter stepEmitter2;
    private Rigidbody rb;
    private Animator anim;

    public Transform targetTr;
    public Transform oakTree;
    public Transform gameOverTr;
    public Transform attachTr;
    public Collider col;

    public static bool IsDie = false;
    public static bool IsAttachCar = false;
    public static bool IsRushToCar = false;
    public float rotSpeed = 3f;

    private bool IsChase = false;
    private bool IsReveal = false;
    private bool IsGameOver = false;
    private bool IsCanRespawn = false;
    private bool IsTeleport = false;
    private float reSpawnCool = 30f;



    public static bool IsEnding = false;
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        stepEmitter = AudioManager.Instance.InitializeEventEmitter(FMODEvents.instance.creatureStep, this.gameObject);
        stepEmitter2 = AudioManager.Instance.InitializeEventEmitter(FMODEvents.instance.creatureStep2, this.gameObject);
        AudioManager.Instance.PlayOneShot(FMODEvents.instance.creatureHowl, this.transform.position);
        IsReveal = true;
        stepEmitter2.Play();
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
        if (!IsGameOver && IsChase && !IsAttachCar)
        {
            IsReveal = false;
            NavCheck();
            distCheck();
            ReSpawn();
        }
        LookTarget();

        if (IsReveal)
        {
            anim.SetBool("IsRun2", true);
            Vector3 revealPos = new Vector3(targetTr.position.x - 21f, targetTr.position.y - 1.7f, targetTr.position.z + 2f);
            this.transform.position = Vector3.MoveTowards(transform.position, revealPos, 20f * Time.deltaTime);

            if (transform.position == revealPos)
            {
                anim.SetBool("IsRun2", false);
                IsReveal = false;
            }
        }

        if (IsRushToCar && !IsTeleport)
        {
            IsTeleport = true;
            navMeshAgent.enabled = false;
            Vector3 targetPos = new Vector3(targetTr.position.x + 10f, targetTr.position.y + 5f, targetTr.position.z + 10f);
            this.transform.position = targetPos;
            navMeshAgent.enabled = true;
            navMeshAgent.speed = 50f;
        }
        
        if (IsAttachCar && !GameManager.Instance.IsEnding)
        {
            navMeshAgent.enabled = false;
            stepEmitter.Stop();
            anim.SetBool("IsRun", false);
            this.transform.position = attachTr.position;
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
            IsCanRespawn = true;
            navMeshAgent.speed = 30f;
        }
        else if (IsRushToCar) navMeshAgent.speed = 50f;
        else navMeshAgent.speed = 10f;
    }
    public void ChaseStart()
    {
        StartCoroutine("WaitChase");
        IsChase = true;
        anim.SetBool("IsRun", true);
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Oak"))
        {
            stepEmitter.Stop();
            anim.SetTrigger("DoDie");
            IsChase = false;
        }

        if (col.gameObject.CompareTag("Car"))
        {
            if (!IsGameOver && !GameManager.Instance.IsRushToTree && !IsRushToCar)
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.instance.carCol, this.transform.position);
                IsGameOver = true;
                EventManager.Instance.SetEvent(4);
                EventManager.Instance.PlayEvent();
                stepEmitter.Stop();
            }
            else if(!IsGameOver && IsRushToCar)
            {
                IsAttachCar = true;
            }
        }

    }

    IEnumerator WaitChase()
    {
        yield return new WaitForSeconds(2f);
        stepEmitter.Play();
    }

    public void JumpToCar()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.instance.creatureGameOver, targetTr.position);
        LookTarget();
        anim.SetTrigger("DoAttack");
        rb.isKinematic = true;
    }

    void ReSpawn()
    {
        reSpawnCool += Time.deltaTime;

        if (IsCanRespawn && reSpawnCool > 30f)
        {
            float ranX = Random.Range(20, 30);
            float ranY = Random.Range(5, 10);
            float ranZ = Random.Range(20, 30);

            AudioManager.Instance.PlayOneShot(FMODEvents.instance.creatureHowl, this.transform.position);
            this.transform.position = new Vector3(targetTr.position.x + ranX, targetTr.position.y + ranY, targetTr.position.z + ranZ);
            IsCanRespawn = false;
            reSpawnCool = 0f;
        }
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

    public void StandUp()
    {
        stepEmitter2.Stop();
    }
}
