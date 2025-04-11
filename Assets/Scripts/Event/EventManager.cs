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
    public PlayableDirector playableDirector;

    public void SetEvent(int num)
    {
        playableDirector = playableDirectors[num];
    }
    public void PlayEvent()
    {
        //playableDirector.gameObject.SetActive(true);
        playableDirector.Play();
    }
    
}
