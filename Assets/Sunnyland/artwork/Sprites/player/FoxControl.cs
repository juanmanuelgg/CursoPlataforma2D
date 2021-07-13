using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FoxControl : MonoBehaviour
{
    PlayerInput playerInput;
    float vel;
    Boolean movingToTheRigth;
    Rigidbody2D rigidbody;
    Animator animator;

    void Awake()
    {
        playerInput = new PlayerInput();
        vel = 0;
        movingToTheRigth = true;

        playerInput.Movement.Move.performed += ctx => Move(ctx);
        playerInput.Movement.Move.canceled += ctx => Wait();

        playerInput.Movement.Jump.performed += ctx => Jump();

        playerInput.Movement.Crouch.performed += ctx => Crouch();
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
    void Wait()
    {
        vel = 0;
        animator.SetTrigger("Wait");
    }

    void Move(InputAction.CallbackContext ctx)
    {
        vel = ctx.ReadValue<float>();
        animator.SetTrigger("Walk");
    }
    void Jump()
    {
        Debug.Log("Jump");
    }

    void Crouch()
    {
        Debug.Log("Crouch");
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
        Vector2 v = new Vector2(vel, rigidbody.velocity.y);
        rigidbody.velocity = v;

        if(movingToTheRigth && vel < 0) {
            movingToTheRigth = false;
            Flip();
        } else if (!movingToTheRigth && vel > 0) {
            movingToTheRigth = true;
            Flip();
        }
    }

    void OnDisable()
    {
        playerInput.Movement.Disable();
    }
}
