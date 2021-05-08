using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuScript : MonoBehaviour
{
    private float _maxSanity;
    private float _maxLight;
    private ProtagonistScript _protagonistScript;
    [SerializeField] private GameObject textBossAttackUI;
    
    void Start()
    {
        Load();
    }
    
    void Update()
    {
        if (_protagonistScript != null)
        {
            UISanityBar.Instance.SetValue(_protagonistScript.GetSanity() / _maxSanity);
            UILightBar.Instance.SetValue(_protagonistScript.GetLightIntensity() / _maxLight);
        }
    }

    public void Load()
    {
        StartCoroutine("LoadProtagonistInfos", 0.2f);
    }

    public IEnumerator LoadProtagonistInfos(float delay)
    {
        _protagonistScript = null;
        yield return new WaitForSeconds(delay);
        GameObject protagonist = GameObject.FindWithTag("Player");
        _protagonistScript = protagonist.GetComponent<ProtagonistScript>();
        _maxSanity = _protagonistScript.GetSanity();
        _maxLight = _protagonistScript.GetLightIntensity();
        
        UISanityBar.Instance.SetValue(1);
        UILightBar.Instance.SetValue(1);
        
        //Le texte disparait si le joueur quitte le radius du boss
        StartCoroutine("ShowBossTextToPlayer", 0.5f);
    }
    
    public IEnumerator ShowBossTextToPlayer(float delay)
    {
        while (true)
        {
            textBossAttackUI.SetActive(_protagonistScript.GetIsPlayerInBossFOV());
            yield return new WaitForSeconds(delay);
        }
    }
}
