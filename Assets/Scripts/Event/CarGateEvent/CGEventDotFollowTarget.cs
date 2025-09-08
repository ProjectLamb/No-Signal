using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGEventDotFollowTarget : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    private float speed = 5.0f;
    private float elapsed = 0f;
    private float duration = 7.0f;
    public static bool isWaveActived = false;

    
    
    // Update is called once per frame
    void Update()
    {   
        
        if(isWaveActived && duration > elapsed)
        {   
            elapsed += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}
