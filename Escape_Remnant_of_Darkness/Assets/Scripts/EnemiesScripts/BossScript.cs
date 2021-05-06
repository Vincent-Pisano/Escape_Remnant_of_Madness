using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BossScript : EnemyScript 
{
    private Animator _animatorLeftEye;
    private Animator _animatorRightEye;
    private Animator _animatorMiddleEye;
    private FieldOfView _controllerFieldOfView;

    private float _nbrHit = 0;

    // Start is called before the first frame update
    void Start()
    {
        _animatorLeftEye = transform.GetChild(0).gameObject.GetComponent<Animator>();
        _animatorRightEye = transform.GetChild(1).gameObject.GetComponent<Animator>();
        _animatorMiddleEye = transform.GetChild(2).gameObject.GetComponent<Animator>();
        _controllerFieldOfView = GetComponent<FieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimateEyes();
        DecayPlayersSanity();
    }

    private void AnimateEyes()
    {
        _animatorLeftEye.SetFloat("horizontal", _directionLookAt.x);
        _animatorRightEye.SetFloat("horizontal", _directionLookAt.x);

        if (_isTargetVisible)
        {
            SetDetection(15, true);
        }
        else
        {
            SetDetection(12, false);
        }
    }

    private void SetDetection(int viewRadius, bool isDetected)
    {
        _controllerFieldOfView.SetViewRadius(viewRadius);
        _animatorLeftEye.SetBool("isDetected", isDetected);
        _animatorRightEye.SetBool("isDetected", isDetected);
        _animatorMiddleEye.SetBool("isDetected", isDetected);
    }

    private void DecayPlayersSanity()
    {
        if (_isTargetVisible)
        {
            _controllerFieldOfView.GetTarget().gameObject.GetComponent<ProtagonistScript>().SetIsInBossFOV(true);
        }
    }

    public void DealDamage()
    {
        switch (_nbrHit)
        {
            case 0 :
                _animatorLeftEye.SetBool("isVanquished", true);
                break;
            case 1 :
                _animatorRightEye.SetBool("isVanquished", true);
                break;
            case 2 :
                _animatorMiddleEye.SetBool("isVanquished", true);
                StartCoroutine("BossDying", 0.1f);
                break;
        }
        _nbrHit++;
    }
    
    public IEnumerator BossDying(float delay)
    {
        Light2D lightIntensity = gameObject.GetComponent<Light2D>();
        print(lightIntensity);
        while (true)
        {
            if (lightIntensity.intensity <= 7f)
            {
                lightIntensity.intensity += 0.15f;
                print(lightIntensity);
            }
            else
            {
                gameObject.SetActive(false);
                break;
            }
            yield return new WaitForSeconds(delay);
        }
        

    }
}
