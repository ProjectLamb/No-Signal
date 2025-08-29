using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform carTr;
    public bool[] IsPassedLines;
    public bool[] IsPassedFirst;
    private Vector3[] orgLines;
    private Vector3[] curLines;
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
        IsPassedFirst = new bool[lineRenderer.positionCount];

        orgLines = new Vector3[lineRenderer.positionCount];
        curLines = new Vector3[lineRenderer.positionCount];

        lineRenderer.GetPositions(orgLines);
        lineRenderer.alignment = LineAlignment.View;

        curLines[0] = new Vector3(carTr.position.x, carTr.position.y + 10f, carTr.position.z);
        IsPassedLines[0] = true;

        if (SaveLoadManager.Instance.IsTrafficClear)
        {
            for (int i = 1; i <= 13; i++)
            {
                IsPassedLines[i] = true;
            }
        }
        if (SaveLoadManager.Instance.IsCargateClear)
        {
            for (int i = 1; i <= 16; i++)
            {
                IsPassedLines[i] = true;
            }
        }
        if (SaveLoadManager.Instance.IsDeerClear || SaveLoadManager.Instance.IsChaseEvent)
        {
            for (int i = 1; i <= 17; i++)
            {
                IsPassedLines[i] = true;
            }
        }
    }
    void Update()
    {
        for (int i = 0; i < orgLines.Length; i++)
        {
            if (GameManager.Instance.IsEnding)
            {
                curLines[i] = orgLines[i];
                continue;
            }

            dis = Vector3.Distance(new Vector3(carTr.position.x, 0, carTr.position.z), new Vector3(orgLines[i].x, 0, orgLines[i].z));

            if (i < 14 && dis < 20f) IsPassedLines[i] = true;
            if (i >= 14 && dis < 50f) IsPassedLines[i] = true; 

            if (IsPassedLines[i])
            {
                curLines[i] = new Vector3(carTr.position.x, carTr.position.y + 10f, carTr.position.z);
            }
            else
            {
                curLines[i] = orgLines[i];
            }
            lineRenderer.SetPositions(curLines);
        
        }
    }
}
