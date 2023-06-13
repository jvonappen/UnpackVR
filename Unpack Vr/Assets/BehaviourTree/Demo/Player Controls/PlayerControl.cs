using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    // Used by new input system.
    private InputAction move;
    public PlayerInput playerInput;



        [Header("Speed Settings :")]
    //These variables control settings related playe speed and acceleration.
    [Tooltip("The force you want the player to be pushed with.")] 
    public float movementSpeed = 1.0f;
    [Tooltip("Acceleration is the rate of change of the velocity of an object with respect to time.")]
    public float acceleration = 1.0f;
    public float currentAcceleration;
    [Tooltip("Acceleration is the rate of change of the velocity of an object with respect to time.")]
    public float deceleration = 1.0f;
    [Tooltip("The max speed you want the player to go at.")]
    public float maxSpeed = 5.0f;
    public float maxSpeedAir = 10.0f;


        [Header("Jump Settings :")]
    //These are your jumping variables.
    [Tooltip("The Force the player will be pushed in the up direction. ")]
    public float jumpForce = 1.0f;
    [Tooltip("The Force the player will be pushed in the downwards direction while not grounded. ")]
    public float fallForce = 1.0f;
    [Tooltip("The amount of jumps the player has. ")]
    public int amountJumps = 2;
    [Tooltip("The speed in which the player moves while in the air. ")]
    public float airControl = 1.0f;

        [Header("Grabbing Settings :")]
    //Shown for debugging.
    public bool holding = false;
    public GameObject holdingObj = null;
    public GameObject holdingZone;

        [Header("Rigidbody Settings :")]
    public Rigidbody rb;

        [Header("Other Settings :")]
    public GameObject[] fists;
    int backForth;
    public Camera cam;

        [Header("Debug :")]
    public bool grounded = false;
    public bool moving = false;
    public int jumpCount = 0;
    public float timer = 0;
    public bool turned = false;
    public float velocity;

   // New input system has need to bind functions to player input actions, we do this in "OnEnable" OR "Start", I prefer "OnEnable" so
   // in the situation you have multiple Input Maps, you can disable them through "OnDisable". Another option is disabling this script.
    private void OnEnable()
    {
        cam = Camera.main; // Main Camera is assumed to be the players POV camera.

        move = playerInput.actions["Move"]; // Reads the players directional Input.
        move.Enable();

        playerInput.actions["Jump"].performed += DoJump; // Binds DoJump to the action of "Jump"
        playerInput.actions["Jump"].Enable();

        playerInput.actions["Grab"].performed += DoGrab; // Binds DoGrab to the action of "Grab"
        playerInput.actions["Grab"].Enable();

        playerInput.actions["Interact"].performed += DoInteract; // Binds DoInteract to the action of "Interact"
        playerInput.actions["Interact"].Enable();


        playerInput.actions["Punch"].performed += DoPunch; // Binds DoPunch to the action of "Punch"
        playerInput.actions["Punch"].Enable();

        rb = GetComponent<Rigidbody>();

        velocity = rb.velocity.magnitude;

        backForth = 0; // NOT MOVEMENT RELATED (Related to the action of punching)

    }



    // This is unrelated to movement player movement, allows the player to interact with items in the world.
    // If the item is marked as interactable then it is assumed they have an "InteractbleScript" attached to
    // it. The idea being that "InteractbleScript" is used for polymorphism and each object has an interaction
    // unique to it.
    private void DoInteract(InputAction.CallbackContext obj)
    {

        //RaycastHit ObjectHit;
        //Physics.Raycast(cam.transform.position, cam.transform.forward, out ObjectHit);

        //if (ObjectHit.collider.gameObject.tag == "Interactable" && ObjectHit.distance < 2f)
        //{
        //    ObjectHit.collider.gameObject.GetComponent<InteractbleScript>().DoYaThing();
        //}


    }

    private void DoPunch(InputAction.CallbackContext obj) 
    { 
    }
    // This is unrelated to movement player movement, allows the player to punch with their physics based fists!

    // This is unrelated to movement player movement, allows the player to pick up grabbable objects in the world.
    // Future update is to merge this into the interactable system but I'm still not sure its the right call to make.
    private void DoGrab(InputAction.CallbackContext obj)
    {
        //GameObject object1;
        //RaycastHit ObjectHit;
        //Physics.Raycast(cam.transform.position,cam.transform.forward, out ObjectHit);

        //if(ObjectHit.distance < 2 && !holding)
        //{
        //    if (ObjectHit.rigidbody != null)
        //    {
        //        object1 = ObjectHit.transform.gameObject;

        //        if (object1.tag == "Grabable")
        //        {
        //            holdingObj = object1;
        //            holding = true;


        //            holdingZone.GetComponent<FixedJoint>().connectedBody = holdingObj.GetComponent<Rigidbody>();

        //            if (holdingObj.GetComponent<MeepMoopScript>())
        //            {
        //                holdingObj.GetComponent<MeepMoopScript>().Grabbed();
        //            }
        //        }
        //    }

        //}
        //else if (holding)
        //{
        //    holdingZone.GetComponent<FixedJoint>().connectedBody = null;
        //    holdingObj.GetComponent<Rigidbody>().velocity = cam.transform.forward * 10;

        //    if (holdingObj.GetComponent<MeepMoopScript>())
        //    {
        //        holdingObj.GetComponent<MeepMoopScript>().Grabbed();
        //    }


        //    holding = false;
        //    holdingObj = null;

        //}

    }
    private void DoJump(InputAction.CallbackContext obj)
    {

            // Shoots a raycast downwards from the player.
            RaycastHit centerhit;
            Physics.Raycast(transform.position, -Vector3.up, out centerhit); 

            // ^ This should be replaced to better reflect a capsule collider, as it will only check whats directly below the player from its center.
            // This still works but wouldnt hurt to be more accurate.

            Vector3 velocity = rb.velocity;

            // First Jump or Only Jump:
            if (centerhit.distance <= 1.45f && jumpCount < amountJumps) // The reason for the 1.45f is so that the player doesnt have to directly touch the ground to jump.
                                                                        // Assumes players scale is Vector3(1,1,1).
            {
                velocity.y += jumpForce;
                jumpCount++;
                rb.velocity += velocity;

            }
            // Multiple Jumps:
            else if (centerhit.distance > 1.45f && jumpCount < amountJumps - 1)
            {
                velocity.y += jumpForce / 2; // Reasons for the "/ 2", I wanted any jump after the first to be half the height of the first for gameplay.
                jumpCount++;
                rb.velocity += velocity;
            }
    }

    //This should be filled with each player input action, I'm just lazy >:)
    private void OnDisable()
    {
        playerInput.actions["Jump"].performed -= DoJump;
    }


    private void FixedUpdate()
    {

        checkXVelocity(); // clamp our velocity, before we do anything else.

        Vector2 moveDir = move.ReadValue<Vector2>(); // Read our directional inputs.

        // Grounded movement:
        if (moveDir != new Vector2(0, 0) && grounded) 
        {
            Vector3 force = ((transform.forward * moveDir.y) + (transform.right * moveDir.x)).normalized * movementSpeed * currentAcceleration * Time.deltaTime; // Uses movement speed while grounded.
            rb.AddForce(force, ForceMode.Impulse);
        }
        // Air movement:
        else if (moveDir != new Vector2(0, 0) &&  !grounded)
        {
            Vector3 force = ((transform.forward * moveDir.y) + (transform.right * moveDir.x)).normalized * airControl * currentAcceleration * Time.deltaTime; // Uses air control while in the air. 
            rb.AddForce(force, ForceMode.Impulse);
        }

        // Makes the player fall down & stops floatyness (custom gravity for the player)
        if (!grounded) 
        {
            rb.AddForce(Vector2.down * fallForce, ForceMode.Impulse);
        }

        // Want to make jumps more floaty for some reason? Try something like this? 

        //////if (!grounded)
        //////{
        //////    rb.AddForce(Vector2.up * floatyForce, ForceMode.Impulse); 
                // you would need to make a new variable for floaty force. Rigid body gravity will still push you downwards so you will need to
                // find a middle point where it doesnt exceed gravity pushing your downwards. Otherwise the player upon jumping will float to the clouds.
        //////}


    }

    void Update()
    {
        velocity = rb.velocity.magnitude;

        RaycastHit groundCheck;


        Physics.SphereCast(new Vector3 (transform.position.x, transform.position.y + 1, transform.position.z), transform.localScale.x, -Vector3.up, out groundCheck);

        if (groundCheck.distance <= 1.15f)
        {
            grounded = true;
            jumpCount = 0;
        }
        else
        {
            grounded = false;
        }


        if (move.ReadValue<Vector2>().x != 0)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }


    }

    void checkXVelocity()
    {

        Vector3 Checking = rb.velocity;
        Checking.y = 0;

        currentAcceleration = acceleration;

        if (Checking.magnitude > maxSpeed && grounded)
        {
            Checking = Checking.normalized * maxSpeed;
        }
        else if (rb.velocity.magnitude > maxSpeed && !grounded)
        {
            Checking = Checking.normalized * maxSpeedAir;
        }

        Checking.y = rb.velocity.y;
        rb.velocity = Checking;

    }

   


}