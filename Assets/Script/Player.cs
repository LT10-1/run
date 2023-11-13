using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour

{
    // Move Info
    [SerializeField] private float moveSpeed = 1f;
    private float jumpForce = 15f;
    private Rigidbody2D rb;
    private bool canDoubleJump;
    [SerializeField] private float doubleJumpForce = 8f;
    [SerializeField] private float defaultJumpForce;


    // Check Ground
    [SerializeField] private bool isGrounded; // Check Ground
    [SerializeField] private float distance;  // Do dai
    [SerializeField] private float cellingDetectedDistance;
    [SerializeField] private LayerMask groundlayer; // Layer
    [SerializeField] private Vector2 wallCheckSize;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private bool wallDectected;

    // Anim
    public Animator anim;
    // Sliding

    public bool isSliding;
    public float slidingDuration;
    public float slidingSpeed;
    public float slidingCounter;
    public float slideCooldown;
    public float slideCooldownCounter;
    private bool cellingDetected;

    public bool ledgeDetected;
    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;
    private Vector2 climbBeginPosition;
    private Vector2 climbEndPosition;

    private bool canGrabLedge = true;
    private bool canClimb;




    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        defaultJumpForce = jumpForce;
    }


    private void Update()
    {
        slidingCounter -= Time.deltaTime;
        slideCooldownCounter -= Time.deltaTime;

        CheckCollision();
        CheckInput();
        CheckForSlide();
        CheckForLedge();
        
        //Anim State

        anim.SetFloat("xInput", rb.velocity.x);          // Running State
        anim.SetBool("isGrounded", isGrounded);          // Jump Check Ground
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isSliding", isSliding);
        anim.SetBool("canClimb", canClimb);

        anim.SetFloat("yInput", rb.velocity.y);           // Check velocity anim State machine (Jump)
    }

   

    private void CheckForLedge()
    {
        if(ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;
            Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;
            climbBeginPosition = ledgePosition + offset1;
            climbEndPosition = ledgePosition + offset2;
            canClimb = true;
        }
        if(canClimb) 
            transform.position = climbBeginPosition; 
        
    }

    private void LedgeClimbOver()
    {
        canClimb = false;
        transform.position = climbEndPosition;
        Invoke("AllowLedgeGrab", 1.4f);
    }

    private void AllowLedgeGrab() => canGrabLedge = true;
        


    private void CheckCollision()
    {
        //Checking Ground
        isGrounded = Physics2D.Raycast(
            transform.position, // Diem Ve
            Vector2.down,       // Huong ve
            distance,           // Do dai
            groundlayer);       // Layer 
        // Checking Wall

        wallDectected = Physics2D.BoxCast(
            wallCheck.position,
            wallCheckSize,
            0,
            Vector2.zero,
            groundlayer);
        // Checking Celling
        cellingDetected = Physics2D.Raycast(
            transform.position,
            Vector2.up,
            cellingDetectedDistance,
            groundlayer);
        Debug.Log(ledgeDetected);
    }

    private void CheckForSlide()
    {
        if(slidingCounter < 0 && !cellingDetected)
            isSliding = false;
    }
          

    private void CheckInput()
    {
        if (!wallDectected)                                   // Neu khong phai wall thi
            Movement(); // Auto MoveRight * Speed

        if (isGrounded) canDoubleJump = true;

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
            SlidingButton();
    }

    private void SlidingButton()
    {
        if (rb.velocity.x != 0 && slideCooldownCounter < 0)
        {
            isSliding = true;
            slidingCounter = slidingDuration;
            slideCooldownCounter = slideCooldown;
        }
    }

    private void Movement()
    {
        if(isSliding )
            rb.velocity = new Vector2 (slidingSpeed,rb.velocity.y);
        else
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        
            
        
    }

    private void Jump()
    {
        if (isSliding)
            return;

        if (isGrounded) // Neu bam nut Jump + ground check ok
        {
            jumpForce = defaultJumpForce;
            
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if (canDoubleJump)
        {
            jumpForce = doubleJumpForce;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            canDoubleJump = false;

        }
    }

    private void OnDrawGizmos()
    {
        // Draw a line from character to layerdistance
        Gizmos.DrawLine(transform.position, new Vector2 (transform.position.x, transform.position.y - distance));
        // Draw a cube next to character to check wall
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
        // Draw a line from character up to check layerground
        Gizmos.DrawLine (transform.position, new Vector2 (transform.position.x, transform.position.y + cellingDetectedDistance));
    } 
}
