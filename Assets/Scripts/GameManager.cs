using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set;}

    private bool switchingScenes = false;

    public bool notifiedPolice { get; set; }

    public bool touchedObjects { get; set; }

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
