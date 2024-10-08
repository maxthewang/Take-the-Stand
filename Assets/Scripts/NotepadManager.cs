using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotepadManager : MonoBehaviour
{
	[SerializeField]
	TextMeshProUGUI notepadInformation;
	[SerializeField]
	GameObject panelObject;
    public PlayerInputActions playerControls;
    private HashSet<string> notedObjects = new HashSet<string>();

    // Start is called before the first frame update
    void Start()
    {
        // Enable the input actions
        playerControls = new PlayerInputActions();
        playerControls.Enable();

        // Subscribe to the notepad toggle action
        playerControls.UI.Notepad.performed += ctx => ToggleNotepad();
    }

	public void AddInformation(string itemName, string itemDescription)
    {
        // Avoid duplicate entries by checking if the object has already been noted
        if (!notedObjects.Contains(itemName))
        {
            notedObjects.Add(itemName); // Add to the set of noted objects
            notepadInformation.text += $"\n\n{itemName}: {itemDescription}";
        }
    }

	private void ToggleNotepad(){
		panelObject.SetActive(!panelObject.activeSelf);
	}

    private void OnDisable()
    {
        // Unsubscribe from the input action when disabled
        playerControls.UI.Notepad.performed -= ctx => ToggleNotepad();
    }
}
