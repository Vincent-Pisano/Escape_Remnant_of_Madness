using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class protagonistScript : MonoBehaviour
{
    [SerializeField][Range(0,5)] private float moveSpeed = 2.4f;

    private Rigidbody2D rb;

    private Vector2 movement;

    private Animator animator;

    private float movementXMem;

    private float movementYMem;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        movement.Normalize();

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.sqrMagnitude != 0)
        {
            movementXMem = movement.x;
            movementYMem = movement.y;
        }
        else
        {
            animator.SetFloat("HorizontalIdle", movementXMem);
            animator.SetFloat("VerticalIdle", movementYMem);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
