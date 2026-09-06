// Assets/_Project/Scripts/PlayerHealth.cs — the whole file
using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int startingLives = 3;
    [SerializeField] private float invulnerabilitySeconds = 1f;

    public int LivesRemaining { get; private set; }

    public event Action<int> LivesChanged;

    public event Action Died;

    private float invulnerableUntilTimeSeconds = float.NegativeInfinity;

    public bool IsInvulnerable => Time.time < invulnerableUntilTimeSeconds;

    private void Awake() =>
        LivesRemaining = startingLives;

    public void TakeDamage()
    {
        if (IsInvulnerable)
            return;

        LivesRemaining--;
        invulnerableUntilTimeSeconds = Time.time + invulnerabilitySeconds;
        LivesChanged?.Invoke(LivesRemaining);

        if (LivesRemaining <= 0)
        {
            Debug.Log("run over — lives reset");
            LivesRemaining = startingLives;
            LivesChanged?.Invoke(LivesRemaining);
        }
        else
            Debug.Log($"lives = {LivesRemaining}");

        Died?.Invoke();
    }
}
