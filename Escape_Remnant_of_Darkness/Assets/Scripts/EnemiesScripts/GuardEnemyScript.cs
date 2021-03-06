using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardEnemyScript : EnemyScript
{
    [Range(0,10)]
    [SerializeField] private float moveSpeed = 6;
    
    
    void Update()
    {
        AnimateMovement();
    }
    
    private void FixedUpdate()
    {
        if (_isTargetVisible)
        {
            _rigidbody.MovePosition (_rigidbody.position + _directionLookAt * (moveSpeed * Time.fixedDeltaTime));
        }
    }

    private void AnimateMovement()
    {
        _animator.SetFloat("Horizontal", _directionLookAt.x);
        _animator.SetFloat("Vertical", _directionLookAt.y);
        _animator.SetFloat("Speed", _directionLookAt.sqrMagnitude);
        if (_isTargetVisible) return;
        
        //If Idle
        _animator.SetFloat("Speed", 0);
        _animator.SetFloat("HorizontalIdle", _directionLookAt.x);
        _animator.SetFloat("VerticalIdle", _directionLookAt.y);
    }
}
