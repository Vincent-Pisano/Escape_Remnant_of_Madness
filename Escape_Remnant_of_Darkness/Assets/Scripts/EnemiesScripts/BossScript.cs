using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : EnemyScript 
{

    //[SerializeField] private Transform protagonist;
    
    private Animator _animatorLeftEye;
    private Animator _animatorRightEye;
    private Animator _animatorMiddleEye;
    private FieldOfView _controllerRadius;
    
    // Start is called before the first frame update
    void Start()
    {
        _animatorLeftEye = transform.GetChild(0).gameObject.GetComponent<Animator>();
        _animatorRightEye = transform.GetChild(1).gameObject.GetComponent<Animator>();
        _animatorMiddleEye = transform.GetChild(2).gameObject.GetComponent<Animator>();
        _controllerRadius = GetComponent<FieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {
        
        _animatorLeftEye.SetFloat("horizontal", _directionLookAt.x);
        _animatorRightEye.SetFloat("horizontal", _directionLookAt.x);
        _animatorMiddleEye.SetBool("isAwakened", _isTargetVisible);

        if (_isTargetVisible)
        {
            _controllerRadius.SetViewRadius(15);
            _animatorLeftEye.SetBool("isDetected", true);
            _animatorRightEye.SetBool("isDetected", true);
        }
        else
        {
            _controllerRadius.SetViewRadius(12);
            _animatorLeftEye.SetBool("isDetected", false);
            _animatorRightEye.SetBool("isDetected", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("mirrorMonster"))
        {
            _animatorLeftEye.SetBool("isVanquished", true);
        }
    }
}