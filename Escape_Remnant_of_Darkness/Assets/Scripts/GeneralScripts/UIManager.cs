using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameMenu;
    [SerializeField] private GameObject optionMenu;
    [SerializeField] private GameObject dummyCamera;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject endMenu;

    public void Start()
    {
        print("Instance (UIManager) " + GameManager.Instance.GetInstanceID());
        GameManager.Instance.onGameStateChanged.AddListener(HandleGameStateChanged);
    }

    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        mainMenu.gameObject.SetActive(currentState == GameManager.GameState.PREGAME);
        dummyCamera.gameObject.SetActive(currentState == GameManager.GameState.PREGAME  || currentState == GameManager.GameState.GAMEOVER || currentState == GameManager.GameState.ENDGAME);
        pauseMenu.SetActive(currentState == GameManager.GameState.PAUSE);
        gameMenu.SetActive(currentState == GameManager.GameState.PAUSE || currentState == GameManager.GameState.RUNNING);
        gameOverMenu.SetActive(currentState == GameManager.GameState.GAMEOVER);
        endMenu.SetActive(currentState == GameManager.GameState.ENDGAME);
        
        if ((previousState == GameManager.GameState.PREGAME || 
             previousState == GameManager.GameState.GAMEOVER || 
             previousState == GameManager.GameState.ENDGAME) &&
            currentState == GameManager.GameState.RUNNING)
        {
            gameMenu.GetComponent<GameMenuScript>().Load();
        }
    }

    public void ToggleOptionsMenu(bool isShowing)
    {
        optionMenu.SetActive(isShowing);
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
