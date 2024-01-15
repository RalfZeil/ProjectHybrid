using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;
public class ActionSound : MonoBehaviour
{
    [field: SerializeField] public EventReference audioEvent { get; private set; }
    private EventInstance audioEventInstance;
    private FMOD.ATTRIBUTES_3D attributes;
    [SerializeField] private float minDistance = 1f; // minimum distance for sound falloff
    [SerializeField] private float maxDistance = 20f; // maximum distance for sound falloff
    [SerializeField] private bool updateSoundLocation;
    [SerializeField] private float delay = 0f;


    void Start()
    {
        audioEventInstance = AudioManager.instance.CreateEventInstance(audioEvent);
        attributes = RuntimeUtils.To3DAttributes(gameObject);
        audioEventInstance.set3DAttributes(attributes);
        PlayActionSound();
    }
    public void PlayActionSound()
    {
        audioEventInstance.setProperty(EVENT_PROPERTY.MINIMUM_DISTANCE, minDistance);
        audioEventInstance.setProperty(EVENT_PROPERTY.MAXIMUM_DISTANCE, maxDistance);
        StartCoroutine(StartSoundCoroutine(delay));
    }

    public void Update()
    {
        if (updateSoundLocation)
        {
            attributes.position = RuntimeUtils.ToFMODVector(transform.position);
            audioEventInstance.set3DAttributes(attributes);
        }
    }

    public void SetSoundLocation()
    {
        attributes.position = RuntimeUtils.ToFMODVector(transform.position);
        audioEventInstance.set3DAttributes(attributes);
    }

    public void StopActionSound()
    {
        audioEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void OnDestroy()
    {
        audioEventInstance.release();
    }

    IEnumerator StartSoundCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioEventInstance.start();
    }
}