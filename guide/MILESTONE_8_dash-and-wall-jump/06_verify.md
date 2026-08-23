# M8 · Verify — Dash & wall-jump (a movement state machine)
> Nav: [← Jump off the wall](05_wall-jump.md) · [Overview](00_overview.md) · [Coins, enemies, damage, lives & checkpoints →](../MILESTONE_9_coins-enemies-lives-checkpoints/00_overview.md)

## Done-when gate (the real test — check every box by hand)

Observed in **Play Mode in the Editor**, `Level01` open, Game view focused.

- [ ] **The dash covers a fixed distance.** From a standstill, one press of **Left Shift** moves the player
      about **5 units** in the direction it faces — measured on the Inspector's Position X, reading between
      `5.0` and `5.5`.
- [ ] **The dash is flat and cannot be spammed.** Dashing off a ledge crosses in a straight line with no
      fall until it ends; a second press within the **0.6-second** cooldown does nothing, and after it, works.
- [ ] **The dash exits at running speed**, not flying.
- [ ] **The wall slide needs all three conditions.** Airborne, pressing into a `Ground`-layer wall, and
      falling → a steady slide at about 2.5 units per second. Release the direction, land, or rise instead of
      falling → no slide.
- [ ] **A one-way ledge is not a wall.** Pressing into the side of `OneWayLedge` produces no slide.
- [ ] **The wall-jump leaves the wall.** Pressing **Space** while sliding → an arc up and away, on both
      sides, *while still holding the direction into the wall*.
- [ ] **The control lock is doing that.** For roughly 0.15 s after a wall-jump, horizontal input does not
      steer; then it does. Dash still fires during that window.
- [ ] **States are exclusive.** The player never dashes and wall-slides at once, and cannot climb a flat wall
      by mashing jump without re-entering the slide.
- [ ] **Nothing earlier regressed.** Acceleration, coyote time, jump buffering, variable jump height, the
      composite floor, the one-way ledge and the platform ride all behave as their own gates described.
- [ ] **The project is clean.** No red Console entries; after committing, `git status --porcelain` prints
      nothing.

## Files after this milestone (the checkpoint)

_This checkpoint renders the complete contents of every guide-authored file created or modified in this
milestone (listed below). Pre-existing files this milestone only added to are shown as their added region
under "Pre-existing files modified", not reproduced whole. Files not listed were not touched this milestone._

### `Assets/_Project/Scripts/PlayerMovementState.cs`
```csharp
// What the player is doing right now. Exactly one of these is true at a time,
// which is the point: the illegal combinations cannot be represented.
public enum PlayerMovementState
{
    Normal,        // running, falling, jumping — everything from M3 to M7
    Dashing,       // step 03
    WallSliding    // step 04
}
```

### `Assets/_Project/Scripts/PlayerInputReader.cs`
```csharp
using UnityEngine;
using UnityEngine.InputSystem;

// Answers "what is the player asking for?" — and nothing else.
// Acting on it is PlayerMotor's job.
public class PlayerInputReader : MonoBehaviour
{
    // -1 = full left, 0 = nothing, +1 = full right. Read by other components.
    public float HorizontalInput { get; private set; }

    private InputAction moveAction;
    private InputAction jumpAction;

    // When Jump was last pressed, on the same clock as Time.time.
    // Starts far in the past so that nothing is buffered at the first step.
    private float lastJumpPressedTimeSeconds = float.NegativeInfinity;

    // How long ago Jump was pressed. Large means "not recently".
    public float TimeSinceJumpPressedSeconds => Time.time - lastJumpPressedTimeSeconds;

    // True for as long as the button is down — unlike the press timestamp above,
    // which records a single moment.
    public bool IsJumpHeld { get; private set; }

    private InputAction dashAction;

    // Set the frame Dash is pressed; cleared when the motor acts on it.
    public bool DashRequested { get; private set; }

    private void Awake()
    {
        // InputSystem.actions is the project-wide asset.
        // FindAction takes "<action map>/<action>".
        moveAction = InputSystem.actions.FindAction("Player/Move");
        jumpAction = InputSystem.actions.FindAction("Player/Jump");
        dashAction = InputSystem.actions.FindAction("Player/Dash");
    }

    private void Update()
    {
        // ReadValue<Vector2>() returns the composite as (x, y). A platformer
        // only cares about x; y is what a top-down game would use.
        HorizontalInput = moveAction.ReadValue<Vector2>().x;

        // WasPressedThisFrame() is true only on the frame the button goes down —
        // holding the key does not keep it true.
        if (jumpAction.WasPressedThisFrame())
        {
            lastJumpPressedTimeSeconds = Time.time;
        }

        // IsPressed() reports the button's current state, every frame.
        IsJumpHeld = jumpAction.IsPressed();

        if (dashAction.WasPressedThisFrame())
        {
            DashRequested = true;
        }
    }

    // Called by PlayerMotor once it has dealt with the request.
    public void ConsumeJumpRequest()
    {
        // Push the timestamp far into the past so the same press cannot be used twice.
        lastJumpPressedTimeSeconds = float.NegativeInfinity;
    }

    public void ConsumeDashRequest()
    {
        DashRequested = false;
    }
}
```

