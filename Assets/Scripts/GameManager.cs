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
    public AudioSource sharedDiscoverySound;

	private int interactionCount = 0;
    private bool win = false;
    
    //Police trust player amount
    private int trust = 0;

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
        InteractableObject.discoverySound = sharedDiscoverySound;
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

	public void RestartGame(){
		interactionCount = 0;
	}

    // Method to get the interaction count
    public int GetInteractionCount()
    {
        return interactionCount;
    }

    // Method to set the win condition
    public void SetWin(bool winCondition)
    {
        win = winCondition;
    }

    // Increase and Decrease Trust value for interrogation scene
    public void IncreaseTrust()
    {
        trust++;
    }

    public void DecreaseTrust()
    {
        trust--;
    }

    public int GetTrust()
    {
        return trust;
    }
}
