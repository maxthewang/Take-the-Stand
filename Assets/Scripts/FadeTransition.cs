using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeTransition : MonoBehaviour
{
	public static FadeTransition instance;

	public CanvasGroup canvasGroup;

	public bool fading = false;

	public bool fadingToBlack = false;

	public float alphaToReach = 0;

	public float alphaToAdd = 1;

	public string sceneToLoadAfterFade = "";

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
        FadeToClear();
    }

    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            canvasGroup.gameObject.SetActive(true);

			if (fadingToBlack)
            {
				if (canvasGroup.alpha >= alphaToReach)
                {
					fading = false;
					SceneManager.LoadScene(sceneToLoadAfterFade);
					FadeToClear();
					return;
				}
				canvasGroup.alpha += alphaToAdd;
			}
			else
            {
				if (canvasGroup.alpha <= alphaToReach)
                {
					fading = false;
                    canvasGroup.gameObject.SetActive(false);
					return;
				}
				canvasGroup.alpha += alphaToAdd;
			}
		}
    }

	public void FadeToBlack(string newScene){
		sceneToLoadAfterFade = newScene;
		alphaToReach = 1;
		alphaToAdd = 0.01f;
		fading = true;
		fadingToBlack = true;
	}

	public void FadeToClear(){
		alphaToReach = 0;
		alphaToAdd = -0.01f;
		fading = true;
		fadingToBlack = false;
	}

}
