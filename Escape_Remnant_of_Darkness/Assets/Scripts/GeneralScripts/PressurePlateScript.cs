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
    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _isActivated = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.tag.Equals("Player") || _isActivated) return;
        
        _isActivated = true;
        if (_audioSource != null)
            _audioSource.Play();
        
        if (objectToDeactivate != null)
            objectToDeactivate.SetActive(false);
        
        if (objectToActivate != null)
            objectToActivate.SetActive(true);
    }
}
