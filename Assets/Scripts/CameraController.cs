using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public float mouseSensitivity = 6.0f;
    public float minTurnAngle = -90.0f;
    public float maxTurnAngle = 90.0f;
    private float rotX;
	[SerializeField]
	private GameObject playerCameraObject;
	private Camera playerCamera;

	private Interactable currentlyHoveredObject;

    void Start()
    {
        
		playerCamera = playerCameraObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        MouseAiming();
		OutlineObject();
		if(Input.GetKeyDown(KeyCode.E)){
			Interact();
		}
    }

    void MouseAiming ()
    {
    // get the mouse inputs
    float y = Input.GetAxis("Mouse X") * mouseSensitivity;
    rotX += Input.GetAxis("Mouse Y") * mouseSensitivity;
    // clamp the vertical rotation
    rotX = Mathf.Clamp(rotX, minTurnAngle, maxTurnAngle);
    // rotate the camera
    transform.eulerAngles = new Vector3(-rotX, transform.eulerAngles.y + y, 0);
    }

	private void OutlineObject(){
		
		RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit)) {
            Transform objectHit = hit.transform;
			if(Vector3.Distance(objectHit.position, transform.position) > 5f){
				if(currentlyHoveredObject != null){
					currentlyHoveredObject.TurnOffShader();
					currentlyHoveredObject = null;
				}
				return;
			}
			Interactable interactableObject = objectHit.GetComponent<Interactable>();
			if((interactableObject != currentlyHoveredObject || interactableObject == null) && currentlyHoveredObject != null){
				currentlyHoveredObject.TurnOffShader();
				currentlyHoveredObject = null;
			}
			if(interactableObject != null && interactableObject != currentlyHoveredObject){
				interactableObject.TurnOnShader();
				currentlyHoveredObject = interactableObject;
			}
            // Do something with the object that was hit by the raycast.
        }
	}

	private void Interact(){
		RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit)) {
            Transform objectHit = hit.transform;
			if(Vector3.Distance(objectHit.position, transform.position) > 5f){
				return;
			}
			Interactable interactableObject = objectHit.GetComponent<Interactable>();
			if(interactableObject != null){
				interactableObject.Interact();
			}
            // Do something with the object that was hit by the raycast.
        }
	}	
}
