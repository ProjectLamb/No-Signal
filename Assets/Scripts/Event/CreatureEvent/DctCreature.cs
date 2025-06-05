using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DctCreature : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform targetTr;
    public float rotSpeed = 3f;

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
    }

    // Update is called once per frame
    void Update()
    {
        LookTarget();
        //navMeshAgent.destination = targetTr.position;
        this.transform.position = Vector3.MoveTowards(transform.position, targetTr.position, 2f * Time.deltaTime);
    }

    void LookTarget()
    {
        Quaternion creatureRot = Quaternion.LookRotation(targetTr.position - transform.position);
        this.transform.rotation = creatureRot;

        creatureRot.eulerAngles = new Vector3(0, creatureRot.eulerAngles.y, 0);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, creatureRot, Time.deltaTime * rotSpeed);
    }
}
