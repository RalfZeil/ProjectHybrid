using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using FMOD.Studio;
using FMODUnity;

public class playKick : MonoBehaviour
{

    Image imageKick;
    Animator animatorKick;
    PlayerInputActions playerInputActions;

    private EventInstance audioEventInstance;
    private FMOD.ATTRIBUTES_3D attributes;
    private GameObject player;


    void Start()
    {
        imageKick = GetComponent<Image>();
        animatorKick = GetComponent<Animator>();
        //hide both 
        imageKick.enabled = false;
        animatorKick.enabled = false;
        //bind interact to kick
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

        // Enable the Image and Animator components
        imageKick.enabled = true;
        animatorKick.enabled = true;
        player = GameObject.FindGameObjectWithTag("Player");
        attributes = RuntimeUtils.To3DAttributes(player);
        audioEventInstance.set3DAttributes(attributes);
        audioEventInstance.start();

        // Play the animation
        animatorKick.Play("kickAnim", -1, 0f);

        // Schedule a coroutine to disable the components after the animation has finished
        float animationLength = animatorKick.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(DisableComponentsAfterDelay(animationLength));
        // play oneshot sound with fmod
        //AudioManager.instance.PlayOneShot(FMODEvents.instance.kickDoor, transform.position);

    }

    IEnumerator DisableComponentsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Disable the Image and Animator components
        imageKick.enabled = false;
        animatorKick.enabled = false;
    }
}
