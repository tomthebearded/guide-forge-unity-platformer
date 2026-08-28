# M5 · Verify — Game feel
> Nav: [← Stop and play it](05_reality-check.md) · [Overview](00_overview.md) · [The level as a Tilemap →](../MILESTONE_6_tilemap-level/00_overview.md)

> ⚠️ **Superseded 2026-08-28** — the coyote refill in the checkpoint below is now guarded so mashing jump
> can't double-jump in mid-air, and the "one press, one jump" gate box checks for it. **If you executed M5
> before 2026-08-28**, apply the fix under *Before you continue — corrections* in
> [../MILESTONE_6_tilemap-level/01_import-the-art.md](../MILESTONE_6_tilemap-level/01_import-the-art.md), then
> re-run this gate.

## Done-when gate (the real test — check every box by hand)

Observed in **Play Mode in the Editor**, `Level01` open, Game view focused. Where a box refers to "your
baseline", it means the apex you measured in
[M4 step 05](../MILESTONE_4_gravity-and-jumping/05_measure-the-jump.md) — not a number from this page. If you
retuned in [step 05](05_reality-check.md), your own values replace the guide's throughout.

**Every measured number this milestone gates on was read during the steps, while `JumpApexProbe` was still
attached.** [Step 05](05_reality-check.md) deletes the probe, so this gate checks that you *have* those
numbers and that the behaviour they described is still visible — it never asks you to measure an apex with
the instrument removed.

- [ ] **Movement has weight.** Holding **D** from rest → the square builds up to speed over roughly an eighth
      of a second; releasing → it coasts down over a similar interval; pressing the opposite direction →
      it decelerates through zero and turns, never snapping.
- [ ] **Air control is weaker than ground control.** Steering mid-jump works but responds visibly more slowly.
- [ ] **A late jump is forgiven.** Run off the end of the strip and press **Space** within about a tenth of a
      second → the square jumps. After a clear pause in the air → nothing.
- [ ] **The coyote window is what does it.** Set **Coyote Time Seconds** to `0` → the same run-off-and-press
      produces no jump at all. Restore `0.1` → it works again.
- [ ] **An early jump is forgiven.** Press **Space** while still falling, shortly before landing → the square
      takes off the instant it touches down, with no pause on the ground.
- [ ] **The buffer is what does it.** Set **Jump Buffer Seconds** to `0` → the early press is discarded and
      the square lands and stays. Restore `0.12` → it works again.
- [ ] **One press, one jump — still.** However early or late a press is, it never produces two jumps;
      **mashing** Space through a take-off yields exactly one jump, and pressing again in mid-air does nothing.
- [ ] **Height is a decision.** The *numbers* for this box were read at
      [step 04](04_variable-jump-height.md), while the apex probe was still attached: a held jump printed your
      M4 baseline, a quick tap printed under 70% of it. Confirm you recorded both, then confirm the effect is
      still there by eye — a tap clears visibly less height than a hold, and tapping faster lowers it
      further. (The probe is gone by now, which is why the measured half of this box belongs to step 04.)
- [ ] **Falling is faster than rising** on every jump.
- [ ] **You have played it for five minutes** and decided the movement is worth building on.
- [ ] **The measuring tool is gone.** `Player` has no `Jump Apex Probe (Script)`, and
      `Assets/_Project/Scripts/JumpApexProbe.cs` does not exist.
- [ ] **The project is clean.** No red Console entries; after committing, `git status --porcelain` prints
      nothing.

## Files after this milestone (the checkpoint)

