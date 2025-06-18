using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{

    public static FMODEvents instance { get; private set; }


    [Header("Player SFX")]
    [field: SerializeField] public EventReference playerFootSteps { get; private set; }


    [Header("Car SFX")]
    [field: SerializeField] public EventReference carRide { get; private set; }
    [field: SerializeField] public EventReference carDrive { get; private set; }
    [field: SerializeField] public EventReference carCrash { get; private set; }
    [field: SerializeField] public EventReference carLight { get; private set; }
    [field: SerializeField] public EventReference carCol { get; private set; }

    [Header("Radar SFX")]
    [field: SerializeField] public EventReference radar { get; private set; }

    [Header("DeerEvent SFX")]
    [field: SerializeField] public EventReference deerCrying { get; private set; }
    [field: SerializeField] public EventReference deerFootsteps { get; private set; }

    [Header("BoomGateEvent SFX")]
    [field: SerializeField] public EventReference boomGateBarSound { get; private set; }
    [field: SerializeField] public EventReference fuseOff { get; private set; }

    [Header("TrafficLightEvent SFX")]
    [field: SerializeField] public EventReference humanFall { get; private set; }
    [field: SerializeField] public EventReference radio_TL { get; private set; }

    [Header("Creature SFX")]
    [field: SerializeField] public EventReference creatureHowl { get; private set; }
    [field: SerializeField] public EventReference creatureStep { get; private set; }

    [Header("Radio")]
    [field: SerializeField] public EventReference radio { get; private set; }
    [field: SerializeField] public EventReference radio2 { get; private set; }
    [field: SerializeField] public EventReference radio3 { get; private set; }
    [field: SerializeField] public EventReference radio4 { get; private set; }
    [field: SerializeField] public EventReference radio5 { get; private set; }
    [field: SerializeField] public EventReference radio6 { get; private set; }
    [field: SerializeField] public EventReference radio7 { get; private set; }

    [Header("OST")]
    [field: SerializeField] public EventReference title { get; private set; }

    [Header("SoundAndLight")]
    [field: SerializeField] public EventReference soundLoud { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the Scene");
        }
        instance = this;
    }

}
