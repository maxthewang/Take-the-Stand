using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

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
    private List<string> correctStrings = new List<string> {"Quite well actually.", "Out the side door.", "Gasoline and a lighter.", "An old farmhouse.", $"{GameManager.instance.GetInteractionCount()}", "I think so.", "We stopped at a gas station.", "The flames would've disintegrated it all.", "In the nighttime.", "I was the only one.", "Yes, I believe so.", "It was Warren.", "In through the window.", "It must've been the gun.", "Warren was a liar.", "There was a document."};

	public List<TMP_Text> buttonTexts;

    [SerializeField]
    private PlayerInputActions playerControls;
    private InputAction nextMessageAction;

	[SerializeField]
	private GameObject leftButton;

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
        if (correctStrings.Contains(selectedMessage)) {
            GameManager.instance.IncreaseTrust();
        } else {
            GameManager.instance.DecreaseTrust();
        }
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
		EventSystem.current.SetSelectedGameObject(null);
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

	private async void ShowChoiceBox(){
		// Show the choice box
		choiceBox.SetActive(true);
		StartCoroutine(SetButton());
	}

	IEnumerator SetButton(){
		yield return new WaitForSeconds(0.1f);
		EventSystem.current.SetSelectedGameObject(leftButton);
	}

	private void DisplayOptions()
    {
        HideRegularDialogueBox();
        MultipleChoice multipleChoiceToDisplay = (MultipleChoice)currentMessages[activeMessage];
        
        StopAllCoroutines();
        StartCoroutine(TypeMessage(choiceMessageText, multipleChoiceToDisplay.message));

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

            Actor actorToDisplay = currentActors[messageToDisplay.actorid];
            actorName.text = actorToDisplay.name;
            actorImage.sprite = actorToDisplay.sprite;

            StopAllCoroutines();
            StartCoroutine(TypeMessage(messageText, messageToDisplay.message));
        }
    }

    public void NextMessage()
    {
        activeMessage++;
        boxSound.pitch = Random.Range(1.0f, 1.8f);
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
				FadeTransition.instance.FadeToBlack("CrimeScene");
            }

            int interactionCount = GameManager.instance.GetInteractionCount();
            if (interactionCount == 0)
            {
				FadeTransition.instance.FadeToBlack("CrimeScene");
            }
            else
            {
                int trust = GameManager.instance.GetTrust();
                if (trust >= 15)
                {
					FadeTransition.instance.FadeToBlack("Win Scene");
                }
                else
                {
					FadeTransition.instance.FadeToBlack("Lose Scene");
                }
            }
        }
    }
    
    private void OnNextMessage(InputAction.CallbackContext context)
    {
        if (isActive && !(currentMessages[activeMessage] is MultipleChoice))
        {
            NextMessage();
        }
    }

	IEnumerator TypeMessage(TMP_Text textObject, string message)
	{
		textObject.text = "";
		foreach (char letter in message.ToCharArray())
		{
			textObject.text += letter;
			yield return new WaitForSeconds(0.02f);
		}
        if (boxSound.isPlaying)
        {
            boxSound.Stop();
        }
	}
}
