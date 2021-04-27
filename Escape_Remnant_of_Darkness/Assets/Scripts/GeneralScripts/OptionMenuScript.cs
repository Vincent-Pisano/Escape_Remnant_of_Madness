using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionMenuScript : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private List<Sprite> checkMarkToggle;
    [SerializeField] private AudioMixer audioMixer;
    
    //S'occuper de la gestion du volumne une fois implémentée
    
    // Start is called before the first frame update
    void Start()
    {
        backButton.onClick.AddListener(HandleOptionsClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setVolume(float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
    }
    
    private void HandleOptionsClicked()
    {
        GameManager.Instance.ToggleOptions();
    }
    
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        setCheckMarkFullScreen(isFullscreen);
    }

    public void setCheckMarkFullScreen(bool isFullScreen)
    {
        if(isFullScreen)
            fullScreenToggle.graphic.gameObject.GetComponent<Image>().sprite = checkMarkToggle[1];
        else
            fullScreenToggle.graphic.gameObject.GetComponent<Image>().sprite = checkMarkToggle[0];
    }
}
