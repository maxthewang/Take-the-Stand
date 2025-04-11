using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public float minTurnAngle = -90.0f;
    public float maxTurnAngle = 90.0f;

	public float xNum, yNum;
    private float rotX;
	[SerializeField]
	private GameObject playerCameraObject;
	public Camera playerCamera;
	private Outlined currentlyHoveredObject;
    public PlayerInputActions playerControls;
    public PlayerController playerController;
    private float zoomOutFOV = 60;
    private float zoomInFOV = 25;
    private float targetFOV;
    private float currentFOV;
    private InputAction zoom;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

	public GameObject notepadGameObject;

    void Start()
    {
        currentFOV = zoomOutFOV;
        targetFOV = zoomOutFOV;
        Camera.main.fieldOfView = currentFOV;

        zoom = playerControls.Player.Zoom;
        zoom.Enable();
    }

    // Update is called once per frame
    void Update()
    {
		OutlineObject();
        
        if(zoom.ReadValue<float>() > 0.5f)
        {
            // Zoom in
            targetFOV = zoomInFOV;
            playerController.SetSensitivityMultiplier(0.1f);
        }
        else
        {
            // Zoom out
            targetFOV = zoomOutFOV;
            playerController.SetSensitivityMultiplier(1.0f);
        }

        currentFOV = Mathf.Lerp(currentFOV, targetFOV, Time.deltaTime * 5.0f);
        Camera.main.fieldOfView = currentFOV;
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
			Debug.Log("Hit something and turning off shader | interactable obj null: " +  (interactableObject == null) + " | " + "curr hoveredobj != null: " + (currentlyHoveredObject != null));
			if(interactableObject == null){
				Debug.Log("This is the object name since its null " + objectHit.name);
			}
				currentlyHoveredObject.TurnOffShader();
				currentlyHoveredObject = null;
			}
			if(interactableObject != null && interactableObject != currentlyHoveredObject){
			Debug.Log("Hit something and turning on shader");
				interactableObject.TurnOnShader();
				currentlyHoveredObject = interactableObject;
			}
            // Do something with the object that was hit by the raycast.
        }
	}

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
