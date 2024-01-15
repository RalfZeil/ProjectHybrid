using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMOD.Studio;

public class BeatSync : MonoBehaviour
{
    public float BPM = 120;
    public bool soundIndependent;
    private float lastTime;

    private float timePerBeat;
    private int lastBeat;

    public UnityEvent OnBeat;

    private EventInstance beatEventInstance;
    private FMOD.ATTRIBUTES_3D attributes;

    void Start()
    {
        timePerBeat = 60 / BPM;
        lastBeat = 0;
        lastTime = Time.time;
        beatEventInstance = AudioManager.instance.CreateEventInstance(FMODEvents.instance.kick);
        attributes = FMODUnity.RuntimeUtils.To3DAttributes(gameObject);
        beatEventInstance.set3DAttributes(attributes);

    }

    void Update()
    {
        if (soundIndependent)
        {
            if (Time.time - lastTime > timePerBeat)
            {
                OnBeat.Invoke();
                lastTime = Time.time;
                Debug.Log("Beat");
            }
            return;
        }
        attributes = FMODUnity.RuntimeUtils.To3DAttributes(gameObject);
        beatEventInstance.set3DAttributes(attributes);

        PLAYBACK_STATE playbackState;
        beatEventInstance.getPlaybackState(out playbackState);
        bool isPlaying = playbackState != PLAYBACK_STATE.STOPPED;

        if (lastBeat == Mathf.FloorToInt(GetEventPlaybackTime() / timePerBeat))
        {
            lastBeat++;
            OnBeat.Invoke();
            Debug.Log("Beat");
        }

        if (!isPlaying)
        {
            beatEventInstance.start(); // Start the FMOD event
            lastBeat = 0;
        }
    }

    float GetEventPlaybackTime()
    {
        int time;
        beatEventInstance.getTimelinePosition(out time);
        return time / 1000.0f; // FMOD time is in milliseconds
    }

    private void OnDestroy()
    {
        beatEventInstance.stop(STOP_MODE.IMMEDIATE);
        beatEventInstance.release(); // Release the FMOD event instance
    }
}
