using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private Button OptionsButton;
    [SerializeField] private Button QuitButton;

    private void Start()
    {
        QuitButton.onClick.AddListener(HandleQuitClicked);
        OptionsButton.onClick.AddListener(HandleOptionsClicked);
    }

    private void HandleQuitClicked()
    {
        GameManager.Instance.QuitGame();
    }
   
    private void HandleOptionsClicked()
    {
        GameManager.Instance.ToggleOptions();
    }
}