### `Assets/_Project/Scripts/PlayerMotor.cs`
```csharp
using UnityEngine;

// Turns the intent from PlayerInputReader into motion on the Rigidbody2D.
// RequireComponent makes Unity add the dependencies automatically and refuse to
// let you remove them while this script is attached.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputReader))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private float moveSpeedUnitsPerSecond = 7f;
    [SerializeField] private float groundAccelerationUnitsPerSecondSquared = 60f;
    [SerializeField] private float airAccelerationUnitsPerSecondSquared = 35f;

    [Header("Ground check")]
    [SerializeField] private Vector2 groundCheckSizeUnits = new Vector2(0.9f, 0.12f);
    [SerializeField] private float groundCheckDistanceBelowCentreUnits = 0.5f;
    [SerializeField] private LayerMask groundLayers;

    // True while a solid surface is directly under the feet.
    public bool IsGrounded { get; private set; }

    [Header("Jump")]
    [SerializeField] private float jumpVelocityUnitsPerSecond = 14f;
    [SerializeField] private float coyoteTimeSeconds = 0.10f;
    [SerializeField] private float jumpBufferSeconds = 0.12f;
    [SerializeField] private float fallGravityMultiplier = 1.8f;
    [SerializeField] private float lowJumpGravityMultiplier = 2.2f;

    [Header("Dash")]
    [SerializeField] private float dashDistanceUnits = 5f;
    [SerializeField] private float dashDurationSeconds = 0.15f;
    [SerializeField] private float dashCooldownSeconds = 0.60f;

    private float dashEndTimeSeconds;
    private float nextDashAllowedTimeSeconds;

    // +1 while facing right, -1 while facing left. A dash with no input uses this.
    private int facingDirection = 1;

    [Header("Wall")]
    [SerializeField] private Vector2 wallCheckSizeUnits = new Vector2(0.12f, 0.8f);
    [SerializeField] private float wallCheckDistanceFromCentreUnits = 0.5f;
    [SerializeField] private LayerMask wallLayers;
    [SerializeField] private float wallSlideSpeedUnitsPerSecond = 2.5f;
    [SerializeField] private Vector2 wallJumpVelocity = new Vector2(9f, 13f);
    [SerializeField] private float wallJumpControlLockSeconds = 0.15f;

    // Until this moment passes, horizontal input does not steer.
    private float horizontalControlLockedUntilTimeSeconds;

    // -1 = wall on the left, +1 = wall on the right, 0 = neither.
    public int WallDirection { get; private set; }

    // The Inspector's Gravity Scale, captured once so the multipliers below
    // always scale the original value rather than compounding on themselves.
    private float baseGravityScale;

    // Counts down while airborne; refilled while grounded; spent by a jump.
    private float coyoteTimeRemainingSeconds;

    // The one state the player is in. Every transition in this class is an
    // assignment to this field, so they are easy to find.
    private PlayerMovementState state = PlayerMovementState.Normal;

    private Rigidbody2D body;
    private PlayerInputReader input;

    private void Awake()
    {
        // GetComponent finds another component on this same GameObject.
        // Doing it once in Awake and caching it avoids the lookup every step.
        body = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInputReader>();
        baseGravityScale = body.gravityScale;
    }

    private void UpdateGroundedState()
    {
        Vector2 boxCentre = (Vector2)transform.position + Vector2.down * groundCheckDistanceBelowCentreUnits;

        // OverlapBox(point, size, angle, layerMask) returns the first collider it finds
        // in that box on those layers, or null if there is none.
        IsGrounded = Physics2D.OverlapBox(boxCentre, groundCheckSizeUnits, 0f, groundLayers) != null;
    }

    private void UpdateWallContact()
    {
        Vector2 centre = body.position;
        Vector2 offset = Vector2.right * wallCheckDistanceFromCentreUnits;

        bool wallOnRight = Physics2D.OverlapBox(centre + offset, wallCheckSizeUnits, 0f, wallLayers) != null;
        bool wallOnLeft = Physics2D.OverlapBox(centre - offset, wallCheckSizeUnits, 0f, wallLayers) != null;

        // If somehow both, prefer the one being pressed towards; ties go to the right.
        WallDirection = wallOnRight ? 1 : wallOnLeft ? -1 : 0;
    }

    private bool IsPressingIntoWall()
    {
        if (WallDirection == 0 || Mathf.Approximately(input.HorizontalInput, 0f))
        {
            return false;
        }

        // Same sign means the input points at the wall the player is touching.
        return Mathf.Sign(input.HorizontalInput) == WallDirection;
    }

    // FixedUpdate runs on the physics clock — 50 times a second by default.
    private void FixedUpdate()
    {
        // Facts first: every state is entitled to know these before it decides anything.
        UpdateGroundedState();
        UpdateWallContact();
        UpdateCoyoteTimer();

        // Then exactly one behaviour runs.
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
        if (IsGrounded)
        {
            coyoteTimeRemainingSeconds = coyoteTimeSeconds;
        }
        else
        {
            coyoteTimeRemainingSeconds -= Time.fixedDeltaTime;
        }
    }

    private void TickNormalState()
    {
        if (!Mathf.Approximately(input.HorizontalInput, 0f))
        {
            facingDirection = input.HorizontalInput > 0f ? 1 : -1;
        }

        if (input.DashRequested && Time.time >= nextDashAllowedTimeSeconds)
        {
            StartDash();
            return;   // the rest of this state does not run on the step the dash begins
        }

        // Drop a request that arrived during the cooldown, so it cannot fire late.
        input.ConsumeDashRequest();

        bool horizontalControlLocked = Time.time < horizontalControlLockedUntilTimeSeconds;

        if (!horizontalControlLocked)
        {
            float desiredHorizontalSpeed = input.HorizontalInput * moveSpeedUnitsPerSecond;
            float accelerationThisStep = IsGrounded
                ? groundAccelerationUnitsPerSecondSquared
                : airAccelerationUnitsPerSecondSquared;

            // MoveTowards walks the current value towards the target by at most the third
            // argument — never overshooting it.
            float newHorizontalSpeed = Mathf.MoveTowards(
                body.linearVelocity.x,
                desiredHorizontalSpeed,
                accelerationThisStep * Time.fixedDeltaTime);

            body.linearVelocity = new Vector2(newHorizontalSpeed, body.linearVelocity.y);
        }

        bool jumpIsBuffered = input.TimeSinceJumpPressedSeconds <= jumpBufferSeconds;

        if (jumpIsBuffered && coyoteTimeRemainingSeconds > 0f)
        {
            // Replace the vertical velocity outright rather than adding to it, so a jump
            // always reaches the same height however the player was already moving.
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpVelocityUnitsPerSecond);

            // Spend both windows, so one press produces exactly one jump.
            coyoteTimeRemainingSeconds = 0f;
            input.ConsumeJumpRequest();
        }

        ApplyJumpGravityMultipliers();

        if (!IsGrounded && !horizontalControlLocked && IsPressingIntoWall() && body.linearVelocity.y < 0f)
        {
            state = PlayerMovementState.WallSliding;
        }
    }

    private void StartDash()
    {
        state = PlayerMovementState.Dashing;
        dashEndTimeSeconds = Time.time + dashDurationSeconds;
        nextDashAllowedTimeSeconds = Time.time + dashCooldownSeconds;
        input.ConsumeDashRequest();

        // No gravity for the duration: a dash is a straight line.
        body.gravityScale = 0f;

        // Distance over duration is the speed the dash must hold.
        float dashSpeedUnitsPerSecond = dashDistanceUnits / dashDurationSeconds;
        body.linearVelocity = new Vector2(facingDirection * dashSpeedUnitsPerSecond, 0f);
    }

    private void TickDashingState()
    {
        // Nothing to do while it runs: the velocity set at the start is the dash.
        if (Time.time < dashEndTimeSeconds)
        {
            return;
        }

        state = PlayerMovementState.Normal;
        body.gravityScale = baseGravityScale;

        // Come out at running speed rather than at dash speed.
        float exitSpeed = Mathf.Clamp(body.linearVelocity.x, -moveSpeedUnitsPerSecond, moveSpeedUnitsPerSecond);
        body.linearVelocity = new Vector2(exitSpeed, body.linearVelocity.y);
    }

    private void TickWallSlidingState()
    {
        // Leave the moment any of the three conditions stops holding.
        if (IsGrounded || !IsPressingIntoWall())
        {
            state = PlayerMovementState.Normal;
            body.gravityScale = baseGravityScale;
            return;
        }

        // Clamp the fall rather than replacing it: Mathf.Max keeps the larger
        // (less negative) of the two, so gravity may pull slower but never faster.
        float clampedFallSpeed = Mathf.Max(body.linearVelocity.y, -wallSlideSpeedUnitsPerSecond);
        body.linearVelocity = new Vector2(0f, clampedFallSpeed);

        bool jumpIsBuffered = input.TimeSinceJumpPressedSeconds <= jumpBufferSeconds;

        if (jumpIsBuffered)
        {
            // Away from the wall and up. -WallDirection is "the other way".
            body.linearVelocity = new Vector2(-WallDirection * wallJumpVelocity.x, wallJumpVelocity.y);

            horizontalControlLockedUntilTimeSeconds = Time.time + wallJumpControlLockSeconds;
            input.ConsumeJumpRequest();

            state = PlayerMovementState.Normal;
            body.gravityScale = baseGravityScale;
        }
    }

    private void ApplyJumpGravityMultipliers()
    {
        if (body.linearVelocity.y < 0f)
        {
            // Falling: come down faster than you went up.
            body.gravityScale = baseGravityScale * fallGravityMultiplier;
        }
        else if (body.linearVelocity.y > 0f && !input.IsJumpHeld)
        {
            // Rising, but the button is already released: cut the climb short.
            body.gravityScale = baseGravityScale * lowJumpGravityMultiplier;
        }
        else
        {
            // Rising with the button held, or standing still.
            body.gravityScale = baseGravityScale;
        }
    }

    // Unity calls this in the Editor while the object is selected. Editor-only: it
    // is stripped from a build and costs nothing at runtime.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 boxCentre = transform.position + Vector3.down * groundCheckDistanceBelowCentreUnits;
        Gizmos.DrawWireCube(boxCentre, new Vector3(groundCheckSizeUnits.x, groundCheckSizeUnits.y, 0f));

        Gizmos.color = Color.yellow;
        Vector3 wallOffset = Vector3.right * wallCheckDistanceFromCentreUnits;
        Vector3 wallBoxSize = new Vector3(wallCheckSizeUnits.x, wallCheckSizeUnits.y, 0f);
        Gizmos.DrawWireCube(transform.position + wallOffset, wallBoxSize);
        Gizmos.DrawWireCube(transform.position - wallOffset, wallBoxSize);
    }
}
```

