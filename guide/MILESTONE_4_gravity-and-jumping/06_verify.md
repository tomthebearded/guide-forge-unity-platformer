# M4 · Verify — Gravity, jumping & the ground check
> Nav: [← Measure the jump](05_measure-the-jump.md) · [Overview](00_overview.md) · [Game feel →](../MILESTONE_5_game-feel/00_overview.md)

## Done-when gate (the real test — check every box by hand)

Observed in **Play Mode in the Editor**, `Level01` open, Game view focused, Console visible.

- [ ] **Gravity has weight.** `Player` → `Rigidbody 2D` → **Gravity Scale** reads `4`, and
      **Project Settings > Physics 2D > Gravity** still reads `0, -9.81`.
- [ ] **The ground is on its own layer.** The `Ground` object's Layer reads `Ground`; `Player`'s reads
      `Default`; `Player Motor (Script)` → **Ground Layers** reads `Ground`.
- [ ] **The ground check is visibly correct.** Selecting `Player` shows the green wireframe box under its
      feet, overlapping the strip's top surface while resting and clear of it while airborne.
- [ ] **Jump fires once per press, only from the ground.** Space → the square rises and lands. Space again
      mid-air → nothing. Space held down → one jump only. Space after running off the strip → nothing.
- [ ] **The jump is repeatable and measured.** Six jumps from a standstill print six `apex = …` lines with
      the **same** value to two decimals, between `2.20` and `2.60` units. Write that value down: M5 compares
      against it.
- [ ] **Height scales with the square of the launch speed.** Setting **Jump Velocity Units Per Second** to `7`
      prints an apex near a quarter of your baseline; `14` restores it.