_This checkpoint renders the complete contents of every guide-authored file created or modified in this
milestone (listed below). Pre-existing files this milestone only added to are shown as their added region
under "Pre-existing files modified", not reproduced whole. Files not listed were not touched this milestone._

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

    // When Jump was last pressed, stamped from Update on the frame clock.
    // Starts far in the past so that nothing is buffered at the first step.
    private float lastJumpPressedTimeSeconds = float.NegativeInfinity;

    // How long ago Jump was pressed. Large means "not recently".
    // Mathf.Max is not decoration: Time.time reports the frame clock in Update and
    // the physics clock in FixedUpdate, and physics can be up to one step behind —
    // so a fresh press would otherwise read as a negative age.
    public float TimeSinceJumpPressedSeconds => Mathf.Max(0f, Time.time - lastJumpPressedTimeSeconds);

    // True for as long as the button is down — unlike the press timestamp above,
    // which records a single moment.
    public bool IsJumpHeld { get; private set; }

    private void Awake()
    {
        // InputSystem.actions is the project-wide asset.
        // FindAction takes "<action map>/<action>".
        moveAction = InputSystem.actions.FindAction("Player/Move");
        jumpAction = InputSystem.actions.FindAction("Player/Jump");
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
    }

    // Called by PlayerMotor once it has dealt with the request.
    public void ConsumeJumpRequest()
    {
        // Push the timestamp far into the past so the same press cannot be used twice.
        lastJumpPressedTimeSeconds = float.NegativeInfinity;
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

    // The Inspector's Gravity Scale, captured once so the multipliers below
    // always scale the original value rather than compounding on themselves.
    private float baseGravityScale;

    // Counts down while airborne; refilled while grounded; spent by a jump.
    private float coyoteTimeRemainingSeconds;

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

    // FixedUpdate runs on the physics clock — 50 times a second by default.
    private void FixedUpdate()
    {
        UpdateGroundedState();

        // Refill only while grounded AND not rising. The ground check can still see the floor
        // for a step after takeoff; refilling then would re-arm the window mid-rise, and a
        // freshly pressed jump would fire in the air.
        if (IsGrounded && body.linearVelocity.y <= 0f)
        {
            coyoteTimeRemainingSeconds = coyoteTimeSeconds;
        }
        else if (!IsGrounded)
        {
            coyoteTimeRemainingSeconds -= Time.fixedDeltaTime;
        }

        float desiredHorizontalSpeed = input.HorizontalInput * moveSpeedUnitsPerSecond;
        float accelerationThisStep = IsGrounded
            ? groundAccelerationUnitsPerSecondSquared
            : airAccelerationUnitsPerSecondSquared;

        // MoveTowards walks the current value towards the target by at most the third
        // argument — never overshooting it. Multiplying by fixedDeltaTime turns
        // "units per second squared" into "units per second, this step".
        float newHorizontalSpeed = Mathf.MoveTowards(
            body.linearVelocity.x,
            desiredHorizontalSpeed,
            accelerationThisStep * Time.fixedDeltaTime);

        body.linearVelocity = new Vector2(newHorizontalSpeed, body.linearVelocity.y);

        bool jumpIsBuffered = input.TimeSinceJumpPressedSeconds < jumpBufferSeconds;

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
    }
}
```

### Deleted this milestone
- `Assets/_Project/Scripts/JumpApexProbe.cs` — removed in [step 05](05_reality-check.md) once tuning was
  finished. It is in your Git history if you want it back for M8's dash.

### Editor checkpoint

| GameObject | Component | Field | Exact value (guide's defaults) |
|---|---|---|---|
| `Player` | `Player Motor (Script)` | Move Speed Units Per Second | `7` |
| `Player` | `Player Motor (Script)` | Ground Acceleration Units Per Second Squared | `60` |
| `Player` | `Player Motor (Script)` | Air Acceleration Units Per Second Squared | `35` |
| `Player` | `Player Motor (Script)` | Ground Check Size Units / Distance Below Centre | `0.9, 0.12` / `0.5` |
| `Player` | `Player Motor (Script)` | Ground Layers | `Ground` |
| `Player` | `Player Motor (Script)` | Jump Velocity Units Per Second | `14` |
| `Player` | `Player Motor (Script)` | Coyote Time Seconds | `0.1` |
| `Player` | `Player Motor (Script)` | Jump Buffer Seconds | `0.12` |
| `Player` | `Player Motor (Script)` | Fall Gravity Multiplier | `1.8` |
| `Player` | `Player Motor (Script)` | Low Jump Gravity Multiplier | `2.2` |
| `Player` | `Rigidbody 2D` | Gravity Scale | `4` (the multipliers scale this at runtime) |

If [step 05](05_reality-check.md) led you to different numbers, **yours are correct** — and
[`../foundation/conventions.md`](../foundation/conventions.md) should now hold them.

### Pre-existing files modified
- `Assets/_Project/Scenes/Level01.unity` — the new `PlayerMotor` fields and the removal of the probe
  component. Edited through the Editor.

### Unchanged this milestone
- `Assets/InputSystem_Actions.inputactions` — still untouched; M8 adds the first new action.
- `ProjectSettings/TagManager.asset`, `ProjectSettings/QualitySettings.asset`,
  `ProjectSettings/Physics2DSettings.asset` — unchanged since M4, M2 and never respectively.

## Troubleshooting

| Symptom | Likely cause → fix |
|---|---|
| The player can double-jump | `coyoteTimeRemainingSeconds = 0f;` missing inside the jump block — or, if it only happens while mashing, the refill isn't guarded by `&& body.linearVelocity.y <= 0f`, so the ground check re-arms coyote for a step after take-off. |
| One press produces a bouncing loop | `input.ConsumeJumpRequest();` sits outside the `if`, so the same timestamp keeps qualifying. |
| Gravity grows every jump until the player is nailed to the floor | `baseGravityScale` is read from `body.gravityScale` inside the multiplier method instead of once in `Awake`. |
| Tap and hold give identical heights | `IsJumpHeld` is never set, or `IsPressed()` is being called from `FixedUpdate`. |
| The first jump of a session fires by itself | `lastJumpPressedTimeSeconds` initialised to `0` instead of `float.NegativeInfinity`. |
| Movement feels mushy | Acceleration too low for the top speed; `60` pairs with `7`. |
| A jump off a ledge works but feels like cheating | Lower `Coyote Time Seconds` to `0.06`; that is a taste setting, and yours wins. |

## Handoff
- **You now have:** the M1 project and clean repository; `Level01` with a `Ground` strip on the `Ground`
  layer; and a player that accelerates into a run and coasts out of it, steers weakly in the air, forgives a
  jump pressed a tenth of a second late or twelve hundredths early, jumps higher when you hold the button, and
  falls faster than it rises. Two scripts do all of it: `PlayerInputReader` (Move, Jump press timestamp, Jump
  held) and `PlayerMotor` (ground check, acceleration, coyote time, buffer, gravity multipliers). The apex
  probe has been removed.
- **Open / deferred:** the world is still one grey strip 30 units long, and the player is a coloured square.
  There is nothing to collect, nothing to avoid, and no reason to go anywhere. M6 gives you a real level to
  run through.
- **Next:** **[M6 — The level as a Tilemap](../MILESTONE_6_tilemap-level/00_overview.md)** — you import the
  CC0 art, paint a cavern on a grid, and give it a single collider the player cannot catch on.

---
> Nav: [← Stop and play it](05_reality-check.md) · [Overview](00_overview.md) · [The level as a Tilemap →](../MILESTONE_6_tilemap-level/00_overview.md)
