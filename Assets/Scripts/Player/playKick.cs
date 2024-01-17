using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using FMOD.Studio;
using FMODUnity;

public class playKick : MonoBehaviour
{

    Animator animatorKick;
    PlayerInputActions playerInputActions;

    private EventInstance audioEventInstance;
    private FMOD.ATTRIBUTES_3D attributes;
    private GameObject player;
    [SerializeField] float delay = 0.5f;


    void Start()
    {
        animatorKick = GetComponent<Animator>();
        animatorKick.enabled = false;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        playerInputActions.Character.Interact.performed += ctx => KickOpenDoor();
        audioEventInstance = AudioManager.instance.CreateEventInstance(FMODEvents.instance.kickDoor);
        player = GameObject.FindGameObjectWithTag("Player");
        attributes = RuntimeUtils.To3DAttributes(player);
        audioEventInstance.set3DAttributes(attributes);
    }

    void OnDestroy()
    {
        playerInputActions.Disable();
        playerInputActions.Character.Interact.performed -= ctx => KickOpenDoor();
    }

    public void KickOpenDoor()
    {
        animatorKick.enabled = true;
        player = GameObject.FindGameObjectWithTag("Player");

        // Play the animation
        animatorKick.Play("rig|rig|mixamo_com|Layer0", -1, 0f);
        StartCoroutine(PlayMusicAfterDelay(delay));
    }

    IEnumerator PlayMusicAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        attributes = RuntimeUtils.To3DAttributes(player);
        audioEventInstance.set3DAttributes(attributes);
        audioEventInstance.start();
    }
}
