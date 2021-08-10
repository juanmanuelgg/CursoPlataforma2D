using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogControl : MonoBehaviour
{
    private int MAX_JUMPS = 1;
    private int FROG_JMP_Y = 400;
    private int FROG_JMP_X = 100;
    private float PROB_JMP=0.015f;

    private bool movingToTheRigth;
    private bool isTouchingFloor;
    private bool isJumping;
    private int jumps;

    private Animator animator;
    private Rigidbody2D rbd;
    
    // Start is called before the first frame update
    void Start()
    {
        movingToTheRigth = true;
        isTouchingFloor = true;
        isJumping = false;
        jumps = 0;
        animator = GetComponent<Animator>();
        rbd = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Random.value < PROB_JMP) Jump();
    }

    void Jump()
    {
        if (isTouchingFloor && !isJumping && jumps < MAX_JUMPS)
        {
            float jmpDirection = (Random.value - 0.5f) < 0f ? -1f : 1f;
            Vector2 jmp = new Vector2(-jmpDirection * FROG_JMP_X, FROG_JMP_Y);
            animator.SetTrigger("Jump");
            rbd.AddForce(jmp);
            jumps++;
            isJumping = true;
            isTouchingFloor = false;
            if (movingToTheRigth && jmpDirection < -0.1f)
            {
                movingToTheRigth = false;
                Flip();
            }
            else if (!movingToTheRigth && jmpDirection > 0.1f)
            {
                movingToTheRigth = true;
                Flip();
            }
        } else
        {
            isJumping = false;
        }
    }

    void Flip()
    {
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            isTouchingFloor = true;
            jumps = 0;
            isJumping = false;
            float angle = rbd.rotation;
            while (angle > 0)
            {
                angle--;
                rbd.rotation = angle;
            }
            while (angle < 0)
            {
                angle++;
                rbd.rotation = angle;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            isTouchingFloor = false;
            isJumping = true;
        }
    }
}