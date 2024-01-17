using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class heartBeatSound : MonoBehaviour
{
    private Transform monster;
    private float distance;
    private float heartBeatVolume;
    private float heartBeatPitch;
    private EventInstance playerHeartBeat;
    private FMOD.ATTRIBUTES_3D attributes;

    // Start is called before the first frame update
    void Start()
    {
        monster = GameObject.FindGameObjectWithTag("Monster").transform;
        playerHeartBeat = AudioManager.instance.CreateEventInstance(FMODEvents.instance.playerHeartBeat);
        attributes = FMODUnity.RuntimeUtils.To3DAttributes(transform);
        playerHeartBeat.set3DAttributes(attributes);
        playerHeartBeat.start();
    }

    // Update is called once per frame
    void Update()
    {
        attributes = FMODUnity.RuntimeUtils.To3DAttributes(transform);
        playerHeartBeat.set3DAttributes(attributes);

        distance = Vector3.Distance(monster.position, transform.position);
        heartBeatVolume = 1 - (distance / 30);
        print(heartBeatVolume);
        heartBeatPitch = 1 + (distance / 10);
        playerHeartBeat.setParameterByName("HeartBeatVolume", heartBeatVolume);
        //playerHeartBeat.setParameterByName("HeartBeatPitch", heartBeatPitch);
    }
}
