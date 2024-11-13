using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableTrigger : InteractableObject
{
	public InteractableObject objectConnectedTo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public override void Interact()
	{
		objectConnectedTo.Interact();
	}

}
