using UnityEngine;

// Anything that falls out of the level lands here. Costs a life regardless of
// invulnerability: falling off the world is never survivable.
[RequireComponent(typeof(Collider2D))]
public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerHealth health))
            health.KillIgnoringInvulnerability();
    }
}
