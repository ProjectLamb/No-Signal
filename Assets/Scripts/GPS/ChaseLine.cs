using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform carTr;
    public bool[] IsPassedLines;
    public bool[] IsPassedFirst;
    private Vector3[] orgLines;
    private Vector3[] curLines;
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
    }

    // Update is called once per frame
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
            if (dis < 50f) IsPassedLines[i] = true;

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
