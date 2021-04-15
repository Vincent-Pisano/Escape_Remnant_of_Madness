using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Experimental.Rendering.Universal;

public class protagonistScript : MonoBehaviour
{
    [SerializeField][Range(0,5)] private float moveSpeed = 2.4f;

    private Rigidbody2D rb;

    private Vector2 movement;

    private Animator animator;

    private float movementXMem;

    private float movementYMem;
    
    //Madness
    private float sanity;

    [SerializeField] [Range(0, 10)] private float insanityAccumulation = 2;

    [SerializeField] [Range(0, 1)] private float lightFallOff = 0.000016f;

    private GameObject _pointLight;

    private Light2D _lightIntensity;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sanity = 100;
        _pointLight = transform.GetChild(0).gameObject;
        _lightIntensity = _pointLight.GetComponent<Light2D>();
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

        if (_lightIntensity.intensity > 0.25f)
        {
            /*print("falloff = " + lightFallOff);
            print("par frame" + lightFallOff * Time.fixedDeltaTime);*/
            _lightIntensity.intensity -= lightFallOff * Time.fixedDeltaTime / 15;
        }
        else
        {
            _lightIntensity.intensity = 0.25f;
        }
        //print(_lightIntensity.intensity);
    }
}
