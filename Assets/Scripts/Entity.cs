using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator myAnimator;

    [Header("Collision Info")]
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [Space]
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    protected bool isGrounded;
    protected bool isWallDetected;


    protected int facingDir = 1;
    protected bool facingRight = true;


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponentInChildren<Animator>(); 


        if (wallCheck == null) //saves us from our stupidity if we forget to assign this as a serialize field transform
        {
            wallCheck = transform;
        }
    }

    protected virtual void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down,
            groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(wallCheck.position, new Vector2 (facingDir, 0), wallCheckDistance, whatIsGround);

    }

    protected virtual void Update()
    {
        CollisionChecks();
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x,
            groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + (facingDir* wallCheckDistance), wallCheck.position.y));
    }

    

    protected virtual void FlipTransform()
    {
        facingDir = -facingDir;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }




    
}
