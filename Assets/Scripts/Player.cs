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
        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
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
