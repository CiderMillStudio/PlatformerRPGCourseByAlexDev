using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Entity
{
    bool isAttacking;
    
    [Header("Move Info")]
    [SerializeField] float moveSpeed;


    [Header("Player Detection")]
    [SerializeField] float playerCheckDistance;
    [SerializeField] LayerMask whatIsPlayer;
    RaycastHit2D isPlayerDetected;
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Movement();

        if (!isGrounded || isWallDetected)
        {
            FlipTransform();
        }

        if (isPlayerDetected)
        {
            if (isPlayerDetected.distance > 1)
            {
                rb.velocity = new Vector2(moveSpeed * 1.5f * facingDir, rb.velocity.y);
                Debug.Log("I'm approaching my victim... (the skeleton is approaching the player)");
                isAttacking = false;
            }
            else
            {
                Debug.Log("ATTACKK!!!");
                isAttacking = true;
            }
        }


    }

    private void Movement()
    {
        if(!isAttacking)
            rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y);
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();

        isPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right, (facingDir * playerCheckDistance), whatIsPlayer);


    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + playerCheckDistance * facingDir, transform.position.y));
    }
}
