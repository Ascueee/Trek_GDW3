using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Vars")]
    [SerializeField] float moveSpeed;
    [SerializeField] float groundDrag;
    [SerializeField] float groundMultiplier;
    [SerializeField] Transform orientation;
    float horizontalInput;
    float verticalInput;
    float resetMoveSpeed;
    Vector3 moveDirection;
    Rigidbody rb;

    [Header("Jump Vars")]
    [SerializeField] float jumpForce;
    [SerializeField] float airMultiplier;
    [SerializeField] KeyCode jumpKey;
    bool readyToJump;
    bool exitingSlope;

    [Header("Crouch Variables")]
    [SerializeField] float crouchSpeed;
    [SerializeField] float crouchYScale;
    [SerializeField] float startYScale;
    KeyCode crouchKey = KeyCode.LeftControl;
    bool canCrouch;

    [Header("Slipe Handling")]
    [SerializeField] float maxSlopeAngle;
    RaycastHit slopeHit;

    [Header("Wall Running Controller")]
    [SerializeField] float wallRunSpeed;
    public bool isWallRunning;


    [Header("Ground Collision")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask ground;
    bool grounded;

    public bool freeze;
    public bool activeGrapple;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startYScale = transform.localScale.y;
        resetMoveSpeed = moveSpeed;
    }
    private void Update()
    {
        MyInput();
        GroundCheck();
        OnWall();
        DragControl();
        SpeedControl();


    }

    private void FixedUpdate()
    {
        Movement();
        Jump();
        Crouch();
    }

    //------------GENERAL MOVEMENT METHODS START----------------

    /// <summary>
    /// This method just gets the players input
    /// </summary>
    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //KeyBinds for jump
        if(Input.GetKey(jumpKey) && grounded)
        {
            readyToJump = true;
        }

        if (Input.GetKey(crouchKey))
        {
            canCrouch = true;
        }
        if (Input.GetKeyUp(crouchKey))
        {
            canCrouch = false;
        }

        if (freeze)
        {
            //rb.velocity = Vector3.zero;
            
        }
    }

    void Movement()
    {
        if (activeGrapple)
            return;


        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //checks if the player is on a slope
        if (OnSlope() && exitingSlope == false)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * groundMultiplier * 2f, ForceMode.Force);

            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * groundMultiplier, ForceMode.Force);  
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * groundMultiplier * airMultiplier, ForceMode.Force);

    }

    void Jump()
    {
        if(readyToJump)
        {
            exitingSlope = true;
            readyToJump = false;
            //reset the y velocity
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);  
        }
        else if (grounded)
        {
            exitingSlope = false;
        }

    }


    void DragControl()
    {
        if (grounded && !activeGrapple)
            rb.drag = groundDrag;
        else
            rb.drag = 0.3f;
    }



    /// <summary>
    /// Checks if the players velocity is above the player speed and if so then limit the maximum felocity;
    /// </summary>
    void SpeedControl()
    {
        if (activeGrapple)
            return;
        //limiting speed on slopes
        if (OnSlope() && exitingSlope == false)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        //limiting speed on ground and in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            //limit velocity if larger then speed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

    }

    void Crouch()
    {
        if (canCrouch)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 0.5f, ForceMode.Impulse);
            moveSpeed = crouchSpeed;
        }
        else if(canCrouch == false)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            moveSpeed = resetMoveSpeed;
        }
    }

    private bool enableMovementOnNextTouch;
    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;

        velocityToSet = CalculateGrappleMovement(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);
    }

    private Vector3 velocityToSet;
    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;
    }
    public Vector3 CalculateGrappleMovement(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ  / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    //-----------GENERAL MOVEMENT METHODS ENDS-----------------

    //------------COLLISION METHODS START----------------

    /// <summary>
    /// This method checks if the player is colliding with the ground
    /// </summary>
    void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);
    }

    /// <summary>
    /// This method checks the angle that the player is when they hit a slope
    /// </summary>
    /// <returns></returns>
    bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    void OnWall()
    {
        if (isWallRunning)
        {
            moveSpeed = wallRunSpeed;
        }
        else if (!isWallRunning)
        {
            moveSpeed = resetMoveSpeed;
        }
    }

    public void ResetRestrictions()
    {
        activeGrapple = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();

            GetComponent<Grapple>().StopGrapple();
        }
    }

    Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    //------------COLLISION METHODS ENDS----------------

    //------------GETTER AND SETTER METHODS START----------------

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }
}
