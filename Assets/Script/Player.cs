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
    

    private bool playerUnlocked;
    private bool canDoubleJump;
    [SerializeField] private float doubleJumpForce;
    

    [Header("Collision info")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;

    [SerializeField] private bool wallDetected;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        
    }

    void Update()
    {
        if (playerUnlocked && !wallDetected)
        {
            Move();

        }

        CheckInput();
        CheckCollision();
        AnimatorController();

        if(isGrounded)
            canDoubleJump = true;
    }

    private void AnimatorController()
    {
        anim.SetFloat("xInput", rb.velocity.x);
        anim.SetFloat("yInput", rb.velocity.y);
        
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("canDoubleJump", canDoubleJump);
    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, whatIsGround);
    }

    void CheckInput()
    {
        if(Input.GetButtonDown("Fire2"))
            playerUnlocked = true;

        if (Input.GetButtonDown("Jump"))
        {
            JumpButton();
        }
    }


            

    private void Move()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }


    private void JumpButton()
    {
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




    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2 (transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }

}
