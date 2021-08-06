using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FoxControl : MonoBehaviour
{
    private PlayerInput playerInput;
    
    // Move
    private float velx;
    private bool movingToTheRigth;
    // Crouch
    public bool isCrouch;
    // Jump
    private bool isJumping;

    private Rigidbody2D rigidbody2d;
    private Animator animator;

    void Awake()
    {
        playerInput = new PlayerInput();
        velx = 0;
        movingToTheRigth = true;
        isCrouch = false;
        isJumping = false;

        playerInput.Movement.Move.performed += ctx => Move(ctx);
        playerInput.Movement.Move.canceled += ctx => Wait();

        playerInput.Movement.Crouch.performed += ctx => Crouch();
        playerInput.Movement.Crouch.canceled += ctx => Wait();

        playerInput.Movement.Jump.performed += ctx => Jump();

        // playerInput.Movement.Climb.performed += ctx => Climb(ctx);
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
    void Wait()
    {
        isCrouch = false;
        velx = 0;
        animator.SetTrigger("Wait");
    }
    void Move(InputAction.CallbackContext ctx)
    {
        velx = ctx.ReadValue<float>();
        animator.SetTrigger("Walk");
    }
    void Crouch()
    {
        isCrouch = true;
        velx = 0;
        animator.SetTrigger("Crouch");
    }

    void Jump()
    {
        isJumping = true;
        animator.SetTrigger("Jump");
    }

    /*
    void Climb(InputAction.CallbackContext ctx)
    {
        isClimbing = enableClimbing;

        if (enableClimbing)
        {
            animator.SetTrigger("Climb");
            vely = ctx.ReadValue<float>();
        }
    }
    */
    void Flip()
    {
       var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    //==================================================================================

    // FixedUpdate is called once per frame, this one is used to interact with Rigidbody 2D
    void FixedUpdate()
    {
        if (isCrouch)
        {
            isJumping = false;
            velx = 0;
        }

        Vector2 v = new Vector2(velx * 3f, rigidbody2d.velocity.y);
        rigidbody2d.velocity = v;

        if(movingToTheRigth && velx < -0.1f) {
            movingToTheRigth = false;
            Flip();
        } else if (!movingToTheRigth && velx > 0.1f) {
            movingToTheRigth = true;
            Flip();
        }

        if (isJumping)
        {
            Vector2 f = new Vector2(0, 600);
            rigidbody2d.AddForce(f);
            isJumping = false;
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        enableClimbing = collision.gameObject.name == "TreeForClimbing";
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enableClimbing = false;
        isClimbing = false;
    }
    */

    void OnDisable()
    {
        playerInput.Movement.Disable();
    }
}
