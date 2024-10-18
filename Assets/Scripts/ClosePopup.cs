using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ClosePopup : MonoBehaviour
{
    public Image popupImage;
    public float fadeDuration = 1f;
    private PlayerInputActions playerControls;
    private InputAction startAction;

    void Awake()
    {
        playerControls = new PlayerInputActions();
        startAction = playerControls.Player.Interact;
    }

    void OnEnable()
    {
        startAction.performed += OnStartGame;
        playerControls.Enable();
    }

    void OnDisable()
    {
        startAction.performed -= OnStartGame;
        playerControls.Disable();
    }

    void Start()
    {
        // Initially, ensure the on-screen image is active
        if (popupImage != null)
        {
            popupImage.gameObject.SetActive(true);
        }

        // Pause the game time
        Time.timeScale = 0f;
    }

    private void OnStartGame(InputAction.CallbackContext context)
    {
        if (popupImage != null)
        {
            StartCoroutine(FadeOut());
        }

        // Start the game time
        Time.timeScale = 1f;
    }
    IEnumerator FadeOut()
    {
        // Get the current color of the image
        Color imageColor = popupImage.color;
        float startAlpha = imageColor.a;

        // Gradually reduce the alpha value over time
        for (float t = 0f; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            // Set the alpha based on the normalized time (t/duration)
            imageColor.a = Mathf.Lerp(startAlpha, 0f, normalizedTime);
            popupImage.color = imageColor;

            yield return null; // Wait for the next frame
        }
        
        popupImage.gameObject.SetActive(false);
    }
}
