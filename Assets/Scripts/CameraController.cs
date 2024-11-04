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
    public float zoom1 = 60;
    public float zoom2 = 30;

	public float xNum, yNum;
    private float rotX;
	[SerializeField]
	private GameObject playerCameraObject;
	public Camera playerCamera;
	private Outlined currentlyHoveredObject;

    void Start()
    {
        Camera.main.fieldOfView = zoom1;
    }

    // Update is called once per frame
    void Update()
    {
        MouseAiming();
		OutlineObject();
        if(Input.GetMouseButton(1)/* || Input.GetAxis("rightTrigger") == 1*/)
        {
            Debug.Log("Zooming in.");
            Camera.main.fieldOfView = zoom2;
        }
        if(Input.GetMouseButtonUp(1) /* || Input.GetAxis("rightTrigger") == 0*/)
        {
            Debug.Log("Zooming out.");
            Camera.main.fieldOfView = zoom1;
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
    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + y, 0);
    }

	private void OutlineObject(){
		
		RaycastHit hit;

		Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        
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