- [ ] **Running still works.** Holding **D** or **A** moves at 7 units per second, and jumping while running
      produces an arc that keeps its horizontal speed.
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

    // Set the frame Jump is pressed; stays set until the physics step consumes it.
    public bool JumpRequested { get; private set; }

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
            JumpRequested = true;
        }
    }

    // Called by PlayerMotor once it has dealt with the request.
    public void ConsumeJumpRequest()
    {
        JumpRequested = false;
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

    [Header("Ground check")]
    [SerializeField] private Vector2 groundCheckSizeUnits = new Vector2(0.9f, 0.12f);
    [SerializeField] private float groundCheckDistanceBelowCentreUnits = 0.5f;
    [SerializeField] private LayerMask groundLayers;

    // True while a solid surface is directly under the feet.
    public bool IsGrounded { get; private set; }

    [Header("Jump")]
    [SerializeField] private float jumpVelocityUnitsPerSecond = 14f;

    private Rigidbody2D body;
    private PlayerInputReader input;

    private void Awake()
    {
        // GetComponent finds another component on this same GameObject.
        // Doing it once in Awake and caching it avoids the lookup every step.
        body = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInputReader>();
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

        float desiredHorizontalSpeed = input.HorizontalInput * moveSpeedUnitsPerSecond;

        // Set X, keep Y: the Y component is gravity's, and overwriting it would
        // cancel the fall every step.
        body.linearVelocity = new Vector2(desiredHorizontalSpeed, body.linearVelocity.y);

        if (input.JumpRequested && IsGrounded)
        {
            // Replace the vertical velocity outright rather than adding to it, so a jump
            // always reaches the same height however the player was already moving.
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpVelocityUnitsPerSecond);
        }

        // Consume it either way: an unusable request must not wait around and fire late.
        input.ConsumeJumpRequest();
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

### `Assets/_Project/Scripts/JumpApexProbe.cs`
```csharp
using UnityEngine;

// A tuning tool, not gameplay: measures how high the player rises between leaving
// the ground and landing again, and prints it. Deleted at the end of M5.
[RequireComponent(typeof(PlayerMotor))]
public class JumpApexProbe : MonoBehaviour
{
    private PlayerMotor motor;
    private bool wasGroundedLastStep = true;
    private float heightWhenLeavingGroundUnits;
    private float highestHeightSinceLeavingUnits;

    private void Awake()
    {
        motor = GetComponent<PlayerMotor>();
    }

    private void FixedUpdate()
    {
        bool isGroundedNow = motor.IsGrounded;
        float currentHeightUnits = transform.position.y;

        if (wasGroundedLastStep && !isGroundedNow)
        {
            // Just left the ground: start a new measurement.
            heightWhenLeavingGroundUnits = currentHeightUnits;
            highestHeightSinceLeavingUnits = currentHeightUnits;
        }
        else if (!isGroundedNow)
        {
            // Airborne: Mathf.Max keeps the highest value seen so far.
            highestHeightSinceLeavingUnits = Mathf.Max(highestHeightSinceLeavingUnits, currentHeightUnits);
        }
        else if (!wasGroundedLastStep)
        {
            // Just landed: report the arc that finished.
            float apexUnits = highestHeightSinceLeavingUnits - heightWhenLeavingGroundUnits;
            Debug.Log($"apex = {apexUnits:F2} units");
        }

        wasGroundedLastStep = isGroundedNow;
    }
}
```

### Editor checkpoint

| GameObject / where | Component | Field | Exact value |
|---|---|---|---|
| `Player` | `Rigidbody 2D` | Gravity Scale | `4` |
| `Player` | `Player Motor (Script)` | Move Speed Units Per Second | `7` |
| `Player` | `Player Motor (Script)` | Ground Check Size Units | `0.9, 0.12` |
| `Player` | `Player Motor (Script)` | Ground Check Distance Below Centre Units | `0.5` |
| `Player` | `Player Motor (Script)` | Ground Layers | `Ground` |
| `Player` | `Player Motor (Script)` | Jump Velocity Units Per Second | `14` |
| `Player` | `Jump Apex Probe (Script)` | — | no fields |
| `Player` | GameObject | Layer | `Default` |
| `Ground` | GameObject | Layer | `Ground` |
| Project Settings → Tags and Layers | Layers | User layers added | `Ground`, `OneWay`, `Hazard` |
| Project Settings → Physics 2D | Gravity | Y | `-9.81` (unchanged) |

### Pre-existing files modified
- `Assets/_Project/Scenes/Level01.unity` — `Gravity Scale`, the ground-check fields, the `Ground Layers` mask,
  the layer assignment on `Ground`, and the `JumpApexProbe` component. Edited through the Editor.
- `ProjectSettings/TagManager.asset` — three user layers added in [step 02](02_ground-layer.md).

### Unchanged this milestone
- `Assets/InputSystem_Actions.inputactions` — `Jump` was *read*, not edited. M8 adds the first new action.
- `ProjectSettings/QualitySettings.asset`, `cavern-dash/.gitignore`, `cavern-dash/.gitattributes` — unchanged
  since M2 and M1 respectively.

## Troubleshooting

| Symptom | Likely cause → fix |
|---|---|
| Space does nothing | **Ground Layers** is `Nothing`, so `IsGrounded` is never true. This is the milestone's most common failure. |
| The player jumps in mid-air | The `IsGrounded` condition is missing, or `UpdateGroundedState()` is not the first line of `FixedUpdate`. |
| Holding Space jumps repeatedly | `IsPressed()` was used instead of `WasPressedThisFrame()`. |
| The player falls through the strip after the gravity change | `Collision Detection` is `Discrete`; set it to `Continuous`. |
| The apex printed is `0.00` | The ground check box is far too tall, so the player is never "airborne". Size must be `0.9, 0.12`. |
| The apex differs between jumps by more than `0.1` | You are holding a direction while jumping, or two probes are attached. Measure from a standstill. |
| The green gizmo is missing | Gizmos are toggled off in the Scene view toolbar, or `Player` is not selected. |

## Handoff
- **You now have:** the M1 project and clean repository; `Level01` holding the `Ground` strip on its own
  `Ground` layer and a `Player` that runs at 7 units per second, falls at gravity scale `4`, knows whether it
  is grounded through an overlap query you can see in the Scene view, and jumps once per press to a height you
  have measured and written down. Three scripts: `PlayerInputReader`, `PlayerMotor`, and the temporary
  `JumpApexProbe`. Three layers exist — `Ground` in use, `OneWay` and `Hazard` reserved.
- **Open / deferred:** the jump is correct and unforgiving — a press a fraction early is lost, stepping off a
  ledge removes the jump instantly, and tapping the button gives exactly the same height as holding it.
  Movement starts and stops dead, with no weight. All of that is M5.
- **Next:** **[M5 — Game feel](../MILESTONE_5_game-feel/00_overview.md)** — the milestone where the jump stops
  being arithmetic and starts being a game. It is also the reality-check gate: you stop and play.

---
> Nav: [← Measure the jump](05_measure-the-jump.md) · [Overview](00_overview.md) · [Game feel →](../MILESTONE_5_game-feel/00_overview.md)
