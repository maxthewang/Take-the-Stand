using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{
    public string interactionText = "Click to interact.";
    public string clueMessage = "This object doesn't give you any information.";
    public static AudioSource discoverySound;
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
                Interact();
            }
        }
    }

    private void Interact()
    {   
        discoverySound.Play();
        
        // Use the centralized text manager to show the interaction message
        InteractionTextManager.instance.ShowInteractionText($"{gameObject.name} discovered!\nCheck the notepad to see the clue you revealed.");

        // Notify the GameManager that an interaction has occurred
        GameManager.instance.AddInteraction();

        if (notepadManager != null)
        {
            notepadManager.AddInformation(gameObject.name, clueMessage);
        }
    }
}
