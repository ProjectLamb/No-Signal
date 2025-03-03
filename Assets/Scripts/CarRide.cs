using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class CarRide : MonoBehaviour
{
    [SerializeField] private EventReference carEntered;
    public MonoBehaviour CarController;
    public Camera carCam;
    public GameObject player;
    public GameObject carEnterUI;

    private bool IsCanDrive;
    private bool IsRide;
    void Start()
    {
        CarController.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && IsCanDrive && !IsRide)
        {
            IsRide = true;
            //차 타는소리 재생
            AudioManager.instance.PlayOneShot(carEntered,this.transform.position);

            player.transform.SetParent(this.transform);
            player.gameObject.SetActive(false);

            carCam.enabled = true;
            CarController.enabled = true;
        }

        if(Input.GetKeyDown(KeyCode.F) && IsCanDrive)
        {
            CarController.enabled = false;
            player.gameObject.SetActive(true);
            carCam.enabled = false;
            IsRide = false;
        }
    }
    
    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            carEnterUI.SetActive(true);
            IsCanDrive = true;   
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            carEnterUI.SetActive(false);
            IsCanDrive = false;
        }
    }
}
