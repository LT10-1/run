using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator anim;



    [Header("Move Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [SerializeField] private bool isRunning;
    private bool runBegin;

    [Header("Collision info")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private float GroundCheckDistance;
    [SerializeField] private LayerMask whatisGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (runBegin)
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        AnimatorController();
        CheckCollision();
        CheckInput();
    }

    private void AnimatorController()
    {
        isRunning = rb.velocity.x != 0f;

        // if (isRunning == true && isGrounded)
        anim.SetBool("isRunning", isRunning);
    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, GroundCheckDistance, whatisGround);
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire2"))
            runBegin = true;

        if (Input.GetButtonDown("Jump") && isGrounded )
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - GroundCheckDistance));
    }
}
