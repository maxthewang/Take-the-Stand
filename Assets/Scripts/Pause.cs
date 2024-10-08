using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Reference to the pause menu UI panel
    private bool isPaused = false;  // Track if the game is currently paused

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
        pauseMenuUI.SetActive(false);   // Hide the pause menu
        Time.timeScale = 1f;            // Resume normal time
        isPaused = false;               // Mark the game as not paused
    }

    // Function to pause the game
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);    // Show the pause menu
        Time.timeScale = 0f;            // Freeze time
        isPaused = true;                // Mark the game as paused
    }

    // Optional: Add a method to quit the game from the pause menu
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}