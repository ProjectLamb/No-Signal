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
        if (NavMesh.SamplePosition(this.transform.position, out hit, 5.0f, NavMesh.AllAreas))
        {
            this.transform.position = hit.position;
        }
        stepEmitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.creatureStep, this.gameObject);
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
            navMeshAgent.destination = targetTr.position;
        }
        LookTarget();
        //this.transform.position = Vector3.MoveTowards(transform.position, targetTr.position, 2f * Time.deltaTime);
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
