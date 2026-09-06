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

    [Header("Dash")]
    [SerializeField] private float dashDistanceUnits = 5f;
    [SerializeField] private float dashDurationSeconds = 0.15f;
    [SerializeField] private float dashCooldownSeconds = 0.60f;

    [Header("Wall")]
    [SerializeField] private Vector2 wallCheckSizeUnits = new(0.12f, 0.8f);
    [SerializeField] private float wallCheckDistanceFromCentreUnits = 0.5f;
    [SerializeField] private LayerMask wallLayers;
    [SerializeField] private float wallSlideSpeedUnitsPerSecond = 2.5f;
    [SerializeField] private Vector2 wallJumpVelocity = new Vector2(9f, 13f);
    [SerializeField] private float wallJumpControlLockSeconds = 0.15f;

    public int WallDirection { get; private set; }
    public bool IsGrounded { get; private set; }

    private Rigidbody2D body;
    private PlayerInputReader input;
    private float coyoteTimeRemainingSeconds;
    private float baseGravityScale;
    private PlayerMovementState state = PlayerMovementState.Normal;
    private float dashEndTimeSeconds;
    private float nextDashAllowedTimeSeconds;
    private int facingDirection = 1;
    private float horizontalControlLockedUntilTimeSeconds;

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
        UpdateWallContact();

        switch (state)
        {
            case PlayerMovementState.Normal:
                TickNormalState();
                break;
            case PlayerMovementState.Dashing:
                TickDashingState();
                break;
            case PlayerMovementState.WallSliding:
                TickWallSlidingState();
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
        if (!Mathf.Approximately(input.HorizontalInput, 0f))
            facingDirection = input.HorizontalInput > 0f ? 1 : -1;

        bool horizontalControlLocked = Time.time < horizontalControlLockedUntilTimeSeconds;

        if (!horizontalControlLocked)
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
        }


        if (input.DashRequested && Time.time >= nextDashAllowedTimeSeconds)
        {
            StartDash();
            return;
        }

        input.ConsumeDashRequest();

        bool jumpIsBuffered = input.TimeSinceJumpPressedSeconds < jumpBufferSeconds;

        if (jumpIsBuffered && coyoteTimeRemainingSeconds > 0f)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpVelocityUnitsPerSecond);

            coyoteTimeRemainingSeconds = 0f;
            input.ConsumeJumpRequest();
        }

        ApplyJumpGravityMultipliers();

        if (!IsGrounded && !horizontalControlLocked && IsPressingIntoWall() && body.linearVelocity.y < 0f)
            state = PlayerMovementState.WallSliding;

    }

    private void StartDash()
    {
        state = PlayerMovementState.Dashing;
        dashEndTimeSeconds = Time.time + dashDurationSeconds;
        nextDashAllowedTimeSeconds = Time.time + dashCooldownSeconds;
        input.ConsumeDashRequest();

        body.gravityScale = 0f;

        float dashSpeedUnitsPerSecond = dashDistanceUnits / dashDurationSeconds;
        body.linearVelocity = new Vector2(facingDirection * dashSpeedUnitsPerSecond, 0f);
    }

    private void TickDashingState()
    {
        if (Time.time < dashEndTimeSeconds)
            return;

        state = PlayerMovementState.Normal;
        body.gravityScale = baseGravityScale;

        float exitSpeed = Mathf.Clamp(body.linearVelocity.x, -moveSpeedUnitsPerSecond, moveSpeedUnitsPerSecond);
        body.linearVelocity = new Vector2(exitSpeed, body.linearVelocity.y);
    }

    private void TickWallSlidingState()
    {
        if (IsGrounded || !IsPressingIntoWall())
        {
            state = PlayerMovementState.Normal;
            body.gravityScale = baseGravityScale;
            return;
        }

        float clampedFallSpeed = Mathf.Max(body.linearVelocity.y, -wallSlideSpeedUnitsPerSecond);
        body.linearVelocity = new Vector2(0f, clampedFallSpeed);

        bool jumpIsBuffered = input.TimeSinceJumpPressedSeconds < jumpBufferSeconds;

        if (jumpIsBuffered)
        {
            body.linearVelocity = new Vector2(-WallDirection * wallJumpVelocity.x, wallJumpVelocity.y);

            horizontalControlLockedUntilTimeSeconds = Time.time + wallJumpControlLockSeconds;
            input.ConsumeJumpRequest();

            state = PlayerMovementState.Normal;
            body.gravityScale = baseGravityScale;
        }
    }

    private void UpdateGroundedState()
    {
        Vector2 boxCentre = (Vector2)transform.position + Vector2.down * groundCheckDistanceBelowCentreUnits;
        IsGrounded = Physics2D.OverlapBox(boxCentre, groundCheckSizeUnits, 0f, groundLayers) != null;
    }

    private void UpdateWallContact()
    {
        Vector2 centre = body.position;
        Vector2 offset = Vector2.right * wallCheckDistanceFromCentreUnits;

        bool wallOnRight = Physics2D.OverlapBox(centre + offset, wallCheckSizeUnits, 0f, wallLayers) != null;
        bool wallOnLeft = Physics2D.OverlapBox(centre - offset, wallCheckSizeUnits, 0f, wallLayers) != null;

        WallDirection = wallOnRight ? 1 : wallOnLeft ? -1 : 0;
    }

    private bool IsPressingIntoWall() =>
        WallDirection != 0 && !Mathf.Approximately(input.HorizontalInput, 0f) && Mathf.Sign(input.HorizontalInput) == WallDirection;

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
