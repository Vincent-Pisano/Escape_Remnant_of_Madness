using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ProtagonistScript : MonoBehaviour
{
    private readonly float MIN_LIGHT_INTENSITY = 0.2f;
    private float MAX_SANITY;
    private float MAX_LIGHT_INTENSITY;

    [SerializeField][Range(0,5)] private float moveSpeed = 2.4f;
    [SerializeField] [Range(0, 300)] private float lightDurationInSeconds = 20f; 
    [SerializeField] [Range(0, 2)] private float lightFallOff = 0.2f; 
    [SerializeField] [Range(0, 10)] private float viewRadius = 0.2f; 
    
    //Movements
    private Rigidbody2D _rigidbody;
    private Vector2 _velocity;
    private Vector2 _directionLookAt;
    private Animator _animator;
    
    //Lights
    private GameObject _pointLight;
    private Light2D _light;
    private float _intensityLoss;
    private LayerMask _targetMask;

    //Madness
    [SerializeField] [Range(0, 400)] private float sanity = 300f;
    private bool _isPlayerInSafeZone;
    private bool _isPlayerVanquished;
    public bool _isPlayerInBossFOV;
    [SerializeField] [Range(0, 0.5f)] private float sanityDecay = 0.15f;
    
    //SpeedBoosting
    private float memSpeed;
    [SerializeField] [Range(0, 5)] private float bonusSpeed = 2.5f;
    [SerializeField][Range(0,5)] private float speedDuration = 1.5f;
    private bool boosting;
    
    //Dashing
    [SerializeField][Range(5,20)] private float _dashSpeed = 20f;
    [SerializeField][Range(0,1)] private float _dashTime = 0.1f;
    [SerializeField] [Range(0, 3f)] private float cooldown = 2f;
    private float _initialCooldown;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        //Lights
        _pointLight = transform.GetChild(0).gameObject;
        _light = _pointLight.GetComponent<Light2D>();
        MAX_LIGHT_INTENSITY = _light.intensity;
        _intensityLoss = (_light.intensity - MIN_LIGHT_INTENSITY) / lightDurationInSeconds * lightFallOff;
        _targetMask = LayerMask.GetMask("LightSource");
        
        MAX_SANITY = sanity;
        memSpeed = moveSpeed;
        _isPlayerVanquished = false;
        
        _initialCooldown = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isPlayerVanquished)
        {
            if (sanity > 0)
            {
                _velocity.x = Input.GetAxis("Horizontal");
                _velocity.y = Input.GetAxis("Vertical");
                _velocity.Normalize();
                
                AnimateMovement();
                
                PlayerDashing();

                CheckAfterDamageBoost();
            }
            else
            {
                _velocity.x = 0;
                _velocity.y = 0;
                _velocity.Normalize();
                _isPlayerVanquished = true;
                _animator.SetBool("Vanquished", _isPlayerVanquished);
            }
        }
        else
        {
            StartCoroutine("GameOver");
        }
    }
    
    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _velocity * (moveSpeed * Time.fixedDeltaTime));
    }

    private void PlayerDashing()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        if (Input.GetKeyDown("space") && cooldown <= 0)
        {
            StartCoroutine(DashTime());
            cooldown = _initialCooldown;
        }
    }

    void OnEnable()
    {
        _isPlayerVanquished = false;
        StartCoroutine("ManageLight", lightFallOff);
        StartCoroutine("FindLightSources", .2f);
    }

    private void CheckAfterDamageBoost()
    {
        if (boosting)
        {
            speedDuration -= Time.deltaTime;
            if (speedDuration <= 0)
            {
                moveSpeed = memSpeed;
                speedDuration = 1.5f;
                boosting = false;
            }
            else
            {
                moveSpeed = memSpeed + bonusSpeed;
            }
        }
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

    public IEnumerator FindLightSources(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, _targetMask);
            if (targetsInViewRadius.Length > 0)
            {
                _isPlayerInSafeZone = true;
            }
            else
                _isPlayerInSafeZone = false;
        }
    }

    public IEnumerator ManageLight(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (_isPlayerInSafeZone)
            {
                if (_light.intensity < MAX_LIGHT_INTENSITY)
                    _light.intensity += _intensityLoss;
                if (sanity < MAX_SANITY)
                    sanity++;
            }
            else
            {
                _light.intensity -= _intensityLoss;
                if (_light.intensity < MIN_LIGHT_INTENSITY)
                    _light.intensity = MIN_LIGHT_INTENSITY;
                ReduceSanity();
            }
        }
    }

    private void ReduceSanity()
    {
        if (sanity >= 0)
        {
            if (_light.intensity > 0.48f)
            {
                sanity -= 0.125f;
            }

            if (_light.intensity <= 0.48f && _light.intensity > 0.20f)
            {
                sanity -= 0.25f;
            }

            if (_light.intensity >= 0.2f)
            {
                sanity -= 0.625f;
            }
            
            if (_isPlayerInBossFOV)
            {
                sanity -= sanityDecay;
                _isPlayerInBossFOV = false;
            }
        }
    }

    IEnumerator DashTime()
    {
        float startTime = Time.time;
        
        while (Time.time < startTime + _dashTime)
        {
            moveSpeed = _dashSpeed;
            yield return null;
        }
        
        moveSpeed = memSpeed;
    }
    
    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3f);
        sanity = MAX_SANITY;
        _light.intensity = MAX_LIGHT_INTENSITY;
        GameManager.Instance.GameOver();
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            sanity -= 40;
            boosting = true;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Finish"))
        {
            print("END LEVEL");
            GameManager.Instance.LoadNextLevel();
        }
    }
    

    public float GetSanity()
    {
        return sanity;
    }

    public void SetSanity(float sanity)
    {
        this.sanity = sanity;
    }
    
    public float GetLightIntensity()
    {
        return _light.intensity - MIN_LIGHT_INTENSITY;
    }
    
    public float GetViewRadius()
    {
        return viewRadius;
    }
    
    public bool GetIsPlayerInBossFOV()
    {
        return _isPlayerInBossFOV;
    }

    public void SetIsInBossFOV(bool isPlayerInBossFOV)
    {
        _isPlayerInBossFOV = isPlayerInBossFOV;
    }
}
