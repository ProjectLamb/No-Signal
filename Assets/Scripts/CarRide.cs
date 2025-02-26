using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRide : MonoBehaviour
{
    // Start is called before the first frame update
    public MonoBehaviour CarController;
    public Camera carCam;
    public GameObject player;
    public GameObject carEnterUI;

    private bool IsCanDrive;
    void Start()
    {
        CarController.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsCanDrive) carEnterUI.SetActive(true);
        else carEnterUI.SetActive(false);

        if(Input.GetKeyDown(KeyCode.E) && IsCanDrive)
        {
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
            IsCanDrive = false;
        }
    }
    
    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            Debug.Log("충돌");
            IsCanDrive = true;   
            //CarController.enabled = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            IsCanDrive = false;
        }
    }
}
