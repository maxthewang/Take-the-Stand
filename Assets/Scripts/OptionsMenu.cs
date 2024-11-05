using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class OptionsMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    [SerializeField]
    private PlayerInputActions playerControls;
    private bool isPaused = false;

    void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    void OnEnable()
    {
        playerControls.UI.Options.performed += OnPause;  // Subscribe to the Pause action
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.UI.Options.performed -= OnPause;  // Unsubscribe when disabled
        playerControls.Disable();
    }

    void Start()
    {
        // Lock/unlock cursor based on the active scene
        if (SceneManager.GetActiveScene().name == "CrimeScene")
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // Callback function for the Pause action
    private void OnPause(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            ResumeGame();  // If the game is paused, resume it
        }
        else
        {
            PauseGame();   // If the game is running, pause it
        }
    }

    // Function to resume the game
    public void ResumeGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);   // Hide the pause menu
        }
        isPaused = false;                   // Mark the game as not paused
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Function to pause the game
    public void PauseGame()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);    // Show the pause menu
        }
        isPaused = true;                    // Mark the game as paused
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Optional: Add a method to quit the game from the pause menu
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

	public void RestartGame(){
		GameManager.instance.RestartGame();
		FadeTransition.instance.FadeToBlack("CrimeScene");
	}
}
