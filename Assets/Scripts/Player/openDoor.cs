using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class openDoor : MonoBehaviour
{

    Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
        //hide both 
        animator.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Enable the Image and Animator components

            // Play the animation
            animator.Play("kickAnim", -1, 0f);


        }
    }
}

