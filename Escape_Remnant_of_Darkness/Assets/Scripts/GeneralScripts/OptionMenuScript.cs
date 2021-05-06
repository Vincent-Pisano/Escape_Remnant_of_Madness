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
    
    void Start()
    {
        backButton.onClick.AddListener(HandleOptionsClicked);
    }

    public void SetVolume(float volume)
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
        SetCheckMarkFullScreen(isFullscreen);
    }

    public void SetCheckMarkFullScreen(bool isFullScreen)
    {
        if(isFullScreen)
            fullScreenToggle.graphic.gameObject.GetComponent<Image>().sprite = checkMarkToggle[1];
        else
            fullScreenToggle.graphic.gameObject.GetComponent<Image>().sprite = checkMarkToggle[0];
    }
}
