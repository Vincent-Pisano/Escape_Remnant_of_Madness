using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MirrorMonsterScript : EnemyScript
{
    
    void Start()
    {
        _directionLookAt = new Vector2();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (_directionLookAt.sqrMagnitude != 0)
        {
            _animator.SetBool("targetFound", true);
            AnimateMovement();
        }

    }
    
    private void FixedUpdate()
    {
        //print(_directionLookAt);
    }
}
