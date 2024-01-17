using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMOD.Studio;
using UnityEngine.UI;
using UnityEditor;

public class BeatSync : MonoBehaviour
{
    private PlayerInputActions playerInput;

    public enum Instrument
    {
        Kick,
        Snare,
        HiHat,
        Guitar,
        GuitarSolo,
        Bass
    }

    public float BPM = 120;
    private float lastTime;

    private float timePerBeat;
    private int lastBeat;

    public UnityEvent OnBeat;
    private int currentInstrumentLevel = 0; // New field to track current level of instruments
    private Dictionary<Instrument, EventInstance> instrumentEvents;
    private FMOD.ATTRIBUTES_3D attributes;

    // Define your beat patterns here
    private Dictionary<Instrument, int[]> beatPatterns;

    void Start()
    {

        timePerBeat = 60 / BPM;
        lastBeat = 0;
        lastTime = (float)AudioSettings.dspTime;
        attributes = FMODUnity.RuntimeUtils.To3DAttributes(gameObject);

        instrumentEvents = new Dictionary<Instrument, EventInstance>
        {
            { Instrument.Kick, AudioManager.instance.CreateEventInstance(FMODEvents.instance.kick) },
            { Instrument.Snare, AudioManager.instance.CreateEventInstance(FMODEvents.instance.snare) },
            { Instrument.HiHat, AudioManager.instance.CreateEventInstance(FMODEvents.instance.hihat) },
            { Instrument.Guitar, AudioManager.instance.CreateEventInstance(FMODEvents.instance.guitar) },
            //{ Instrument.GuitarSolo, AudioManager.instance.CreateEventInstance(FMODEvents.instance.guitarSolo) },
            //{ Instrument.Bass, AudioManager.instance.CreateEventInstance(FMODEvents.instance.bass) },
            // Initialize other instruments similarly
        };

        foreach (var inst in instrumentEvents.Values)
        {
            inst.set3DAttributes(attributes);
        }

        beatPatterns = new Dictionary<Instrument, int[]>
        {
            { Instrument.Kick, new[] { 1, 1, 1, 1, 1, 1, 1, 1 } }, // Example pattern: Play on all 8 beats
            { Instrument.Snare, new[] { 0, 1, 0, 1, 0, 1, 0, 1 } }, // Example pattern: Play on even beats
            { Instrument.HiHat, new[] { 1, 1, 1, 1, 1, 1, 1, 1 } }, // Example pattern: Play on all 8 beats
            { Instrument.Guitar, new[] { 1, 0, 0, 0, 1, 0, 0, 0 } },
            //{ Instrument.GuitarSolo, new[] { 1, 0, 0, 0, 0, 0, 0, 0 } },
        };

        playerInput = new PlayerInputActions();
        playerInput.Enable();

        playerInput.Character.ChangeBeats.performed += ctx => ChangePlayingBeats(ctx.ReadValue<float>());
        EnableInstruments(currentInstrumentLevel);

    }

    void FixedUpdate()
    {
        attributes = FMODUnity.RuntimeUtils.To3DAttributes(gameObject);
        foreach (var inst in instrumentEvents.Values)
        {
            inst.set3DAttributes(attributes);
        }
        float timeSinceLastBeat = Time.time - lastTime;
        float eventPlaybackTime = GetEventPlaybackTime();
        if (timeSinceLastBeat >= timePerBeat)
        {
            int currentBeat = (lastBeat + 1) % 8; // Update to 8 for an 8-beat bar
            PlayInstrumentsOnBeat(currentBeat);

            lastBeat = currentBeat;
            lastTime = Time.time;
        }
    }

    private void PlayInstrumentsOnBeat(int beat)
    {
        OnBeat.Invoke();
        foreach (var instrument in beatPatterns.Keys)
        {
            if (beatPatterns[instrument][beat] == 1)
            {
                PlayInstrument(instrument);
            }
        }
    }

    private void PlayInstrument(Instrument instrument)
    {
        if (instrumentEvents.TryGetValue(instrument, out EventInstance eventInstance))
        {
            //eventInstance.stop(STOP_MODE.IMMEDIATE);
            eventInstance.start();
        }
    }

    float GetEventPlaybackTime()
    {
        // Use one of the instrument events as a reference, typically the kick
        if (instrumentEvents.ContainsKey(Instrument.Kick))
        {
            int time;
            instrumentEvents[Instrument.Kick].getTimelinePosition(out time);
            return time / 1000.0f; // FMOD time is in milliseconds
        }
        else
        {
            return 0.0f; // Default value if the kick is not available
        }
    }

    private void OnDestroy()
    {
        foreach (var inst in instrumentEvents.Values)
        {
            inst.stop(STOP_MODE.IMMEDIATE);
            inst.release(); // Release the FMOD event instance
        }
    }
    private void EnableInstruments(int level)
    {
        // Update the beat patterns for 8-beat bars
        beatPatterns[Instrument.Kick] = level >= 0 ? new[] { 1, 1, 1, 1, 1, 1, 1, 1 } : new[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        beatPatterns[Instrument.Snare] = level >= 1 ? new[] { 0, 1, 0, 1, 0, 1, 0, 1 } : new[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        beatPatterns[Instrument.HiHat] = level >= 2 ? new[] { 1, 1, 1, 1, 1, 1, 1, 1 } : new[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        beatPatterns[Instrument.Guitar] = level >= 3 ? new[] { 1, 0, 0, 0, 0, 0, 0, 0 } : new[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        //beatPatterns[Instrument.GuitarSolo] = level >= 3 ? new[] { 1, 0, 0, 0, 0, 0, 0, 0 } : new[] { 0, 0, 0, 0, 0, 0, 0, 0 };
    }
    void ChangePlayingBeats(float change)
    {
        if (change == 1)
        {
            // Add next instrument
            currentInstrumentLevel++;
            if (currentInstrumentLevel > 4) currentInstrumentLevel = 4; // Ensure it doesn't exceed the number of instruments
        }
        else if (change == -1)
        {
            // Remove last instrument
            currentInstrumentLevel--;
            if (currentInstrumentLevel < 0) currentInstrumentLevel = 0; // Ensure it doesn't go below 0
        }

        EnableInstruments(currentInstrumentLevel); // Update enabled instruments
    }
}
