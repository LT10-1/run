using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Move info")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;
    private bool canDoubleJump;
    [SerializeField] private float doubleJumpForce;
    

    private bool playerUnlocked;

    [Header("Slide info")]
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideTime;
    [SerializeField] private float slideTimeCounter;
    [SerializeField] private bool isSliding;
    [SerializeField] private float slideCooldown;
    [SerializeField] private float slideCooldownCounter;
    


    [Header("Collision info")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;
    [SerializeField] private float cellingCheckDistance;
    [SerializeField] private bool cellingDetected;



    [SerializeField] private bool wallDetected;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        
    }

    void Update()
    {
        slideTimeCounter -= Time.deltaTime;
        slideCooldownCounter -= Time.deltaTime;

        if (playerUnlocked)
            Move();
        

        CheckInput();
        CheckCollision();
        AnimatorController();
        CheckForSlide();
        

        if(isGrounded)
            canDoubleJump = true;
    }

    private void CheckForSlide()
    {
        if(slideTimeCounter < 0 && !cellingDetected)
        {
            
            isSliding = false;
        }
       
    }




    private void SlideButton()
    {
        if(rb.velocity.x != 0 && slideCooldownCounter < 0)
        {
            isSliding = true;
            slideTimeCounter = slideTime;
            slideCooldownCounter = slideCooldown;
        }
    }



    private void Move()
    {
        if(wallDetected)
            return;
        if(isSliding)
            rb.velocity = new Vector2(slideSpeed, rb.velocity.y);
        else
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        
        
    }
        
        
        

        


    private void JumpButton()
    {
        if (isSliding)
            return;
        
        if(isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
           
        }
        else if (canDoubleJump)
        {
       
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
            canDoubleJump = false;
        }
    }

    void CheckInput()
    {
        
        if(Input.GetButtonDown("Fire2"))
            playerUnlocked = true;

        if (Input.GetButtonDown("Jump"))
            JumpButton();

        if (Input.GetKeyDown(KeyCode.LeftShift))
            SlideButton();
    }

    private void AnimatorController()
    {
        anim.SetFloat("xInput", rb.velocity.x);
        anim.SetFloat("yInput", rb.velocity.y);
        
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isSliding", isSliding);
    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        cellingDetected = Physics2D.Raycast(transform.position, Vector2.up, cellingCheckDistance, whatIsGround);
        wallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2 (transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + cellingCheckDistance));
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }

}
