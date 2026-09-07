using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private PlayerHealth health;

    private InputAction pauseAction;
    private InputActionMap playerMap;

    private void Awake()
    {
        pauseAction = InputSystem.actions.FindAction("UI/Pause");
        playerMap = InputSystem.actions.FindActionMap("Player");
    }

    private void OnEnable() =>
        health.RunEnded += ShowGameOver;

    private void OnDisable()
    {
        health.RunEnded -= ShowGameOver;

        Time.timeScale = 1f;
        pauseAction.Enable();
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        playerMap.Enable();
        pauseAction.Enable();
        SceneManager.LoadScene(SceneRouter.MenuScene);
    }

    private void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        playerMap.Disable();
        pauseAction.Disable();
    }
}
