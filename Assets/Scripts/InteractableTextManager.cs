using UnityEngine;
using TMPro;
using System.Collections;

public class InteractionTextManager : MonoBehaviour
{
    public static InteractionTextManager instance;
    public TextMeshProUGUI interactionMessageText;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void ShowInteractionText(string message)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);  // Stop any existing fade coroutine
        }

        interactionMessageText.text = message;
        interactionMessageText.alpha = 1.0f;  // Ensure the text is fully visible
        fadeCoroutine = StartCoroutine(FadeInteractionText());
    }

    private IEnumerator FadeInteractionText()
    {
        // Show the text for 2 seconds
        yield return new WaitForSeconds(2.0f);

        // Gradually fade out the text over 2 seconds
        float fadeDuration = 2.0f;
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            interactionMessageText.alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeDuration);
            yield return null;
        }

        interactionMessageText.text = "";  // Clear the text once it's fully faded
    }
}