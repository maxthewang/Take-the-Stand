using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NotepadSceneManager : MonoBehaviour
{
    [SerializeField]
    PlayerInputActions playerControls;
    void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    void Start()
    {
        playerControls.Enable();
        
        // Slide in the notepad when the scene starts
        StartCoroutine(SlideNotepadIn());
        
        playerControls.UI.AdvanceDialogue.performed += ctx => OpenNextScene();
    }

    void OpenNextScene()
    {
        if (SceneManager.GetActiveScene().name != "NotepadScene") return;

        playerControls.UI.AdvanceDialogue.performed -= ctx => OpenNextScene();

        // Slide out the notepad before switching scenes
        StartCoroutine(SlideNotepadOut());
    }

    private IEnumerator SlideNotepadIn()
    {
        yield return NotepadManager.instance.SlideNotepad(true); // Open notepad with slide animation
    }

    private IEnumerator SlideNotepadOut()
    {
        yield return NotepadManager.instance.SlideNotepad(false); // Close notepad with slide animation

        // Wait briefly to ensure animation completes before switching scenes
        yield return new WaitForSeconds(0.1f); 

        FadeTransition.instance.FadeToBlack("Interrogation");
    }
}