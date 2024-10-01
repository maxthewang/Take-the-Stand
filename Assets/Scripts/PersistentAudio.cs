using UnityEngine;

public class PersistentAudioSource : MonoBehaviour
{
    public AudioSource myAudioSource;
    void Start()
    {
        DontDestroyOnLoad(gameObject);  // This will ensure this audio source persists across scenes
    }
}
