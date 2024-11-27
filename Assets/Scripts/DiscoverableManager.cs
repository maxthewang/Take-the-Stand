using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiscoverableManager : MonoBehaviour
{
    public static DiscoverableManager instance;
    
    [SerializeField] public TextMeshProUGUI discoverableCountText;
    [SerializeField] public Transform discoverablesParent; // Reference to parent object containing all discoverables

    private int totalDiscoverables;
    private int discoveredCount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
			DiscoverableManager.instance.discoverableCountText = discoverableCountText;
			DiscoverableManager.instance.discoverablesParent = discoverablesParent;
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    private void Start()
    {
        // Automatically count the number of children under the discoverablesParent
        totalDiscoverables = discoverablesParent.childCount;
        UpdateUI();
    }

    // This method will be called when a discoverable is interacted with
    public void DiscoverObject()
    {
		Debug.Log("Discovered");
        discoveredCount++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        NotepadManager.instance.discoverableText.text = $"Discovered:\n{discoveredCount}/{totalDiscoverables}";
    }

	public void RestartGame(){
		discoveredCount = 0;
		UpdateUI();
	}
}