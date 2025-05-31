using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;
//using FMODUnity;

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
    public float maxSpeedRatio = 350f;

    private float speed;
    private float rpm;
    private float pitch;
    private float speedRatio;
    private float radioCh;

    public Vector3 _centerOfMass;
    public Transform crowCheat;
    public Transform boomgateCheat;
    public Transform deerCheat;
    public Transform trafficLightcrowCheat;
    public Transform creatureCheat;
    public GameObject creature;

    public List<Wheel> wheels;
    public GameObject steeringWheel; //�ڵ�
    public Image deerBlack;

    private Quaternion initialSteeringRotation;

    float moveInput;
    float steerInput;

    private Rigidbody carRb;

    private EventInstance carDrive;
    private EventInstance carLight;
    private EventInstance deerCrying;
    private EventInstance radio;
    private EventInstance radio2;
    private EventInstance radio3;
    private EventInstance radio4;
    private EventInstance radio5;
    private EventInstance radio6;
    private EventInstance radio7;

    public GameObject HeadLight;

    private bool IsHeadlightsOn = false;
    private bool IsEngineStart = false;
    
    float maxSpeed = 100f;
    bool hasReachedMaxSpeed = false;

    void Awake()
    {
        carDrive = AudioManager.instance.CreateInstance(FMODEvents.instance.carDrive);
        carLight = AudioManager.instance.CreateInstance(FMODEvents.instance.carLight);
        deerCrying = AudioManager.instance.CreateInstance(FMODEvents.instance.deerCrying);

        radio = AudioManager.instance.CreateInstance(FMODEvents.instance.radio);
        radio2 = AudioManager.instance.CreateInstance(FMODEvents.instance.radio2);
        radio3 = AudioManager.instance.CreateInstance(FMODEvents.instance.radio3);
        radio4 = AudioManager.instance.CreateInstance(FMODEvents.instance.radio4);
        radio5 = AudioManager.instance.CreateInstance(FMODEvents.instance.radio5);
        radio6 = AudioManager.instance.CreateInstance(FMODEvents.instance.radio6);
        radio7 = AudioManager.instance.CreateInstance(FMODEvents.instance.radio7);
    }
    
    void Start()
    {
        rpm = 0;
        radioCh = 0;
        carDrive.getPitch(out pitch);

        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;

        HeadLight.SetActive(false);

        if (steeringWheel != null) //�ڵ�
        {
            initialSteeringRotation = steeringWheel.transform.localRotation; // �ʱ� ȸ���� ����
        }
    }

    void Update()
    {
        if (!BoomGateEventTrigger.isBoomEvent)
        {
            GetInputs();
            AnimateWheels();
        }

        // 엔진사운드 시스템
        EngineSound();

        if (Input.GetKeyDown(KeyCode.W) && !IsEngineStart)
        {
            carDrive.start();
            IsEngineStart = true;
        }

        // 치트코드1
        if (Input.GetKeyDown(KeyCode.F1))
        {
            this.transform.position = crowCheat.position;
            this.transform.rotation = crowCheat.rotation;
        }
        // 치트코드2
        if (Input.GetKeyDown(KeyCode.F2))
        {
            this.transform.position = boomgateCheat.position;
            this.transform.rotation = boomgateCheat.rotation;
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            this.transform.position = deerCheat.position;
            this.transform.rotation = deerCheat.rotation;
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            this.transform.position = trafficLightcrowCheat.position;
            this.transform.rotation = trafficLightcrowCheat.rotation;
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            this.transform.position = creatureCheat.position;
            this.transform.rotation = creatureCheat.rotation;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (BoomGateEventTrigger.isBoomEvent) return;
            ToggleHeadlights();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (BoomGateEventTrigger.isBoomEvent) return;
            TurnRadio();
        }
        
        if (Input.GetKeyDown(KeyCode.P)) // 어디 꼈을때 위치 재조정
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 3f, this.transform.position.z);
        }
    }

    void FixedUpdate()
    {
        if (!BoomGateEventTrigger.isBoomEvent)
        {
            Move();
            Steer();
            Brake();


            float currentSpeed = carRb.velocity.magnitude;
            //Debug.Log("현재 속도: " + currentSpeed.ToString("F2") + " m/s");
            float currentSpeedKmh = currentSpeed * 3.6f;
            //Debug.Log("현재 속도: " + currentSpeedKmh.ToString("F2") + " km/h");

            if (currentSpeed > maxSpeed)
            {
                carRb.velocity = carRb.velocity.normalized * maxSpeed;

                if (!hasReachedMaxSpeed)
                {
                    Debug.Log("최고 속도 도달");
                    hasReachedMaxSpeed = true;
                }
            }
            else
            {
                hasReachedMaxSpeed = false;
            }
        }
    }

    void GetInputs()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    void Move()
    {
        for (int i = 0; i < wheels.Count; i++)
        {
            wheels[i].wheelCollider.motorTorque = moveInput * 600 * maxAcceleration * Time.deltaTime;
            
            if (wheels[i].wheelCollider.motorTorque <= 0)
            {
                IsEngineStart = false;
            }
        }
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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }

    void AnimateWheels()
    {
        foreach (var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "CrowEvent")
        {
            CrowEvent.IsEventStart = true;
            Destroy(col.gameObject);
        }

        if (col.gameObject.tag == "DeerEvent")
        {
            DeerEvent.IsEventStart = true;
            Destroy(col.gameObject);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.deerCrying, this.transform.position);
        }

        if (col.gameObject.tag == "Creature")
        {
            creature.SetActive(true);
            Destroy(col.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Deer")
        {
            StartCoroutine("WaitForDeer");
            AudioManager.instance.PlayOneShot(FMODEvents.instance.carCrash, this.transform.position);
        }
    }

    IEnumerator WaitForDeer()
    {
        yield return new WaitForSeconds(0.1f);
        deerBlack.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        deerBlack.gameObject.SetActive(false);
    }

    void ToggleHeadlights()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.carLight, this.transform.position);
        IsHeadlightsOn = !IsHeadlightsOn;
        HeadLight.SetActive(IsHeadlightsOn);
    }

    void TurnRadio()
    {
        switch (radioCh)
        {
            case 0:
                radio.start();
                radioCh++;
                break;
            case 1:
                radio.stop(STOP_MODE.IMMEDIATE);
                radio2.start();
                radioCh++;
                break;
            case 2:
                radio2.stop(STOP_MODE.IMMEDIATE);
                radio3.start();
                radioCh++;
                break;
            case 3:
                radio3.stop(STOP_MODE.IMMEDIATE);
                radio4.start();
                radioCh++;
                break;
            case 4:
                radio4.stop(STOP_MODE.IMMEDIATE);
                radio5.start();
                radioCh++;
                break;
            case 5:
                radio5.stop(STOP_MODE.IMMEDIATE);
                radio6.start();
                radioCh++;
                break;
            case 6:
                radio6.stop(STOP_MODE.IMMEDIATE);
                radio7.start();
                radioCh++;
                break;
            case 7:
                radio7.stop(STOP_MODE.IMMEDIATE);
                radioCh = 0;
                break;
        }
    }

    void EngineSound()
    {
        speed = wheels[0].wheelCollider.rpm * 2f * Mathf.PI / 10f;
        speedRatio = speed * Mathf.Clamp(moveInput, 0.5f, 1f) / maxSpeedRatio;
        pitch = Mathf.Lerp(0.3f, 1f, speedRatio);
        carDrive.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));
        carDrive.setPitch(pitch);

        if (speed > 0.1f)
        {
            rpm += 0.1f;
            carDrive.setParameterByName("RPM", rpm);
        }
        else
        {
            rpm -= 0.1f;
            carDrive.setParameterByName("RPM", rpm);
        }

        if (speed < 0.1f && speed > -0.1f)
        {
            IsEngineStart = false;
        }
    }

}
