# M7 · Verify — Moving & one-way platforms
> Nav: [← Carry the rider](03_carry-the-rider.md) · [Overview](00_overview.md) · [Dash & wall-jump (a movement state machine) →](../MILESTONE_8_dash-and-wall-jump/00_overview.md)

## Done-when gate (the real test — check every box by hand)

Observed in **Play Mode in the Editor**, `Level01` open, Game view focused, with the Hierarchy visible.

- [ ] **Up through, down onto.** Jumping into `OneWayLedge` from below → the player passes through and lands
      on top of it. Standing on it → it holds. Walking off its side → the player falls without sticking.
- [ ] **The one-way ledge is jumpable from.** Pressing **Space** while standing on the ledge jumps — proof
      that **Ground Layers** covers both `Ground` and `OneWay`.
- [ ] **The platform travels.** `MovingPlatform` slides six units, reverses, and repeats, smoothly and
      indefinitely, with a cyan path drawn in the Scene view when it is selected.
- [ ] **The rider is carried.** Standing on the platform → the player travels with it in both directions,
      without sliding off and without judder.
- [ ] **Parenting is what carries it.** During Play Mode the Hierarchy shows `Player` indented under
      `MovingPlatform` while riding, and back at the root after stepping off.
- [ ] **Nothing is deformed.** The player's `Transform` Scale reads `1, 1, 1` while riding.
- [ ] **Only the top counts.** Walking into the platform's side, or jumping up into its underside, does
      **not** attach the player to it.
- [ ] **Jumping off a moving platform behaves.** A jump taken while riding rises and lands normally instead of
      being flung.
- [ ] **The rest of M6 still holds.** The player still runs the painted floor at full speed without catching.
- [ ] **The project is clean.** No red Console entries; after committing, `git status --porcelain` prints
      nothing.

## Files after this milestone (the checkpoint)

_This checkpoint renders the complete contents of every guide-authored file created or modified in this
milestone (listed below). Pre-existing files this milestone only added to are shown as their added region
under "Pre-existing files modified", not reproduced whole. Files not listed were not touched this milestone._

### `Assets/_Project/Scripts/MovingPlatform.cs`
```csharp
using UnityEngine;

// Drives a kinematic platform back and forth along a straight path.
// The path is expressed as an offset from wherever the platform starts, so the
// platform can be dragged anywhere in the scene without retyping coordinates.
[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector2 travelOffsetUnits = new Vector2(6f, 0f);
    [SerializeField] private float speedUnitsPerSecond = 2f;

    private Rigidbody2D body;
    private Vector2 startPosition;
    private Vector2 endPosition;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        startPosition = body.position;
        endPosition = startPosition + travelOffsetUnits;
    }

    private void FixedUpdate()
    {
        // PingPong walks a value from 0 up to the length and back down again, for ever.
        float distanceAlongPath = Mathf.PingPong(Time.time * speedUnitsPerSecond, travelOffsetUnits.magnitude);

        // Lerp interpolates between two points: 0 gives the first, 1 the second,
        // 0.5 the midpoint. Dividing by the path length turns distance into that 0-1.
        Vector2 target = Vector2.Lerp(startPosition, endPosition, distanceAlongPath / travelOffsetUnits.magnitude);

        // MovePosition moves the body through the physics engine, so contacts are
        // resolved on the way. Writing transform.position would teleport it.
        body.MovePosition(target);
    }

    // Draws the path in the Scene view so the travel is visible without pressing Play.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 from = transform.position;
        Vector3 to = from + (Vector3)travelOffsetUnits;
        Gizmos.DrawLine(from, to);
        Gizmos.DrawWireSphere(to, 0.15f);
    }
}
```

### `Assets/_Project/Scripts/PlatformRiderCarrier.cs`
```csharp
using UnityEngine;

// Carries whatever is standing on top of this platform, by making it a child while
// it rides. A child follows its parent for free — which is why this object's own
// transform must stay at scale 1, 1, 1.
[RequireComponent(typeof(Collider2D))]
public class PlatformRiderCarrier : MonoBehaviour
{
    [SerializeField] private LayerMask riderLayers;

    // How far below the platform's top surface a rider's feet may be and still count
    // as standing on it. Physics keeps a hair of separation, so this is not zero.
    [SerializeField] private float standingToleranceUnits = 0.1f;

    private Collider2D platformCollider;

    private void Awake()
    {
        platformCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsRider(collision.collider) || !IsStandingOnTop(collision.collider))
        {
            return;
        }

        collision.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Only un-parent what this platform actually adopted.
        if (collision.transform.parent == transform)
        {
            collision.transform.SetParent(null);
        }
    }

    private bool IsRider(Collider2D other)
    {
        // A LayerMask is a bitmask: bit N is set when layer N is selected.
        // Shifting 1 into the object's layer position and masking tests membership.
        return (riderLayers.value & (1 << other.gameObject.layer)) != 0;
    }

    private bool IsStandingOnTop(Collider2D other)
    {
        // bounds is the collider's world-space box. The rider is on top when the
        // bottom of its box is at or above the top of the platform's.
        float platformTopY = platformCollider.bounds.max.y;
        return other.bounds.min.y >= platformTopY - standingToleranceUnits;
    }
}
```

