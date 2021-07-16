using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpossumBehaviourScript : MonoBehaviour
{
    public float vel = -1f;
    Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 v = new Vector2(vel, rigidbody.velocity.y);
        rigidbody.velocity = v;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Flip();
    }

    void Flip()
    {
        vel *= -1;

        var s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }
}
