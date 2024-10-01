using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CrimeSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    GameManager gameManager;
    private float currentTime;
    private float timeInCrimeScene = 10.0f;

    private Image timerImage;
    void Start()
    {
        gameManager = GameManager.instance;
        timerImage = GameObject.Find("Canvas/Timer").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        timerImage.fillAmount = (timeInCrimeScene - currentTime) / timeInCrimeScene;
        //Debug.Log(currentTime + " " + timeInCrimeScene);
        if(currentTime >= timeInCrimeScene){
            gameManager.ChangeSceneTest();
            //SceneManager.LoadScene("Assets/Scenes/Test Scene2.unity");
        }
    }
}
