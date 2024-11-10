using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NotepadManager : MonoBehaviour
{
	public static NotepadManager instance;
	[SerializeField]
	TextMeshProUGUI notepadInformation;

	
	[SerializeField]
	GameObject panelObject;
	[SerializeField]
	CanvasGroup timeSlowedObject;
	[SerializeField]
    PlayerInputActions playerControls;
    [SerializeField]
    private AudioSource openingSound;
    [SerializeField]
    private AudioSource closingSound;
    private Coroutine fadeCoroutine;
    private HashSet<string> notedObjects = new HashSet<string>();
	private Dictionary<string, string[]> cluePairs = new Dictionary<string, string[]>();

	private List<string> orderOfObjects = new List<string>();
	private int currentPage = 0;

	[SerializeField]
	TextMeshProUGUI notepadNameLeft;
	[SerializeField]
	TextMeshProUGUI notepadInfoLeft;
	[SerializeField]
	TextMeshProUGUI notepadNameRight;
	[SerializeField]
	TextMeshProUGUI notepadInfoRight;
    void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    // Start is called before the first frame update
    void Start()
    {
		if(instance != null){
			Destroy(this);
			return;
		}
		instance = this;
		DontDestroyOnLoad(this);
        playerControls.Enable();

        // Subscribe to the notepad toggle action
        playerControls.UI.Notepad.performed += ctx => ToggleNotepad();
		playerControls.UI.FlipPageLeft.performed += ctx => flipPage(true);
		playerControls.UI.FlipPageRight.performed += ctx => flipPage(false);
    }

	public void AddInformation(string dictionaryItemName, string textItemName, string itemDescription)
    {
        // Only discover the object if it's new and not already noted
        if (cluePairs.ContainsKey(dictionaryItemName))
        {
            DiscoverableManager.instance.DiscoverObject();
			ReplaceInformation(dictionaryItemName, textItemName, itemDescription);
        }
		else{
			orderOfObjects.Add(dictionaryItemName);
        	cluePairs[dictionaryItemName] = new string[2]{textItemName, itemDescription};
		}

    }

	private void ReplaceInformation(string dictionaryItemName, string textItemName, string newItemDescription){
		string oldDescription = cluePairs[dictionaryItemName][1];
		string[] oldDescriptionArray = oldDescription.Split(" ");
		string[] newDescriptionArray = newItemDescription.Split(" ");
		string finalReplacement = "";
		for(int i = 0; i < newDescriptionArray.Length; i++){
			if(Array.IndexOf(oldDescriptionArray, newDescriptionArray[i]) == -1){
				finalReplacement += "<color=#8B0000><b>" + newDescriptionArray[i] + "</b></color>" + " ";
			}
			else{
				finalReplacement += newDescriptionArray[i] + " ";
			}
		}

		currentPage = FindPageOfItem(dictionaryItemName);
		if(currentPage % 2 != 0){
			currentPage--;
		}

        cluePairs[dictionaryItemName][0] = $"<color=#8B0000><b>{textItemName}</b></color>";
		cluePairs[dictionaryItemName][1] = finalReplacement;
	}

	private int FindPageOfItem(string itemName){
		for(int i = 0; i < orderOfObjects.Count; i++){
			if(orderOfObjects[i].Equals(itemName)){
				return i;
			}
		}
		return -1;
	}

	private void ShowPages(int firstPageNum){
		string firstPageName = cluePairs[orderOfObjects[firstPageNum]][0];
		string firstPageInfo = cluePairs[orderOfObjects[firstPageNum]][1];
		notepadNameLeft.text = firstPageName;
		notepadInfoLeft.text = firstPageInfo;
		notepadNameRight.text = "";
		notepadInfoRight.text = "";
		if(firstPageNum + 1 < orderOfObjects.Count){
			string secondPageName = cluePairs[orderOfObjects[firstPageNum + 1]][0];
			string secondPageInfo = cluePairs[orderOfObjects[firstPageNum + 1]][1];
			notepadNameRight.text = secondPageName;
			notepadInfoRight.text = secondPageInfo;
		}
	}

	public void flipPage(bool flipLeft){
		if(!flipLeft){
			if(currentPage + 2 >= orderOfObjects.Count)
				return;
			currentPage += 2;
			ShowPages(currentPage);
		}
		else{
			if(currentPage - 2 < 0)
				return;
			currentPage -= 2;
			ShowPages(currentPage);
		}
	}

	private void CompileNotepadInformation(){
		string newNotepadInfo = "";
		foreach(var notepadInfo in cluePairs){
			newNotepadInfo += notepadInfo.Value;
		}
		notepadInformation.text = newNotepadInfo;
	}


	private void ToggleNotepad()
    {
		if(SceneManager.GetActiveScene().name != "CrimeScene"){
			return;
		}
        bool isActive = panelObject.activeSelf; // Store current state
		ShowPages(currentPage);

        if (isActive)
        {
            // Notepad closed
            panelObject.SetActive(false);
            Time.timeScale = 1f;
            closingSound.Play();
        }
        else
        {
            // Notepad opened
            panelObject.SetActive(true);
            Time.timeScale = 0.25f;
            openingSound.Play();
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);  // Stop any existing fade coroutine
            }

            timeSlowedObject.alpha = 1.0f;  // Ensure the text is fully visible
            fadeCoroutine = StartCoroutine(FadeSlowedText());
        }
    }

	public void OpenNotepad(){
		ShowPages(currentPage);
		panelObject.SetActive(true);
	}

	public void CloseNotepad(){
		panelObject.SetActive(false);
	}

    private IEnumerator FadeSlowedText()
    {
        // Show the text for 2 seconds
        yield return new WaitForSeconds(0.2f);

        // Gradually fade out the text over 2 seconds
        float fadeDuration = 0.1f;
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            timeSlowedObject.alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeDuration);
            yield return null;
        }
    }

    private void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.UI.Notepad.performed -= ctx => ToggleNotepad();
        }
    }
}