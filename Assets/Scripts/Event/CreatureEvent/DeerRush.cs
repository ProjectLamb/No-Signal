using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMODUnity;

public class DeerRush : MonoBehaviour
{
    public Transform destTr;
    public Transform midDestTr;
    public Transform finalDestTr;
    private NavMeshAgent navMeshAgent;
    private Animator anim;
    private bool IsFinDest = false;
    private float dis;
    private StudioEventEmitter emitter;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        destTr = midDestTr;
        anim.SetBool("IsRun", true);
        dis = Vector3.Distance(this.transform.position, destTr.position);
        if (navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.destination = destTr.position;
        }
        else
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(this.transform.position, out hit, 50.0f, NavMesh.AllAreas))
                navMeshAgent.Warp(hit.position);
        }

        emitter = AudioManager.Instance.InitializeEventEmitter(FMODEvents.instance.deerRush, this.gameObject);
        emitter.Play();
    }

    void Update()
    {
        if (navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.destination = destTr.position;
            dis = Vector3.Distance(this.transform.position, destTr.position);
        }
        else
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(this.transform.position, out hit, 50.0f, NavMesh.AllAreas))
                navMeshAgent.Warp(hit.position);
        }
        if (dis < 100f && IsFinDest)
        {
            this.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("DMS"))
        {
            destTr = finalDestTr;
            navMeshAgent.speed = 50f;
            IsFinDest = true;
        }
    }
}
