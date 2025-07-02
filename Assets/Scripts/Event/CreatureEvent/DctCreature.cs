using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMOD.Studio;
using FMODUnity;

public class DctCreature : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform targetTr;
    public Transform targetForward;
    public float rotSpeed = 3f;
    private StudioEventEmitter stepEmitter;

    void Awake()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(this.transform.position, out hit, 50.0f, NavMesh.AllAreas))
        {
            navMeshAgent.Warp(hit.position);
        }
        stepEmitter = AudioManager.Instance.InitializeEventEmitter(FMODEvents.instance.creatureStep, this.gameObject);
        stepEmitter.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (CarController.IsGameOver)
        {
            navMeshAgent.enabled = false;
            this.transform.position = targetForward.position;
        }
        else
        {
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
    }

    void LookTarget()
    {
        Quaternion creatureRot = Quaternion.LookRotation(targetTr.position - transform.position);
        this.transform.rotation = creatureRot;

        creatureRot.eulerAngles = new Vector3(0, creatureRot.eulerAngles.y, 0);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, creatureRot, Time.deltaTime * rotSpeed);
    }

    void OnCollsionEnter(Collision col)
    {
        if (col.gameObject.tag == "Car")
        {
            this.transform.position = targetForward.position;
        }
    }
}