### Editor checkpoint

| GameObject / where | Component | Field | Exact value |
|---|---|---|---|
| `InputSystem_Actions` | `Player` map | new action | `Dash`, type `Button`, bound to `<Keyboard>/leftShift` and `<Gamepad>/buttonWest` |
| `Player` | `Player Motor (Script)` | Dash Distance Units / Duration / Cooldown | `5` / `0.15` / `0.6` |
| `Player` | `Player Motor (Script)` | Wall Check Size Units / Distance From Centre | `0.12, 0.8` / `0.5` |
| `Player` | `Player Motor (Script)` | Wall Layers | `Ground` (**not** `OneWay`) |
| `Player` | `Player Motor (Script)` | Wall Slide Speed Units Per Second | `2.5` |
| `Player` | `Player Motor (Script)` | Wall Jump Velocity | `9, 13` |
| `Player` | `Player Motor (Script)` | Wall Jump Control Lock Seconds | `0.15` |
| `Player` | `Player Motor (Script)` | Ground Layers | `Ground` **and** `OneWay` (unchanged from M7) |

### Pre-existing files modified
- `Assets/InputSystem_Actions.inputactions` — the `Dash` action and its two bindings, added in
  [step 01](01_add-the-dash-action.md).
- `Assets/_Project/Scenes/Level01.unity` — the new `PlayerMotor` fields and the `Wall Layers` mask. Edited
  through the Editor.

