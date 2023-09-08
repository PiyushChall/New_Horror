using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    //Variable to control the speed
    private float movespeed;

    public float walkSpeed;
    public float sprintSpeed;
    public float crouchSpeed;

    //Varible to control the slippery ness of the player;
    public float groundDrag;

    //Reference to the orientation
    public Transform orientation;

    //used for the raycast
    public float playerHeight;

    //A LayerMask Variable to determine the ground
    public LayerMask whatIsGround;

    //A Bool Value to apply conditions
    bool grounded;

    //Player Inputs
    float horizontalInput;
    float verticalInput;

    //For the Direction
    Vector3 movementDirection;

    //Reference to the Rigidbody of the Player
    Rigidbody rb;

    //determines the force of the player jumps
    public float jumpForce;

    //prevents the player from continiously jumping and escaping the reality
    public float jumpCooldown;

    //Increases the speed when in air
    public float airMultiplier;

    //bool to check if the player is ready to jump
    bool readyToJump = true;

    //set the space bar as the jump key
    public KeyCode JumpKey = KeyCode.Space;

    //set the left-Shift as the sprint key
    public KeyCode SprintKey = KeyCode.LeftShift;

    //set the left-control as the sprint key
    public KeyCode CrouchKey = KeyCode.LeftControl;

    //Making movement state for the player
    public moveState state;
    public enum moveState
    {
        walking,
        sprinting,
        crouching,
        air
    }
    public float crouchYScale;
    private float startYscale;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYscale = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        //This shoots a downwards raycast of half the size of the player plus some more.
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInputs();

        speedControl();

        stateHandeler();

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }
    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void MyInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //jump
        if (Input.GetKey(JumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
        //Crouch
        if (Input.GetKey(CrouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        }
        //Stop Crouch
        if (Input.GetKeyUp(CrouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYscale, transform.localScale.z);
        }
    }

    private void stateHandeler()
    {
        //Sprinting
        if (Input.GetKey(SprintKey))
        {
            state = moveState.sprinting;
            movespeed = sprintSpeed;
        }
        //Crouching
        if (Input.GetKey(CrouchKey))
        {
            state = moveState.crouching;
            movespeed = crouchSpeed;
        }
        //Walking
        else if (grounded)
        {
            state = moveState.walking;
            movespeed = walkSpeed;
        }
        //air
        else
        {
            state = moveState.air;
        }
    }

    private void PlayerMove()
    {
        //This Line calculates the movement-direction of the player.
        //This way the player will always walk in the direction where the camera is looking.
        movementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //This line will simply add force to the player.
        if (grounded)
        {
            rb.AddForce(movementDirection.normalized * movespeed * 10, ForceMode.Force);
        }

        //in air
        else if (!grounded)
        {
            rb.AddForce(movementDirection.normalized * movespeed * 10 * airMultiplier, ForceMode.Force);
        }
    }
    private void speedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVelocity.magnitude > movespeed)
        {
            Vector3 limitVelocity = flatVelocity.normalized * movespeed;
            rb.velocity = new Vector3(limitVelocity.x, rb.velocity.y, limitVelocity.z);
        }
    }
    private void Jump()
    {
        //reset the y_velocity
        //this way the player will always jump the exact same height
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}
