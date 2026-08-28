using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputReader))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private float moveSpeedUnitsPerSecond = 7f;
    [SerializeField] private float groundAccelerationUnitsPerSecondSquared = 60f;
    [SerializeField] private float airAccelerationUnitsPerSecondSquared = 35f;

    [Header("Ground check")]
    [SerializeField] private Vector2 groundCheckSizeUnits = new(0.9f, 0.12f);
    [SerializeField] private float groundCheckDistanceBelowCentreUnits = 0.5f;
    [SerializeField] private LayerMask groundLayers;

    [Header("Jump")]
    [SerializeField] private float jumpVelocityUnitsPerSecond = 14f;

    public bool IsGrounded { get; private set; }

    private Rigidbody2D body;
    private PlayerInputReader input;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInputReader>();
    }

    void FixedUpdate()
    {
        UpdateGroundedState();

        float desiredHorizontalSpeed = input.HorizontalInput * moveSpeedUnitsPerSecond;
        float accelerationThisStep = IsGrounded
            ? groundAccelerationUnitsPerSecondSquared
            : airAccelerationUnitsPerSecondSquared;

        float newHorizontalSpeed = Mathf.MoveTowards(
            body.linearVelocity.x,
            desiredHorizontalSpeed,
            accelerationThisStep * Time.fixedDeltaTime);

        body.linearVelocity = new Vector2(newHorizontalSpeed, body.linearVelocity.y);

        if (input.JumpRequested && IsGrounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpVelocityUnitsPerSecond);
        }

        input.ConsumeJumpRequest();
    }

    private void UpdateGroundedState()
    {
        Vector2 boxCentre = (Vector2)transform.position + Vector2.down * groundCheckDistanceBelowCentreUnits;
        IsGrounded = Physics2D.OverlapBox(boxCentre, groundCheckSizeUnits, 0f, groundLayers) != null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 boxCentre = transform.position + Vector3.down * groundCheckDistanceBelowCentreUnits;
        Gizmos.DrawWireCube(boxCentre, new Vector3(groundCheckSizeUnits.x, groundCheckSizeUnits.y, 0f));
    }
}
