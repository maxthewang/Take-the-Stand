using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiscoverableManager : MonoBehaviour
{
    public static DiscoverableManager instance;
    
    [SerializeField] private TextMeshProUGUI discoverableCountText;
    [SerializeField] private Transform discoverablesParent; // Reference to parent object containing all discoverables

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
        discoveredCount++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        discoverableCountText.text = $"Discovered: {discoveredCount}/{totalDiscoverables}";
    }
}