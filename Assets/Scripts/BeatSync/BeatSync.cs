using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeatSync : MonoBehaviour
{
    public float BPM = 120;
    
    private float timePerBeat;
    private AudioSource beatAudio;
    private int lastBeat;

    public UnityEvent OnBeat;

    void Start()
    {
        timePerBeat = 60/BPM;
        beatAudio =  GetComponent<AudioSource>();
        lastBeat = 0;
    }

    void Update()
    {
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
