using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMOD.Studio;

public class BeatSync : MonoBehaviour
{
    public enum Instrument
    {
        Kick,
        Snare,
        HiHat,
        Guitar,
        Bass
    }

    public float BPM = 120;
    public bool soundIndependent;
    private float lastTime;

    private float timePerBeat;
    private int lastBeat;

    public UnityEvent OnBeat;

    private Dictionary<Instrument, EventInstance> instrumentEvents;
    private FMOD.ATTRIBUTES_3D attributes;

    // Define your beat patterns here
    private Dictionary<Instrument, int[]> beatPatterns;

    void Start()
    {
        timePerBeat = 60 / BPM;
        lastBeat = 0;
        lastTime = Time.time;
        attributes = FMODUnity.RuntimeUtils.To3DAttributes(gameObject);

        instrumentEvents = new Dictionary<Instrument, EventInstance>
        {
            { Instrument.Kick, AudioManager.instance.CreateEventInstance(FMODEvents.instance.kick) },
            { Instrument.Snare, AudioManager.instance.CreateEventInstance(FMODEvents.instance.snare) },
            { Instrument.HiHat, AudioManager.instance.CreateEventInstance(FMODEvents.instance.hihat) },
            { Instrument.Guitar, AudioManager.instance.CreateEventInstance(FMODEvents.instance.guitar) },
            { Instrument.Bass, AudioManager.instance.CreateEventInstance(FMODEvents.instance.bass) },
            // Initialize other instruments similarly
        };

        foreach (var inst in instrumentEvents.Values)
        {
            inst.set3DAttributes(attributes);
        }

        // Initialize beat patterns for each instrument
        beatPatterns = new Dictionary<Instrument, int[]>
        {
            { Instrument.Kick, new[] { 1, 1, 1, 1 } }, // Example pattern: Play on beats 1 and 3
            { Instrument.Snare, new[] { 0, 1, 0, 1 } }, // Example pattern: Play on beats 2 and 4
            { Instrument.HiHat, new[] { 1, 1, 1, 1 } }, // Example pattern: Play on all beats
            { Instrument.Guitar, new[] { 1, 0, 1, 1 } },
            { Instrument.Bass, new[] { 1, 1, 1, 1 } },
            // Define other patterns
        };
    }

    void Update()
    {
        if (soundIndependent)
        {
            // Sound independent update
            if (Time.time - lastTime > timePerBeat)
            {
                PlayInstrumentsOnBeat(lastBeat);
                lastBeat++;
                OnBeat.Invoke();
                lastTime = Time.time;
                Debug.Log("Beat");
            }
        }
        else
        {
            // Sound dependent update
            foreach (var inst in instrumentEvents)
            {
                inst.Value.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                PLAYBACK_STATE playbackState;
                inst.Value.getPlaybackState(out playbackState);
                bool isPlaying = playbackState != PLAYBACK_STATE.STOPPED;

                if (!isPlaying)
                {
                    inst.Value.start(); // Start the FMOD event
                }
            }

            int currentBeat = Mathf.FloorToInt(GetEventPlaybackTime() / timePerBeat);
            if (lastBeat != currentBeat)
            {
                PlayInstrumentsOnBeat(currentBeat);
                lastBeat = currentBeat;
                OnBeat.Invoke();
                Debug.Log("Beat");
            }
        }
    }

    private void PlayInstrumentsOnBeat(int beat)
    {
        foreach (var instrument in beatPatterns.Keys)
        {
            int beatIndex = beat % beatPatterns[instrument].Length;
            if (beatPatterns[instrument][beatIndex] == 1)
            {
                instrumentEvents[instrument].start();
            }
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
}
