using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Slider volumeSlider;
    private AudioSource audioSource;

    [SerializeField] private GameObject optionsMenu;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        audioSource = audioPanel.GetComponent<AudioSource>();
    }

    public void SetMouseSensitivity()
    {
        player.SetSensitivity(mouseSensitivitySlider.value);
    }

    public void SetVolume()
    {
        audioSource.volume = volumeSlider.value;
    }

}
