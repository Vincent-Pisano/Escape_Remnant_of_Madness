using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuScript : MonoBehaviour
{
    private float maxSanity;
    private float maxLight;
    private ProtagonistScript _protagonistScript;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Load();
    }

    // Update is called once per frame
    void Update()
    {
        if (_protagonistScript != null)
        {
            UISanityBar.instance.SetValue(_protagonistScript.GetSanity() / maxSanity);
            UILightBar.instance.SetValue(_protagonistScript.GetLightIntensity() / maxLight);
        }
        
    }

    public void Load()
    {
        StartCoroutine("LoadProtagonistInfos", 0.2f);
    }

    public IEnumerator LoadProtagonistInfos(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject protagonist = GameObject.FindWithTag("Player");
        _protagonistScript = protagonist.GetComponent<ProtagonistScript>();
        maxSanity = _protagonistScript.GetSanity();
        maxLight = _protagonistScript.GetLightIntensity();
    }
}
