// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CrossCheckTrigger : MonoBehaviour
// {
//     public GameObject Car;
//     void OnTriggerEnter(Collider collider)
//     {
//         if(collider.gameObject == Car)
//         //리팩터링 필요
//         {   
//             if(eventStart != null)
//         StopCoroutine(eventStart);
    
//         eventStart = StartCoroutine(TLEvent_GameOver());
//         };
//     }

//     private IEnumerator TLEvent_GameOver(){
        
//     }
// }
