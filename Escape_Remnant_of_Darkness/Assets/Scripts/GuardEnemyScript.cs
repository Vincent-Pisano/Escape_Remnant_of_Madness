using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardEnemyScript : MonoBehaviour
{
    private readonly float MAX_ANGLE = 40.0f;
    private readonly float MIN_ANGLE = -40.0f;
    
    [SerializeField][Range(0,5)] private float moveSpeed = 2.4f;
    [SerializeField] private float lookingSpeed = 3f;
    
    private float angle = 40.0f;

    private bool sensAngle = true;
    
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
        if (CheckCollidWithPlayer())
            Debug.Log("Hit Player");

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

    private Boolean CheckCollidWithPlayer()
    {
        Vector2 directionLookAt = movement;
        if (Mathf.Approximately(0f, directionLookAt.x) &&
            Mathf.Approximately(0f, directionLookAt.y))
            directionLookAt = Vector2.down;
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 1),
            (Quaternion.AngleAxis(angle, Vector3.forward) * directionLookAt).normalized * 5f,
            Color.red);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1),
            (Quaternion.AngleAxis(angle, Vector3.forward) * directionLookAt).normalized,
            5f,
            LayerMask.GetMask("Player", "Wall"));
        if (hit && !hit.collider.gameObject.name.Equals("Tilemap_wall"))
        {
            return true;
        }
        return false;
    }

    private void FixedUpdate()
    {
        SetAngleLook();

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void SetAngleLook()
    {
        if (angle >= MAX_ANGLE)
            sensAngle = true;
        else if (angle <= MIN_ANGLE)
            sensAngle = false;
        if (sensAngle)
            angle -= lookingSpeed;
        else
            angle += lookingSpeed;
    }
}
