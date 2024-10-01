using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // For using Dictionary

public class ButtonCreator : MonoBehaviour
{
    // Dictionary to track button presses
    private Dictionary<string, bool> buttonPresses = new Dictionary<string, bool>();

    void Start()
    {
        // Initialize button press states (all set to false initially)
        buttonPresses.Add("Take Canister", false);
        buttonPresses.Add("Notify Police", false);
        buttonPresses.Add("Flee", false);

        // Create a Canvas if not already present
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObject = new GameObject("Canvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
        }

        // Create 3 buttons in a vertical column, aligned to the top-right corner
        CreateButton("Button1", new Vector2(-60, -20), "Take Canister", () => ButtonClicked("Take Canister"), canvas);
        CreateButton("Button2", new Vector2(-60, -60), "Notify Police", () => ButtonClicked("Notify Police"), canvas);
        CreateButton("Button3", new Vector2(-60, -100), "Flee", () => ButtonClicked("Flee"), canvas);
    }

    // Method to create a button
    void CreateButton(string name, Vector2 position, string buttonText, UnityEngine.Events.UnityAction onClickAction, Canvas canvas)
    {
        // Create a button game object and add it to the canvas
        GameObject buttonObject = new GameObject(name);
        buttonObject.transform.SetParent(canvas.transform, false);

        // Add RectTransform and configure it
        RectTransform rectTransform = buttonObject.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(160, 30); // Button size

        // Align the buttons to the upper-right corner (anchor and pivot)
        rectTransform.anchorMin = new Vector2(1, 1); // Top-right corner
        rectTransform.anchorMax = new Vector2(1, 1); // Top-right corner
        rectTransform.pivot = new Vector2(1, 1);     // Set pivot to the top-right
        rectTransform.anchoredPosition = position;   // Button's position relative to the top-right corner

        // Add a background image to the button
        Image image = buttonObject.AddComponent<Image>();
        image.color = Color.white;

        // Add the Button component
        Button button = buttonObject.AddComponent<Button>();

        // Add button text
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(buttonObject.transform, false);

        Text text = textObject.AddComponent<Text>();
        text.text = buttonText;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.black; // Set text color to black

        // Adjust RectTransform for the text
        RectTransform textRectTransform = textObject.GetComponent<RectTransform>();
        textRectTransform.sizeDelta = rectTransform.sizeDelta;
        textRectTransform.anchoredPosition = Vector2.zero;

        // Add button click event
        button.onClick.AddListener(onClickAction);
    }

    // What happens when a button is clicked
    void ButtonClicked(string buttonText)
    {
        Debug.Log(buttonText + " clicked!");

        // Update the button press state
        if (buttonPresses.ContainsKey(buttonText))
        {
            buttonPresses[buttonText] = true; // Mark as pressed
        }

        // Example: Check if all buttons have been pressed
        if (AllButtonsPressed())
        {
            Debug.Log("All actions have been taken!");
        }
    }

    // Check if all buttons have been pressed at least once
    bool AllButtonsPressed()
    {
        foreach (var pressed in buttonPresses.Values)
        {
            if (!pressed)
            {
                return false; // If any button hasn't been pressed, return false
            }
        }
        return true; // All buttons have been pressed
    }

    // You can add a method to get the state of any button later in the game
    public bool HasButtonBeenPressed(string buttonText)
    {
        return buttonPresses.ContainsKey(buttonText) && buttonPresses[buttonText];
    }
}
