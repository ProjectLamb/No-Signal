using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCam : MonoBehaviour
{
    private float moveSmoothness;
    private float rotSmoothness;

    private Vector3 moveOffset;
    private Vector3 rotOffset;

    public Transform target;


    // Update is called once per frame
    void Update()
    {
        //CameraMove();
        //CameraRot();

        this.transform.position = this.target.position;
    }

    void CameraMove()
    {
        Vector3 targetPos = new Vector3();
        targetPos = target.transform.TransformPoint(moveOffset);

        transform.position = Vector3.Lerp(transform.position, targetPos, moveSmoothness * Time.deltaTime);
    }

    void CameraRot()
    {
        var dir = target.transform.position - transform.position;
        var rot = new Quaternion();

        rot = Quaternion.LookRotation(dir + rotOffset, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotSmoothness * Time.deltaTime);
    }
}
