using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class HoldableObject : MonoBehaviour
{
    public string interactionText = "Press E to interact"; // Text displayed for interaction
    public string interactedText = "Already Interacted";
    private bool isPlayerInRange = false;

    // Reference to the UI Text element
    public Text interactionMessageText;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            // Show interaction text on UI
            ShowInteractionText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exits the trigger
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            // Hide interaction text from UI
            HideInteractionText();
        }
    }

    private void Update()
    {
        // Check for interaction input if player is in range
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void Interact()
    {
        // Define what happens when the player interacts
        interactionMessageText.text = "You're now holding the " + gameObject.name + ".";
        // Notify the GameManager that an interaction has occurred
        GameManager.instance.AddInteraction();
        // Add interaction logic here (e.g., opening a dialog, picking up an item)
        gameObject.SetActive(false);
    }

    private void ShowInteractionText()
    {
        interactionMessageText.text = interactionText; // Set the interaction text
    }

    private void HideInteractionText()
    {
        interactionMessageText.text = interactedText; // Clear the interaction text
    }
}
