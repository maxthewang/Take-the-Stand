using UnityEngine;
using UnityEngine.UI; // Required for UI elements
using System.Collections;

public class ImageFader : MonoBehaviour
{
    public Image imageToFade;  // Assign the Image component in the Inspector
    public float fadeDuration = 2f; // Duration for the fade-out

    private void Start()
    {
        // Start the fade-out process
        StartCoroutine(FadeOut());
    }

    // Coroutine to fade out the image
    IEnumerator FadeOut()
    {
        // Get the current color of the image
        Color imageColor = imageToFade.color;
        float startAlpha = imageColor.a;

        // Gradually reduce the alpha value over time
        for (float t = 0f; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            // Set the alpha based on the normalized time (t/duration)
            imageColor.a = Mathf.Lerp(startAlpha, 0f, normalizedTime);
            imageToFade.color = imageColor;

            yield return null; // Wait for the next frame
        }

        // Ensure the image is fully transparent at the end
        imageColor.a = 0f;
        imageToFade.color = imageColor;
    }
}
