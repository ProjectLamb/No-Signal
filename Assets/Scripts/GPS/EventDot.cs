using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(StudioEventEmitter))]
public class EventDot : MonoBehaviour
{

    [SerializeField] private ParticleSystem wave;

    private StudioEventEmitter emitter;
    void Start()
    {
        StartCoroutine("WaveCoroutine");
        emitter = AudioManager.instance.InitializeEventEmitter(FMODEvents.instance.radar, this.gameObject);
        emitter.Play();
    }

    IEnumerator WaveCoroutine()
    {
        while (true)
        {
            if (wave != null)
            {
                Instantiate(wave, transform.position, Quaternion.Euler(90f,0,0));
                // 생성될때 소리 재생
                //AudioManager.instance.PlayOneShot(FMODEvents.instance.radar, this.transform.position);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}
