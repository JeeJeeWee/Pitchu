using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public Transform GroundCheck;
    public Transform AttackPoint;

    public float MoveSpeed;
    public float JumpForce;
    public float AttackForceX;
    public float AttackForceY;
    public float GroundCheckRadius;
    public float AttackRadius;
    private float movementInputDirection;
    private float facingDirection;

    public int AmountOfJumps;
    public int AttackDamage;
    private int JumpsLeft;

    private bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool canJump;
   
    public LayerMask whatIsGround;
    public LayerMask Enemie;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        JumpsLeft = AmountOfJumps;
        facingDirection = 1;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        updateAnimations();
        CheckIfCanJump();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, whatIsGround);
    }

    private void CheckIfCanJump()
    {
        if(isGrounded && rb.velocity.y <= 0)
        {
            JumpsLeft = AmountOfJumps;
        }
        
        if(JumpsLeft <=0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }

    }

    private void CheckMovementDirection()
    {
        if(isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if(!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if(rb.velocity.x != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void updateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if(Input.GetButtonDown("Attack"))
        {
            Attack();
        }
    }

    private void Jump()
    {
        if(canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
            JumpsLeft--;
        }
       
    }

    private void Attack()
    {  
        anim.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRadius, Enemie);

        foreach(Collider2D Enemie in hitEnemies)
        {
            Enemie.GetComponent<EnemieScript>().GetHit(AttackDamage, facingDirection * AttackForceX, AttackForceY);
        }
    }

    private void ApplyMovement()
    {
        rb.velocity = new Vector2(movementInputDirection*MoveSpeed, rb.velocity.y);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
        facingDirection *= -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GroundCheck.position, GroundCheckRadius);
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRadius);
    }
}
