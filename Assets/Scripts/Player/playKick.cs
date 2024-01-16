using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playKick : MonoBehaviour
{

    Image imageKick;
    Animator animatorKick;

    void Start()
    {
        imageKick = GetComponent<Image>();
        animatorKick = GetComponent<Animator>();
        //hide both 
        imageKick.enabled = false;
        animatorKick.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Enable the Image and Animator components
            imageKick.enabled = true;
            animatorKick.enabled = true;

            // Play the animation
            animatorKick.Play("kickAnim", -1, 0f);

            // Schedule a coroutine to disable the components after the animation has finished
            float animationLength = animatorKick.GetCurrentAnimatorStateInfo(0).length;
            StartCoroutine(DisableComponentsAfterDelay(animationLength));
        }
    }

    IEnumerator DisableComponentsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Disable the Image and Animator components
        imageKick.enabled = false;
        animatorKick.enabled = false;
    }
}
