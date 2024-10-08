using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class NotepadManager : MonoBehaviour
{
	[SerializeField]
	TextMeshProUGUI notepadInformation;
	[SerializeField]
	GameObject panelObject;


    // Start is called before the first frame update
    void Start()
    {
		DontDestroyOnLoad(this);
		AddInformation("New Test Item", "This is the new Test items description");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N)){
			ToggleNotepad();
		}
    }

	public void AddInformation(string itemName, string itemDescription){
		notepadInformation.text += "\n\n" + itemName + ": " + itemDescription;
	}

	private void ToggleNotepad(){
		panelObject.SetActive(!panelObject.activeSelf);
	}
}
