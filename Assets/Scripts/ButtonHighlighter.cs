using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHighlighter : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject pumpkinHighlight;
	void OnDisable(){
		if(pumpkinHighlight == null){
			return;
		}
        pumpkinHighlight.SetActive(false);
	}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelect(BaseEventData eventData){
		if(pumpkinHighlight == null){
			return;
		}
        pumpkinHighlight.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData){
		if(pumpkinHighlight == null){
			return;
		}
        pumpkinHighlight.SetActive(false);
    }

}
