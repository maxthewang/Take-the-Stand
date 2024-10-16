using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{
    public string interactionText = "Click to interact.";
    public string clueMessage = "This object doesn't give you any information.";
    public TextMeshProUGUI interactionMessageText;
    [SerializeField]
    private PlayerInputActions playerControls;
    private NotepadManager notepadManager;
    private InputAction interactAction;

    void Awake()
    {
        playerControls = new PlayerInputActions();
        interactAction = playerControls.Player.Interact;
    }

    void OnEnable()
    {
        interactAction.performed += OnInteract;
        playerControls.Enable();
    }

    void OnDisable()
    {
        interactAction.performed -= OnInteract;
        playerControls.Disable();
    }

    void Start()
    {
        // Hide interaction text initially
        notepadManager = FindObjectOfType<NotepadManager>();
        HideInteractionText();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        // Adjust the screen center based on pixelation scale
        float adjustedScreenWidth = 300;
        float adjustedScreenHeight = 200;

        // Create a ray from the camera using the adjusted center point
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(adjustedScreenWidth / 2, adjustedScreenHeight / 2, 0));
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 5.0f);

        // Perform the raycast
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Check if the clicked object is this object
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Debug.Log("Hit object: " + hit.collider.gameObject.name);
                ShowInteractionText();
                Interact();
            }
        }
    }

    private void Interact()
    {
        // Define what happens when the player interacts
        interactionMessageText.text = $"{gameObject.name} discovered!\n\n{clueMessage}";
        // Notify the GameManager that an interaction has occurred
        GameManager.instance.AddInteraction();

        if (notepadManager != null)
        {
            notepadManager.AddInformation(gameObject.name, clueMessage);
        }
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
