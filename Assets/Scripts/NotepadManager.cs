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

	public void AddInformation(string itemName, string itemDescription)
    {
        // Avoid duplicate entries by checking if the object has already been noted
        if (!notedObjects.Contains(itemName))
        {
            notedObjects.Add(itemName); // Add to the set of noted objects
            notepadInformation.text += $"\n{itemName}: {itemDescription}";
            DiscoverableManager.instance.DiscoverObject();
        }
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
