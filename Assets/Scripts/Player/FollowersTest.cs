using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowersTest : MonoBehaviour
{
    public Transform objetivo;
    private float speed = 5;
    private Rigidbody2D rb;
    private Animator animator;
    private ChildPlayerBehaviour cb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cb = GetComponent<ChildPlayerBehaviour>();


    }

    void FixedUpdate()
    {
        if (objetivo != null)
        {
            cb.SetSpeed(5f);
            Vector2 direccion = (objetivo.position - transform.position).normalized;
            Vector2 nuevaPos = (Vector2)transform.position + direccion * speed * Time.fixedDeltaTime;

            rb.MovePosition(nuevaPos);
        }
    }
}

