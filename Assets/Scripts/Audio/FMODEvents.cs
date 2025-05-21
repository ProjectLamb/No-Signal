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

    [Header("Radar SFX")]
    [field: SerializeField] public EventReference radar { get; private set; }

    [Header("Creature SFX")]
    [field: SerializeField] public EventReference creature { get; private set; }

    [Header("OST")]
    [field: SerializeField] public EventReference title { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the Scene");
        }
        instance = this;
    }

}
