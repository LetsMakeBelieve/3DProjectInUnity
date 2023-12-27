using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;         //speed
    public float jumpForce;         //jump force
    public float jumpCooldown;      //jump cooldown
    public float airMultiplier;     //air multiplier
    bool readyToJump = true;        //ready to jump

    [Header("Slope Handling")]
    public float groundDrag;        //drag

    [Header("Ground Check")]
    public float playerHeight;      //height of player
    public LayerMask whatIsGround;  //what is ground
    public bool grounded;           //is player grounded

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Slope Handling")]
    public float maxSlopeAngle;     //max slope angle
    private RaycastHit slopeHit;    //raycast hit
    private bool exitingSlope;      //exiting slope

    [Header("Effects")]
    public Transform playerCam;     //camera

    public Transform orientation;   //orientation of player
    private float horizontalInput;          //input for x axis
    private float verticalInput;            //input for y axis
    private Vector3 moveDirection;          //vector of direction
    private Rigidbody rb;                   //reference to rigidbody

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void FixedUpdate(){
        MovePlayer();
    }

    private void Update(){
        MyInput();
        speedControl();

        //ground check  
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if(grounded){
            rb.drag = groundDrag;
        }else{
            rb.drag = 0;
        }
    }

    private void MyInput(){
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && grounded){
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    
    private void MovePlayer(){
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //on ground
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        //in air
        else if(!grounded){
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void speedControl(){
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity if needed
        if(flatVel.magnitude > moveSpeed){
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump(){
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump(){
        readyToJump = true;
    }
}
