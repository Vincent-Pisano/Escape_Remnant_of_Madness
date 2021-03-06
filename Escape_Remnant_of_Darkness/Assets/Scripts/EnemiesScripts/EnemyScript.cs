using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;
    protected Vector2 _directionLookAt;
    protected bool _isTargetVisible;
    protected Animator _animator;
    [SerializeField] private Vector2 directionLookAtRest = Vector2.down;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _isTargetVisible = false;
    }

    public Vector2 GetDirection()
    {
        return _directionLookAt;
    }
    
    public void SetDirection(Vector2 directionLookAt)
    {
        _isTargetVisible = true;
        _directionLookAt = directionLookAt;
    }
    
    public void ResetDirection()
    {
        _isTargetVisible = false;
        _directionLookAt = directionLookAtRest;
    }
}
