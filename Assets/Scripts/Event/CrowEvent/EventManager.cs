using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EventManager : MonoBehaviour
{
    private static EventManager instance;
    public static EventManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<EventManager>();
            }
            return instance;
        }
    }
    public List<PlayableDirector> playableDirectors;
    public PlayableDirector currentEvent;

    public void SetEvent(int num)
    {
        currentEvent = playableDirectors[num];
    }
    public void PlayEvent()
    {
        //playableDirector.gameObject.SetActive(true);
        currentEvent.Play();
    }
    
}
