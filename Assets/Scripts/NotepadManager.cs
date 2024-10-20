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
        {;
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
