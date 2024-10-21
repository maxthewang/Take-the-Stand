using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class NotepadManager : MonoBehaviour
{
	[SerializeField]
	TextMeshProUGUI notepadInformation;
	[SerializeField]
	GameObject panelObject;
	[SerializeField]
    PlayerInputActions playerControls;
    [SerializeField]
    private AudioSource openingSound;
    [SerializeField]
    private AudioSource closingSound;
    private HashSet<string> notedObjects = new HashSet<string>();
	private Dictionary<string, string> cluePairs = new Dictionary<string, string>();

    void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerControls.Enable();

        // Subscribe to the notepad toggle action
        playerControls.UI.Notepad.performed += ctx => ToggleNotepad();
    }

	public void AddInformation(string dictionaryItemName, string textItemName, string itemDescription)
    {
        // Only discover the object if it's new and not already noted
        if (cluePairs.ContainsKey(dictionaryItemName))
        {
            DiscoverableManager.instance.DiscoverObject();
			ReplaceInformation(dictionaryItemName, textItemName, itemDescription);
        }
		else{
        	cluePairs[dictionaryItemName] = $"\n{textItemName}: {itemDescription}";
		}

        CompileNotepadInformation();
    }

	private void ReplaceInformation(string dictionaryItemName, string textItemName, string newItemDescription){
		string oldDescription = cluePairs[dictionaryItemName];
		string[] oldDescriptionArray = oldDescription.Split(" ");
		string[] newDescriptionArray = newItemDescription.Split(" ");
		string finalReplacement = "";
		for(int i = 0; i < newDescriptionArray.Length; i++){
			if(Array.IndexOf(oldDescriptionArray, newDescriptionArray[i]) == -1){
				finalReplacement += "<color=red><b>" + newDescriptionArray[i] + "</b></color>" + " ";
			}
			else{
				finalReplacement += newDescriptionArray[i] + " ";
			}
		}

        cluePairs[dictionaryItemName] = $"\n<color=red><b>{textItemName}</b></color>: {finalReplacement}";
	}

	private void CompileNotepadInformation(){
		string newNotepadInfo = "";
		foreach(var notepadInfo in cluePairs){
			newNotepadInfo += notepadInfo.Value;
		}
		notepadInformation.text = newNotepadInfo;
	}


	private void ToggleNotepad()
    {
        bool isActive = panelObject.activeSelf; // Store current state

        if (isActive)
        {
            // Notepad closed
            panelObject.SetActive(false);
            Time.timeScale = 1f;
            closingSound.Play();
        }
        else
        {
            // Notepad opened
            panelObject.SetActive(true);
            Time.timeScale = 0f;
            openingSound.Play();
        }
    }

    private void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.UI.Notepad.performed -= ctx => ToggleNotepad();
        }
    }
}