### Unchanged this milestone
- `Assets/_Project/Scripts/MovingPlatform.cs`, `Assets/_Project/Scripts/PlatformRiderCarrier.cs` — unchanged
  since M7.
- `Packages/manifest.json`, `ProjectSettings/*` — unchanged since M6 and earlier.

## Troubleshooting

| Symptom | Likely cause → fix |
|---|---|
| Left Shift does nothing | The `Dash` action was not saved with **Save Asset**, or the name in `FindAction` differs. |
| The dash never ends | The `Dashing` case is missing from the `switch`. |
| Gravity stays at zero after a dash | `TickDashingState` restores `baseGravityScale` only after its early `return` — check the order. |
| The player rockets away after dashing | The exit clamp is missing. |
| No wall slide | **Wall Layers** is `Nothing`, or the wall boxes are mis-sized. |
| The player slides up walls | The `body.linearVelocity.y < 0f` condition is missing from the transition. |
| The wall-jump snaps back into the wall | The control lock is not applied — action 3 of step 05 must *replace* the horizontal block. |
| The player climbs a wall by mashing Space | The `!horizontalControlLocked` guard is missing from the wall-slide transition. |
| The dash goes the wrong way | `facingDirection` is updated after the early `return` instead of before it. |

## Handoff
- **You now have:** the M1 project and clean repository; a tilemap cavern with a composite collider, a one-way
  ledge and a rider-carrying moving platform; and a player whose movement is an explicit three-state machine —
  `Normal` (run, jump, coyote time, buffering, variable height), `Dashing` (five units in 0.15 s, on a 0.6 s
  cooldown, gravity suspended) and `WallSliding` (a 2.5 units-per-second cling, with a `(9, 13)` wall-jump and
  a 0.15-second control lock). Input covers `Move`, `Jump` and `Dash` on keyboard and gamepad.
- **Open / deferred:** the cavern is a playground with no purpose — nothing to collect, nothing that can hurt
  you, nowhere that counts as the end, and no consequence for falling off the world.
- **Next:** **[M9 — Coins, enemies, damage, lives & checkpoints](../MILESTONE_9_coins-enemies-lives-checkpoints/00_overview.md)** —
  the milestone where the level becomes a game you can win and lose.

---
> Nav: [← Jump off the wall](05_wall-jump.md) · [Overview](00_overview.md) · [Coins, enemies, damage, lives & checkpoints →](../MILESTONE_9_coins-enemies-lives-checkpoints/00_overview.md)
