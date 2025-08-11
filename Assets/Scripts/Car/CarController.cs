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
    public static bool IsEndingStart = false;
    public static bool IsTutorialEnd = false;

    public Vector3 _centerOfMass;
    public Transform modelTr;
    public Transform crowCheat;
    public Transform boomgateCheat;
    public Transform deerCheat;
    public Transform trafficLightCheat;
    public Transform creatureCheat;
    public Transform junctionCheat;
    public Transform oakTree;

    public GameObject creature;
    public GameObject creatureDct;
    public List<Wheel> wheels;
    public GameObject steeringWheel;
    public GameObject HeadLight;
    public GameObject GameOverPanel;
    public GameObject gameEndPanel;
    public GameObject soundGauge;
    public GameObject warnText;
    public GameObject originBody;
    public GameObject brokenBody;
    public GameObject letterBox;
    public GameObject insectSound;
    public GameObject closedTree;
    public GameObject deerRush;
    public GameObject deerEvent;
    public GameObject boomGateEvent;

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
    private float treeSearchRad = 100f;

    private bool hasReachedMaxSpeed = false;
    private bool IsEngineStart = false;
    private bool IsSoundWarning = false;
    private bool IsCreatureDct = false;
    private bool IsDctDie = false;
    private bool IsRadioOn = false;
    private bool IsRadioTime = false;
    private bool IsChaseEventStart = false;
    private bool IsChased = false;
    private bool IsRushTreeStart = false;
    private bool IsFinalCreature = false;
    private bool IsWrongWay = false;
    private bool IsAxelPressed = false;

    private Rigidbody carRb;
    private Quaternion initialSteeringRotation;

    private EventInstance carDrive;
    private EventInstance soundLoud;
    private EventInstance radio;
    private EventInstance radio2;
    private EventInstance radio3;
    private EventInstance radio4;
    private EventInstance radio5;
    private EventInstance radio6;
    private EventInstance chaseBackground;

    void Awake()
    {
        carDrive = AudioManager.Instance.CreateInstance(FMODEvents.instance.carDrive);
        soundLoud = AudioManager.Instance.CreateInstance(FMODEvents.instance.soundLoud);
        chaseBackground = AudioManager.Instance.CreateInstance(FMODEvents.instance.chaseBackground);

        radio = AudioManager.Instance.CreateInstance(FMODEvents.instance.radio);
        radio2 = AudioManager.Instance.CreateInstance(FMODEvents.instance.radio2);
        radio3 = AudioManager.Instance.CreateInstance(FMODEvents.instance.radio3);
        radio4 = AudioManager.Instance.CreateInstance(FMODEvents.instance.radio4);
        radio5 = AudioManager.Instance.CreateInstance(FMODEvents.instance.radio5);
        radio6 = AudioManager.Instance.CreateInstance(FMODEvents.instance.radio6);
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

        GameManager.Instance.IsDeathEvent = false;
        // 스폰 위치 지정
        if (SaveLoadManager.Instance.IsTrafficClear)
        {
            this.transform.position = boomgateCheat.position;
            this.transform.rotation = boomgateCheat.rotation;
        }
        if (SaveLoadManager.Instance.IsCargateClear)
        {
            this.transform.position = deerCheat.position;
            this.transform.rotation = deerCheat.rotation;
        }
        if (SaveLoadManager.Instance.IsDeerClear)
        {
            this.transform.position = creatureCheat.position;
            this.transform.rotation = creatureCheat.rotation;
        }
        if (SaveLoadManager.Instance.IsChaseEvent)
        {
            this.transform.position = creatureCheat.position;
            this.transform.rotation = creatureCheat.rotation;
        }
    }

    void Update()
    {
        if (IsGameOver)
        {
            if (vhsVolume.profile.TryGet(out vvs))
            {
                if (vvs._weight.value < 0.99f)
                {
                    vvs._weight.value += 0.02f;
                }
                else
                    GameOverPanel.SetActive(true);
            }
            return;
        }
        if (IsWrongWay && closedTree != null && !IsGameOver)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, closedTree.transform.position, 50f * Time.deltaTime);
            return;
        }
        if (GameManager.Instance.IsTutorial || GameManager.Instance.IsCargateEvent) return;
        if (IsEndingStart) RushToTree();

        if (IsChaseEventStart)
        {
            TurnOffRadio();
            soundGauge.SetActive(false);

            carRb.velocity = Vector3.Lerp(carRb.velocity, Vector3.zero, 1f * Time.deltaTime);
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 1000 * brakeAcceleration * Time.deltaTime;
                if (wheel.wheelCollider.motorTorque < 0)
                {
                    wheel.wheelCollider.motorTorque = 0;
                }
            }
            if (carRb.velocity.magnitude > 0.5f) EngineSound();
            return;
        }

        GetInputs();
        AnimateWheels();
        // 엔진사운드 시스템
        EngineSound();
        Vibrate();
        if (!IsChased)
        {
            SoundDetect();
            RandomRadio();
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) IsAxelPressed = true;
        else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) IsAxelPressed = false;

        if (IsAxelPressed && !IsEngineStart)
        {
            IsEngineStart = true;
            carDrive.start();
        }
        if (IsTutorialEnd)
        {
            IsTutorialEnd = false;
            carDrive.start();
        }
        
        // 치트코드 모음
        if (Input.GetKeyDown(KeyCode.F2))
        {
            this.transform.position = trafficLightCheat.position;
            this.transform.rotation = trafficLightCheat.rotation;
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            this.transform.position = boomgateCheat.position;
            this.transform.rotation = boomgateCheat.rotation;
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            this.transform.position = deerCheat.position;
            this.transform.rotation = deerCheat.rotation;
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            this.transform.position = creatureCheat.position;
            this.transform.rotation = creatureCheat.rotation;
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            this.transform.position = junctionCheat.position;
            this.transform.rotation = junctionCheat.rotation;
        }

        // 헤드라이트 키
        if (Input.GetKeyDown(KeyCode.F) && !BoomGateEventTrigger.isBoomEvent)
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
        if (GameManager.Instance.IsTutorial || GameManager.Instance.IsCargateEvent) return;
        if (IsGameOver) return;
        if (!BoomGateEventTrigger.isBoomEvent && !IsChaseEventStart)
        {
            Move();
            Steer();
            Brake();

            float currentSpeed = carRb.velocity.magnitude;
            float currentSpeedKmh = currentSpeed * 3.6f;

            if (currentSpeed > maxSpeed)
            {
                carRb.velocity = carRb.velocity.normalized * maxSpeed;

                if (!hasReachedMaxSpeed)
                {
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
            wheels[i].wheelCollider.motorTorque = moveInput * 650 * maxAcceleration * Time.deltaTime;
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
        if (col.gameObject.CompareTag("CrowEvent"))
        {
            CrowEvent.IsEventStart = true;
            Destroy(col.gameObject);
        }

        if (col.gameObject.CompareTag("BoomGateEvent"))
        {
            carDrive.stop(STOP_MODE.IMMEDIATE);
            TurnOffRadio();
            GameManager.Instance.IsCargateEvent = true;
        }

        if (col.gameObject.CompareTag("DeerEvent"))
        {
            DeerEvent.IsEventStart = true;
            Destroy(col.gameObject);
            AudioManager.Instance.PlayOneShot(FMODEvents.instance.deerCrying, this.transform.position);
            soundFill.fillAmount += 0.05f;
        }

        if (col.gameObject.CompareTag("CreatureEvent"))
        {
            soundFill.fillAmount = 0f;
            IsSoundWarning = false;
            vvs._weight.value = 0.3f;

            this.GetComponent<Animator>().enabled = false;
            letterBox.SetActive(true);

            SaveLoadManager.Instance.IsTrafficClear = true;
            SaveLoadManager.Instance.SaveGameData();

            cinemachineBrain.enabled = true;

            EventManager.Instance.SetEvent(1);
            EventManager.Instance.PlayEvent();
            Destroy(col.gameObject);
        }
        if (col.gameObject.CompareTag("Junction"))
        {
            GameManager.Instance.IsJunctionEvent = true;
            Creature.IsJunction = true;
            AudioManager.Instance.PlayOneShot(FMODEvents.instance.goLeft, this.transform.position);

            col.gameObject.SetActive(false);
            // 크리처 등장 (isCancreatureattach)
            // 크리처 유리에 붙음
            // 내비게이션 우회
            // 전 음성 재생
        }
        if (col.gameObject.CompareTag("DeerRush") && IsChased)
        {
            deerRush.SetActive(true);
        }
        if (col.gameObject.CompareTag("LeftWay"))
        {
            FindTree();
            IsWrongWay = true;
            col.gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (!IsChased && !BoomGateEventTrigger.isBoomEvent && !GameManager.Instance.IsCargateEvent)
        {
            if (!col.gameObject.CompareTag("Road") && !col.gameObject.CompareTag("Creature") && !col.gameObject.CompareTag("Car"))
            {
                soundFill.fillAmount += 0.02f; // 사운드 소리 
                AudioManager.Instance.PlayOneShot(FMODEvents.instance.carCol, this.transform.position);
            }
        }
        if (col.gameObject.CompareTag("Deer"))
        {
            originBody.SetActive(false);
            brokenBody.SetActive(true);
            StartCoroutine("WaitForDeer");
            AudioManager.Instance.PlayOneShot(FMODEvents.instance.carCrash, this.transform.position);
            soundFill.fillAmount += 0.1f;
        }
        if (col.gameObject.CompareTag("Creature") && !GameManager.Instance.IsJunctionEvent)
        {
            TurnOffRadio();
            cinemachineBrain.enabled = true;
            carRb.isKinematic = true;
            carDrive.stop(STOP_MODE.IMMEDIATE);
        }
        if (col.gameObject.CompareTag("Oak") && IsChased)
        {
            IsEndingStart = false;
            chaseBackground.stop(STOP_MODE.IMMEDIATE);
            EventManager.Instance.SetEvent(2);
            EventManager.Instance.PlayEvent();
        }
        if (col.gameObject.CompareTag("Tree") && IsWrongWay)
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.instance.carCrash, this.transform.position);
            IsGameOver = true;
            carDrive.stop(STOP_MODE.IMMEDIATE);
            AudioManager.Instance.PlayOneShot(FMODEvents.instance.introNoise, this.transform.position);
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

        SaveLoadManager.Instance.IsDeerClear = true;
        boomGateEvent.SetActive(false);
        deerEvent.SetActive(false);
        SaveLoadManager.Instance.SaveGameData();
    }

    void ToggleHeadlights()
    {
        if (IsChaseEventStart) return;
        AudioManager.Instance.PlayOneShot(FMODEvents.instance.carLight, this.transform.position);
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
        if (carRb.velocity.magnitude < 0.1f && carRb.velocity.magnitude > -0.1f)
        {
            IsEngineStart = false;
            carDrive.stop(STOP_MODE.ALLOWFADEOUT);
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
        if (IsDctDie) return; // 게이지를 100을 이미 채웠다면
        //엔진 사운드 감지
        if (IsEngineStart && engineSoundFill < 0.1f)
        {
            soundFill.fillAmount += 0.01f * Time.deltaTime;
            engineSoundFill += 0.01f * Time.deltaTime;
        }
        if (!IsEngineStart && engineSoundFill > 0f)
        {
            soundFill.fillAmount -= 0.02f * Time.deltaTime;
            engineSoundFill -= 0.02f * Time.deltaTime;
        }
        if (IsRadioOn) soundFill.fillAmount += 0.02f * Time.deltaTime;
        // 까마귀 울음소리리
        if (IsCrowCaw)
        {
            IsCrowCaw = false;
            soundFill.fillAmount += 0.05f;
        }
        if (!IsEngineStart && !IsRadioOn) soundFill.fillAmount -= 0.02f * Time.deltaTime;
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
            AudioManager.Instance.PlayOneShot(FMODEvents.instance.creatureHowl, this.transform.position);
            IsCreatureDct = true;
            IsDctDie = true;
            GameManager.Instance.IsDeathEvent = true;

            StartCoroutine("BadGameOver");
        }

    }

    IEnumerator BadGameOver()
    {
        yield return new WaitForSeconds(1.5f);
        carRb.isKinematic = true;
        soundLoud.stop(STOP_MODE.ALLOWFADEOUT);
        cinemachineBrain.enabled = true;
        carDrive.stop(STOP_MODE.IMMEDIATE);
        TurnOffRadio();
        creatureDct.SetActive(true);
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
        if ((int)radioPassedTime != 0 && (int)radioPassedTime % 15 == 0 && !IsRadioTime && !IsRadioOn)
        {
            IsRadioTime = true;
            int ran = UnityEngine.Random.Range(0, 100);
            if (ran <= (int)radioPassedTime && !IsRadioOn)
            {
                IsRadioOn = true;
                radioCh = (int)UnityEngine.Random.Range(1, 7);
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
                }
                radioPassedTime = 0f;
                // 초기화
            }
        }
        if ((int)radioPassedTime % 15 != 0) IsRadioTime = false;
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
        }
        IsRadioOn = false;
    }
    public void ChaseCarBreak()
    {
        IsChaseEventStart = true;
        insectSound.SetActive(false);

        SaveLoadManager.Instance.IsChaseEvent = true;
        SaveLoadManager.Instance.SaveGameData();
        deerEvent.SetActive(false);
        boomGateEvent.SetActive(false);
    }
    public void ChaseCarStop()
    {
        carDrive.stop(STOP_MODE.ALLOWFADEOUT);
        HeadLight.SetActive(false);
        //carRb.isKinematic = true;
    }

    public void CreatureReveal()
    {
        HeadLight.SetActive(true);
        creature.SetActive(true);
        chaseBackground.start();
    }

    public void ChaseCarStart()
    {
        cinemachineBrain.enabled = false;
        LetterBox.IsLetterBoxOut = true;
        carDrive.start();
        carRb.isKinematic = false;
        IsChaseEventStart = false;
        IsChased = true;
    }
    public void WarnTextOn()
    {
        this.GetComponent<Animator>().enabled = true;
        StartCoroutine("WarnTextEffect");
    }

    public void RushToTree()
    {
        if (!IsRushTreeStart)
        {
            IsRushTreeStart = true;
            carRb.isKinematic = false;
            AudioManager.Instance.PlayOneShot(FMODEvents.instance.creatureAttach, this.transform.position);
        }
        this.transform.position = Vector3.MoveTowards(transform.position, oakTree.position, 20f * Time.deltaTime);
        float distance = Vector3.Distance(transform.position, oakTree.position);
        if (distance < 5f && !IsFinalCreature)
        {
            IsFinalCreature = true;
            AudioManager.Instance.PlayOneShot(FMODEvents.instance.carSliding, this.transform.position);
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

    public void Die()
    {
        IsGameOver = true;
    }

    void FindTree()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, treeSearchRad);
        float closestDistance = Mathf.Infinity;
        GameObject nearest = null;

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Tree"))
            {
                float distance = (hit.transform.position - transform.position).sqrMagnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearest = hit.gameObject;
                }
            }
        }

        closedTree = nearest;
    }
}
