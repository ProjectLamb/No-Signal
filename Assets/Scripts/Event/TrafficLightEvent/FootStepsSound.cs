using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class FootStepsSound : MonoBehaviour
{

    [SerializeField] private ParticleSystem wave;
    public GameObject target;
    public Checkiscarcrash Checkiscarcrash;
    private StudioEventEmitter emitter;
    void Start()
    {
        this.transform.position = target.transform.position + new Vector3(0, 100f, 0);
        StartCoroutine("WaveCoroutine");
        emitter = AudioManager.Instance.InitializeEventEmitter(FMODEvents.instance.playerFootSteps, this.gameObject);
        emitter.Play();
    }

    void FixedUpdate()
    {   
        if(Checkiscarcrash.isCrashed != true)
        {
        this.transform.position = target.transform.position + new Vector3(0, 10f, 0);
        }
        else {
            StopCoroutine("WaveCoroutine");
            // Vector3 StartPos = target.transform.position;
            // Vector3 TargetPos = target.transform.position + new Vector3(10.0f, 0, 0);
            // target.transform.position = Vector3.Lerp(StartPos,TargetPos, 0.8f);
            this.gameObject.SetActive(false);
            }
    }

    IEnumerator WaveCoroutine()
    {
        while (true)
        {
            if (wave != null)
            {
                Instantiate(wave, transform.position, Quaternion.Euler(90f,0,0));
                wave.transform.position = target.transform.position;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}