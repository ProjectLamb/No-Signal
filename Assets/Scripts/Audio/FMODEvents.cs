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
    [field: SerializeField] public EventReference carStarting { get; private set; }
    [field: SerializeField] public EventReference headlightBoom { get; private set; }
    [field: SerializeField] public EventReference carEngineOff { get; private set; }
    [field: SerializeField] public EventReference carStartingSuccess { get; private set; }
    [field: SerializeField] public EventReference carUturn { get; private set; }
    [field: SerializeField] public EventReference creatureAttach { get; private set; }
    [field: SerializeField] public EventReference carSliding { get; private set; }
    [field: SerializeField] public EventReference carCrash2 { get; private set; }
    [field: SerializeField] public EventReference carBoom { get; private set; }

    [Header("Radar SFX")]
    [field: SerializeField] public EventReference radar { get; private set; }

    [Header("DeerEvent SFX")]
    [field: SerializeField] public EventReference deerCrying { get; private set; }
    [field: SerializeField] public EventReference deerFootsteps { get; private set; }
    [field: SerializeField] public EventReference deerRush { get; private set; }

    [Header("BoomGateEvent SFX")]
    [field: SerializeField] public EventReference boomGateBarSound { get; private set; }
    [field: SerializeField] public EventReference fuseOff { get; private set; }

    [Header("TrafficLightEvent SFX")]
    [field: SerializeField] public EventReference radio_TL { get; private set; }

    [Header("Creature SFX")]
    [field: SerializeField] public EventReference creatureHowl { get; private set; }
    [field: SerializeField] public EventReference creatureStep { get; private set; }
    [field: SerializeField] public EventReference creatureClick { get; private set; }
    [field: SerializeField] public EventReference creatureDeath { get; private set; }
    [field: SerializeField] public EventReference chaseBackground { get; private set; }
    [field: SerializeField] public EventReference creatureCrying { get; private set; }
    [field: SerializeField] public EventReference creatureDie { get; private set; }
    [field: SerializeField] public EventReference creatureBreathe { get; private set; }
    [field: SerializeField] public EventReference creatureGameOver { get; private set; }


    [Header("Radio")]
    [field: SerializeField] public EventReference radio { get; private set; }
    [field: SerializeField] public EventReference radio2 { get; private set; }
    [field: SerializeField] public EventReference radio3 { get; private set; }
    [field: SerializeField] public EventReference radio4 { get; private set; }
    [field: SerializeField] public EventReference radio5 { get; private set; }
    [field: SerializeField] public EventReference radio6 { get; private set; }

    [Header("Navi")]
    [field: SerializeField] public EventReference navi { get; private set; }
    [field: SerializeField] public EventReference chaseNavi { get; private set; }
    [field: SerializeField] public EventReference goLeft { get; private set; }
    [field: SerializeField] public EventReference naviStart { get; private set; }
    [field: SerializeField] public EventReference naviResearch { get; private set; }
    [field: SerializeField] public EventReference naviBeep { get; private set; }

    [Header("OST")]
    [field: SerializeField] public EventReference title { get; private set; }
    [field: SerializeField] public EventReference gravel { get; private set; }

    [Header("SoundAndLight")]
    [field: SerializeField] public EventReference soundLoud { get; private set; }

    [Header("ETC")]
    [field: SerializeField] public EventReference introNoise { get; private set; }
    [field: SerializeField] public EventReference earSound { get; private set; }
    [field: SerializeField] public EventReference insect { get; private set; }
    [field: SerializeField] public EventReference insect2 { get; private set; }
    [field: SerializeField] public EventReference cricket { get; private set; }
    [field: SerializeField] public EventReference cricket2 { get; private set; }
    [field: SerializeField] public EventReference tutorialPage { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the Scene");
        }
        instance = this;
    }

}
