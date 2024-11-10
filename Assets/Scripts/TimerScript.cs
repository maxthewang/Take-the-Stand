using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour
{
    public float TimeLeft;
    public bool TimerOn = false;
    public AudioSource sirenSound;
    private bool sirenPlaying = false;

    public TextMeshProUGUI TimerTxt;

	public GameObject notepadObject;
   
    void Start()
    {
        TimerOn = true;
		
    }

    void Update()
    {
        if(TimerOn)
        {
            if(TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);

                if(TimeLeft <= 30f && !sirenPlaying)
                {
                    sirenSound.Play();
                    sirenPlaying = true;
                } else if (sirenPlaying) {
                    sirenSound.volume = (30 - TimeLeft) / 30;
                }
            }
            else
            {
                Debug.Log("Time is UP!");
                TimeLeft = 0;
                TimerOn = false;
				FadeTransition.instance.FadeToBlack("Interrogation");
            }
        }
    }

    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}