using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Reference to the pause menu UI panel
    private bool isPaused = false;  // Track if the game is currently paused

    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        // Check if the player presses the Esc key
        if (Input.GetKeyDown(KeyCode.Escape))
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
}