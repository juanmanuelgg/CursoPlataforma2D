using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FoxControl : MonoBehaviour
{
    private int FOX_VEL = 5;
    private int FOX_JMP = 420;
    private int MAX_JUMPS = 1;
    public float KILL_OFFSET = 5f;

    private PlayerInput playerInput;
    
    // Move
    private float velx;
    private bool movingToTheRigth;
    private bool isMoving;
    // Crouch
    public bool isCrouch;
    // Jump
    private bool isTouchingFloor;
    private bool isJumping;
    private int jumps;
    // Grab
    private bool isGrabing;
    // Climb
    private bool isClimbing;
    private bool enableClimbing;

    private Rigidbody2D rigidbody2d;
    private Animator animator;


    void Awake()
    {
        playerInput = new PlayerInput();
        velx = 0;
        jumps = 0;
        movingToTheRigth = true;
        isMoving = false;
        isCrouch = false;
        isJumping = false;
        isGrabing = false;
        isClimbing = false;
        enableClimbing = false;

        // Move
        playerInput.Movement.Move.performed += ctx => Move(ctx);
        playerInput.Movement.Move.canceled += ctx => Wait();

        // Crouch
        playerInput.Movement.Crouch.performed += ctx => Crouch();
        playerInput.Movement.Crouch.canceled += ctx => Wait();

        // Jump
        playerInput.Movement.Jump.performed += ctx => Jump();

        // Climb
        playerInput.Movement.Climb.performed += ctx => Climb(ctx);
        playerInput.Movement.Move.canceled += ctx => Wait();
        
        // Grab
        playerInput.Movement.Grab.performed += ctx => Grab(ctx);
        playerInput.Movement.Grab.canceled += ctx => Wait();
    }

    void OnEnable()
    {
        playerInput.Movement.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    //==================================================================================
    void Move(InputAction.CallbackContext ctx)
    {
        isMoving = true;
        velx = ctx.ReadValue<float>();
    }
    void Crouch()
    {
        isCrouch = true;
        velx = 0;
    }

    void Jump()
    {
        if (!isJumping && !isCrouch && (isTouchingFloor || jumps < MAX_JUMPS))
        {
            isJumping = true;
            animator.SetTrigger("Jump");
        }
    }

    void Climb(InputAction.CallbackContext ctx)
    {
        isClimbing = enableClimbing;

        if (enableClimbing)
        {
            animator.SetTrigger("Climb");
            //posy = ctx.ReadValue<float>();
        }
    }

    void Grab(InputAction.CallbackContext ctx)
    {
        isGrabing = true;
        animator.SetTrigger("Grab");
    }
    void Flip()
    {
       var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    void Wait()
    {
        velx = 0;
        isCrouch = false;
        isMoving = false;
        isJumping = false;
        isGrabing = false;
        isClimbing = false;
        enableClimbing = false;
    }
    //==================================================================================

    // FixedUpdate is called once per frame, this one is used to interact with Rigidbody 2D
    void FixedUpdate()
    {
        if (isCrouch)
        {
            isJumping = false;
            animator.SetTrigger("Crouch");
        }        

        velx = (isMoving)? velx: 0;
        Vector2 v = new Vector2(velx * FOX_VEL, rigidbody2d.velocity.y);
        rigidbody2d.velocity = v;
        if (!isMoving && !isCrouch && !isJumping && !isGrabing && !isClimbing) animator.SetTrigger("Wait");
        else if (isMoving) animator.SetTrigger("Walk");
        
        if (movingToTheRigth && velx < -0.1f) {
            movingToTheRigth = false;
            Flip();
        } else if (!movingToTheRigth && velx > 0.1f) {
            movingToTheRigth = true;
            Flip();
        }

        if (isJumping)
        {
            Vector2 f = new Vector2(0, FOX_JMP);
            rigidbody2d.AddForce(f);
            jumps++;
            isJumping = false;
        }

        if (isGrabing)
        {

        }

        if (isClimbing)
        {
           
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Diamond")
        {
            Destroy(collision.gameObject);
            animator.SetTrigger("Grab");
            Debug.Log("Diamond +1");
        }
        if (collision.gameObject.name.StartsWith("Floor") || collision.gameObject.name.Contains("Enemy"))
        {
            isTouchingFloor = true;
            jumps = 0;
            isJumping = false;
            // animator.SetTrigger("Walk");
            animator.SetTrigger("Wait");

            if (collision.gameObject.name == "Enemy")
            {
                // Debug.Log("Fox: "+ rigidbody2d.position.y + ", Enemy:" + collision.gameObject.transform.position.y + " Enemy Offset:" + (collision.gameObject.transform.position.y + KILL_OFFSET));
                if(rigidbody2d.position.y > (collision.gameObject.transform.position.y + KILL_OFFSET)){
                    Destroy(collision.gameObject);
                } else {
                    animator.SetTrigger("Damage");
                }
            }
        }
        enableClimbing = (collision.gameObject.name == "ForClimbing");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Floor" || collision.gameObject.name == "Killzone") {
            isCrouch = false;
            isTouchingFloor = false;
        }
    }

    void OnDisable()
    {
        playerInput.Movement.Disable();
    }
}
