using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
   [SerializeField] private Button resumeButton;
   [SerializeField] private Button restartButton;
   [SerializeField] private Button optionsButton;
   [SerializeField] private Button quitButton;

   private void Start()
   {
      resumeButton.onClick.AddListener(HandleResumeClicked);
      restartButton.onClick.AddListener(HandleRestartClicked);
      quitButton.onClick.AddListener(HandleQuitClicked);
      optionsButton.onClick.AddListener(HandleOptionsClicked);
   }

   private void HandleResumeClicked()
   {
      GameManager.Instance.TogglePause();
   }

   private void HandleRestartClicked()
   {
      GameManager.Instance.RestartGame(GameManager.GameState.PREGAME);
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
