using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputReader))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private float moveSpeedUnitsPerSeconds = 7f;

    private Rigidbody2D body;
    private PlayerInputReader input;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInputReader>();
    }

    void FixedUpdate()
    {
        float desiredHorizontalSpeed = input.HorizontalInput * moveSpeedUnitsPerSeconds;
        body.linearVelocity = new Vector2(desiredHorizontalSpeed, body.linearVelocity.y);
    }
}
