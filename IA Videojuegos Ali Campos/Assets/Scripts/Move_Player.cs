using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Player : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(1 * speed, rb.velocity.y);
    }
}
