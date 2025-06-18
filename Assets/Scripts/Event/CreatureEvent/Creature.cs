using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMOD.Studio;
using FMODUnity;

public class Creature : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private EventInstance creatureHowl;
    private EventInstance creatureDeath;
    private StudioEventEmitter stepEmitter;
    private Rigidbody rigid;
    private Animator anim;

    public Transform targetTr;
    public Transform targetForward;
    public Transform oakTree;

    public float rotSpeed = 3f;
    public static bool IsDie = false;
    private bool IsChase = false;
    private bool IsReveal = false;
    public static  bool IsEnding = false;
    //private bool 

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        creatureHowl = AudioManager.instance.CreateInstance(FMODEvents.instance.creatureHowl);
        creatureDeath = AudioManager.instance.CreateInstance(FMODEvents.instance.creatureDeath);

        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.creatureHowl, this.transform.position);
        stepEmitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.creatureStep, this.gameObject);
        IsReveal = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDie)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.creatureDeath, this.transform.position);
        }
        if (CarController.IsGameOver)
            {
                navMeshAgent.enabled = false;
                this.transform.position = targetForward.position;
            }
        if (!CarController.IsGameOver && IsChase)
        {
            stepEmitter.Play();
            if (navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.destination = targetTr.position;
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
            stepEmitter.Play();
            Vector3 revealPos = new Vector3(targetTr.position.x - 50f, targetTr.position.y + 13f, targetTr.position.z + 5f);
            this.transform.position = Vector3.MoveTowards(transform.position, revealPos, 50f * Time.deltaTime);

            if (transform.position == revealPos)
            {
                IsReveal = false;
                stepEmitter.Stop();
            }
        }

        if (IsEnding)
        {
            this.transform.position = targetForward.position;
        }
    }

    void LookTarget()
    {
        Quaternion creatureRot = Quaternion.LookRotation(targetTr.position - transform.position);
        this.transform.rotation = creatureRot;

        creatureRot.eulerAngles = new Vector3(0, creatureRot.eulerAngles.y, 0);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, creatureRot, Time.deltaTime * rotSpeed);
    }

    public void ChaseStart()
    {
        StartCoroutine("WaitChase");
        IsChase = true;
    }
    public void RunIntoTree()
    {
        IsEnding = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Oak")
        {
            stepEmitter.Stop();
            anim.SetTrigger("DoDie");
            IsEnding = false;
            IsChase = false;
        }

        if (collision.gameObject.name == "Car")
        {
            anim.SetTrigger("DoAttack");
        }
    }
    IEnumerator WaitChase()
    {
        yield return new WaitForSeconds(2f);
        stepEmitter.Play();
    }
}
