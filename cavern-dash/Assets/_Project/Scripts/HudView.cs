using TMPro;
using UnityEngine;

public class HudView : MonoBehaviour
{
    [SerializeField] private TMP_Text coinsLabel;
    [SerializeField] private TMP_Text livesLabel;

    [SerializeField] private PlayerStats stats;
    [SerializeField] private PlayerHealth health;

    private void OnEnable()
    {
        stats.CoinsChanged += ShowCoins;
        health.LivesChanged += ShowLives;
    }

    private void Start()
    {
        ShowCoins(stats.CoinsCollected);
        ShowLives(health.LivesRemaining);
    }

    private void OnDisable()
    {
        stats.CoinsChanged -= ShowCoins;
        health.LivesChanged -= ShowLives;
    }

    private void ShowCoins(int coinsCollected) =>
        coinsLabel.text = $"Coins: {coinsCollected}";

    private void ShowLives(int livesRemaining) =>
        livesLabel.text = $"Lives: {livesRemaining}";
}
