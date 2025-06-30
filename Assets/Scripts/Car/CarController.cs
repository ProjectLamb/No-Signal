using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using VolFx;
using Cinemachine;

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
    public float maxSpeed = 100f;
    public static float lightOffTime = 0f;
    public static bool IsHeadlightsOn = false;
    public static bool IsCrowCaw = false;
    public static bool IsGameOver = false;
    public static bool IsChaseEventStart = false;
    public static bool IsEndingStart = false;

    public Vector3 _centerOfMass;
    public Transform modelTr;
    public Transform crowCheat;
    public Transform boomgateCheat;
    public Transform deerCheat;
    public Transform trafficLightcrowCheat;
    public Transform creatureCheat;
    public Transform oakTree;
    public GameObject creature;
    public GameObject creatureDct;
    public List<Wheel> wheels;
    public GameObject steeringWheel;
    public GameObject HeadLight;
    public GameObject gameOverPanel;
    public GameObject gameEndPanel;
    public GameObject soundGauge;
    public GameObject warnText;
    public GameObject originBody;
    public GameObject brokenBody;
    public GameObject letterBox;
    public Image deerBlack;
    public Image soundFill;
    public Volume vhsVolume;
    private VhsVol vvs;
    public CinemachineBrain cinemachineBrain;

    private int radioCh;
    private float speed;
    private float rpm;
    private float pitch;
    private float speedRatio;
    private float moveInput;
    private float steerInput;
    private float vibeSpeed = 1f;
    private float intensity = 0.001f;
    private float engineSoundFill = 0f;
    private float radioPassedTime = 0;
    private bool hasReachedMaxSpeed = false;
    private bool IsEngineStart = false;
    private bool IsSoundWarning = false;
    private bool IsCreatureDct = false;
    private bool IsPrepareToDead = false;
    private bool IsRadioOn = false;
    private bool IsChased = false;
    private bool IsRushTreeStart = false;
    private bool IsFinalCreature = false;

    private Rigidbody carRb;
    private Quaternion initialSteeringRotation;

    private EventInstance carDrive;
    private EventInstance carLight;
    private EventInstance carCol;
    private EventInstance deerCrying;
    private EventInstance creatureHowl;
    private EventInstance soundLoud;
    private EventInstance radio;
    private EventInstance radio2;
    private EventInstance radio3;
    private EventInstance radio4;
    private EventInstance radio5;
    private EventInstance radio6;
    private EventInstance radio7;
    private EventInstance chaseBackground;
    private EventInstance creatureAttach;
    private EventInstance carSliding;
    private EventInstance carCrash2;

    void Awake()
    {   
        carDrive = AudioManager.instance.CreateInstance(FMODEvents.instance.carDrive);
        carLight = AudioManager.instance.CreateInstance(FMODEvents.instance.carLight);
        carCol = AudioManager.instance.CreateInstance(FMODEvents.instance.carCol);
        deerCrying = AudioManager.instance.CreateInstance(FMODEvents.instance.deerCrying);
        soundLoud = AudioManager.instance.CreateInstance(FMODEvents.instance.soundLoud);
        creatureHowl = AudioManager.instance.CreateInstance(FMODEvents.instance.creatureHowl);
        chaseBackground = AudioManager.instance.CreateInstance(FMODEvents.instance.chaseBackground);
        creatureAttach = AudioManager.instance.CreateInstance(FMODEvents.instance.creatureAttach);
        carSliding = AudioManager.instance.CreateInstance(FMODEvents.instance.carSliding);
        carCrash2 = AudioManager.instance.CreateInstance(FMODEvents.instance.carCrash2);

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
        if (GameManager.Instance.IsTutorial) return;
        if (GameManager.Instance.IsTutorialFirst) return;
        if (IsEndingStart) RushToTree();
        if (IsGameOver)
        {
            if (vhsVolume.profile.TryGet(out vvs))
            {
                if (vvs._weight.value < 0.99f)
                {
                    vvs._weight.value += 0.02f;
                }
                else
                    gameOverPanel.SetActive(true);
            }
            return;
        }

        if (IsChaseEventStart)
        {
            TurnOffRadio();
            soundGauge.SetActive(false);
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 1000 * brakeAcceleration * Time.deltaTime;
                if (wheel.wheelCollider.motorTorque < 0)
                {
                    wheel.wheelCollider.motorTorque = 0;
                    carRb.velocity = Vector3.Lerp(carRb.velocity, Vector3.zero, 5f * Time.deltaTime);
                    if (carRb.velocity.magnitude < 1.5f) carRb.velocity = new Vector3(0, 0, 0);
                }
            }
            return;
        }
        if (!BoomGateEventTrigger.isBoomEvent && !IsChaseEventStart)
        {
            GetInputs();
            AnimateWheels();
        }
        if (!IsChased && !BoomGateEventTrigger.isBoomEvent)
        {
            // 엔진사운드 시스템
            EngineSound();
            Vibrate();
            SoundDetect();
            RandomRadio();
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) && !IsEngineStart)
        {
            IsEngineStart = true;
            carDrive.start();
        }

        // 치트코드 모음
        if (Input.GetKeyDown(KeyCode.F1))
        {
            this.transform.position = crowCheat.position;
            this.transform.rotation = crowCheat.rotation;
        }

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

        // 헤드라이트 키
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleHeadlights();
        }
        // 라디오 키
        if (Input.GetKeyDown(KeyCode.R) && IsRadioOn)
        {
            TurnOffRadio();
        }

        if (!IsHeadlightsOn) lightOffTime += Time.deltaTime;
        else if (IsHeadlightsOn) lightOffTime = 0f;

        if (Input.GetKeyDown(KeyCode.P)) // 어디 꼈을때 위치 재조정
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 3f, this.transform.position.z);
        }
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.IsTutorial) return;
        if (GameManager.Instance.IsTutorialFirst) return;
        if (IsGameOver) return;
        if (!BoomGateEventTrigger.isBoomEvent && !IsChaseEventStart)
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
        }
    }
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
                wheel.wheelCollider.brakeTorque = 350 * brakeAcceleration * Time.deltaTime;
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
            soundFill.fillAmount += 0.05f;
        }

        if (col.gameObject.tag == "CreatureEvent")
        {
            letterBox.SetActive(true);
            GameManager.Instance.IsTrafficClear = true;
            GameManager.Instance.IsDeerClear = true;
            cinemachineBrain.enabled = true;
            EventManager.Instance.SetEvent(1);
            EventManager.Instance.PlayEvent();
            Destroy(col.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        soundFill.fillAmount += 0.02f; // 사운드 소리 
        if (collision.gameObject.tag != "Road")
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.carCol, this.transform.position);
        }
        if (collision.gameObject.tag == "Deer")
        {
            GameManager.Instance.IsDeerClear = true;
            originBody.SetActive(false);
            brokenBody.SetActive(true);
            StartCoroutine("WaitForDeer");
            AudioManager.instance.PlayOneShot(FMODEvents.instance.carCrash, this.transform.position);
            soundFill.fillAmount += 0.1f;
        }
        if (collision.gameObject.tag == "Creature" && !IsChased)
        {
            IsGameOver = true;
        }
        if (collision.gameObject.tag == "Oak" && IsChased)
        {
            IsEndingStart = false;
            chaseBackground.stop(STOP_MODE.IMMEDIATE);
            EventManager.Instance.SetEvent(2);
            EventManager.Instance.PlayEvent();
        }
    }

    public void EndPanelOn()
    {
        gameEndPanel.SetActive(true);
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
        if (IsChaseEventStart) return;
        AudioManager.instance.PlayOneShot(FMODEvents.instance.carLight, this.transform.position);
        IsHeadlightsOn = !IsHeadlightsOn;
        HeadLight.SetActive(IsHeadlightsOn);
        soundFill.fillAmount += 0.05f;
    }

    void EngineSound()
    {
        speed = wheels[0].wheelCollider.rpm * 2f * Mathf.PI / 10f;
        speedRatio = speed * Mathf.Clamp(moveInput, 0.5f, 1f) / maxSpeedRatio;
        if (speedRatio < 0) speedRatio *= -1;
        pitch = Mathf.Lerp(0.3f, 1f, speedRatio);

        carDrive.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.transform));
        carDrive.setPitch(pitch);

        // 속도가 있으면면
        if (speed > 0.1f || speed < -0.1f)
        {
            rpm += 0.1f;
            carDrive.setParameterByName("RPM", rpm);
        }
        else
        {
            rpm -= 0.1f;
            carDrive.setParameterByName("RPM", rpm);
        }

        // 속도가 거의 0이면 엔진소리 off
        if (speed < 0.1f && speed > -0.1f)
        {
            IsEngineStart = false;
        }

        // 차량이 움직이지 못하는 상태면 엔진 off
        if (carRb.isKinematic == true)
        {
            rpm = 0;
            carDrive.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
    void Vibrate()
    {
        modelTr.localPosition = intensity * new Vector3(
            Mathf.PerlinNoise(vibeSpeed * Time.time, 1),
            Mathf.PerlinNoise(vibeSpeed * Time.time, 2),
            Mathf.PerlinNoise(vibeSpeed * Time.time, 3));
    }

    void SoundDetect()
    {
        if (IsPrepareToDead) return; // 게이지를 100을 이미 채웠다면

        //엔진 사운드 감지
        if (IsEngineStart)
        {
            if (engineSoundFill < 0.1f)
            {
                soundFill.fillAmount += 0.01f * Time.deltaTime;
                engineSoundFill += 0.01f * Time.deltaTime;
            }
        }
        if (!IsEngineStart)
        {
            if (engineSoundFill > 0f)
            {
                soundFill.fillAmount -= 0.02f * Time.deltaTime;
                engineSoundFill -= 0.02f * Time.deltaTime;
            }
        }
        // 까마귀 울음소리리
        if (IsCrowCaw)
        {
            IsCrowCaw = false;
            soundFill.fillAmount += 0.05f;
        }
        // 소리바가 70퍼 이상이면
        if (soundFill.fillAmount >= 0.7f)
        {
            if (vhsVolume.profile.TryGet(out vvs))
            {
                if (vvs._weight.value < 0.7f)
                    vvs._weight.value += 0.05f * Time.deltaTime;
            }
        }
        if (soundFill.fillAmount >= 0.7f && !IsSoundWarning)
        {
            IsSoundWarning = true;
            StartCoroutine("SoundLoudWarn");
            soundLoud.start();
        }
        if (soundFill.fillAmount < 0.7f)
        {
            Color soundFillCol = soundFill.color;
            soundFillCol.a = 1;
            soundFill.color = soundFillCol;

            IsSoundWarning = false;
            StopCoroutine("SoundLoudWarn");
            soundLoud.stop(STOP_MODE.ALLOWFADEOUT);
            IsCreatureDct = false;

            if (vhsVolume.profile.TryGet(out vvs))
            {
                if (vvs._weight.value > 0.3f)
                    vvs._weight.value -= 0.05f;
            }
        }

        if (soundFill.fillAmount > 0.99f && !IsCreatureDct)
        {
            IsCreatureDct = true;
            IsPrepareToDead = true;
            soundLoud.stop(STOP_MODE.ALLOWFADEOUT);

            float creatureRanX = UnityEngine.Random.Range(30, 50);
            float creatureRanZ = UnityEngine.Random.Range(30, 50);
            Vector3 creaturePos = new Vector3(this.transform.position.x + creatureRanX, this.transform.position.y + 10f, this.transform.position.z + creatureRanZ);
            Vector3 creatureRot = this.transform.position - creaturePos;
            Quaternion creatureLook = Quaternion.LookRotation(creatureRot);
            creatureDct.transform.position = creaturePos;
            creatureDct.transform.rotation = creatureLook;
            creatureDct.SetActive(true);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.creatureHowl, this.transform.position);
        }
        if (IsRadioOn) soundFill.fillAmount += 0.02f * Time.deltaTime;
    }

    IEnumerator SoundLoudWarn()
    {
        Color soundFillCol = soundFill.color;
        soundFillCol.a = 0;
        while (true)
        {
            soundFill.color = soundFillCol;
            yield return new WaitForSeconds(0.5f);

            soundFillCol.a = 1;

            soundFill.color = soundFillCol;
            yield return new WaitForSeconds(0.5f);

            soundFillCol.a = 0;
        }
    }

    public void RandomRadio()
    {
        radioPassedTime += Time.deltaTime;

        if ((int)radioPassedTime != 0 && (int)radioPassedTime % 10 == 0)
        {
            int ran = UnityEngine.Random.Range(0, 60);
            if (ran <= (int)radioPassedTime && !IsRadioOn)
            {
                IsRadioOn = true;
                radioCh = (int)UnityEngine.Random.Range(1, 8);
                switch (radioCh)
                {
                    case 1:
                        radio.start();
                        break;
                    case 2:
                        radio2.start();
                        break;
                    case 3:
                        radio3.start();
                        break;
                    case 4:
                        radio4.start();
                        break;
                    case 5:
                        radio5.start();
                        break;
                    case 6:
                        radio6.start();
                        break;
                    case 7:
                        radio7.start();
                        break;
                }
            }
        }
    }

    private void TurnOffRadio()
    {
        if (BoomGateEventTrigger.isBoomEvent) return;
        switch (radioCh)
        {
            case 1:
                radio.stop(STOP_MODE.IMMEDIATE);
                break;
            case 2:
                radio2.stop(STOP_MODE.IMMEDIATE);
                break;
            case 3:
                radio3.stop(STOP_MODE.IMMEDIATE);
                break;
            case 4:
                radio4.stop(STOP_MODE.IMMEDIATE);
                break;
            case 5:
                radio5.stop(STOP_MODE.IMMEDIATE);
                break;
            case 6:
                radio6.stop(STOP_MODE.IMMEDIATE);
                break;
            case 7:
                radio7.stop(STOP_MODE.IMMEDIATE);
                break;
        }
        IsRadioOn = false;
    }
    public void ChaseCarBreak()
    {
        IsChaseEventStart = true;
    }
    public void ChaseCarStop()
    {
        carDrive.stop(STOP_MODE.ALLOWFADEOUT);
        HeadLight.SetActive(false);
        carRb.isKinematic = true;
    }

    public void CreatureReveal()
    {
        HeadLight.SetActive(true);
        creature.SetActive(true);
        chaseBackground.start();
    }

    public void ChaseCarStart()
    {
        LetterBox.IsLetterBoxOut = true;
        carDrive.start();
        carRb.isKinematic = false;
        IsChaseEventStart = false;
        IsChased = true;
    }
    public void WarnTextOn()
    {
        StartCoroutine("WarnTextEffect");
    }

    public void RushToTree()
    {
        if (!IsRushTreeStart)
        {
            Creature.IsEnding = true;
            IsRushTreeStart = true;
            carRb.isKinematic = false;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.creatureAttach, this.transform.position);
        }
        this.transform.position = Vector3.MoveTowards(transform.position, oakTree.position, 20f * Time.deltaTime);
        float distance = Vector3.Distance(transform.position, oakTree.position);
        if (distance < 5f && !IsFinalCreature)
        {
            IsFinalCreature = true;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.carSliding, this.transform.position);
        }
    }

    IEnumerator WarnTextEffect()
    {
        warnText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        warnText.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        warnText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        warnText.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        warnText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        warnText.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        warnText.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        warnText.SetActive(false);
    }
}
