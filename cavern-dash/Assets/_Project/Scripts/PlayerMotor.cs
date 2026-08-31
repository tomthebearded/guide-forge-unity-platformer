using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputReader))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private float moveSpeedUnitsPerSecond = 7f;
    [SerializeField] private float groundAccelerationUnitsPerSecondSquared = 60f;
    [SerializeField] private float airAccelerationUnitsPerSecondSquared = 35f;
    [SerializeField] private float coyoteTimeSeconds = 0.10f;


    [Header("Ground check")]
    [SerializeField] private Vector2 groundCheckSizeUnits = new(0.9f, 0.12f);
    [SerializeField] private float groundCheckDistanceBelowCentreUnits = 0.5f;
    [SerializeField] private LayerMask groundLayers;

    [Header("Jump")]
    [SerializeField] private float jumpVelocityUnitsPerSecond = 14f;
    [SerializeField] private float jumpBufferSeconds = 0.12f;
    [SerializeField] private float fallGravityMultiplier = 1.8f;
    [SerializeField] private float lowJumpGravityMultiplier = 2.2f;

    public bool IsGrounded { get; private set; }

    private Rigidbody2D body;
    private PlayerInputReader input;
    private float coyoteTimeRemainingSeconds;
    private float baseGravityScale;
    private PlayerMovementState state = PlayerMovementState.Normal;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInputReader>();
        baseGravityScale = body.gravityScale;
    }

    private void FixedUpdate()
    {
        UpdateGroundedState();
        UpdateCoyoteTimer();

        switch (state)
        {
            case PlayerMovementState.Normal:
                TickNormalState();
                break;
        }
    }

    private void UpdateCoyoteTimer()
    {
        if (IsGrounded && body.linearVelocity.y <= 0f)
            coyoteTimeRemainingSeconds = coyoteTimeSeconds;
        else if (!IsGrounded)
            coyoteTimeRemainingSeconds -= Time.fixedDeltaTime;
    }

    private void TickNormalState()
    {
        float desiredHorizontalSpeed = input.HorizontalInput * moveSpeedUnitsPerSecond;
        float accelerationThisStep = IsGrounded
            ? groundAccelerationUnitsPerSecondSquared
            : airAccelerationUnitsPerSecondSquared;

        float newHorizontalSpeed = Mathf.MoveTowards(
            body.linearVelocity.x,
            desiredHorizontalSpeed,
            accelerationThisStep * Time.fixedDeltaTime);

        body.linearVelocity = new Vector2(newHorizontalSpeed, body.linearVelocity.y);

        bool jumpIsBuffered = input.TimeSinceJumpPressedSeconds < jumpBufferSeconds;

        if (jumpIsBuffered && coyoteTimeRemainingSeconds > 0f)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpVelocityUnitsPerSecond);

            coyoteTimeRemainingSeconds = 0f;
            input.ConsumeJumpRequest();
        }

        ApplyJumpGravityMultipliers();
    }

    private void UpdateGroundedState()
    {
        Vector2 boxCentre = (Vector2)transform.position + Vector2.down * groundCheckDistanceBelowCentreUnits;
        IsGrounded = Physics2D.OverlapBox(boxCentre, groundCheckSizeUnits, 0f, groundLayers) != null;
    }

    private void ApplyJumpGravityMultipliers()
    {
        if (body.linearVelocity.y < 0f)
            body.gravityScale = baseGravityScale * fallGravityMultiplier;
        else if (body.linearVelocity.y > 0f && !input.IsJumpHeld)
            body.gravityScale = baseGravityScale * lowJumpGravityMultiplier;
        else
            body.gravityScale = baseGravityScale;
    }

}
