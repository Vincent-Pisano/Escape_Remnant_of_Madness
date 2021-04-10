using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardEnemyScript : MonoBehaviour
{
    private readonly float MAX_ANGLE = 40.0f;
    private readonly float MIN_ANGLE = -40.0f;
    
    private float angle = 40.0f;
    private float lookingSpeed = 2f;

    private bool sensAngle = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 1), 
                        (Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.left).normalized * 5f, 
                        Color.red);
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1),
            (Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.left).normalized, 
                    5f, 
                           LayerMask.GetMask("Player", "Wall"));
        if (hit && !hit.collider.gameObject.name.Equals("Tilemap_wall"))
        {
            Debug.Log("Hit something : " + hit.collider.name);
        }
    }

    private void FixedUpdate()
    {
        if (angle == MAX_ANGLE)
            sensAngle = true;
        else if (angle == MIN_ANGLE)
            sensAngle = false;
        if (sensAngle)
            angle -= lookingSpeed;
        else
            angle += lookingSpeed;
    }
}
