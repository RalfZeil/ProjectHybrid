using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class openDoor : MonoBehaviour
{

    Animator animator;
    [SerializeField] string animationName;

    void Start()
    {
        animator = GetComponent<Animator>();
        //hide both 
        animator.enabled = false;
    }

    public void OpenDoorAnimation()
    {
        animator.enabled = true;
        animator.Play(animationName);
    }
}

