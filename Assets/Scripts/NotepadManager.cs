using System.Collections.Generic;
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
    private AudioSource notepadSound;
    private HashSet<string> notedObjects = new HashSet<string>();

	private Dictionary<string, string> cluePairs = new Dictionary<string, string>();


    // Start is called before the first frame update
    void Start()
    {
        // Enable the input actions
        playerControls = new PlayerInputActions();
        playerControls.Enable();

        // Subscribe to the notepad toggle action
        playerControls.UI.Notepad.performed += ctx => ToggleNotepad();
    }

	public void AddInformation(string dictionaryItemName, string textItemName, string itemDescription)
    {
        // Avoid duplicate entries by checking if the object has already been noted
        if (cluePairs.ContainsKey(dictionaryItemName))
        {
            notedObjects.Add(dictionaryItemName); // Add to the set of noted objects
            DiscoverableManager.instance.DiscoverObject();
		}
            cluePairs[dictionaryItemName] = $"\n{textItemName}: {itemDescription}";
		CompileNotepadInformation();
    }

	private void CompileNotepadInformation(){
		string newNotepadInfo = "";
		foreach(var notepadInfo in cluePairs){
			newNotepadInfo += notepadInfo.Value;
		}
		notepadInformation.text = newNotepadInfo;
	}

	private void ToggleNotepad(){
		panelObject.SetActive(!panelObject.activeSelf);
        notepadSound.Play();
	}

    private void OnDisable()
    {
        playerControls.UI.Notepad.performed -= ctx => ToggleNotepad();
    }
}
