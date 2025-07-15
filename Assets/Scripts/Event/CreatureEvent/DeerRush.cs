using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeerRush : MonoBehaviour
{
    public Transform destTr;
    private NavMeshAgent navMeshAgent;
    private Animator anim;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        anim.SetBool("IsRun", true);
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
    }

    void Update()
    {
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
    }
}
