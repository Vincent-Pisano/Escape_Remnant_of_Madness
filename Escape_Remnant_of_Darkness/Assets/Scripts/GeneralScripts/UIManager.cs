using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _gameMenu;
    [SerializeField] private GameObject _dummyCamera;
    [SerializeField] private GameObject _pauseMenu;

    public void Start()
    {
        print("Instance (UIManager) " + GameManager.Instance.GetInstanceID());
        GameManager.Instance.onGameStateChanged.AddListener(HandleGameStateChanged);
    }

    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        _mainMenu.gameObject.SetActive(currentState == GameManager.GameState.PREGAME);
        _dummyCamera.gameObject.SetActive(currentState == GameManager.GameState.PREGAME);
        _pauseMenu.SetActive(currentState == GameManager.GameState.PAUSE);
        _gameMenu.SetActive(currentState != GameManager.GameState.PREGAME);
        
    }

    public void Update()
    {
         if (GameManager.Instance.CurrentGameState == GameManager.GameState.PREGAME)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.Instance.StartGame();
            }
        }
    }
    
}
