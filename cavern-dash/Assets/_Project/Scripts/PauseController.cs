using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private InputAction pauseAction;
    private InputActionMap playerMap;

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        pauseAction = InputSystem.actions.FindAction("UI/Pause");
        playerMap = InputSystem.actions.FindActionMap("Player");
    }

    private void Update()
    {
        if (pauseAction.WasPressedThisFrame())
            SetPaused(!IsPaused);
    }

    public void Resume() =>
        SetPaused(false);

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        playerMap.Enable();
        SceneManager.LoadScene(SceneRouter.MenuScene);
    }

    private void SetPaused(bool paused)
    {
        IsPaused = paused;
        pausePanel.SetActive(paused);

        Time.timeScale = paused ? 0f : 1f;

        if (paused)
            playerMap.Disable();
        else
            playerMap.Enable();
    }

    private void OnDisable() =>
        Time.timeScale = 1f;
}
