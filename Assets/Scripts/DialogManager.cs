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
    private List<string> correctStrings = new List<string> { "Quite well actually.", "Out the side door.", "Gasoline and a lighter.", "An old farmhouse.", $"{GameManager.instance.GetInteractionCount()}", "I think so.", "We stopped at a gas station.", "The flames would've disintegrated it all.", "In the nighttime.", "I was the only one.", "Yes, I believe so.", "It was Warren.", "In through the window.", "It must've been the gun.", "Warren was a liar.", "There was a document." };

    public List<TMP_Text> buttonTexts;

    [SerializeField]
    private PlayerInputActions playerControls;
    private InputAction nextMessageAction;
    private InterrogatorAnimationManager interrogatorAnimationManager;
    private AudioSource interrogatorVoice;
    private AudioSource interrogatorResponseVoice;
    private AudioSource mainCharacterVoice;
    private bool isPlayingLine = false;

    [SerializeField]
    private GameObject leftButton;
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private CanvasGroup feedbackCanvasGroup;
    private Coroutine typingCoroutine;

    Message[] currentMessages;
    Actor[] currentActors;
    public int activeMessage = 0;

    private void Start()
    {
        mainCharacterVoice = GameObject.FindWithTag("Player").GetComponentInChildren<AudioSource>();
        if (mainCharacterVoice == null)
        {
            mainCharacterVoice = GameObject.Find("MainCharVoice").GetComponent<AudioSource>();
            if (mainCharacterVoice == null)
            {
                Debug.LogError("Main character voice not found");
            }
        }
        interrogatorAnimationManager = GameObject.FindWithTag("Interrogator").GetComponent<InterrogatorAnimationManager>();
        if (interrogatorAnimationManager == null)
        {
            interrogatorAnimationManager = GameObject.Find("Interrogator").GetComponent<InterrogatorAnimationManager>();
            if (interrogatorAnimationManager == null)
            {
                Debug.LogError("Interrogator animation manager not found");
            }
        }
        interrogatorVoice = GameObject.Find("InterrogatorVoice").GetComponent<AudioSource>();
        if (interrogatorVoice == null)
        {
            Debug.LogError("Interrogator voice not found");
        }
        interrogatorResponseVoice = GameObject.Find("InterrogatorResponseVoice").GetComponent<AudioSource>();
        if (interrogatorResponseVoice == null)
        {
            Debug.LogError("Interrogator response voice not found");
        }

        feedbackCanvasGroup.alpha = 0f;
    }

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
        DisplayMessage();
    }

    public void ChooseOptionFromOptionsMenu(Dictionary<string, Message[]> optionStrings, string selectedMessage)
    {
        Message[] remainingMessages = currentMessages.Skip(activeMessage + 1).ToArray();
        currentMessages = optionStrings[selectedMessage].Concat(remainingMessages).ToArray();

        activeMessage = 0;
        isPlayingLine = false;
        if (correctStrings.Contains(selectedMessage))
        {
            // Correct
            StartCoroutine(ShowFeedback("+1", Color.green));
            interrogatorResponseVoice.clip = MultipleChoice.LoadRandomPositiveResponse();
            GameManager.instance.IncreaseTrust();
        }
        else
        {
            // Incorrect
            StartCoroutine(ShowFeedback("-1", Color.red));
            interrogatorAnimationManager.PlayRandomNegativeAnimation();
            interrogatorResponseVoice.clip = MultipleChoice.LoadRandomNegativeResponse();
            GameManager.instance.DecreaseTrust();
        }
        if (interrogatorResponseVoice.clip == null)
        {
            Debug.LogError("No response found");
        }
        else
        {
            Debug.Log("Playing response: " + interrogatorResponseVoice.clip.name, interrogatorResponseVoice.clip);
        }

        interrogatorResponseVoice.Play();
        if (!interrogatorResponseVoice.isPlaying)
        {
            Debug.LogError("Voice line not playing");
        }
        DisplayMessage();
        ShowRegularDialogueBox();
        HideChoiceBox();
    }

    public void SelectOption(TMP_Text buttonName)
    {
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

    private void HideRegularDialogueBox()
    {
        // Hide the dialogue box
        regularDialogueBox.SetActive(false);
    }

    private void ShowRegularDialogueBox()
    {
        // Show the dialogue box
        regularDialogueBox.SetActive(true);
    }

    private void HideChoiceBox()
    {
        // Hide the choice box
        choiceBox.SetActive(false);
    }

    private async void ShowChoiceBox()
    {
        // Show the choice box
        choiceBox.SetActive(true);
        StartCoroutine(SetButton());
    }

    IEnumerator SetButton()
    {
        yield return new WaitForSeconds(0.1f);
        EventSystem.current.SetSelectedGameObject(leftButton);
    }

    private void DisplayOptions()
    {
        HideRegularDialogueBox();
        MultipleChoice multipleChoiceToDisplay = (MultipleChoice)currentMessages[activeMessage];

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeMessage(choiceMessageText, multipleChoiceToDisplay.message));

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
            interrogatorAnimationManager.PlayCalmDown();
            Message messageToDisplay = currentMessages[activeMessage];
            if (messageToDisplay is MultipleChoice)
            {
                interrogatorAnimationManager.PlayLeanForward();
                DisplayOptions();
                return;
            }

            Actor actorToDisplay = currentActors[messageToDisplay.actorid];
            actorName.text = actorToDisplay.name;
            actorImage.sprite = actorToDisplay.sprite;
            StopPlayingVoicelines();
            if (messageToDisplay.actorid.Equals(0) && messageToDisplay.voiceline != null)
            {
                interrogatorVoice.clip = messageToDisplay.voiceline;
                Debug.Log("Playing voiceline: " + messageToDisplay.voiceline.name, interrogatorVoice.clip);
                isPlayingLine = true;
                interrogatorVoice.Play();
                if (SceneManager.GetActiveScene().name == "Intro")
                {
                    interrogatorAnimationManager.PlayRandomNegativeAnimation();
                }
                else
                {
                    interrogatorAnimationManager.PlayCalmDown();
                }
            }
            else if (messageToDisplay.actorid.Equals(1) && messageToDisplay.voiceline != null)
            {
                mainCharacterVoice.clip = messageToDisplay.voiceline;
                isPlayingLine = true;
                mainCharacterVoice.Play();
            }
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            typingCoroutine = StartCoroutine(TypeMessage(messageText, messageToDisplay.message));
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
                if (trust >= 12)
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

    private IEnumerator ShowFeedback(string text, Color color)
    {
        feedbackText.text = text;
        feedbackText.color = color;
        feedbackCanvasGroup.alpha = 1f;

        Vector3 originalPos = feedbackText.transform.localPosition;
        Vector3 targetPos = originalPos + Vector3.up * 50;

        float elapsed = 0f;
        while (elapsed < 1.0f)
        {
            elapsed += Time.deltaTime;
            feedbackText.transform.localPosition = Vector3.Lerp(originalPos, targetPos, elapsed / 1.0f);
            feedbackCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / 1.0f);
            yield return null;
        }

        feedbackText.text = "";
        feedbackText.transform.localPosition = originalPos;
    }


    public void StopPlayingVoicelines()
    {
        if (isPlayingLine)
        {
            Debug.Log("Stopping voicelines");
            interrogatorVoice.Stop();
            mainCharacterVoice.Stop();
            isPlayingLine = false;
        }
    }
}
