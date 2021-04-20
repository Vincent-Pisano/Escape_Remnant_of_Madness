using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuScript : MonoBehaviour
{

    [SerializeField] private Slider sanityBar;

    private ProtagonistScript _protagonistScript;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject protagonist = GameObject.FindWithTag("Player");
        _protagonistScript = protagonist.GetComponent<ProtagonistScript>();
        sanityBar.maxValue = _protagonistScript.getSanity();
    }

    // Update is called once per frame
    void Update()
    {
        sanityBar.value = _protagonistScript.getSanity();
    }
}
