using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform carTr;
    public bool[] IsPassedLines;
    private Vector3[] orgLines;
    private Vector3 defVec;
    private float dis;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        IsPassedLines = new bool[lineRenderer.positionCount];
        orgLines = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(orgLines);
        lineRenderer.alignment = LineAlignment.View;

        defVec = new Vector3(carTr.position.x, carTr.position.y + 10f, carTr.position.z);
        lineRenderer.SetPosition(0, defVec);
    }

    // Update is called once per frame
    void Update()
    {
        defVec = new Vector3(carTr.position.x, carTr.position.y + 10f, carTr.position.z);
        lineRenderer.SetPosition(0, defVec);

        for (int i = 1; i < orgLines.Length; i++)
        {
            dis = Vector3.Distance(new Vector3(carTr.position.x, 0, carTr.position.z), new Vector3(orgLines[i].x, 0, orgLines[i].z));
            if (dis < 5f) IsPassedLines[i] = true;
            if (IsPassedLines[i])
            {
                lineRenderer.SetPosition(i, new Vector3(carTr.position.x, carTr.position.y + 10f, carTr.position.z));
            }
            else
            {
                lineRenderer.SetPosition(i, new Vector3(orgLines[i].x, carTr.position.y + 10f, orgLines[i].z));
            }
        }
    }
}
