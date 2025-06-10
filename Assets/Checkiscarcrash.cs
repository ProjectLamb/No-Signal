using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkiscarcrash : MonoBehaviour
{
    // Start is called before the first frame update
    
    public GameObject Car;
    // Update is called once per frame
    void OnCollisonEnter(Collision collision){
    if(collision.gameObject == Car){
        //사운드 재생
        Debug.Log("칫 결계인가");
    }
    }
}
