using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.PlayerLoop;

public class MirrorMonsterScript : EnemyScript
{
    private GameObject _lightBeamGO;
    private Light2D _lightBeam;
    private FieldOfView _fieldOfView;
    private bool _isRespawning;
    [SerializeField]private Transform respawnPoint;
    void Start()
    {
        _directionLookAt = new Vector2();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _fieldOfView = GetComponent<FieldOfView>();
        _lightBeamGO = transform.GetChild(0).gameObject;
        _lightBeam = _lightBeamGO.GetComponent<Light2D>();
        _isRespawning = false;
    }
    
    void FixedUpdate()
    {
        if (!_isRespawning)
        {
            if (_isTargetVisible)
            {
                _lightBeam.intensity = 1;
                _animator.SetBool("targetFound", true);
                GameObject target = _fieldOfView.GetTarget().gameObject;
                if (target.tag.Equals("Boss"))
                {
                    StartCoroutine("Respawn", target);
                }
                else
                {
                    AnimateMovement();
                }

                _lightBeamGO.transform.localEulerAngles = new Vector3(0f, 0f, _fieldOfView.AngleFromDir(_directionLookAt));
            }
            else
            {
                _lightBeam.intensity = 0;
            }
        }
    }

    IEnumerator Respawn(GameObject boss)
    {
        _isRespawning = true;
        float oldLightIntensity = _lightBeam.intensity;
        
        //Blind the Boss (Deal 1 instance of Damage)
        while (_lightBeam.intensity <= 10f)
        {
            _lightBeam.intensity++;
            yield return new WaitForSeconds(0.1f);
        }
        boss.GetComponent<BossScript>().DealDamage();
        
        //Move to Respawn Point and Reset
        transform.position = respawnPoint.position;
        _lightBeamGO.GetComponent<Light2D>().intensity = oldLightIntensity;
        yield return new WaitForSeconds(2f);
        _isRespawning = false;
    }

    protected void AnimateMovement()
    {
        _animator.SetFloat("Horizontal", _directionLookAt.x);
        _animator.SetFloat("Vertical", _directionLookAt.y);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            _rigidbody.velocity = Vector2.zero;
        }
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Vector3 directionPush = (other.transform.position - transform.position).normalized;
            _rigidbody.AddForce(-directionPush);
        }
    }
}
