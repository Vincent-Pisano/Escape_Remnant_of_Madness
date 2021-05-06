using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.Serialization;


// Donne au GameManager le controle sur l'ordre de 'loading' des sous-systemes.
public class GameManager : Singleton<GameManager>
{
    private readonly string[] LEVELS_NAME = {"FirstLevel", "BossLevel"};

    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSE,
        GAMEOVER,
        ENDGAME,
    }

    public Events.EventGameState onGameStateChanged;
    
    public List<AsyncOperation> _loadOperations = new List<AsyncOperation>();
    public GameObject[] systemPrefabs;
    private List<GameObject> _instanceSystemPrefabs = new List<GameObject>();
    private GameState _currentGameState = GameState.PREGAME;
    
    private int _currentLevelIndex = -1;
    private bool _isOptionMenuClicked = false;

    public void Start()
    {
        DontDestroyOnLoad(this);
        InstanciateSystemPrefab();
    }
    
    public void Update() 
    {
        if (!(_currentGameState == GameState.PREGAME ||
             _currentGameState == GameState.GAMEOVER)
            && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void InstanciateSystemPrefab()
    {
        GameObject prefabInstance;
        for (int i = 0; i < systemPrefabs.Length; i++)
        {
            GameObject prefab = systemPrefabs[i];
            prefabInstance = Instantiate(prefab);
            _instanceSystemPrefabs.Add(prefabInstance);
            if (prefab.name.Equals("Protagonist"))
            {
                _instanceSystemPrefabs[i].SetActive(false);
            }
        }        
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_instanceSystemPrefabs != null)
        {
            foreach (var prefabInstance in _instanceSystemPrefabs)
            {
                Destroy(prefabInstance);
            }
            _instanceSystemPrefabs.Clear();
        }
    }

    public void LoadLevel(string levelName)
    {
        //_currentLevelIndex = levelName;
        
        AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if (loadSceneAsync == null)  // La scene existe dans le build setting
        {
            print("error loading scene : " + levelName);
            return;
        }
        loadSceneAsync.completed += OnLoadSceneComplete;
        _loadOperations.Add(loadSceneAsync);
    }

    public void UnloadLevel(string levelName)
    {
        AsyncOperation unloadSceneAsync = SceneManager.UnloadSceneAsync(levelName);
        if (unloadSceneAsync == null)
        {
            print("error unloading scene : " + levelName);
            return;
        }

        unloadSceneAsync.completed += OnUnloadSceneComplete;
    }

    private void OnLoadSceneComplete(AsyncOperation ao)
    {
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);
            // Ici on peut aviser les composantes qui ont besoin de savoir que le level est loadé
            _instanceSystemPrefabs[1].transform.position = Vector2.zero;
            if (_loadOperations.Count == 0)
            {
                UpdateGameState(GameState.RUNNING);
            }
        }
        print("load completed");
    }
    private void OnUnloadSceneComplete(AsyncOperation obj)
    {
        print("unload completed");
    }

    void UpdateGameState(GameState newGameState)
    {
        var previousGameState = _currentGameState;
        _currentGameState = newGameState;
        switch (_currentGameState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1;
                break;
            case GameState.RUNNING:
                Time.timeScale = 1;
                break;
            case GameState.PAUSE:
                Time.timeScale = 0;
                break;
            case GameState.GAMEOVER:
                Time.timeScale = 1;
                break;
            case GameState.ENDGAME:
                Time.timeScale = 1;
                break;
        }
        
        onGameStateChanged.Invoke(_currentGameState, previousGameState);
    }

    public GameState CurrentGameState
    {
        get => _currentGameState;
        private set => _currentGameState = value;

        //get { return _currentGameState; }
        //private set { _currentGameState = value; }
    }

    public void StartGame()
    {
        
        LoadLevel(LEVELS_NAME[++_currentLevelIndex]);
        _instanceSystemPrefabs[1].SetActive(true);
    }
    
    public void LoadNextLevel()
    {
        UnloadLevel(LEVELS_NAME[_currentLevelIndex]);
        _currentLevelIndex++;
        if (_currentLevelIndex == LEVELS_NAME.Length)
        {
            EndGame();
        }
        else
        {
            LoadLevel(LEVELS_NAME[_currentLevelIndex]);
        }
    }
    
    public void GameOver()
    {
        RestartGame(GameState.GAMEOVER);
    }
    
    public void EndGame()
    {
        UpdateGameState(GameState.ENDGAME);
        _instanceSystemPrefabs[1].SetActive(false);
    }

    public void TogglePause()
    {
        UpdateGameState(_currentGameState == GameState.RUNNING ? GameState.PAUSE : GameState.RUNNING);
    }

    public void RestartGame(GameState currentGameState)
    {
        UpdateGameState(currentGameState);
        UnloadLevel(LEVELS_NAME[_currentLevelIndex]);
        _instanceSystemPrefabs[1].SetActive(false);
        _currentLevelIndex = -1;
    }

    public void QuitGame()
    {
        print("Quitting game");
        Application.Quit();
    }
    
    public void ToggleOptions()
    {
        _isOptionMenuClicked = !_isOptionMenuClicked;
        _instanceSystemPrefabs[0].GetComponent<UIManager>().ToggleOptionsMenu(_isOptionMenuClicked);
    }
}
