using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Layer Masks")]
    [SerializeField] LayerMask whatIsWall;
    [SerializeField] LayerMask whatIsGround;

    [Header("WallRunning Vars")]
    [SerializeField] float wallRunForce;
    KeyCode jumpKey = KeyCode.Space;
    [SerializeField] float wallJumpUpForce;
    [SerializeField] float wallJumpSideForce;
    [SerializeField] float maxWallRunTime;
    [SerializeField] float wallRunTimer;
    float horizontalInput;
    float verticalInput;

    [Header("Detection")]
    [SerializeField] float wallCheckDistance;
    [SerializeField] float minJumpHeight;
    RaycastHit leftWallHit;
    RaycastHit rightWallHit;
    bool wallLeft;
    bool wallRight;

    [Header("Exit Wall Jump")]
    [SerializeField] float exitWallTime;
    float exitWallTimer;
    bool exitingWall;

    [Header("Wall Jump Gravity")]
    [SerializeField] float gravityCounterForce;

    [Header("Refrence Scripts")]
    [SerializeField] Transform orientation;
    [SerializeField] PlayerCam cam;
    Rigidbody rb;
    PlayerMovement pm;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (pm.isWallRunning)
        {
            WallRunMovement();
        }
    }

    void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    void StateMachine()
    {
        //Getting Inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //Check if the player can WallRun and wallRun State
        if((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        {
            if (!pm.isWallRunning)
                StartWallRun();

            if (wallRunTimer > 0)
                wallRunTimer -= Time.deltaTime;

            if(wallRunTimer <= 0 && pm.isWallRunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }

            if (Input.GetKeyDown(jumpKey))
                WallJump();

        }

        //state two exiting out the wall jump
        else if (exitingWall)
        {
            if (pm.isWallRunning)
                StopWallRun();
            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;

            if (exitWallTimer <= 0f)
                exitingWall = false;
        }
        //not in wall run State and final state
        else
        {
            if(pm.isWallRunning)
                StopWallRun();
        }
    }

    void StartWallRun()
    {
        pm.isWallRunning = true;
        wallRunTimer = maxWallRunTime;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //apply cam effects
        cam.SetFov(100f);
        if (wallLeft) cam.SetTilt(-5f);
        if (wallRight) cam.SetTilt(5f);

    }

    void WallRunMovement()
    {
      
        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        //checks how far the player is facing
        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        //forward force
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);


        //push player to wall
        if(!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100, ForceMode.Force);

        rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
    }

    void StopWallRun()
    {
        pm.isWallRunning = false;
        cam.SetFov(90f);
        cam.SetTilt(0f);
    }

    void WallJump()
    {

        //enter exiting wallJump
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        //Add Force to player
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);

    }


}
