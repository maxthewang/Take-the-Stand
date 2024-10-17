using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DialogManager : MonoBehaviour
{
    public Image actorImage;
    public TMP_Text actorName;
    public TMP_Text messageText;
    public GameObject regularDialogueBox;
	public GameObject choiceBox;
    public static bool isActive = false;

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
		currentMessages = optionStrings[selectedMessage];
		activeMessage = 0;
		DisplayMessage();
		ShowRegularDialogueBox();
		HideChoiceBox();
	}

	public void SelectOption(TMP_Text buttonName){
		MultipleChoice multipleChoiceToDisplay = (MultipleChoice)currentMessages[activeMessage];
		ChooseOptionFromOptionsMenu(multipleChoiceToDisplay.optionStrings, buttonName.text);	
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

	private void DisplayOptions(){
		HideRegularDialogueBox();
		MultipleChoice multipleChoiceToDisplay = (MultipleChoice)currentMessages[activeMessage];
		int i = 0;
		foreach (string key in multipleChoiceToDisplay.optionStrings.Keys){
			buttonTexts[i].text = key;
			i++;
		}
		ShowChoiceBox();
	}

    void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessage];
		if(messageToDisplay.message == "Choose an Option"){
			// Display the options menu and hide the dialogue menu
			DisplayOptions();
			return;
		}
        messageText.text = messageToDisplay.message;

        Actor actorToDisplay = currentActors[messageToDisplay.actorid];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;

    }

    public void NextMessage()
    {
        activeMessage++;
        if (activeMessage < currentMessages.Length)
        {
            DisplayMessage();
        } else
        {
            Debug.Log("Conversation Ended");
            isActive = false;

            if (SceneManager.GetActiveScene().name == "Intro")
            {
                SceneManager.LoadScene("CrimeScene");
            }

            int interactionCount = GameManager.instance.GetInteractionCount();
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
