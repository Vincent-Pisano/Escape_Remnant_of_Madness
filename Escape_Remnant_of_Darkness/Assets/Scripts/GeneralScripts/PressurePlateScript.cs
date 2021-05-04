using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateScript : MonoBehaviour
{
    [SerializeField] private GameObject objectToDeactivate;
    [SerializeField] private GameObject objectToActivate;
    private AudioSource _audioSource;
    private bool _isActivated;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _isActivated = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isActivated)
        {
            _audioSource.Play();
            _isActivated = true;
            objectToDeactivate.SetActive(false);
            objectToActivate.SetActive(true);
        }
        
    }
}
