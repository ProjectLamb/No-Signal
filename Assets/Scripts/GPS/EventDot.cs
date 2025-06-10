using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class EventDot : MonoBehaviour
{

    [SerializeField] private ParticleSystem wave;
    public GameObject target;

    private StudioEventEmitter emitter;
    void Start()
    {
        this.transform.position = target.transform.position + new Vector3(0, 100f, 0);
        StartCoroutine("WaveCoroutine");
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.radar, this.gameObject);
        emitter.Play();
    }

    void FixedUpdate()
    {
        this.transform.position = target.transform.position + new Vector3(0, 10f, 0);
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
