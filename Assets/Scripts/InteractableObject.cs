using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public string interactionText = "Click to interact.";
    public string clueMessage = "This object doesn't give you any information.";
	public string unfoundClueMessage = "This ______ doesn't give you any ___________.";
    public static AudioSource discoverySound;
    public AudioSource grabSound;
    public AudioSource voiceLine;
    [SerializeField]
    private PlayerInputActions playerControls;
    private NotepadManager notepadManager;
    private InputAction interactAction;
    private bool isDiscovered = false;
	public Sprite itemSprite;
    private Animator animator;

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
        animator = GameObject.Find("Player").GetComponent<Animator>();
        // Hide interaction text initially
        notepadManager = FindObjectOfType<NotepadManager>();
        if(notepadManager != null){
            notepadManager.AddInformation(name, new string('_', name.Length), unfoundClueMessage, null);
        }
    }

    protected virtual void OnInteract(InputAction.CallbackContext context)
    {
        // Adjust the screen center based on pixelation scale
        float adjustedScreenWidth = 640;
        float adjustedScreenHeight = 360;

        // Create a ray from the camera using the adjusted center point
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(adjustedScreenWidth / 2, adjustedScreenHeight / 2, 0));
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 5.0f);

        // Perform the raycast
        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            // Check if the clicked object is this object
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                Interact();
            }
        }
        animator.SetTrigger("Grab");
    }

    public virtual void Interact()
    {   
        if (!isDiscovered)  // Ensure discovery logic only happens once per item
        {
            discoverySound.pitch = Random.Range(0.8f, 1.0f);
            discoverySound.Play();
            grabSound.Play();

            // Use the centralized text manager to show the interaction message
            InteractionTextManager.instance.ShowInteractionText($"{gameObject.name} discovered!\nCheck the notepad to see the clue you revealed.");

            if (notepadManager != null)
            {
                // Update the notepad with the actual clue information
                notepadManager.AddInformation(name, gameObject.name, clueMessage, itemSprite);
            }

            GameManager.instance.AddInteraction();
            isDiscovered = true; // Mark the item as discovered

            voiceLine.Play();
        }
    }
}
