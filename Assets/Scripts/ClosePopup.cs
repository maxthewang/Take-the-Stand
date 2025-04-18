using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ClosePopup : MonoBehaviour
{
    public UnityEngine.UI.Image popupImage;
    public UnityEngine.UI.Image xButtonImage;
    public float fadeDuration = 1f;
    private PlayerInputActions playerControls;
    private InputAction startAction;
	public TimerScript timerScript;

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
    }

    private void OnStartGame(InputAction.CallbackContext context)
    {
        if (popupImage != null)
        {
            StartCoroutine(FadeOut());
        }

        // Start the game time
        timerScript.TimerOn = true;
    }
    IEnumerator FadeOut()
    {
        // Get the current color of the image
        Color popupColor = popupImage.color;
        Color xButtonColor = xButtonImage.color;
        float startAlpha = popupColor.a;

        // Gradually reduce the alpha value over time
        for (float t = 0f; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            // Set the alpha based on the normalized time (t/duration)
            popupColor.a = Mathf.Lerp(startAlpha, 0f, normalizedTime);
            popupImage.color = popupColor;
            xButtonColor.a = Mathf.Lerp(startAlpha, 0f, normalizedTime);
            xButtonImage.color = xButtonColor;

            yield return null; // Wait for the next frame
        }
        
        popupImage.gameObject.SetActive(false);
        xButtonImage.gameObject.SetActive(false);
    }
}
