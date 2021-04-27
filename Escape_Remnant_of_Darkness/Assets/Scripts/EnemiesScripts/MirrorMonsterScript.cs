using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.PlayerLoop;

public class MirrorMonsterScript : EnemyScript
{
    private Light2D _lightBeam;
    private GameObject _lightBeamGO;
    void Start()
    {
        _directionLookAt = new Vector2();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _lightBeamGO = transform.GetChild(0).gameObject;
        _lightBeam = _lightBeamGO.GetComponent<Light2D>();
    }
    
    void Update()
    {
        if (_directionLookAt.sqrMagnitude != 0)
        {
            _animator.SetBool("targetFound", true);
            AnimateMovement();
        }
        else
        {
            _directionLookAt = _directionLookAt;
        }
        _lightBeamGO.transform.localEulerAngles = new Vector3(0f,0f, AngleFromDir(_directionLookAt));
    }
    
    private float AngleFromDir(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.x, direction.y) * 180 / Mathf.PI;
        if (angle < 0)
        {
            angle = 360 + angle;
        }
        return -angle;
    }
}
