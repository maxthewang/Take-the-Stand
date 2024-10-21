using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Linq;

public class DialogManager : MonoBehaviour
{
    public Image actorImage;
    public TMP_Text actorName;
    public TMP_Text messageText;
    public GameObject regularDialogueBox;
	public GameObject choiceBox;
    public TMP_Text choiceMessageText;
    public AudioSource boxSound;
    public static bool isActive = true;

	public List<TMP_Text> buttonTexts;

    [SerializeField]
    private PlayerInputActions playerControls;
    private InputAction nextMessageAction;

    Message[] currentMessages;
    Actor[] currentActors;
    public int activeMessage = 0;

    void Awake()
    {
        playerControls = new PlayerInputActions();
        nextMessageAction = playerControls.UI.AdvanceDialogue;
    }

    void OnEnable()
    {
        nextMessageAction.performed += OnNextMessage;
        playerControls.Enable();
    }

    void OnDisable()
    {
        nextMessageAction.performed -= OnNextMessage;
        playerControls.Disable();
    }

    public void OpenDialog(Message[] messages, Actor[] actors)
    {
        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;
        isActive = true;

        Debug.Log("Started Conversation! num messages = " + messages.Length);
        DisplayMessage();
    }

	public void ChooseOptionFromOptionsMenu(Dictionary<string, Message[]> optionStrings, string selectedMessage){
		Message[] remainingMessages = currentMessages.Skip(activeMessage + 1).ToArray();
        currentMessages = optionStrings[selectedMessage].Concat(remainingMessages).ToArray();

		activeMessage = 0;
		DisplayMessage();
		ShowRegularDialogueBox();
		HideChoiceBox();
	}

	public void SelectOption(TMP_Text buttonName){
        DisableButtons();
		MultipleChoice multipleChoiceToDisplay = (MultipleChoice)currentMessages[activeMessage];
		ChooseOptionFromOptionsMenu(multipleChoiceToDisplay.optionStrings, buttonName.text);
	}

    // Method to disable buttons
    private void DisableButtons()
    {
        foreach (var buttonText in buttonTexts)
        {
            buttonText.transform.parent.GetComponent<Button>().interactable = false;
        }
    }

    // Method to re-enable buttons before showing new options
    private void EnableButtons()
    {
        foreach (var buttonText in buttonTexts)
        {
            buttonText.transform.parent.GetComponent<Button>().interactable = true;
        }
    }

	private void HideRegularDialogueBox(){
		// Hide the dialogue box
		regularDialogueBox.SetActive(false);
	}

	private void ShowRegularDialogueBox(){
		// Show the dialogue box
		regularDialogueBox.SetActive(true);
	}

	private void HideChoiceBox(){
		// Hide the choice box
		choiceBox.SetActive(false);
	}

	private void ShowChoiceBox(){
		// Show the choice box
		choiceBox.SetActive(true);
	}

	private void DisplayOptions()
    {
        HideRegularDialogueBox();
        MultipleChoice multipleChoiceToDisplay = (MultipleChoice)currentMessages[activeMessage];
        choiceMessageText.text = multipleChoiceToDisplay.message;
        EnableButtons();

        int i = 0;
        foreach (string key in multipleChoiceToDisplay.optionStrings.Keys)
        {
            buttonTexts[i].text = key;
            i++;
        }

        ShowChoiceBox();
    }

    void DisplayMessage()
    {
        if (activeMessage < currentMessages.Length)
        {
            Message messageToDisplay = currentMessages[activeMessage];
            if (messageToDisplay is MultipleChoice)
            {
                DisplayOptions();
                return;
            }

            messageText.text = messageToDisplay.message;

            Actor actorToDisplay = currentActors[messageToDisplay.actorid];
            actorName.text = actorToDisplay.name;
            actorImage.sprite = actorToDisplay.sprite;
        }
    }

    public void NextMessage()
    {
        activeMessage++;
        boxSound.Play();
        if (activeMessage < currentMessages.Length)
        {
            DisplayMessage();
        }
        else
        {
            Debug.Log("Conversation Ended");
            isActive = false;

            if (SceneManager.GetActiveScene().name == "Intro")
            {
                SceneManager.LoadScene("CrimeScene");
            }

            // int interactionCount = GameManager.instance.GetInteractionCount();
            int interactionCount = 1;
            if (interactionCount == 0)
            {
                SceneManager.LoadScene("CrimeScene");
            }
        }
    }
    
    private void OnNextMessage(InputAction.CallbackContext context)
    {
        if (isActive)
        {
            NextMessage();
        }
    }
}
