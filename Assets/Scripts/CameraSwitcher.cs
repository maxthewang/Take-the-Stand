using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraSwitcher : MonoBehaviour
{
    public DialogManager dialogManager;  // Reference to the DialogManager
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if we're on the second message
        if (dialogManager != null && dialogManager.activeMessage == 2)
        {
            // Switch to the alternate camera
            animator.SetTrigger("Zoom Out");
        }
        if (dialogManager != null && DialogManager.isActive == false)
        {
            // Switch to the alternate camera
			FadeTransition.instance.FadeToBlack("CrimeScene");
        }
        
    }
}
