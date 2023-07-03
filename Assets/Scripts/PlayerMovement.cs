using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float climbSpeed = 10f;
    [SerializeField] float swimmingSpeed = 2f;
    [SerializeField] GameObject arrow;
    [SerializeField] Transform bow;

    Animator myAnimator;
    Rigidbody2D myRigidbody;
    Vector2 moveInput;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    ParticleSystem myParticle;
    float gravityScaleAtStart;
    bool isAlive;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myParticle = GetComponent<ParticleSystem>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        isAlive = true;
    }
    
    void Update()
    {
        if (!isAlive) {return;}
        Run();
        FlipSprite();
        ClimbLadder();
        Swimming();
        Die();
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) {return;}

        Instantiate(arrow, bow.position, transform.rotation);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector3(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
        
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) {return;}
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) {return;}
        if (value.isPressed && myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
                   
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
        }

    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Lader")))
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }
        Vector2 climbingVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbingVelocity;
        myRigidbody.gravityScale = 0f;
        
        bool playerHasClimbingSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasClimbingSpeed);
    }

    void Swimming()
    {
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Water")))
        {
            Vector2 playerVelocity = new Vector2(moveInput.x * swimmingSpeed, moveInput.y * swimmingSpeed);
            myRigidbody.velocity = playerVelocity;
            myRigidbody.gravityScale = 2f;
        }
    }

    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity += new Vector2(20f, 20f);
            myParticle.Play();
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
    
}
