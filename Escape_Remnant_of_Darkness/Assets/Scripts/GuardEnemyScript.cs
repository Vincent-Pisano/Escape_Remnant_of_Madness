using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardEnemyScript : MonoBehaviour
{
    private readonly float MAX_ANGLE = 70.0f;
    private readonly float MIN_ANGLE = -70.0f;

    [Range(1, 15)]
    [SerializeField]
    private float viewRadius = 1;
    [SerializeField][Range(0,5)] private float moveSpeed = 2.4f;
    [SerializeField] private float lookingSpeed = 5f;
    [SerializeField] private float checkDuration = 10f;
    [SerializeField] private Transform target = null;
    [SerializeField] private bool isVisible = false;
    
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
        /*DetectTarget();
        if (target != null)
        {
            isVisible = CheckTargetVisible();
        }*/

        checkDuration--;
        isVisible = CheckCollidWithPlayer();
        if (!isVisible)
            SetAngleLook();
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private bool CheckCollidWithPlayer()
    {
        Vector2 directionLookAt = movement;
        print(isVisible);
        if (isVisible && checkDuration == 0)
        {
            directionLookAt = target.transform.position;
        }
        else {
            if (Mathf.Approximately(0f, directionLookAt.x) && 
                Mathf.Approximately(0f, directionLookAt.y))
            {
                directionLookAt = Vector2.down;
            }
        }
        
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1),
            (Quaternion.AngleAxis(angle, Vector3.forward) * directionLookAt).normalized, 5f,
            LayerMask.GetMask("Player", "Wall"));

        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 1),
            (Quaternion.AngleAxis(angle, Vector3.forward) * directionLookAt).normalized * 5f,
            Color.red);
        
        if (hit && hit.collider.gameObject.name.Equals("Protagonist"))
        {
            target = hit.collider.gameObject.transform;
            return true;
        }
        target = null;
        return false;
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
    
    /*
    * Start Collider Around Guard
    
    private bool CheckTargetVisible()
    {
        var result = Physics2D.Raycast(transform.position, target.position - transform.position, viewRadius, LayerMask.GetMask("Player"));
        if (result.collider != null)
        {
            return (LayerMask.GetMask("Player") & (1 << result.collider.gameObject.layer)) != 0;
        }
        return false;
    }

    private void DetectTarget()
    {
        if (target == null)
            CheckIfPlayerInRange();
        else if (target != null)
            DetectIfOutOfRange();
    }

    private void DetectIfOutOfRange()
    {
        if (target == null || target.gameObject.activeSelf == false || Vector2.Distance(transform.position, target.position) > viewRadius + 1)
        {
            target = null;
        }
    }
    
    private void CheckIfPlayerInRange()
    {
        Collider2D collision = Physics2D.OverlapCircle(transform.position, viewRadius, LayerMask.GetMask("Player"));
        if (collision != null)
        {
            target = collision.transform;
        }
    }

    /*
     * End Collider Around Guard
     */
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
}
