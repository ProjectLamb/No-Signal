using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMOD.Studio;
using FMODUnity;

public class DctCreature : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator anim;
    public Transform targetTr;
    public Transform gameOverTr;
    public Collider col;
    public float rotSpeed = 3f;
    private StudioEventEmitter stepEmitter;
    public static bool IsGameOver = false;

    Rigidbody rb;

    void Awake()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        IsGameOver = true;
        GameManager.Instance.IsDeathEvent = true;
        EventManager.Instance.SetEvent(3);
        EventManager.Instance.PlayEvent();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGameOver)
        {
            navMeshAgent.enabled = false;
            return;
        }
        else
        {
            if (navMeshAgent.isOnNavMesh)
            {
                navMeshAgent.SetDestination(targetTr.position);
            }
            else
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(this.transform.position, out hit, 50.0f, NavMesh.AllAreas))
                    navMeshAgent.Warp(hit.position);
            }
            LookTarget();
        }
    }    


    void LookTarget()
    {
        Quaternion creatureRot = Quaternion.LookRotation(targetTr.position - transform.position);
        creatureRot.eulerAngles = new Vector3(0, creatureRot.eulerAngles.y, 0);
        this.transform.rotation = creatureRot;
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
        Vector3 offset = targetTr.forward * 35f + targetTr.up * -5f;
        this.transform.position = targetTr.position + offset;
    }
}
