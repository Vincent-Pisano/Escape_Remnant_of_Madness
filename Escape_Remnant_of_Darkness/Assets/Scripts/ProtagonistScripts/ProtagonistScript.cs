using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ProtagonistScript : MonoBehaviour
{
    //Movements
    [SerializeField][Range(0,5)] private float moveSpeed = 2.4f;
    private Rigidbody2D _rigidbody;
    private Vector2 _velocity;
    private Vector2 _directionLookAt;
    private Animator _animator;
    
    //Lights
    [SerializeField] [Range(0, 300)] private float lightDurationInSeconds = 40f; 
    [SerializeField] [Range(0, 2)] private float lightFallOff = 0.2f; 
    [SerializeField] [Range(0, 10)] private float lightDetectionRadius = 2f; 
    private float _maxLightIntensity;
    private readonly float _minLightIntensity = 0.2f;
    private GameObject _pointLight;
    private Light2D _light;
    private float _intensityLoss;
    private LayerMask _targetMask;
    
    //Madness
    private float _maxSanity;
    [SerializeField] [Range(0, 400)] private float sanity = 300f;
    [SerializeField] [Range(0, 0.5f)] private float sanityDecayInBossFOV = 0.15f;
    private bool _isPlayerInSafeZone;
    private bool _isPlayerVanquished;
    private bool _isPlayerInBossFOV;
    
    //SpeedBoosting (after collision with Enemy)
    [SerializeField] [Range(0, 5)] private float bonusSpeed = 2.5f;
    [SerializeField][Range(0,5)] private float bonusSpeedDuration = 1.5f;
    private float _memorySpeed;
    private bool _isBoosting;
    
    //Dashing
    [SerializeField][Range(5,20)] private float dashSpeed = 20f;
    [SerializeField][Range(0,1)] private float dashTime = 0.1f;
    [SerializeField] [Range(0, 3f)] private float dashCooldown = 2f;
    private float _initialCooldown;
    
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _maxSanity = sanity;
        
        _memorySpeed = moveSpeed;
        _initialCooldown = dashCooldown;
        
        //Lights
        _pointLight = transform.GetChild(0).gameObject;
        _light = _pointLight.GetComponent<Light2D>();
        _maxLightIntensity = _light.intensity;
        _intensityLoss = (_light.intensity - _minLightIntensity) / lightDurationInSeconds * lightFallOff;
        _targetMask = LayerMask.GetMask("LightSource");
    }
    
    void OnEnable()
    {
        _isPlayerVanquished = false;
        if (_maxLightIntensity != 0f && _maxSanity != 0f)
        {
            _light.intensity = _maxLightIntensity;
            sanity = _maxSanity;
        }

        StartCoroutine("ManageLight", lightFallOff);
        StartCoroutine("FindLightSources", .2f);
    }
    
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
        if (dashCooldown > 0)
        {
            dashCooldown -= Time.deltaTime;
        }
        if (Input.GetKeyDown("space") && dashCooldown <= 0)
        {
            StartCoroutine(DashTime());
            dashCooldown = _initialCooldown;
        }
    }

    private void CheckAfterDamageBoost()
    {
        if (_isBoosting)
        {
            bonusSpeedDuration -= Time.deltaTime;
            if (bonusSpeedDuration <= 0)
            {
                moveSpeed = _memorySpeed;
                bonusSpeedDuration = 1.5f;
                _isBoosting = false;
            }
            else
            {
                moveSpeed = _memorySpeed + bonusSpeed;
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
        while(true)
        {
            yield return new WaitForSeconds(delay);
            Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, lightDetectionRadius, _targetMask);
            _isPlayerInSafeZone = targetsInViewRadius.Length > 0;
        }
    }

    public IEnumerator ManageLight(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (_isPlayerInSafeZone)
            {
                if (_light.intensity < _maxLightIntensity)
                    _light.intensity += _intensityLoss * 2;
                if (_light.intensity > _maxLightIntensity)
                    _light.intensity = _maxLightIntensity;
                
                if (sanity < _maxSanity)
                    sanity += 2;
                if (sanity > _maxSanity)
                    sanity = _maxSanity;
                
            }
            else
            {
                _light.intensity -= _intensityLoss;
                if (_light.intensity < _minLightIntensity)
                    _light.intensity = _minLightIntensity;
                ReduceSanity();
            }
        }
    }

    private void ReduceSanity()
    {
        if (sanity >= 0)
        {
            PassiveSanityDecay();

            BossAttackSanityDecay();
        }
    }

    private void BossAttackSanityDecay()
    {
        if (_isPlayerInBossFOV)
        {
            sanity -= sanityDecayInBossFOV;
            _isPlayerInBossFOV = false;
        }
    }

    private void PassiveSanityDecay()
    {
        //Moins que la lumiere, plus que la sanity descend
        if (_light.intensity > 0.48f)
        {
            sanity -= 0.75f;
        }
        else if (_light.intensity > 0.20f)
        {
            sanity -= 1f;
        }
        else if (_light.intensity >= 0.2f)
        {
            sanity -= 1.5f;
        }
    }

    IEnumerator DashTime()
    {
        float startTime = Time.time;
        
        while (Time.time < startTime + dashTime)
        {
            moveSpeed = dashSpeed;
            yield return null;
        }
        
        moveSpeed = _memorySpeed;
    }
    
    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3f);
        GameManager.Instance.GameOver();
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Enemy"))
        {
            sanity -= 40;
            _isBoosting = true;
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

    public float GetLightIntensity()
    {
        return _light.intensity - _minLightIntensity;
    }
    
    public float GetViewRadius()
    {
        return lightDetectionRadius;
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
