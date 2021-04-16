using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class protagonistScript : MonoBehaviour
{
    private float MIN_LIGHT_INTENSITY = 0.2f;
    
    [SerializeField][Range(0,5)] private float moveSpeed = 2.4f;
    [SerializeField] [Range(0, 300)] private float lightDurationInSeconds = 20f; 
    [SerializeField] [Range(0, 2)] private float lightFallOff = 0.2f; 
    
    //Movements
    private Rigidbody2D _rigidbody;
    private Vector2 _velocity;
    private Vector2 _directionLookAt;
    private Animator _animator;
    
    //Lights
    private GameObject _pointLight;
    private Light2D _light;
    private float _timer;
    private float _intensityPerSeconds;
    private float _delay;
    
    //Madness
    private float _sanity;

    
    // Start is called before the first frame update
    void Start()
    {
        //Movements
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        //Lights
        _pointLight = transform.GetChild(0).gameObject;
        _light = _pointLight.GetComponent<Light2D>();
        _intensityPerSeconds = (_light.intensity - MIN_LIGHT_INTENSITY) / lightDurationInSeconds * lightFallOff;
        StartCoroutine("Light", lightFallOff);

        //Madness
        _sanity = 175;
    }

    // Update is called once per frame
    void Update()
    {
        _velocity.x = Input.GetAxis("Horizontal");
        _velocity.y = Input.GetAxis("Vertical");
        _velocity.Normalize();

        AnimateMovement();
    }

    private void AnimateMovement()
    {
        _animator.SetFloat("Horizontal", _velocity.x);
        _animator.SetFloat("Vertical", _velocity.y);
        _animator.SetFloat("Speed", _velocity.sqrMagnitude);

        if (_velocity.sqrMagnitude != 0)
        {
            _directionLookAt = new Vector2(_velocity.x, _velocity.y);
        }
        else
        {
            _animator.SetFloat("HorizontalIdle", _directionLookAt.x);
            _animator.SetFloat("VerticalIdle", _directionLookAt.y);
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _velocity * (moveSpeed * Time.fixedDeltaTime));
    }
    
    public IEnumerator Light(float delay)
    {
        while (_timer < lightDurationInSeconds)
        {
            yield return new WaitForSeconds(delay);
            _light.intensity -= _intensityPerSeconds;
            _timer += delay;
            SetSanity();
        }
        print(_timer + " seconds passed : " + _light.intensity);
    }

    private void SetSanity()
    {
        //Lower Sanity avec : lightDurationInSeconds - _timer
        //temporairement :
        _sanity--;
    }
}
