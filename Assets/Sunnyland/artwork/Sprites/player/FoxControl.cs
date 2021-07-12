using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxControl : MonoBehaviour
{
    public float vel = 1f;
    Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate is called once per frame, this one is used to interact with Rigidbody 2D
    void FixedUpdate()
    {
        Vector2 v = new Vector2(vel, 0);
        rigidbody.velocity = v;
    }
}
