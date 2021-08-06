using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonControl : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Random.value<0.01f) animator.SetTrigger("Attack");
    }
}
