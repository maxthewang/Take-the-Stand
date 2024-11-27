using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    // Start is called before the first frame update
	public TMP_Text tutorialText;
	private bool interacted = false;
	public TimerScript timerScript;
    void Start()
    {
		if(GameManager.instance.doneTutorial){
			Destroy(gameObject);
			return;
		}
		tutorialText.text = "Move To the Red Arrow with the Left Joystick";
		timerScript.TimerOn = false;
        
    }

    // Update is called once per frame
    void Update()
    {
		if(GameManager.instance.closedNotepadOnce){
			timerScript.TimerOn = true;
			Destroy(gameObject);
		}
		if(!interacted && GameManager.instance.outlinedObject){
			tutorialText.text = "Press the Right Trigger To Interact With the trash can";
		}
		if(!interacted && GameManager.instance.GetInteractionCount() > 0){
			interacted = true;
			tutorialText.text = "Press Left Trigger to see what was added to the Notepad";
		}
		if(interacted){
			if(GameManager.instance.openedNotepadOnce && !GameManager.instance.flippedPagesOnce){
				tutorialText.text = "Press Left and Right Dpad to flip pages";
			}
			else if(GameManager.instance.flippedPagesOnce){
				tutorialText.text = "Close the notepad to continue";
				GameManager.instance.doneTutorial = true;
			}
		}
    }

	private void OnTriggerEnter(Collider other){
		Debug.Log("Entered");
		tutorialText.text = "Aim at the trash can with the Right Joystick";
	}
}
