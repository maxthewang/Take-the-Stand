using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f; 
    public float rotateSpeed = 120.0f; 
    public float jumpForce = 5.0f; 
    private Rigidbody rb;

    private CharacterController controller;
    private Vector3 move_speed;
    private int controllerFlag = 0; // 0 for rigidbody, 1 for character controller
    public float gravity = -5.8f;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        if (controllerFlag == 0) {
            if (Input.GetButtonDown("Jump")) {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
        } else {
            CharacterControllerMovement();
        }

    }

    private void FixedUpdate() {
        if (controllerFlag == 0) {
            float moveVertical = Input.GetAxis("Vertical"); 
            Vector3 movement = speed * moveVertical * transform.forward * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
            float turn = Input.GetAxis("Horizontal") * rotateSpeed * Time.fixedDeltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }
    }

    private void CharacterControllerMovement() {
        // Controls for character controller
        move_speed = transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
        move_speed += transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        move_speed.y += gravity;
        move_speed.Normalize();
        move_speed *= speed;
        if (controller.isGrounded) {
            move_speed.y = 0;
            if (Input.GetButtonDown("Jump")) {
                move_speed.y = jumpForce;
            }
        }
        controller.Move(move_speed);     
    }
}
