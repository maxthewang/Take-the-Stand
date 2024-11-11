using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 4.0f; 
    public float runSpeed = 7.0f;
    public float rotateSpeed = 120.0f; 
    public float jumpForce = 20.0f; 
    public AudioSource walkingSound;
    public PlayerInputActions playerControls;
	public Camera playerCamera;

    private InputAction move;
    private InputAction look;
    private InputAction jump;
    private float baseSensitivity = 0.5f;
    private float turnSensitivity;
    private float targetVolume = 0f;
    private Rigidbody rb;
    private CharacterController controller;
    private Vector3 moveSpeed;
    private int controllerFlag = 0; // 0 for rigidbody, 1 for character controller
    private bool isGrounded; 
    public float gravity = -9.8f;
    private float rotationX = 0.0f; // X rotation for looking up/down

	public GameObject notepadGameObject;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
        look = playerControls.Player.Look;
        look.Enable();
        jump = playerControls.Player.Jump;
        jump.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        look.Disable();
        jump.Disable();
    }
    
    // Start is called before the first frame update
    // This script is responsible for the movement of the player either using rigidbody 
    // or character controller
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controllerFlag = rb == null ? 1 : 0;
        if (controllerFlag == 1) {
            controller = GetComponent<CharacterController>();
        }
        Debug.Log("controllerFlag: " + controllerFlag);

        walkingSound.volume = 0f;
        turnSensitivity = baseSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 lookInput = look.ReadValue<Vector2>();
        LookAround(lookInput);

        isGrounded = controller.isGrounded;

        if (isGrounded) {
            if (jump.triggered) {
                StartCoroutine(Jump());
            }
        }

        if (controller)
        {
            CharacterControllerMovement();
        }


        if (controller.velocity.magnitude != 0)
        {
            targetVolume = 1f;

            if (!walkingSound.isPlaying)
            {
                walkingSound.Play();
            }
        }
        else
        {
            targetVolume = 0f;
        }

        walkingSound.volume = Mathf.Lerp(walkingSound.volume, targetVolume, Time.deltaTime * 5f);
    }

    private void FixedUpdate() {
        if (controllerFlag == 0) {
            Vector2 moveInput = move.ReadValue<Vector2>(); // Get movement input
            Vector3 movement = speed * moveInput.y * transform.forward * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
            float turn = moveInput.x * rotateSpeed * Time.fixedDeltaTime; // Turn based on horizontal input
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }

    public float fallMultiplier = 5f; // Controls how fast the player falls

    private void CharacterControllerMovement() 
    {
		if(notepadGameObject.activeSelf){
			return;
		}
        Vector2 moveInput = move.ReadValue<Vector2>();
        moveSpeed = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized * speed;

        // Apply gravity
        if (!isGrounded) 
        {
            moveSpeed.y += gravity * Time.deltaTime * fallMultiplier; // Adjust fall speed
        }

        // Check if grounded and reset vertical movement
        if (isGrounded) 
        {
            moveSpeed.y = 0;
            if (jump.triggered) 
            {
                moveSpeed.y = jumpForce;
            }
        }

        controller.Move(moveSpeed * Time.deltaTime);     
    }

    public void SetSensitivity(float sensitivityMultiplier)
    {
        turnSensitivity = baseSensitivity * sensitivityMultiplier;
    }

    private void LookAround(Vector2 lookInput)
    {
		if(notepadGameObject.activeSelf){
			return;
		}
        // Get the mouse delta input for looking
        float mouseX = lookInput.x * turnSensitivity;
        float mouseY = lookInput.y * turnSensitivity;

        // Update the rotation for looking up/down (pitch)
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -80f, 80f); // Limit vertical rotation to avoid flipping

        // Apply the rotation to the camera
        transform.localRotation = Quaternion.Euler(0, transform.localEulerAngles.y + mouseX, 0);
		playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

    }

    private IEnumerator Jump()
    {
        float jumpDuration = 0.3f; // Duration to apply jump force
        float jumpTime = 0f;

        while (jumpTime < jumpDuration)
        {
            jumpTime += Time.deltaTime;
            moveSpeed.y = jumpForce * (1 - (jumpTime / jumpDuration)); // Gradually decrease upward force
            controller.Move(moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Ensure the player falls after the jump
        moveSpeed.y = 0; // Reset vertical speed
    }
}
