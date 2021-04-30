using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _gameMenu;
    [SerializeField] private GameObject _optionMenu;
    [SerializeField] private GameObject _dummyCamera;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _gameOverMenu;
    [SerializeField] private GameObject _endMenu;

    public void Start()
    {
        print("Instance (UIManager) " + GameManager.Instance.GetInstanceID());
        GameManager.Instance.onGameStateChanged.AddListener(HandleGameStateChanged);
    }

    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        _mainMenu.gameObject.SetActive(currentState == GameManager.GameState.PREGAME);
        _dummyCamera.gameObject.SetActive(currentState == GameManager.GameState.PREGAME  || currentState == GameManager.GameState.GAMEOVER || currentState == GameManager.GameState.ENDGAME);
        _pauseMenu.SetActive(currentState == GameManager.GameState.PAUSE);
        _gameMenu.SetActive(currentState == GameManager.GameState.PAUSE || currentState == GameManager.GameState.RUNNING);
        _gameOverMenu.SetActive(currentState == GameManager.GameState.GAMEOVER);
        _endMenu.SetActive(currentState == GameManager.GameState.ENDGAME);
        
        if ((previousState == GameManager.GameState.PREGAME || previousState == GameManager.GameState.GAMEOVER || previousState == GameManager.GameState.ENDGAME) &&
            currentState == GameManager.GameState.RUNNING)
        {
            _gameMenu.GetComponent<GameMenuScript>().Load();
        }
    }

    public void ToggleOptionsMenu(bool isShowing)
    {
        _optionMenu.SetActive(isShowing);
    }

    public void Update()
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.PREGAME || 
            GameManager.Instance.CurrentGameState == GameManager.GameState.GAMEOVER)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.Instance.StartGame();
            }
        }
    }
}
