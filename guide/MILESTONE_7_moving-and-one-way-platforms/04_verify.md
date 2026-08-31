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
      without sliding off, without judder, and **without creeping ahead** of it in the travel direction.
- [ ] **It carries without re-parenting.** During Play Mode the Hierarchy shows `Player` **staying at the
      root** the whole time — it is never indented under `MovingPlatform` — yet it still travels with it. The
      Console shows no `Cannot set the parent … while activating` error.
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

// Carries whatever is standing on top of this platform by moving it the same
// amount the platform moves each physics step. It does NOT re-parent the rider:
// re-parenting a Dynamic body fights the physics engine, inherits the platform's
// scale, and SetParent throws while the platform is still activating on load.
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlatformRiderCarrier : MonoBehaviour
{
    [SerializeField] private LayerMask riderLayers;

    // Height of the thin detection strip sitting on the platform's top surface.
    // A rider resting on the platform overlaps it; nothing beside or under does.
    [SerializeField] private float standingToleranceUnits = 0.1f;

    private Collider2D platformCollider;
    private Rigidbody2D platformBody;

    // Where the platform was at the end of the previous physics step, so we can
    // measure how far it has moved since.
    private Vector2 previousPlatformPosition;

    // Non-allocating buffer for the overlap query, plus a filter that restricts it
    // to the rider layers and ignores triggers. We rebuild the rider set every
    // step instead of tracking OnCollisionEnter/Exit — a kinematic platform
    // sliding into a resting body makes those callbacks fire unreliably.
    private readonly Collider2D[] overlapResults = new Collider2D[8];
    private ContactFilter2D riderFilter;

    private void Awake()
    {
        platformCollider = GetComponent<Collider2D>();
        platformBody = GetComponent<Rigidbody2D>();
        previousPlatformPosition = platformBody.position;

        // We carry riders explicitly by the platform's delta, so the surface must
        // NOT also drag them by friction — that double-counts and pushes the rider a
        // hair ahead of the platform. Install a frictionless material so our delta is
        // the only horizontal carry. Skip it if a material was set on purpose.
        if (platformCollider.sharedMaterial == null)
        {
            platformCollider.sharedMaterial = new PhysicsMaterial2D("RiderCarrierFrictionless")
            {
                friction = 0f,
                bounciness = 0f
            };
        }

        riderFilter = new ContactFilter2D
        {
            useLayerMask = true,
            layerMask = riderLayers,
            useTriggers = false
        };
    }

    private void FixedUpdate()
    {
        // The delta is the change in the platform's position since the last step.
        Vector2 platformDelta = platformBody.position - previousPlatformPosition;
        previousPlatformPosition = platformBody.position;

        if (platformDelta == Vector2.zero)
        {
            return;
        }

        // A thin box straddling the platform's top edge, as wide as the platform.
        // Anything standing on it overlaps this strip.
        Bounds platformBounds = platformCollider.bounds;
        Vector2 stripCentre = new Vector2(platformBounds.center.x, platformBounds.max.y + standingToleranceUnits * 0.5f);
        Vector2 stripSize = new Vector2(platformBounds.size.x, standingToleranceUnits);

        int found = Physics2D.OverlapBox(stripCentre, stripSize, 0f, riderFilter, overlapResults);
        for (int i = 0; i < found; i++)
        {
            Rigidbody2D rider = overlapResults[i].attachedRigidbody;
            if (rider == null)
            {
                continue;
            }

            // Move the rider by the same amount as the platform. This adds on top
            // of the rider's own velocity-driven movement: standing still it rides
            // the platform; walking, its input velocity composes with the shift.
            rider.position += platformDelta;
        }
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
| The player is not carried | **Rider Layers** is `Nothing`, or the carrier script sits on `Visual` (no `Rigidbody 2D`/`Collider 2D`) rather than on the parent. |
| Console: `Cannot set the parent … while activating` | An old `SetParent`-based `PlatformRiderCarrier`; replace it with the movement-inheritance version above. |
| The collider does not match the sprite | The platform parent's scale is not `1, 1, 1`, stretching the unit-sized collider. |
| The player is carried while jumping past the platform's edge | `standingToleranceUnits` (the detection strip's height) is far too large; `0.1` is right. |
| The player creeps a hair ahead of the platform | A `Material` is assigned to the platform's `Box Collider 2D`, so the frictionless one is skipped; set its **Friction** to `0` or clear the field. |
| Riding judders | `Interpolate` is `None` on the platform's body. |

## Handoff
- **You now have:** the M1 project and clean repository; a tuned player controller; a painted tilemap cavern
  with a single composite collider; a one-way ledge on the `OneWay` layer that you pass through from below and
  land on from above; and a kinematic moving platform that travels a visible path and carries the player by
  moving it with the platform each physics step — no re-parenting. Two new scripts, `MovingPlatform` and
  `PlatformRiderCarrier`, neither of which required a change to the player's movement code.
- **Open / deferred:** the moveset is still run and jump. There is nothing to reach that a single jump cannot,
  and nothing in the cavern to collect, avoid or aim at. M8 extends the moveset; M9 gives the level a point.
- **Next:** **[M8 — Dash & wall-jump (a movement state machine)](../MILESTONE_8_dash-and-wall-jump/00_overview.md)** —
  two moves that cannot both be true at once, which is how the movement code earns an explicit state machine.

---
> Nav: [← Carry the rider](03_carry-the-rider.md) · [Overview](00_overview.md) · [Dash & wall-jump (a movement state machine) →](../MILESTONE_8_dash-and-wall-jump/00_overview.md)
