using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class LevelExit : MonoBehaviour
{
    [SerializeField] private string nextSceneName = SceneRouter.SecondLevelScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out PlayerStats stats))
            return;

        GameSession.Instance.AddCoins(stats.CoinsCollected);
        SceneManager.LoadScene(nextSceneName);
    }
}
