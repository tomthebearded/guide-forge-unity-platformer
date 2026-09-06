using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerRespawn : MonoBehaviour
{
    private PlayerHealth health;
    private Rigidbody2D body;
    private Vector2 respawnPosition;

    private void Awake()
    {
        health = GetComponent<PlayerHealth>();
        body = GetComponent<Rigidbody2D>();

        respawnPosition = body.position;
    }

    private void OnEnable() =>
        health.Died += RespawnAtCheckpoint;

    private void OnDisable() =>
        health.Died -= RespawnAtCheckpoint;

    public void SetCheckpoint(Vector2 position) =>
        respawnPosition = position;

    private void RespawnAtCheckpoint()
    {
        body.linearVelocity = Vector2.zero;

        body.position = respawnPosition;

        transform.position = respawnPosition;
    }
}
