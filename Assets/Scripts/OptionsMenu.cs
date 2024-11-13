using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class OptionsMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    private bool isPaused = false;
    private bool isSettings = false;
    public GameObject pauseMenuUI;
	[SerializeField]
    private PlayerInputActions playerControls;

    private void Awake()
    {
        // Initialize and enable the player controls
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        // Enable the player controls and subscribe to the Options action
        playerControls.Enable();
        playerControls.UI.Options.performed += OnOptionsPressed;
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        playerControls.UI.Options.performed -= OnOptionsPressed;
        playerControls.Disable();
    }

    void Start()
    {
        // Lock/unlock cursor based on the active scene
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (SceneManager.GetActiveScene().name == "CrimeScene")
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Callback function for the Options action
    private void OnOptionsPressed(InputAction.CallbackContext context)
    {
        if (isSettings)
        {
            CloseSettingsPage();
        }
        else
        {
            OnPause();
        }
    }

    // Callback function for the Pause action
    public void OnPause()
    {
        if (isPaused && !isSettings)
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
        Time.timeScale = 1f;                // Resume time
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);   // Hide the pause menu
        }
        isPaused = false;                   // Mark the game as not paused
        if (SceneManager.GetActiveScene().name == "CrimeScene")
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        // pointer.SetActive(true);
    }

    // Function to pause the game
    public void PauseGame()
    {
        Time.timeScale = 0f;                // Stop time
        // pointer.SetActive(false);
        if (pauseMenuUI != null && SceneManager.GetActiveScene().name != "StartMenu")
        {
            pauseMenuUI.SetActive(true);    // Show the pause menu
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        isPaused = true;                    // Mark the game as paused
    }

    // Setting page
    public void OpenSettingsPage(){
        isSettings = true;
        while (!Cursor.visible) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        while (pauseMenuUI && pauseMenuUI.activeSelf) {
            pauseMenuUI.SetActive(false);
        }
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);   // Show the settings panel
        }

    }

    public void CloseSettingsPage(){
        while (settingsPanel && settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);   // Hide the settings panel
        }
        isSettings = false;
        if (SceneManager.GetActiveScene().name != "StartMenu"){
            PauseGame();   // Pause the game
        }
    }

    // Optional: Add a method to quit the game from the pause menu
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

	public void RestartGame(){
		GameManager.instance.RestartGame();
		FadeTransition.instance.FadeToBlack("CrimeScene");
	}
}
