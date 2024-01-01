using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Movement Physics")]
    [SerializeField] float jumpForce = 4f;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] Rigidbody2D rb;

    [Header("Collision Info")]
    [SerializeField] float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Dash Info")]
    [SerializeField] float dashDuration;
    [SerializeField] float dashTime;
    [SerializeField] float dashSpeed;
    bool isDashing;

    private bool isGrounded;
    
    Animator myAnimator;

    private float xInput;

    private int facingDir = 1;
    private bool facingRight = true;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Movement();

        CheckInputs();

        CollisionChecks();

        dashTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && xInput != 0)
        {
            dashTime = dashDuration;
            isDashing = true;
        }

        FlipController();

        AnimationControllers();
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down,
            groundCheckDistance, whatIsGround);
    }

    void CheckInputs()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

    }
    
    void Movement()
    {

        if (dashTime > 0 && xInput != 0)
            rb.velocity = new Vector2(xInput * dashSpeed, 0);
        else
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
            isDashing = false;
        }
        //rb.velocity.y means "just go with whatever
        //the rigid body says for the y axis!")
    }

    private void Jump()
    {
        if (isGrounded)
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void AnimationControllers()
    {
        bool isMoving = rb.velocity.x != 0;

        myAnimator.SetBool("isMoving", isMoving);

        myAnimator.SetBool("isGrounded", isGrounded);

        myAnimator.SetFloat("yVelocity", rb.velocity.y);

        myAnimator.SetBool("isDashing", isDashing);

    }


    void FlipPlayerTransform()
    {
        facingDir = -facingDir;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }


    void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
            FlipPlayerTransform();

        else if (rb.velocity.x < 0 && facingRight)
            FlipPlayerTransform();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, 
            transform.position.y - groundCheckDistance));
    }
}
