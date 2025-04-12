using UnityEngine;
using TMPro;
using System.Collections;

public class TimerScript : MonoBehaviour
{
    public float TimeLeft;
    public bool TimerOn = false;
    public AudioSource sirenSound;
    public AudioSource clockSound;
    public AudioSource bellSound;
    public AudioSource policeVoiceLine;
    public AudioSource introVoiceLine;
    public AudioSource sirenVoiceLine;
    private bool sirenPlaying = false;
    private bool voicePlayed = false;
    private bool flashing = false;
    private Color flashColor = Color.red;

    public TextMeshProUGUI TimerTxt;

    public GameObject notepadObject;
    public GameObject playerObject;
    public GameObject policeObject;

    void Start()
    {
        TimerOn = true;
    }

    void Update()
    {
        if (TimerOn)
        {
            if (TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                updateTimer(TimeLeft);

                if (TimeLeft <= 30f && !sirenPlaying)
                {
                    sirenSound.Play();
                    sirenVoiceLine.Play();
                    sirenPlaying = true;
                    flashing = true;
                }
                else if (sirenPlaying)
                {
                    sirenSound.volume = (30 - TimeLeft) / 30;
                }

                if (TimeLeft <= 10f && !clockSound.isPlaying)
                {
                    clockSound.Play();
                }
            }
            else
            {
                TimeLeft = 0;
                TimerOn = false;

                bellSound.Play();
                policeVoiceLine.Play();
                AnimatorController animatorController = playerObject.GetComponent<AnimatorController>();
                GunAnimatorController policeController = policeObject.GetComponent<GunAnimatorController>();
                animatorController.OnTimerEnd();
                policeController.OnTimerEnd();

                StartCoroutine(fadeOut());
            }
        }
        if (flashing)
        {
            float alpha = Mathf.PingPong(Time.time * 2f, 0.5f) + 0.5f; // oscillates between 0.5 and 1.0
            TimerTxt.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
        }
    }

    void updateTimer(float currentTime)
    {
        if (!voicePlayed)
        {
            introVoiceLine.Play();
            voicePlayed = true;
        }
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        TimerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(4.0f);

        FadeTransition.instance.FadeToBlack("NotepadScene");
    }

}