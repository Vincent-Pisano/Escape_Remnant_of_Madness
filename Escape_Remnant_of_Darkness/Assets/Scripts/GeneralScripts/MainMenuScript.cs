using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        quitButton.onClick.AddListener(HandleQuitClicked);
        optionsButton.onClick.AddListener(HandleOptionsClicked);
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
