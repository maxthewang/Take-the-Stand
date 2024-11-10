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
    // Start is called before the first frame update
    void Start()
    {
        playerControls.Enable();
        NotepadManager.instance.OpenNotepad();
        playerControls.UI.AdvanceDialogue.performed += ctx => OpenNextScene(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OpenNextScene(){
        if(SceneManager.GetActiveScene().name != "NotepadScene"){
            return;
        }
        playerControls.UI.AdvanceDialogue.performed -= ctx => OpenNextScene();
        Destroy(this);
        FadeTransition.instance.FadeToBlack("Interrogation");
        NotepadManager.instance.CloseNotepad();
    }
}
