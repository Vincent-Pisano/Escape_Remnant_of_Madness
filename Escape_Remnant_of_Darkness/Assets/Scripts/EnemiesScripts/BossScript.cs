﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : EnemyScript 
{
    private Animator _animatorLeftEye;
    private Animator _animatorRightEye;
    private Animator _animatorMiddleEye;
    private FieldOfView _controllerRadius;

    private float _nbrHit = 0;
    
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

    public void DealDamage()
    {
        switch (_nbrHit)
        {
            case 0 :
                _animatorLeftEye.SetBool("isVanquished", true);
                break;
            case 1 :
                _animatorRightEye.SetBool("isVanquished", true);
                break;
            case 2 :
                _animatorMiddleEye.SetBool("isVanquished", true);
                break;
        }
        _nbrHit++;
    }
}