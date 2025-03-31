using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minicar : MonoBehaviour
{
    public enum Axel { Front, Rear };

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    public float maxAcc = 30f;
    public float brakeAcc = 50f;

    public float turnSensitivity = 1f;
    public float maxSteerAngle = 30f;

    public Vector3 centerOfMass;

    public List<Wheel> wheels;

    float moveInput;
    float steerInput;

    private Rigidbody carRb;

    void Awake()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = centerOfMass;
    }

    void Update()
    {
        GetInputs();
        AnimateWheels();
    }

    void LateUpdate()
    {
        Move();
        Steer();
        Brake();
    }

    void GetInputs()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    void Move()
    {
        foreach (Wheel wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * 600 * maxAcc * Time.deltaTime;
        }
    }

    void Steer()
    {
        foreach(Wheel wheel in wheels)
        {
            if(wheel.axel == Axel.Front)
            {
                var steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.5f);
            }
        }
    }

    void AnimateWheels()
    {
     foreach(var wheel in wheels)
     {
        Quaternion rot;
        Vector3 pos;
        wheel.wheelCollider.GetWorldPose(out pos, out rot);
        wheel.wheelModel.transform.position = pos;
        wheel.wheelModel.transform.rotation = rot;
     }   
    }

    void Brake()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            foreach(var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 300 * brakeAcc * Time.deltaTime;
            }
        }
        else
        {
            foreach(var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }
}
