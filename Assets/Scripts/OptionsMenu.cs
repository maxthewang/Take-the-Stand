using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    private bool isPaused = false;
    private bool isSettings = false;
    // public GameObject pointer;
    public GameObject pauseMenuUI;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            print("Pause pressed");
            if (isSettings) {
                CloseSettingsPage();
            }
            else {
                OnPause();
            }
        }
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
        PauseGame();   // Pause the game
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
        isPaused = true;                    // Mark the game as paused
        if (SceneManager.GetActiveScene().name == "CrimeScene")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
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
