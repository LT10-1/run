using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour

{
    // Move Info
    [SerializeField] private float moveSpeed = 1f;
    private float jumpForce = 8f;
    private Rigidbody2D rb;

    //Check Ground
    [SerializeField] public bool isGrounded; // Check Ground
    [SerializeField] private float distance;  // Do dai
    [SerializeField] public LayerMask groundlayer; // Layer

    //Anim
    public Animator anim;
    




    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }


    private void Update()
    {
        //Checking Ground
        isGrounded = Physics2D.Raycast(
            transform.position, // Diem Ve
            Vector2.down,       // Huong ve
            distance,           // Do dai
            groundlayer);       // Layer 

        rb.velocity = new Vector2 (moveSpeed, rb.velocity.y); // Auto MoveRight * Speed

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded ) // Neu bam nut Jump + ground check ok
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        //Anim State
        
        anim.SetFloat("xInput", rb.velocity.x);       // Running State
        anim.SetBool("isGrounded", isGrounded);     // Jump Check Ground
        
        
        if (rb.velocity.x != 0f)                     // Check velocity anim State machine (Jump)
            anim.SetFloat("yInput", rb.velocity.y);    
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2 (transform.position.x, transform.position.y - distance));
    } // Draw a line from character to layerdistance
}
