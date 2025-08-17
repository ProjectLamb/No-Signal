using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform carTr;
    Vector3 defVec;
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();  
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        defVec = new Vector3(carTr.position.x, carTr.position.y + 20f, carTr.position.z);
        lineRenderer.SetPosition(0, defVec);
    }
}
