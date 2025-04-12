using UnityEngine;
using UnityEngine.UI;

public class ButtonIndicator : MonoBehaviour
{
    public float bobSpeed = 2f;        // Speed of the up/down bobbing
    public float bobHeight = 10f;      // Distance of the bobbing
    public float blinkSpeed = 2f;      // Speed of the blinking (alpha pulsing)

    private RectTransform rectTransform;
    private Vector2 startPos;
    private Image image;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        startPos = rectTransform.anchoredPosition;
    }

    void Update()
    {
        // Bobbing
        float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        rectTransform.anchoredPosition = new Vector2(startPos.x, newY);

        // Blinking (alpha pulsing)
        if (image != null)
        {
            Color color = image.color;
            color.a = Mathf.Lerp(0.3f, 1f, (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f);
            image.color = color;
        }
    }
}
