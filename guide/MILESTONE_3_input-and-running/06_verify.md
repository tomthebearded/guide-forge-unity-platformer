# M3 · Verify — Input & running (keyboard and gamepad)
> Nav: [← Move the body with velocity](05_move-with-velocity.md) · [Overview](00_overview.md) · [Gravity, jumping & the ground check →](../MILESTONE_4_gravity-and-jumping/00_overview.md)

## Done-when gate (the real test — check every box by hand)

Observed in **Play Mode in the Editor**, `Level01` open, with the Game view clicked once so it has focus.

- [ ] **The keyboard runs the player.** Hold **D** → the square runs right; hold **A** → it runs left;
      release → it stops in the same frame. Both arrow keys do the same as A and D.
- [ ] **The speed is the configured speed.** While a direction is held, `Rigidbody 2D` → **Info** → **Speed**
      reads `7`. (No Info foldout: from Position X `0`, hold **D** for two seconds and read Position X ≈ `14`.)
- [ ] **The gamepad runs the player, with no gamepad code.** With a pad connected, pushing the left stick
      fully moves the square at the same 7 units per second; pushing it half way moves it visibly slower.
      *(No gamepad to hand? Leave this box open and close it whenever you get hold of one — every later
      milestone that touches input assumes both devices work.)*
- [ ] **The player is a physics body.** Pressing Play from a standstill above the strip → the square falls,
      lands on top of it, and stays there; `Transform` **Position Y** reads between `-3.00` and `-2.98` and
      **Rotation Z** stays `0`.
- [ ] **It cannot get through the floor.** Run from one end of the strip to the other holding a direction: the
      square never sinks, never catches, and never passes through.
- [ ] **The project is clean.** The Console shows no red entries, and after committing,
      `git status --porcelain` prints nothing.

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

    private void Awake()
    {
        // InputSystem.actions is the project-wide asset.
        // FindAction takes "<action map>/<action>".
        moveAction = InputSystem.actions.FindAction("Player/Move");
    }

    private void Update()
    {
        // ReadValue<Vector2>() returns the composite as (x, y). A platformer
        // only cares about x; y is what a top-down game would use.
        HorizontalInput = moveAction.ReadValue<Vector2>().x;
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

    private Rigidbody2D body;
    private PlayerInputReader input;

    private void Awake()
    {
        // GetComponent finds another component on this same GameObject.
        // Doing it once in Awake and caching it avoids the lookup every step.
        body = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInputReader>();
    }

    // FixedUpdate runs on the physics clock — 50 times a second by default.
    private void FixedUpdate()
    {
        float desiredHorizontalSpeed = input.HorizontalInput * moveSpeedUnitsPerSecond;

        // Set X, keep Y: the Y component is gravity's, and overwriting it would
        // cancel the fall every step.
        body.linearVelocity = new Vector2(desiredHorizontalSpeed, body.linearVelocity.y);
    }
}
```

### Deleted this milestone
- `Assets/_Project/Scripts/ConstantMover.cs` — removed in [step 04](04_rigidbody-and-collider.md): it wrote
  `transform.position` directly, which fights a `Rigidbody2D`.

### Editor checkpoint

| GameObject | Component | Field | Exact value |
|---|---|---|---|
| `Player` | `Transform` | Position / Scale | `0, 0, 0` / `1, 1, 1` |
| `Player` | `Rigidbody 2D` | Body Type | `Dynamic` |
| `Player` | `Rigidbody 2D` | Gravity Scale | `1` (M4 changes this) |
| `Player` | `Rigidbody 2D` | Collision Detection | `Continuous` |
| `Player` | `Rigidbody 2D` | Interpolate | `Interpolate` |
| `Player` | `Rigidbody 2D` | Constraints → Freeze Rotation Z | ticked |
| `Player` | `Box Collider 2D` | Size / Offset / Is Trigger | `1, 1` / `0, 0` / unticked |
| `Player` | `Player Motor (Script)` | Move Speed Units Per Second | `7` |
| `Ground` | `Transform` | Position / Scale | `0, -4, 0` / `30, 1, 1` |
| `Ground` | `Sprite Renderer` | Color | `39424E` (cosmetic) |
| `Ground` | `Box Collider 2D` | Size / Offset / Is Trigger | `1, 1` / `0, 0` / unticked |
| Project Settings → Input System Package | Input Actions | Project-wide Actions | `InputSystem_Actions` |

### Pre-existing files modified
- `Assets/_Project/Scenes/Level01.unity` — `Ground` added; `Player` gained `Rigidbody2D`,
  `BoxCollider2D`, `PlayerInputReader` and `PlayerMotor`, and lost `ConstantMover`. Edited through the Editor.

### Unchanged this milestone
- `Assets/InputSystem_Actions.inputactions` — read in [step 01](01_meet-the-input-system.md), deliberately not
  edited. M8 is the first milestone to add an action to it.
- `ProjectSettings/QualitySettings.asset` — unchanged since M2 (VSync still **Don't Sync**).
- `cavern-dash/.gitignore`, `cavern-dash/.gitattributes` — unchanged since M1.

## Troubleshooting

| Symptom | Likely cause → fix |
|---|---|
| Keys do nothing, gamepad does nothing | The **Game** view does not have focus. Click once inside it. |
| `NullReferenceException` from `PlayerInputReader.Update` | `FindAction("Player/Move")` returned `null` — Project-wide Actions is unset or the action was renamed. |
| Input reads `0.00` for ever | The action is disabled. Add `moveAction.Enable();` after `FindAction` in `Awake`. |
| The player hovers, or sinks while running | The whole velocity vector is being overwritten. Preserve `body.linearVelocity.y`. |
| The player passes through the floor at speed | `Collision Detection` is `Discrete` on the `Rigidbody2D`, or the `Ground` collider is a trigger. |
| The player spins after touching an edge | **Freeze Rotation Z** is unticked in the Rigidbody's Constraints. |
| Movement stutters | Velocity written from `Update` instead of `FixedUpdate`, or `Interpolate` set to `None`. |

## Handoff
- **You now have:** the M1 project and clean repository; a `Level01` scene holding a `Ground` strip with a
  collider and a `Player` that is a real physics body — `Rigidbody2D` (Dynamic, interpolated, continuous
  collision, rotation frozen) plus a one-unit `BoxCollider2D`; and two small components, `PlayerInputReader`
  (reads the project-wide `Move` action, keyboard and gamepad alike) and `PlayerMotor` (writes horizontal
  velocity at 7 units per second in `FixedUpdate`). `ConstantMover` is gone.
- **Open / deferred:** gravity is still at the engine's default scale of `1` and feels floaty; there is no
  jump, no way to tell whether the player is standing on anything, and stopping is instantaneous rather than
  weighted. M4 takes the first two, M5 the third.
- **Next:** **[M4 — Gravity, jumping & the ground check](../MILESTONE_4_gravity-and-jumping/00_overview.md)** —
  the player falls at a weight you choose and jumps exactly once per press, because it can finally tell
  whether the floor is under its feet.

---
> Nav: [← Move the body with velocity](05_move-with-velocity.md) · [Overview](00_overview.md) · [Gravity, jumping & the ground check →](../MILESTONE_4_gravity-and-jumping/00_overview.md)
