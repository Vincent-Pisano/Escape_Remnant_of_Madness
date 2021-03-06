using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    [SerializeField] private float viewRadius;
    [SerializeField] private float viewRadiusNear;
    [Range(0,360)]
    [SerializeField] private float viewAngle;

    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstackeMask;

    [SerializeField] private Transform target;

    private EnemyScript _controller;
    private Vector2 _direction;
    private float _directionAngle;

    private void Start()
    {
        _controller = GetComponent<EnemyScript>();
        StartCoroutine("FindTargetsWithDelay", .2f);
    }

    public IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
            _direction = _controller.GetDirection().normalized;

            _directionAngle = AngleFromDir(_direction);
        }
    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }
        return Quaternion.Euler(0f,0f, _directionAngle) * new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 
                            Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    public float AngleFromDir(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.x, direction.y) * 180 / Mathf.PI;
        if (angle < 0)
        {
            angle = 360 + angle;
        }
        return -angle;
    }

    public void FindVisibleTargets()
    {
        this.target = null;
        _controller.ResetDirection();
        Collider2D targetsInViewRadius = Physics2D.OverlapCircle(transform.position, viewRadius, targetMask);
        if (targetsInViewRadius != null)
        {
            Transform target = targetsInViewRadius.transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(_direction, directionToTarget) < viewAngle / 2)
            {
                CheckIfTargetIsHidden(target, directionToTarget);
            }
            else if (Physics2D.OverlapCircleAll(transform.position, viewRadiusNear, targetMask).Contains(targetsInViewRadius))
            {
                CheckIfTargetIsHidden(target, directionToTarget);
            } 
        }
    }

    private void CheckIfTargetIsHidden(Transform target, Vector3 directionToTarget)
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        

        if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstackeMask))
        {
            if (target.gameObject.tag.Equals("Player") && target.gameObject.GetComponent<Animator>().GetBool("Vanquished"))
            {
                this.target = null;
            }
            else
            {
                this.target = target;
                _controller.SetDirection(directionToTarget);
            }
        }
    }

    public float GetViewAngle()
    {
        return viewAngle;
    }
    
    public float GetViewRadius()
    {
        return viewRadius;
    }

    public void SetViewRadius(float radius)
    {
        viewRadius = radius;
    }
    
    public float GetViewRadiusNear()
    {
        return viewRadiusNear;
    }
    
    public Transform GetTarget()
    {
        return target;
    }
}