### Editor checkpoint

| GameObject | Component | Field | Exact value |
|---|---|---|---|
| `OneWayLedge` | `Transform` | Scale / Rotation | `4, 0.25, 1` / `0, 0, 0` |
| `OneWayLedge` | `Box Collider 2D` | Size / Used By Effector | `1, 1` / ticked |
| `OneWayLedge` | `Platform Effector 2D` | Use One Way / Surface Arc / Use Side Friction | ticked / `180` / unticked |
| `OneWayLedge` | GameObject | Layer | `OneWay` |
| `MovingPlatform` | `Transform` | Scale | `1, 1, 1` — load-bearing |
| `MovingPlatform` | `Box Collider 2D` | Size / Offset | `4, 0.5` / `0, 0` |
| `MovingPlatform` | `Rigidbody 2D` | Body Type / Interpolate | `Kinematic` / `Interpolate` |
| `MovingPlatform` | `Moving Platform (Script)` | Travel Offset Units / Speed Units Per Second | `6, 0` / `2` |
| `MovingPlatform` | `Platform Rider Carrier (Script)` | Rider Layers / Standing Tolerance Units | `Default` / `0.1` |
| `MovingPlatform` | GameObject | Layer | `Ground` |
| `MovingPlatform/Visual` | `Transform` | Position / Scale | `0, 0, 0` / `4, 0.5, 1` |
| `Player` | `Player Motor (Script)` | Ground Layers | `Ground` **and** `OneWay` |

### Pre-existing files modified
- `Assets/_Project/Scenes/Level01.unity` — `OneWayLedge` and `MovingPlatform` (with its `Visual` child)
  added; the player's **Ground Layers** mask extended to include `OneWay`. Edited through the Editor.

### Unchanged this milestone
- `Assets/_Project/Scripts/PlayerInputReader.cs`, `Assets/_Project/Scripts/PlayerMotor.cs` — unchanged since
  M5. Both new platform behaviours were added without touching the player's movement code.
- `Assets/InputSystem_Actions.inputactions`, `Packages/manifest.json`, `ProjectSettings/*` — unchanged since
  M6 and earlier.

## Troubleshooting

| Symptom | Likely cause → fix |
|---|---|
| The player bumps into the one-way ledge from below | **Used By Effector** unticked on its `Box Collider 2D`. |
| The player falls through the one-way ledge from above | **Use One Way** unticked, or the ledge is rotated. |
| Standing on the ledge, jump does nothing | **Ground Layers** does not include `OneWay`. |
| The platform falls away on Play | Its `Rigidbody 2D` is `Dynamic`; it must be `Kinematic`. |
| The platform shoves the player through the floor | `transform.position` written instead of `MovePosition`. |
| The player is not carried | **Rider Layers** is `Nothing`, or the carrier script sits on `Visual` rather than the parent. |
| The player stretches when it steps on | The platform parent's scale is not `1, 1, 1`. |
| The player is carried when brushing the platform's side | `IsStandingOnTop` bypassed, or the tolerance is far too large. |
| Riding judders | `Interpolate` is `None` on the platform's body. |

## Handoff
- **You now have:** the M1 project and clean repository; a tuned player controller; a painted tilemap cavern
  with a single composite collider; a one-way ledge on the `OneWay` layer that you pass through from below and
  land on from above; and a kinematic moving platform that travels a visible path and carries the player by
  re-parenting. Two new scripts, `MovingPlatform` and `PlatformRiderCarrier`, neither of which required a
  change to the player's movement code.
- **Open / deferred:** the moveset is still run and jump. There is nothing to reach that a single jump cannot,
  and nothing in the cavern to collect, avoid or aim at. M8 extends the moveset; M9 gives the level a point.
- **Next:** **[M8 — Dash & wall-jump (a movement state machine)](../MILESTONE_8_dash-and-wall-jump/00_overview.md)** —
  two moves that cannot both be true at once, which is how the movement code earns an explicit state machine.

---
> Nav: [← Carry the rider](03_carry-the-rider.md) · [Overview](00_overview.md) · [Dash & wall-jump (a movement state machine) →](../MILESTONE_8_dash-and-wall-jump/00_overview.md)
