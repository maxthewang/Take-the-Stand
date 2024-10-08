using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotepadManager : MonoBehaviour
{
	[SerializeField]
	TextMeshProUGUI notepadInformation;
	[SerializeField]
	GameObject panelObject;
    private HashSet<string> notedObjects = new HashSet<string>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N)){
			ToggleNotepad();
		}
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
}
