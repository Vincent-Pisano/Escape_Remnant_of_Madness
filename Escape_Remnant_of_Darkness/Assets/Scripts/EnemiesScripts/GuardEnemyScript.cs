using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
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
        _rigidbody.MovePosition (_rigidbody.position + _directionLookAt * (moveSpeed * Time.fixedDeltaTime));
    }

    protected void AnimateMovement()
    {
        base.AnimateMovement();
        _animator.SetFloat("Speed", _directionLookAt.sqrMagnitude);
        if (_directionLookAt.sqrMagnitude != 0f)
        {
            _directionLookAt = new Vector2(_directionLookAt.x, _directionLookAt.y);
        }
        else
        {
            _animator.SetFloat("HorizontalIdle", _directionLookAt.x);
            _animator.SetFloat("VerticalIdle", _directionLookAt.y);
        }
    }
}
