using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 10f;
    [SerializeField] float drownSpeed = 0.5f;

    [Header("Sound")]
    [SerializeField] AudioClip DeathSound;
    [SerializeField] float DeathSoundVolume = 0.5f;

    Vector2 moveInput;
    Rigidbody2D playerRB2D;
    Animator myAnimator;
    CapsuleCollider2D playerCollider;
    AudioSource playerAudioSource;
    float gravityScaleAtStart;
    public bool playerIsAlive = true;
    bool jumpingInStairs;
    
    void Start()
    {
        playerRB2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = playerRB2D.gravityScale;
        playerAudioSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        if (playerIsAlive)
        {
            Run();
            FlipSprite();
            ClimbLadder();        
            Die();
        }  
    }

    void OnMove(InputValue value)
    {
        if (playerIsAlive)
        {
            moveInput = value.Get<Vector2>();
        }    
    }

    void OnJump(InputValue value)
    {
        if ((!playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) 
            && !playerCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
            || !playerIsAlive)
            {return;}
        if (value.isPressed)
        {
            if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
            {
                jumpingInStairs = true;
            }
            playerRB2D.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, playerRB2D.velocity.y);
        playerRB2D.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRB2D.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed); 
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRB2D.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRB2D.velocity.x), 1);
        }  
    }


    void ClimbLadder()
    {
        
        if (!playerCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")) || !playerIsAlive)
        {
            playerRB2D.gravityScale = gravityScaleAtStart;
            jumpingInStairs = false;
            myAnimator.SetBool("isClimbing", false);
            myAnimator.SetBool("IsNotMovingInLadder", false); 
            return;
        } 
        if (jumpingInStairs)
        {
            playerRB2D.velocity = new Vector2(playerRB2D.velocity.x, jumpSpeed);
        }
        else
        {
            Vector2 climbVelocity = new Vector2(playerRB2D.velocity.x, moveInput.y * climbSpeed);
            playerRB2D.velocity = climbVelocity;
            playerRB2D.gravityScale = 0; 
            bool playerHasVerticalSpeed = Mathf.Abs(playerRB2D.velocity.y) > Mathf.Epsilon; 
            myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
            myAnimator.SetBool("IsNotMovingInLadder", !playerHasVerticalSpeed);
        }    
    }

    void Die()
    {
        if (playerRB2D.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            playerIsAlive = false;
            myAnimator.SetTrigger("Dying");
            playerRB2D.velocity = new Vector2(0f, jumpSpeed);
            playerRB2D.gravityScale = gravityScaleAtStart;
            Vector3 position = transform.position;
            position.z = -10;
            AudioSource.PlayClipAtPoint(DeathSound, position, DeathSoundVolume);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
        else if (playerRB2D.IsTouchingLayers(LayerMask.GetMask("Water")))
        {
            playerIsAlive = false;
            myAnimator.SetTrigger("Dying");
            playerRB2D.gravityScale = 0;
            playerRB2D.velocity = new Vector2(0f, -drownSpeed);
            playerAudioSource.Play();
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }      
    }

}