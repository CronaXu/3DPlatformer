using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // References to charactercontroller and camera position;
    public CharacterController cc;
    public Transform camTrans;

    // Basic stats
    public float speed = 15f;
    public float jump = 50f;
    public float turnSmoothTime = 0.15f;
    private float turnSmoothVelocity;

    // Simulate gravity
    private Vector3 velocity;
    private bool isGrounded;
    private float gravity = -50f;

    void Update()
    {
        // WASD movement using old input system
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Calculate direction according to input value
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= Mathf.Epsilon)
        {
            // Calculate move angle using input direction and camera direction and then smoothing
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camTrans.eulerAngles.y; ;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Move forward in the direction the player is currently facing
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            cc.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // Press space to jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jump * -0.2f * gravity);
            isGrounded = false;
        }

        // Keep track of gravity
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }

    // Check if player is grounded
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log(isGrounded);
        if (hit.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
}
