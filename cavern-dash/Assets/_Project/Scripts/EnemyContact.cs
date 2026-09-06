using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyContact : MonoBehaviour
{
    [SerializeField] private float stompBounceVelocityUnitsPerSecond = 10f;

    // How far below this enemy's head the player's feet may be and still stomp.
    [SerializeField] private float stompToleranceUnits = 0.1f;

    private Collider2D ownCollider;

    private void Awake() =>
        ownCollider = GetComponent<Collider2D>();

    private void OnTriggerEnter2D(Collider2D other) => HandleContact(other);
    private void OnTriggerStay2D(Collider2D other) => HandleContact(other);

    private void HandleContact(Collider2D other)
    {
        if (!other.TryGetComponent(out PlayerHealth health))
            return;

        Rigidbody2D playerBody = other.attachedRigidbody;

        bool comingDownOnTop =
            playerBody != null &&
            playerBody.linearVelocity.y < 0f &&
            other.bounds.min.y >= ownCollider.bounds.max.y - stompToleranceUnits;

        if (comingDownOnTop)
        {
            playerBody.linearVelocity = new Vector2(playerBody.linearVelocity.x, stompBounceVelocityUnitsPerSecond);
            Destroy(gameObject);
            return;
        }

        health.TakeDamage();
    }
}
