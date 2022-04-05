using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // References to charactercontroller and camera/player position;
    public CharacterController cc;
    public Transform camTrans;

    // Basic stats
    public float speed = 12f;
    public float jump = 60f;
    private float turnSmoothTime = 0.15f;
    private float turnSmoothVelocity;

    // Simulate gravity
    private Vector3 velocity;
    private bool isGrounded;
    private float gravity = -60f;
    private bool rayDown;

    // Lerp stats
    private float lerpDuration = 0.5f;
    private float timeElapsed = 0f;
    private float currentY;

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

        // Transport back if fall down
        if (transform.position.y < 230f)
        {
            cc.enabled = false;
            transform.position = new Vector3(0f, 369.321f, 0f);
            cc.enabled = true;
            velocity = Vector3.zero;
            horizontal = 0f;
            vertical = 0f;
        }

        // Keep track of gravity and check grounded
        rayDown = Physics.Raycast(transform.position, Vector3.down, 1.1f);
        if (!rayDown)
        {
            isGrounded = false;
        }
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
            currentY = velocity.y;
        }

        // Lerp current y velocity to 0 if Grounded so that player won't be laggy on leaning boards
        if (isGrounded)
        {
            if (timeElapsed < lerpDuration)
            {
                velocity.y = Mathf.Lerp(currentY, 0f, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
                timeElapsed = 0f;
            }
        }
        
        cc.Move(velocity * Time.deltaTime);

        
    }

    // Check if player is grounded
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
        if (hit.gameObject.tag == "Ground")
        {
            if (rayDown)
            {
                isGrounded = true;
            }
        }

        
    }
}
