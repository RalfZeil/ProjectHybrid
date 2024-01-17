using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Ambience")]
    [field: SerializeField] public EventReference kidsPlaying { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }
    [field: SerializeField] public EventReference kick { get; private set; }
    [field: SerializeField] public EventReference snare { get; private set; }
    [field: SerializeField] public EventReference hihat { get; private set; }
    [field: SerializeField] public EventReference guitar { get; private set; }
    [field: SerializeField] public EventReference guitarSolo { get; private set; }
    [field: SerializeField] public EventReference bass { get; private set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerFootsteps { get; private set; }
    [field: SerializeField] public EventReference kickDoor { get; private set; }
    [field: SerializeField] public EventReference playerHeartBeat { get; private set; }
    [field: SerializeField] public EventReference monsterScream { get; private set; }

    public static FMODEvents instance { get; private set; }
    private Dictionary<string, string> events;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;

        events = new Dictionary<string, string>
        {
            { "playerFootsteps", "event:/Footsteps" },
            // Add more events here
        };

    }

}