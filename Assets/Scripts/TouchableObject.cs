using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class TouchableObject : MonoBehaviour
{
    public string interactionText = "Press E to interact.";
    public string afterInteractionMessage = "Your fingerprints are on the object.";
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
        interactionMessageText.text = afterInteractionMessage;
        // Notify the GameManager that an interaction has occurred
        GameManager.instance.AddInteraction();
    }

    private void ShowInteractionText()
    {
        interactionMessageText.text = interactionText; // Set the interaction text
    }

    private void HideInteractionText()
    {
        interactionMessageText.text = ""; // Clear the interaction text
    }
}
