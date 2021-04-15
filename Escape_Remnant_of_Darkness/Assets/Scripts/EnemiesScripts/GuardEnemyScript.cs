using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardEnemyScript : MonoBehaviour
{
    [Range(0,10)]
    [SerializeField] private float moveSpeed = 6;
    
    
    private Rigidbody2D _rigidbody;
    private Vector2 _velocity;
    private Vector2 _directionLookAt;
    private Animator _animator;
    private float movementXMem;
    private float movementYMem;
    
    // Start is called before the first frame update
    void Start()
    {
        _directionLookAt = new Vector2();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _velocity.x = Input.GetAxis("Horizontal");
        _velocity.y = Input.GetAxis("Vertical");
        _velocity.Normalize();
        
        _velocity = new Vector2 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized * moveSpeed;

        AnimateMovement();
    }
    
    private void FixedUpdate()
    {
        _rigidbody.MovePosition (_rigidbody.position + _velocity * Time.fixedDeltaTime);
    }

    private void AnimateMovement()
    {
        _animator.SetFloat("Horizontal", _velocity.x);
        _animator.SetFloat("Vertical", _velocity.y);
        _animator.SetFloat("Speed", _velocity.sqrMagnitude);
        if (_velocity.sqrMagnitude != 0)
        {
            _directionLookAt = new Vector2(_velocity.x, _velocity.y);
        }
        else
        {
            _animator.SetFloat("HorizontalIdle", _directionLookAt.x);
            _animator.SetFloat("VerticalIdle", _directionLookAt.y);
        }
    }

    public Vector2 getVelocity()
    {
        if (_velocity.sqrMagnitude == 0)
        {
            return _directionLookAt;
        }
        return _velocity;
    }

    
}
