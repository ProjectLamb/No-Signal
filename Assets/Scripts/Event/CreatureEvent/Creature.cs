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
    private StudioEventEmitter stepEmitter;
    public Transform targetTr;
    public Transform targetForward;
    public float rotSpeed = 3f;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        creatureHowl = AudioManager.instance.CreateInstance(FMODEvents.instance.creatureHowl);
    }
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent.destination = targetTr.position;
        AudioManager.instance.PlayOneShot(FMODEvents.instance.creatureHowl, this.transform.position);
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
    }

    void LookTarget()
    {
        Quaternion creatureRot = Quaternion.LookRotation(targetTr.position - transform.position);
        this.transform.rotation = creatureRot;

        creatureRot.eulerAngles = new Vector3(0, creatureRot.eulerAngles.y, 0);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, creatureRot, Time.deltaTime * rotSpeed);
    }
}
