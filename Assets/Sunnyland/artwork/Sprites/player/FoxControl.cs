using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FoxControl : MonoBehaviour
{
    PlayerInput playerInput;
    float velx;
    float vely;
    Boolean movingToTheRigth;
    Boolean enableClimbing;
    Boolean isMoving;
    Boolean isClimbing;
    Boolean isChrouching;
    Boolean isJumping;
    Rigidbody2D rigidbody;
    Animator animator;

    void Awake()
    {
        playerInput = new PlayerInput();
        velx = 0;
        vely = 0;
        movingToTheRigth = true;
        enableClimbing = false;

        isMoving = false;
        isClimbing = false;
        isChrouching = false;
        isJumping = false;

        playerInput.Movement.Move.performed += ctx => Move(ctx);
        playerInput.Movement.Jump.performed += ctx => Jump();
        playerInput.Movement.Crouch.performed += ctx => Crouch();
        playerInput.Movement.Climb.performed += ctx => Climb(ctx);
    }

    void OnEnable()
    {
        playerInput.Movement.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    //==================================================================================
    void Move(InputAction.CallbackContext ctx)
    {
        animator.SetTrigger("Walk");
        velx = ctx.ReadValue<float>();
        isChrouching = false;
        isMoving = true;
    }
    void Jump()
    {
        animator.SetTrigger("Jump");
        isChrouching = false;
        isClimbing = false;
        isJumping = true;
    }

    void Crouch()
    {
        animator.SetTrigger("Crouch");
        isChrouching = true;
        isMoving = false;
        isClimbing = false;
        isJumping = false;
    }

    void Climb(InputAction.CallbackContext ctx)
    {
        isChrouching = false;
        isClimbing = false;
        isJumping = false;
        isClimbing = enableClimbing;

        if (enableClimbing)
        {
            animator.SetTrigger("Climb");
            vely = ctx.ReadValue<float>();
        }
    }

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
        Vector2 v = enableClimbing && isClimbing? new Vector2(velx * 2f, vely): new Vector2(velx * 2f, rigidbody.velocity.y);
        rigidbody.velocity = v;

        if(movingToTheRigth && velx < 0) {
            movingToTheRigth = false;
            Flip();
        } else if (!movingToTheRigth && velx > 0) {
            movingToTheRigth = true;
            Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enableClimbing = collision.gameObject.name == "TreeForClimbing";
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enableClimbing = false;
        isClimbing = false;
    }

    void OnDisable()
    {
        playerInput.Movement.Disable();
    }
}
