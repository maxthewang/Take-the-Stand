using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set;}

    private Scenes currScene;

    private bool switchingScenes = false;

    public bool notifiedPolice { get; set; }

    public bool touchedObjects { get; set; }

	[SerializeField]
	private Canvas EscapeWindow;

	public Dictionary<string, bool> Inventory = new Dictionary<string, bool>();

	 private int interactionCount = 0 ;

    private enum Scenes{
        CrimeScene,
        InterrogationScene,
        StartScene,
    }

    void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(this);
        }
        else{
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
		Cursor.lockState = CursorLockMode.Locked;
        currScene = Scenes.StartScene;
		CloseEscapeWindow();
    }

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKeyDown(KeyCode.Escape)){
			Debug.Log("Hitting esc key");
			ToggleEscapeWindow();		
		}
    }

    public void ChangeSceneTest(){
        if(switchingScenes){
            return;
        }
        switchingScenes = true;
        StartCoroutine(LoadSceneAsync("Assets/Scenes/Interrogation.unity"));
    }

    IEnumerator LoadSceneAsync(string sceneName){
        yield return new WaitForSeconds(0.2f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while(!asyncLoad.isDone){
            yield return null;
        }
    }

	public void QuitGame(){
		Application.Quit();
	}

	public void CloseEscapeWindow(){
		EscapeWindow.enabled = false;
		Cursor.lockState = CursorLockMode.Locked;	
	}

	public void ToggleEscapeWindow(){
		Debug.Log("Toggled escape window");
		EscapeWindow.enabled = !EscapeWindow.enabled;
		if(EscapeWindow.enabled){
			Cursor.lockState = CursorLockMode.Confined;
		}
		else{
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

    // Method to increment interaction count
    public void AddInteraction()
    {
        interactionCount++;
    }

    // Method to get the interaction count
    public int GetInteractionCount()
    {
        return interactionCount;
    }
}
