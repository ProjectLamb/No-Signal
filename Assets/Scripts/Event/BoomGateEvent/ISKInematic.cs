using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ISKInematic : MonoBehaviour
{
    // Start is called before the first frame update

    public Rigidbody Rigidbody;

   public void IsKinematicOff(){
    Rigidbody carRb = Rigidbody;
    carRb.isKinematic = false;
}

    // Update is called once per frame
}
