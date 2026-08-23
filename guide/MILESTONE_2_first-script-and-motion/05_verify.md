# M2 · Verify — First script & frame-rate-independent motion
> Nav: [← Prove the motion is frame-rate independent](04_frame-rate-independence.md) · [Overview](00_overview.md) · [Input & running (keyboard and gamepad) →](../MILESTONE_3_input-and-running/00_overview.md)

## Done-when gate (the real test — check every box by hand)

Observed in **Play Mode in the Editor**, with the `Level01` scene open and the Console panel visible.

- [ ] **The square moves under your own code.** Press Play → the orange `Player` square glides right and
      leaves the frame; stopping returns it to `X = 0`.
- [ ] **Speed is tunable live.** During Play Mode, set **Move Speed Units Per Second** to `8` → the square
      speeds up within one frame, no recompile. Leaving Play Mode restores `3`.
- [ ] **60 fps: the achieved speed is the configured speed.** With **Target Frame Rate For Testing** = `60`,
      the third Console line reads `x/t=3.0` and `frame` around `17` ms.
- [ ] **15 fps: the achieved speed is unchanged.** With **Target Frame Rate For Testing** = `15`, the third
      Console line reads `x/t=3.0` and `frame` around `67` ms — a quarter of the frames, the same speed.
- [ ] **The project compiles and the tree is clean.** The Console shows no red entries, and after committing,
      `git status --porcelain` prints nothing.

## Files after this milestone (the checkpoint)

_This checkpoint renders the complete contents of every guide-authored file created or modified in this
milestone (listed below). Pre-existing files this milestone only added to are shown as their added region
under "Pre-existing files modified", not reproduced whole. Files not listed were not touched this milestone._

### `Assets/_Project/Scripts/ConstantMover.cs`
```csharp
using UnityEngine;

// Moves whatever it is attached to steadily along +X, at a speed set in the Inspector.
// A stepping stone: M3 replaces it with input-driven, physics-based movement.
public class ConstantMover : MonoBehaviour
{
    [SerializeField] private float moveSpeedUnitsPerSecond = 3f;

    [Header("Frame-rate test")]
    [SerializeField] private int targetFrameRateForTesting = 60;

    private float nextReportTimeSeconds = 1f;   // first report one second in, so the division below is safe

    private void Awake()
    {
        // Ask the platform for a specific frame rate. Only has an effect with VSync off.
        Application.targetFrameRate = targetFrameRateForTesting;
    }

    // Update() is called by Unity once per rendered frame.
    private void Update()
    {
        // Vector3.right is (1, 0, 0). Multiplying by Time.deltaTime converts
        // "units per second" into "units this frame".
        transform.position += Vector3.right * (moveSpeedUnitsPerSecond * Time.deltaTime);

        if (Time.time >= nextReportTimeSeconds)
        {
            // Time.time is seconds since Play started; x / t is the speed actually achieved.
            float achievedSpeedUnitsPerSecond = transform.position.x / Time.time;
            // Debug.Log writes a line to the Console panel.
            Debug.Log($"t={Time.time:F2}s  x={transform.position.x:F2}  x/t={achievedSpeedUnitsPerSecond:F1}  frame={Time.deltaTime * 1000f:F0} ms");
            nextReportTimeSeconds += 1f;
        }
    }
}
```

### Editor checkpoint

| GameObject / where | Component | Field | Exact value |
|---|---|---|---|
| `Player` | `Transform` | Position | `0, 0, 0` |
| `Player` | `Transform` | Rotation / Scale | `0, 0, 0` / `1, 1, 1` (defaults) |
| `Player` | `Sprite Renderer` | Sprite | the built-in `Square` |
| `Player` | `Sprite Renderer` | Color | `E8B04B` (cosmetic) |
| `Player` | `Constant Mover (Script)` | Move Speed Units Per Second | `3` |
| `Player` | `Constant Mover (Script)` | Target Frame Rate For Testing | `60` |
| Project Settings → Quality | Rendering | VSync Count | **Don't Sync** |

### Pre-existing files modified
- `Assets/_Project/Scenes/Level01.unity` — the `Player` GameObject was added to it, with its
  `SpriteRenderer` and `ConstantMover` components. Edited through the Editor, never by hand.
- `ProjectSettings/QualitySettings.asset` — **VSync Count** set to **Don't Sync** in
  [step 04](04_frame-rate-independence.md), action 1.

### Unchanged this milestone
- `cavern-dash/.gitignore`, `cavern-dash/.gitattributes` — unchanged since M1.
- `Packages/manifest.json` — unchanged since M1; no package has been added yet.

## Troubleshooting

| Symptom | Likely cause → fix |
|---|---|
| Nothing moves in Play Mode | `Update` is misspelled, or the script is not attached to `Player`. Unity matches these callbacks by exact name. |
| `x/t` differs between the two frame rates | `Time.deltaTime` is missing from the position line — the defect the whole milestone exists to rule out. |
| `frame` ignores **Target Frame Rate For Testing** | VSync is still on for the *selected* quality level, so `targetFrameRate` is ignored. |
| The Inspector shows no fields on the script | The class does not inherit `MonoBehaviour`, or the file name and class name differ. |
| Values you typed while playing vanished | Working as designed — Play Mode changes are discarded. Type them again while stopped, then save. |
| `git status` lists `QualitySettings.asset` | Expected: that is step 04's VSync change. Commit it. |

## Handoff
- **You now have:** the M1 project and clean repository, plus a `Player` GameObject in `Level01` carrying a
  `SpriteRenderer` and your first script, `ConstantMover`, which moves it at a per-second speed set in the
  Inspector and prints the achieved speed to the Console. VSync is off project-wide, so frame rate is
  something you can pin while testing.
- **Open / deferred:** the player moves on its own and ignores you entirely; there is no input, no gravity,
  and nothing solid in the scene. `ConstantMover` is a teaching stepping stone and M3 deletes it.
- **Next:** **[M3 — Input & running (keyboard and gamepad)](../MILESTONE_3_input-and-running/00_overview.md)** —
  the square starts obeying you, on either device, and stops falling through the world.

---
> Nav: [← Prove the motion is frame-rate independent](04_frame-rate-independence.md) · [Overview](00_overview.md) · [Input & running (keyboard and gamepad) →](../MILESTONE_3_input-and-running/00_overview.md)
