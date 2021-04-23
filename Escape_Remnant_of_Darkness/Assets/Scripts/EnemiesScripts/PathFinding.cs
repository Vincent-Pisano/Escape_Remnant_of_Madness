using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class PathFinding : MonoBehaviour
{

    [SerializeField] private Transform _target;
    private Rigidbody2D _rigidbody2D;
    private GuardEnemyScript _controller;
    private FieldOfView _fieldOfView;
    
    public float nextWayPointDistance = 3f;
    private Path _path;
    private int _currentWayPoint = 0;

    private Seeker _seeker;
    
    

    // Start is called before the first frame update
    void Start()
    {
        _target = null;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _seeker = GetComponent<Seeker>();
        _controller = GetComponent<GuardEnemyScript>();
        _fieldOfView = GetComponent<FieldOfView>();

        InvokeRepeating("UpdatePath", 0f, .2f);
        
    }

    private void Update()
    {
        
    }

    void UpdatePath()
    {
        if (_fieldOfView.GetTarget() != null)
        {
            if (_target == null)
                _target = _fieldOfView.GetTarget();
        }
        if (_target != null)
        {
            if (_seeker.IsDone() && _target != null)
            {
                _seeker.StartPath(_rigidbody2D.position, _target.gameObject.transform.position, OnPathComplete);
            }
        }
        if (_target != null && _fieldOfView.GetTarget() == null) 
        {
            if (_target.gameObject.GetComponent<ProtagonistScript>().IsPlayerVanquished())
            {
                _controller.ResetDirection();
                _target = null;
            }
            else
            {
                StartCoroutine("WaitForSeconds", 3F);
            }
        }
    }

    private IEnumerator WaitForSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        _controller.ResetDirection();
        _target = null;
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
            _currentWayPoint = 0;
        }
    }
    
    private void FixedUpdate()
    {
        if (_path == null)
            return;
        if (_currentWayPoint >= _path.vectorPath.Count)
        {
            return;
        }

        Vector2 direction = ((Vector2) _path.vectorPath[_currentWayPoint] - _rigidbody2D.position).normalized;
        _controller.SetDirection(direction);

        float distance = Vector2.Distance(_rigidbody2D.position, _path.vectorPath[_currentWayPoint]);

        if (distance < nextWayPointDistance)
        {
            _currentWayPoint++;
        }
    }
}
