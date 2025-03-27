using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEffect : MonoBehaviour
{

    [SerializeField] private ParticleSystem wave;


    void Start()
    {
        StartCoroutine("WaveCoroutine");
    }


    void Update()
    {

    }

    IEnumerator WaveCoroutine()
    {
        while (true)
        {
            if (wave != null)
            {
                Instantiate(wave, transform.position, Quaternion.Euler(90f,0,0));
                // 생성될때 소리 재생 필요
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}
