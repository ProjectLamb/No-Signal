using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class BoomGateTimelineController : MonoBehaviour
{   
    public PlayableDirector PlayableDirector;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            PlayableDirector.gameObject.SetActive(true);
            PlayableDirector.Play();
        }
    }
}
