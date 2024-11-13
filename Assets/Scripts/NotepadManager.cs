using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    private Coroutine slideCoroutine;
    private HashSet<string> notedObjects = new HashSet<string>();
	private Dictionary<string, string[]> cluePairs = new Dictionary<string, string[]>();
	private Dictionary<string, Sprite> clueImages = new Dictionary<string, Sprite>();

	private List<string> orderOfObjects = new List<string>();
	private int currentPage = 0;
    private Canvas canvas;

	[SerializeField]
	TextMeshProUGUI notepadNameLeft;
	[SerializeField]
	TextMeshProUGUI notepadInfoLeft;
	[SerializeField]
	TextMeshProUGUI notepadNameRight;
	[SerializeField]
	TextMeshProUGUI notepadInfoRight;
	[SerializeField]
	Image leftImage;
	[SerializeField]
	Image rightImage;
    [SerializeField]
    Sprite defaultIcon;

    private RectTransform panelRectTransform;
    private Vector3 offScreenPosition;
    private Vector3 onScreenPosition;
    private bool isNotepadOpen = false;

    void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    // Start is called before the first frame update
    void Start()
    {
		if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        playerControls.Enable();
        
        canvas = GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            DontDestroyOnLoad(canvas.gameObject);  // Keep only the Canvas persistent
        }

        playerControls.UI.Notepad.performed += ctx => ToggleNotepad();
        playerControls.UI.FlipPageLeft.performed += ctx => flipPage(true);
        playerControls.UI.FlipPageRight.performed += ctx => flipPage(false);

        panelRectTransform = panelObject.GetComponent<RectTransform>();

        // Set off-screen and on-screen positions based on the height of the notepad panel
        onScreenPosition = panelRectTransform.anchoredPosition;
        float offScreenOffset = Screen.height * 0.75f;
        offScreenPosition = onScreenPosition + new Vector3(0, -Screen.height, 0);
        panelRectTransform.anchoredPosition = offScreenPosition;
    }

	public void AddInformation(string dictionaryItemName, string textItemName, string itemDescription, Sprite itemSprite)
    {
        // Only discover the object if it's new and not already noted
        if (cluePairs.ContainsKey(dictionaryItemName))
        {
            DiscoverableManager.instance.DiscoverObject();
			ReplaceInformation(dictionaryItemName, textItemName, itemDescription, itemSprite);
        }
		else{
			orderOfObjects.Add(dictionaryItemName);
        	cluePairs[dictionaryItemName] = new string[2]{textItemName, itemDescription};
			clueImages[dictionaryItemName] = itemSprite;
		}

    }

	private void ReplaceInformation(string dictionaryItemName, string textItemName, string newItemDescription, Sprite itemSprite){
		string oldDescription = cluePairs[dictionaryItemName][1];
		string[] oldDescriptionArray = oldDescription.Split(" ");
		string[] newDescriptionArray = newItemDescription.Split(" ");
		string finalReplacement = "";
		clueImages[dictionaryItemName] = itemSprite;
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
		if(clueImages[orderOfObjects[firstPageNum]] != null){
			leftImage.sprite = clueImages[orderOfObjects[firstPageNum]];
		}
		else{
			leftImage.sprite = defaultIcon;
		}
		if(firstPageNum + 1 < orderOfObjects.Count){
			string secondPageName = cluePairs[orderOfObjects[firstPageNum + 1]][0];
			string secondPageInfo = cluePairs[orderOfObjects[firstPageNum + 1]][1];
			notepadNameRight.text = secondPageName;
			notepadInfoRight.text = secondPageInfo;
			if(clueImages[orderOfObjects[firstPageNum + 1]] != null){
				rightImage.sprite = clueImages[orderOfObjects[firstPageNum + 1]];
			}
			else{
				rightImage.sprite = defaultIcon;
			}
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
        if (SceneManager.GetActiveScene().name != "CrimeScene")
            return;

        isNotepadOpen = !isNotepadOpen;

        // Stop any existing coroutine to prevent conflicts
        if (slideCoroutine != null)
        {
            StopCoroutine(slideCoroutine);
        }

        if (!panelObject.activeSelf)
        {
            panelObject.SetActive(true);
            panelObject.transform.localPosition = new Vector3(0, -Screen.height, 0); // Start offscreen
            StartCoroutine(SlideNotepad(isNotepadOpen));
        }
        else
        {
            StartCoroutine(SlideNotepad(isNotepadOpen));
        }
    }

    public IEnumerator SlideNotepad(bool open)
    {
        if (open && SceneManager.GetActiveScene().name == "CrimeScene")
        {
            // Ensure the text is visible before sliding up
            timeSlowedObject.alpha = 1.0f;
            StartCoroutine(FadeSlowedText());
        }

        float distance = Vector3.Distance(panelRectTransform.anchoredPosition, open ? onScreenPosition : offScreenPosition);
        float openSpeed = 3000f;
        float closeSpeed = 1000f;

        float duration = distance / (open ? openSpeed : closeSpeed);

        Vector3 targetPosition = open ? onScreenPosition : offScreenPosition;
        Vector3 startPosition = panelRectTransform.anchoredPosition;
        float elapsed = 0f;

        if (open) openingSound.Play();
        else closingSound.Play();

        Time.timeScale = open ? 0.25f : 1f;

        // Smooth slide with easing
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);
            panelRectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        panelRectTransform.anchoredPosition = targetPosition;

        if (open)
        {
            ShowPages(currentPage);
        }
        else
        {
            panelObject.SetActive(false);  // Deactivate the panel when closed
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
        yield return new WaitForSeconds(0.3f);

        float fadeDuration = 0.1f;
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            timeSlowedObject.alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeDuration);
            yield return null;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (playerControls != null)
        {
            playerControls.UI.Notepad.performed -= ctx => ToggleNotepad();
        }
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "NotepadScene")
        {
		    panelObject.SetActive(true);
        }
    }
}