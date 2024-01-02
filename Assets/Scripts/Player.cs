using UnityEngine;

public class Player : Entity
{

    [Header("Movement Info")]
    [SerializeField] float jumpForce = 4f;
    [SerializeField] float moveSpeed = 3f;



    [Header("Dash Info")]
    [SerializeField] float dashDuration;
    [SerializeField] float dashTime;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashCoolDown;
    float dashCoolDownTimer;
    bool isDashing;

    [Header("Attack Info")]
    [SerializeField] float comboTime = 0.4f;
    bool isAttacking;
    int comboCounter;
    float comboTimeWindow;




    private float xInput;




    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update(); 
        Movement();

        CheckInputs();

        DecreaseTimers();

        AnimationControllers();
    }

    private void DecreaseTimers()
    {
        dashTime -= Time.deltaTime;
        dashCoolDownTimer -= Time.deltaTime;

        comboTimeWindow -= Time.deltaTime;
    }



    void CheckInputs() //whenever we want to see if the player is pushing a button, we should check that in THIS method, and then execute the effect of that button.
    {
        xInput = Input.GetAxisRaw("Horizontal"); //checks the button by assigning it to a variable (GetAxisRAW gives us only -1, 0, and 1, increases the snappiness/gamefeel)

        if (Input.GetKeyDown(KeyCode.Space)) //checks the button
        {
            Jump();  //executes the effect
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashAbility();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartAttackEvent();
            
        }
    }

    private void StartAttackEvent()
    {
        if (!isGrounded || isDashing) { return; }
        if (comboTimeWindow < 0)
        {
            comboCounter = 0;
        }
        isAttacking = true;
        comboTimeWindow = comboTime;
    }

    private void DashAbility()
    {
        if (dashCoolDownTimer < 0 && !isAttacking)
        {
            dashCoolDownTimer = dashCoolDown;
            dashTime = dashDuration;
            isDashing = true;
        }
    }

    public void AttackIsOver()
    {
        isAttacking = false;

        comboCounter++;
        if (comboCounter > 2)
        {
            comboCounter = 0;
        }
    }



    void Movement() //handles all things relating to movement
    {
        
        if(isAttacking)
        {
                rb.velocity = new Vector2(0, 0);
        }
        
        else if (dashTime > 0)
            rb.velocity = new Vector2(facingDir * dashSpeed, 0);
        else
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
            isDashing = false;
        }
        //rb.velocity.y means "just go with whatever
        //the rigid body says for the y axis!... essentially,
        //prevents you from messing with the rb's current business)
    }

    private void Jump()
    {
        if (isGrounded) //isGrounded is checked via the "CollisionsChecks" method above,
                        //using Raycasts! The Raycast laser beam we made is visually
                        //represented by the OnDrawGizmos() method below
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void AnimationControllers() //Whenever you want to use myAnimator,
                                        //do so HERE in this method, and assign parameters
                                        //to VARIABLES that are controlled elsewhere in
                                        //this script by other methods
    {
        bool isMoving = rb.velocity.x != 0;

        myAnimator.SetBool("isMoving", isMoving); //isMoving is controlled here locally
                                                  //because it isn't used anywhere else!
                                                  //It's better to try to keep variables
                                                  //private at EVERY level, even if that 
                                                  //means private at the method level

        myAnimator.SetBool("isGrounded", isGrounded); //isGrounded is derrived from "CollisionsChecks()"

        myAnimator.SetFloat("yVelocity", rb.velocity.y);

        myAnimator.SetBool("isDashing", isDashing); //isDashing is derrived from the DashAbility() and Movement() methods

        myAnimator.SetBool("isAttacking", isAttacking);

        myAnimator.SetInteger("comboCounter", comboCounter);

    }



}
