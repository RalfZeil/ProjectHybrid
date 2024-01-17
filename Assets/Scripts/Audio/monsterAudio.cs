using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class monsterAudio : MonoBehaviour
{

    private Vector3 previousPosition;
    private float velocity;
    [SerializeField] private float velocitySoundThreshold = 0.2f;
    private EventInstance Footsteps;
    private EventInstance scream;
    private FMOD.ATTRIBUTES_3D attributes;

    // Start is called before the first frame update
    void Start()
    {
        Footsteps = AudioManager.instance.CreateEventInstance(FMODEvents.instance.monsterFootsteps);
        scream = AudioManager.instance.CreateEventInstance(FMODEvents.instance.monsterScream);
        attributes = FMODUnity.RuntimeUtils.To3DAttributes(transform);
        Footsteps.set3DAttributes(attributes);

    }

    // Update is called once per frame
    void Update()
    {
        attributes = FMODUnity.RuntimeUtils.To3DAttributes(transform);
        Footsteps.set3DAttributes(attributes);

        velocity = (transform.position - previousPosition).magnitude / Time.deltaTime;
        previousPosition = transform.position;

        if (velocity > velocitySoundThreshold)
        {
            if (!Footsteps.isValid())
            {
                Footsteps.start();
            }
        }
        else
        {
            if (Footsteps.isValid())
            {
                Footsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }


        //every 5 second play scream
        if (Time.time % 5 == 0)
        {
            if (!scream.isValid())
            {
                scream.set3DAttributes(attributes);
                scream.start();
            }
        }
        else
        {
            if (scream.isValid())
            {
                scream.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }

    }
}
