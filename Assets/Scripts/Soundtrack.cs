using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Soundtrack : MonoBehaviour
{
    public AudioSource myAudioSource;
    public AudioClip startSoundClip;
    public AudioClip crimeSceneClip;
    public AudioClip interrogationClip;

    private string currentScene;

    void Start()
    {
        DontDestroyOnLoad(gameObject); // This prevents the AudioSource from being destroyed when switching scenes

        // Play the initial sound (startSoundClip) and load the first scene when it's done
        myAudioSource.clip = startSoundClip;
        myAudioSource.Play();
        StartCoroutine(LoadSceneAfterSound("CrimeScene"));
    }

    IEnumerator LoadSceneAfterSound(string sceneName)
    {
        // Wait for the audio clip to finish
        yield return new WaitForSeconds(myAudioSource.clip.length);
        
        // Load the target scene
        SceneManager.LoadScene(sceneName);
        
        // Once the scene is loaded, play the corresponding clip
        currentScene = sceneName;
        SwitchMusicForScene();
    }

    void SwitchMusicForScene()
    {
        if (currentScene == "CrimeScene")
        {
            myAudioSource.clip = crimeSceneClip;
            myAudioSource.Play();
            StartCoroutine(LoadSceneAfterSound("Interrogation")); // After CrimeScene is done, move to Interrogation
        }
        else if (currentScene == "Interrogation")
        {
            myAudioSource.clip = interrogationClip;
            myAudioSource.Play();
        }
    }
}
