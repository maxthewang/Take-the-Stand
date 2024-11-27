using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
    // Start is called before the first frame update
	public Vector3 initialPosition;
	public Vector3 finalPosition;
	private Vector3 moveToDestination;
	bool goingDown = false;
    void Start()
    {
		initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		MoveTowardsVector();
		if(goingDown){
			moveToDestination = initialPosition;
		}
		else{
			moveToDestination = finalPosition;
		}
    }

	private void MoveTowardsVector(){
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, moveToDestination, 8f * Time.deltaTime);

            // Stop movement when close enough to the destination
            if ((transform.localPosition - moveToDestination).magnitude < 0.01f)
            {
                goingDown = !goingDown;
            }
	}
}
