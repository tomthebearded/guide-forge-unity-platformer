using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int startingLives = 3;
    [SerializeField] private float invulnerabilitySeconds = 1f;

    public int LivesRemaining { get; private set; }

    public event Action<int> LivesChanged;
    public event Action Died;
    public event Action RunEnded;

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
            RunEnded?.Invoke();
            return;
        }

        Died?.Invoke();
    }

    public void KillIgnoringInvulnerability()
    {
        invulnerableUntilTimeSeconds = float.NegativeInfinity;
        TakeDamage();
    }
}
