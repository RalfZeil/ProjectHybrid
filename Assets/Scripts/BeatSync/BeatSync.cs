using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeatSync : MonoBehaviour
{
    public float BPM = 120;

    public bool soundIndependent;
    private float lastTime;
    
    private float timePerBeat;
    private AudioSource beatAudio;
    private int lastBeat;

    public UnityEvent OnBeat;

    void Start()
    {
        timePerBeat = 60/BPM;
        beatAudio =  GetComponent<AudioSource>();
        lastBeat = 0;

        lastTime = Time.time;
    }

    void Update()
    {
        if(soundIndependent){
            if(Time.time - lastTime > timePerBeat){
                OnBeat.Invoke();
                lastTime = Time.time;
                Debug.Log("Beat");
            }
            return;
        }

        if(lastBeat == Mathf.FloorToInt(beatAudio.time / timePerBeat)){
            lastBeat ++;
            OnBeat.Invoke();
            Debug.Log("Beat");
        }

        if(!beatAudio.isPlaying){
            beatAudio.Play();
            lastBeat = 0;
        }
    }
}
