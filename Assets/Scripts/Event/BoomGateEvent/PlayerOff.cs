using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerOff: MonoBehaviour
{
   public PlayableDirector PlayableDirector;

   public void PlayerActiveOff(){
    CameraFollow.isEvent = false;
    BoomGateEventTrigger.isBoomEvent = false;
    PlayableDirector.gameObject.SetActive(false);
            //BoomGateEventTrigger off

   }
}
