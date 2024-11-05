using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraSwitcher : MonoBehaviour
{
    public DialogManager dialogManager;  // Reference to the DialogManager
    public Camera mainCamera;  // Reference to the main camera
    public Camera altCamera;   // Reference to the alternate camera

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the cameras are set correctly at the start
        if (mainCamera != null && altCamera != null)
        {
            mainCamera.enabled = true;
            altCamera.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if we're on the second message
        if (dialogManager != null && dialogManager.activeMessage == 2)
        {
            // Switch to the alternate camera
            SwitchToAltCamera();
        }
        if (dialogManager != null && DialogManager.isActive == false)
        {
            // Switch to the alternate camera
			FadeTransition.instance.FadeToBlack("CrimeScene");
        }
        
    }

    void SwitchToAltCamera()
    {
        if (mainCamera != null && altCamera != null)
        {
            mainCamera.enabled = false;  // Disable the main camera
            altCamera.enabled = true;    // Enable the alternate camera
        }
    }
}
