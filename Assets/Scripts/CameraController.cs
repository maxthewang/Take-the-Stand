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

	private Outlined currentlyHoveredObject;

    void Start()
    {
        
		playerCamera = playerCameraObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        MouseAiming();
		OutlineObject();
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

        float adjustedScreenWidth = 300;
        float adjustedScreenHeight = 200;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(adjustedScreenWidth / 2, adjustedScreenHeight / 2, 0));
        
        if (Physics.Raycast(ray, out hit)) {
            Transform objectHit = hit.transform;
			if(Vector3.Distance(objectHit.position, transform.position) > 10f){
				if(currentlyHoveredObject != null){
					currentlyHoveredObject.TurnOffShader();
					currentlyHoveredObject = null;
				}
				return;
			}
			Outlined interactableObject = objectHit.GetComponent<Outlined>();
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
}
