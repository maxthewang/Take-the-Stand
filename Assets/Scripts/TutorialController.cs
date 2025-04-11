using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    // Start is called before the first frame update
	public TMP_Text tutorialText;
	private bool interacted = false;
	public TimerScript timerScript;
    public GameObject ControlsImage;
    void Start()
    {
		if(GameManager.instance.doneTutorial){
			Destroy(gameObject);
			return;
		}
		tutorialText.text = "Move to the Red Arrow with the <color=white>Left Joystick</color>";
		timerScript.TimerOn = false;
        
    }

    // Update is called once per frame
    void Update()
    {
		if(GameManager.instance.closedNotepadOnce){
            ControlsImage.SetActive(true);
			Destroy(gameObject);
		}
		if(!interacted && GameManager.instance.outlinedObject){
			tutorialText.text = "Press <color=lightblue>X</color> to Interact with the trash can";
		}
		if(!interacted && GameManager.instance.GetInteractionCount() > 0){
			interacted = true;
			tutorialText.text = "Press <color=yellow>Y</color> to see what was added to the Notepad";
		}
		if(interacted){
			if(GameManager.instance.openedNotepadOnce && !GameManager.instance.flippedPagesOnce){
				tutorialText.text = "Press <color=white>D-Pad</color> (Left & Right) to flip pages";
			}
			else if(GameManager.instance.flippedPagesOnce){
				tutorialText.text = "Close the Notepad with <color=yellow>Y</color> to continue";
				GameManager.instance.doneTutorial = true;
			}
		}
    }

	private void OnTriggerEnter(Collider other){
		Debug.Log("Entered");
		tutorialText.text = "Aim at the trash can with the <color=white>Right Joystick</color>";
	}
}
