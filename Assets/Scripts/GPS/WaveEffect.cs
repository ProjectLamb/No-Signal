using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class WaveEffect : MonoBehaviour
{

    [SerializeField] private ParticleSystem wave;


    void Start()
    {
        StartCoroutine("WaveCoroutine");
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
