using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    [Header("Knockback info")]
    [SerializeField] private Vector2 knockbackDir;
    private bool isKnocked;
    private bool canbeKnocked = true;

    private bool isDead;
    [HideInInspector] public bool extraLife;




    [Header("Speed info")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedMultiplier;
    private float defaultSpeed;
    [Space]
    [SerializeField] private float milestoneIncreaser;
    private float deaultMilestoneIncrease;
    private float speedMilestone;


    [Header("Move info")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;
    private bool canDoubleJump;
    [SerializeField] private float doubleJumpForce;
    

    [HideInInspector] public bool playerUnlocked;

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
    [HideInInspector] public bool ledgeDetected;

    [Header("Ledge info")]
    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;

    private Vector2 climbBeginPosition;
    private Vector2 climbOverPosition;
    private bool canGrabLedge = true;
    private bool canClimb;



    [SerializeField] private bool wallDetected;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        speedMilestone = milestoneIncreaser;
        defaultSpeed = moveSpeed;
        deaultMilestoneIncrease = milestoneIncreaser;
        
    }

    void Update()
    {
        CheckCollision();
        AnimatorController();

        slideTimeCounter -= Time.deltaTime;
        slideCooldownCounter -= Time.deltaTime;

        extraLife = moveSpeed >= maxSpeed;

        if (isDead) 
            return;

        if (isKnocked)
        {
            SpeedReset();
            return;
        }

        if (playerUnlocked)
            Move();
        
        if (isGrounded)
            canDoubleJump = true;

        SpeedController();

        CheckForLedge();
        CheckForSlide();

        CheckInput();
    }

    public void Damage()
    {
        if (extraLife)
            Knockback();
        else
            StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        isDead = true;
        rb.velocity = knockbackDir;
        anim.SetBool("isDead", true);

        yield return new WaitForSeconds(1f);
        rb.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(1f);
        GameManager.Instance.SaveInfo();
        GameManager.Instance.RestartLevel();
    }

    

    private IEnumerator Invcibility()
    {
        Color originalColor = sr.color;
        Color darkenColor = new Color(sr.color.r, sr.color.g, sr.color.b, .5f);

        canbeKnocked = false;
        
        sr.color = darkenColor;
        yield return new WaitForSeconds(.1f);
        
        sr.color = originalColor;
        yield return new WaitForSeconds(.1f);

        sr.color = darkenColor;
        yield return new WaitForSeconds(.1f);

        sr.color = originalColor;
        yield return new WaitForSeconds(.1f);
        
        sr.color = darkenColor;
        yield return new WaitForSeconds(.1f);

        sr.color = originalColor;
        yield return new WaitForSeconds(.1f);

        sr.color = darkenColor;
        yield return new WaitForSeconds(.1f);

        sr.color = originalColor;
        yield return new WaitForSeconds(.1f);
        
        sr.color = darkenColor;
        yield return new WaitForSeconds(.1f);

        sr.color = originalColor;
        canbeKnocked = true;
       

    }

    private void Knockback()
    {
        if(!canbeKnocked) return;
        StartCoroutine(Invcibility());
        isKnocked = true;
        rb.velocity = knockbackDir;
    }

    private void cancelKnockback()
    {
        isKnocked = false;
    }


    private void SpeedReset()
    {
        moveSpeed = defaultSpeed;
        milestoneIncreaser = deaultMilestoneIncrease;
    }

    private void SpeedController()
    {
        if (moveSpeed == maxSpeed)
            return;
        
        if(transform.position.x > speedMilestone) 
        {
            speedMilestone = speedMilestone + milestoneIncreaser;

            moveSpeed = moveSpeed * speedMilestone;
            milestoneIncreaser = milestoneIncreaser * speedMultiplier;

            if(moveSpeed > maxSpeed)
                moveSpeed = maxSpeed;
            
        }
    }
    

    private void CheckForLedge()
    {
        if (ledgeDetected && canGrabLedge)
        { 
            canGrabLedge = false;
            rb.gravityScale = 0;

            Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;

            climbBeginPosition = ledgePosition + offset1;
            climbOverPosition = ledgePosition + offset2;

            canClimb = true;
        } 

        if(canClimb)
            transform.position = climbBeginPosition;
    }

    private void LedgeClimbOver()
    {
        rb.gravityScale = 6;
        canClimb = false;
        transform.position = climbOverPosition;
        Invoke("AllowLedgeGrab", .1f);
    }

    private void AllowLedgeGrab() => canGrabLedge = true;

    private void CheckForSlide()
    {
        if(slideTimeCounter < 0 && !cellingDetected)
            isSliding = false;
        
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
        {
            SpeedReset();
            return;
        }
        if(isSliding && isGrounded)
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
        
        //if(Input.GetButtonDown("Fire2"))
         //   playerUnlocked = true;

        if (Input.GetButtonDown("Jump"))
            JumpButton();

        if (Input.GetKeyDown(KeyCode.LeftShift))
            SlideButton();
    }

    private void AnimatorController()
    {
        anim.SetFloat("xInput", rb.velocity.x);
        anim.SetFloat("yInput", rb.velocity.y);

        anim.SetBool("isKnocked", isKnocked);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isSliding", isSliding);
        anim.SetBool("isClimb", canClimb);

        if (rb.velocity.y < -20)
            anim.SetBool("canRoll", true);
    }

    private void RollAnimFinished() => anim.SetBool("canRoll", false);

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
