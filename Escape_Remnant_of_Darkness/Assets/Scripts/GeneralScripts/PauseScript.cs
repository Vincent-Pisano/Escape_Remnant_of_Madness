using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
   [SerializeField] private Button ResumeButton;
   [SerializeField] private Button RestartButton;
   [SerializeField] private Button OptionsButton;
   [SerializeField] private Button QuitButton;

   private void Start()
   {
      ResumeButton.onClick.AddListener(HandleResumeClicked);
      RestartButton.onClick.AddListener(HandleRestartClicked);
      QuitButton.onClick.AddListener(HandleQuitClicked);
      OptionsButton.onClick.AddListener(HandleOptionsClicked);
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
