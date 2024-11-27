using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{

    public Animator animator;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.shouldAnimateMovement())
        {
            animator.SetBool("Walking", true);
        }
        else
            animator.SetBool("Walking", false);
        
    }

    public void OnTimerEnd()
    {
        animator.SetTrigger("Turn Around");
    }
}
