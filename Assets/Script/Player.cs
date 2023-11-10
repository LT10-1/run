using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move Info")]
    public Rigidbody2D rb;
    public float moveSpeed;
    public float jumpForce;
    private bool runBegin;

    [Header("Collision info")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private float GroundCheckDistance;
    [SerializeField] private LayerMask whatisGround;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (runBegin)
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        CheckCollision();
        CheckInput();
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
