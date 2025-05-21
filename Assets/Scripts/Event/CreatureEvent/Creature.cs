using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMOD.Studio;
using FMODUnity;

public class Creature : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private EventInstance creatureSound;
    public Transform targetTr;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        creatureSound = AudioManager.instance.CreateInstance(FMODEvents.instance.creature);
    }
    // Start is called before the first frame update
    void Start()
    {
        creatureSound.start();
    }

    // Update is called once per frame
    void Update()
    {
        creatureSound.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));
        navMeshAgent.destination = targetTr.position;
    }
}
