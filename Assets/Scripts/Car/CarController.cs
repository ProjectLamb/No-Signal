using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;

public class CarController : MonoBehaviour
{
    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;

    public Vector3 _centerOfMass;
    public Transform cheatTr;
    public Transform cheatTr2;

    public List<Wheel> wheels;
    public GameObject steeringWheel; //�ڵ�
    public Image deerBlack;

    private Quaternion initialSteeringRotation;

    float moveInput;
    float steerInput;
    
    private Rigidbody carRb;
    private EventInstance carDrive;
    private bool IsCanDrive;

    void Start()
    {
        IsCanDrive = true;
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;

        carDrive = AudioManager.instance.CreateInstance(FMODEvents.instance.carDrive);

        if (steeringWheel != null) //�ڵ�
        {
            initialSteeringRotation = steeringWheel.transform.localRotation; // �ʱ� ȸ���� ����
        }
    }

    void Update()
    {
        if(!BoomGateEventTrigger.isBoomEvent)
        {
        GetInputs();
        }
        AnimateWheels();

        //Cheat
        if(Input.GetKeyDown(KeyCode.Keypad8))
        {
            this.transform.position = cheatTr.position;
        }

        if(Input.GetKeyDown(KeyCode.Keypad9))
        {
            this.transform.position = cheatTr2.position;
        }
        
        //UpdateSound();
    }

    void FixedUpdate()
    {  
        if(!BoomGateEventTrigger.isBoomEvent)
        {
        Move();
        Steer();
        Brake();
        }
    }

    void GetInputs()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    void Move()
    {
        foreach (var wheel in wheels) { wheel.wheelCollider.motorTorque = moveInput * 600 * maxAcceleration * Time.deltaTime; }
    }
    /*
    void Steer()
    {
        foreach(var wheel in wheels)
        {
            if(wheel.axel == Axel.Front)
            {
                var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }*/
    void Steer()
    {
        float _steerAngle = steerInput * turnSensitivity * maxSteerAngle;

        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }

        if (steeringWheel != null)
        {
            Quaternion targetRotation = initialSteeringRotation * Quaternion.Euler(0, _steerAngle * 2, 0);
            steeringWheel.transform.localRotation = Quaternion.Lerp(steeringWheel.transform.localRotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            foreach(var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;
            }
        }
        else
        {
            foreach( var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
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

    private void UpdateSound()
    {
        if (moveInput != 0)
        {
            Debug.Log("어 형이야");
            PLAYBACK_STATE playbackState;
            carDrive.getPlaybackState(out playbackState);
            if (carDrive.Equals(PLAYBACK_STATE.STOPPED))
            {
                carDrive.start();
            }
        }
        else
        {
            carDrive.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }

    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "CrowEvent")
        {
            CrowEvent.IsEventStart = true;
            Destroy(col.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Deer")
        {
            StartCoroutine("WaitForDeer");
            AudioManager.instance.PlayOneShot(FMODEvents.instance.carCrash, this.transform.position);
            IsCanDrive = false;
        }
    }

    IEnumerator WaitForDeer()
    {
        yield return new WaitForSeconds(0.1f);
        deerBlack.gameObject.SetActive(true);
    }

    
}
