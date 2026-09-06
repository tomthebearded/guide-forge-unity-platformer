using UnityEngine;
[RequireComponent(typeof(Collider2D))]

[RequireComponent(typeof(SpriteRenderer))]
public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Color inactiveColor = new(0.4f, 0.4f, 0.45f);
    [SerializeField] private Color activeColor = new(0.45f, 0.85f, 0.5f);

    private SpriteRenderer spriteRenderer;
    private bool alreadyReached;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = inactiveColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (alreadyReached || !other.TryGetComponent(out PlayerRespawn respawn))
            return;

        alreadyReached = true;
        spriteRenderer.color = activeColor;

        respawn.SetCheckpoint((Vector2)transform.position + Vector2.up * 0.5f);
    }
}
