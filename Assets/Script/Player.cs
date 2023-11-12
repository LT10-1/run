using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour

{
    // Move Info
    [SerializeField] private float moveSpeed = 1f;
    private float jumpForce = 12f;
    private Rigidbody2D rb;
    private bool canDoubleJump;
    [SerializeField] private float doubleJumpForce = 8f;
    [SerializeField] private float defaultJumpForce;


    //Check Ground
    [SerializeField] private bool isGrounded; // Check Ground
    [SerializeField] private float distance;  // Do dai
    [SerializeField] private LayerMask groundlayer; // Layer
    [SerializeField] private Vector2 wallCheckSize;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private bool wallDectected;

    //Anim
    public Animator anim;
    




    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        defaultJumpForce = jumpForce;
    }


    private void Update()
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

        if(!wallDectected)                                   // Neu khong phai wall thi
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y); // Auto MoveRight * Speed
        
        if (isGrounded) canDoubleJump = true;

        if (Input.GetKeyDown(KeyCode.Space))  
            Jump();

        //Anim State

        anim.SetFloat("xInput", rb.velocity.x);       // Running State
        anim.SetBool("isGrounded", isGrounded);     // Jump Check Ground
        anim.SetBool("canDoubleJump", canDoubleJump);

        if (rb.velocity.x != 0f)                     // Check velocity anim State machine (Jump)
            anim.SetFloat("yInput", rb.velocity.y);
    }

    private void Jump()
    {
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
    } 
}
