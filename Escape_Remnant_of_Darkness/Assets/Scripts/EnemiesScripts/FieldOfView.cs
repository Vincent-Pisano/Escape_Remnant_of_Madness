using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private List<Transform> visibleTargets = new List<Transform>();

    private GuardEnemyScript _controller;
    private Vector2 _direction;
    private float _directionAngle;

    private void Start()
    {
        _controller = GetComponent<GuardEnemyScript>();
        StartCoroutine("FindTargetsWithDelay", .2f);
    }

    public IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
            if (_controller.getVelocity() == new Vector2(0, 0))
            {
                _direction = transform.up;
            }
            else
            {
                _direction = _controller.getVelocity().normalized;
            }
            
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

    private float AngleFromDir(Vector2 direction)
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
        visibleTargets.Clear();
        Collider2D targetsInViewRadius = Physics2D.OverlapCircle(transform.position, viewRadius, targetMask);
        if (targetsInViewRadius != null)
        {
            Transform target = targetsInViewRadius.transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(_direction, directionToTarget) < viewAngle / 2)
            {
                CheckIfPlayerIsHidden(target, directionToTarget);
            }
            else if (Physics2D.OverlapCircleAll(transform.position, viewRadiusNear, targetMask).Contains(targetsInViewRadius))
            {
                CheckIfPlayerIsHidden(target, directionToTarget);
            } 
        }
    }

    private void CheckIfPlayerIsHidden(Transform target, Vector3 directionToTarget)
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstackeMask))
        {
            //do something like follow the player
            visibleTargets.Add(target);
        }
    }

    public float getViewAngle()
    {
        return viewAngle;
    }
    
    public float getViewRadius()
    {
        return viewRadius;
    }
    
    public float getViewRadiusNear()
    {
        return viewRadiusNear;
    }
    
    public List<Transform> getVisibleTargets()
    {
        return visibleTargets;
    }
}
