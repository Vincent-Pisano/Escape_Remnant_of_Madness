using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : EnemyScript
{

    private Animator _animatorLeftEye;
    private Animator _animatorRightEye;
    private Animator _animatorMiddleEye;
    
    // Start is called before the first frame update
    void Start()
    {
        _animatorLeftEye = transform.GetChild(0).gameObject.GetComponent<Animator>();
        _animatorRightEye = transform.GetChild(1).gameObject.GetComponent<Animator>();
        _animatorMiddleEye = transform.GetChild(2).gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _animatorLeftEye.SetFloat("horizontal", _directionLookAt.x);
        _animatorRightEye.SetFloat("horizontal", _directionLookAt.x);
        _animatorMiddleEye.SetBool("isAwakened", _isTargetVisible);
    }
}
