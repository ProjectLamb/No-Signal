using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CarRide : MonoBehaviour
{
    public MonoBehaviour CarController;
    public Camera carCam;

    public GameObject player;
    public GameObject playerBody;
    public GameObject carEnterUI;

    private bool IsCanDrive;
    public static bool IsRide;
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
            AudioManager.instance.PlayOneShot(FMODEvents.instance.carRide,this.transform.position);

            playerBody.transform.SetParent(this.transform);
            playerBody.gameObject.SetActive(false);

            carCam.enabled = true;
            CarController.enabled = true;
            carEnterUI.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.F) && IsCanDrive)
        {
            CarController.enabled = false;
            playerBody.gameObject.SetActive(true);
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
            player.transform.SetParent(this.transform);
            carEnterUI.SetActive(false);
            IsCanDrive = false;
        }
    }
}
