using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovingInteractable : InteractableObject
{
	Vector3 originalPosition;
	public Vector3 moveToDestination;
	bool movingToDestination = false;
    public AudioSource humSound;
    private AudioSource movementSound;
		public Vector3 scaleToReach;
    
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.localPosition;
        movementSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movingToDestination)
        {
            if (movementSound != null && !movementSound.isPlaying)
            {
                movementSound.Play();  // Start playing when movement begins
            }

            // Smoothly move the object toward the destination
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, moveToDestination, 2f * Time.deltaTime);
			transform.localScale = Vector3.MoveTowards(transform.localScale, scaleToReach, 0.20f * Time.deltaTime);

            // Stop movement when close enough to the destination
            if ((transform.localPosition - moveToDestination).magnitude < 0.01f)
            {
                movingToDestination = false;
                movementSound.Stop();

                if (humSound != null && !humSound.isPlaying)
                {
                    humSound.Play();  // Start playing when movement begins
                }
            }
        }
    }

	protected override void OnInteract(InputAction.CallbackContext context)
	{
		
	}

	public override void Interact()
	{
		movingToDestination = true;
	}
}
