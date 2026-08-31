# M7 · Step 03 of 04 — Carry the rider
> Nav: [← A platform that travels](02_moving-platform.md) · [Overview](00_overview.md) · [Verify →](04_verify.md)

**Before you start:** [step 02](02_moving-platform.md) finished — the platform travels, the player can stand
and jump on it, and it slides out from underneath.

## Why / design
Stand on the platform and it leaves without you. Nothing is broken: the platform moves its own body, the
player's body is not touching anything that pushes it sideways, and friction between two colliders is not a
transport system. The player is *resting* on the platform, not *attached* to it. To carry it, something has to
move the player by the platform's own amount each step.

There are two honest ways to do that.

**Re-parent**: make the player a child of the platform while it rides, and a root object again when it leaves.
A child moves with its parent for free. It is the obvious first idea, and it is a trap:

- `transform.SetParent(...)` called from a **physics collision callback** throws
  `Cannot set the parent of the GameObject 'Player' while activating or deactivating the parent GameObject`
  — because the first contacts can fire while the platform is still being activated on scene load, and Unity
  forbids re-parenting in that window.
- A `Dynamic` `Rigidbody2D` is simulated in **world space**. Moving its parent `Transform` out from under it
  fights the body's own physics, which shows up as jitter.
- A child inherits its parent's **scale** — so the platform's scale would deform the player.

**Inherit the movement**: the platform already knows how far it moved this step. Each physics step, it shifts
whatever is standing on it by that same amount — the *delta*, the change in its position since the previous
step. No re-parenting, so none of the three problems above. And because the shift is applied straight to the
rider's `Rigidbody2D.position` rather than bolted on as a "platform velocity" term the motor has to add,
**the player's movement code never learns platforms exist**: the dash and wall-jump you write in M8 keep
working without a line of change. That is the one this guide takes.

> Build vs borrow — **there is nothing to borrow.** Unity has no rider solution for dynamic bodies on moving
> platforms; every engine leaves this to you, and the reason is that "who is standing on me" is a game design
> question dressed as a physics one. Recorded in
> [the decision log](../foundation/decision-log.md#d14--build-vs-borrow-carrying-a-rider-on-a-moving-platform).

The logic lives **on the platform**, not on the player. The platform is the one that knows it moves.

## Do this

1. Create a new MonoBehaviour script in `Assets/_Project/Scripts` named **`PlatformRiderCarrier`** and replace
   its contents with this:

   ```csharp
   // Assets/_Project/Scripts/PlatformRiderCarrier.cs — the whole file
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
           // NOT also drag them by friction — that double-counts and pushes the rider
           // a hair ahead of the platform. Install a frictionless material so our
           // delta is the only horizontal carry. Skip it if one was set on purpose.
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

   Three details are deliberate. **Detection is a fresh overlap query each step, not `OnCollisionEnter2D`/`Exit2D`
   bookkeeping.** It is tempting to add a rider when it starts touching and remove it when it stops — but a
   kinematic platform sliding *into* a body it is already carrying makes the contact solver fire those callbacks
   erratically, so the "who is riding me" set flickers and the rider stutters or is dropped. Asking "what is on
   my top edge right now?" every step is immune to that. **The delta is measured, not read from
   `MovingPlatform`**: the carrier caches `platformBody.position` itself and subtracts, so it works with *any*
   way of moving the platform and needs no reference to the other script. **The top surface is made
   frictionless.** A kinematic body moved with `MovePosition` still drags a resting body *a little* by contact
   friction. Left alone, that partial drag *plus* the full delta we add makes the rider creep ahead of the
   platform — a hair too fast, always in the travel direction. Zeroing the surface friction leaves our delta as
   the only horizontal carry, so the rider tracks the platform exactly.

2. Save, let Unity compile, and drag `PlatformRiderCarrier.cs` onto the **`MovingPlatform`** parent object —
   the same object that carries the `Rigidbody 2D` and `Box Collider 2D` from [step 02](02_moving-platform.md).
   The `[RequireComponent]` attributes mean Unity will refuse to add it anywhere without both, which is your
   guard against dropping it on the wrong object.

3. In the Inspector, set **Rider Layers** to **`Default`** — the layer the `Player` sits on, unchanged since
   [M4 step 02](../MILESTONE_4_gravity-and-jumping/02_ground-layer.md). Leave **Standing Tolerance Units** at
   `0.1`.

4. Save the scene and press **Play**. Jump onto the platform: you travel with it, both ways, without sliding.
   Walk off the end: you fall, and the platform carries on without you.

5. Watch the Hierarchy while the game runs. `Player` **stays at the root of the list** the whole time — it is
   never indented under `MovingPlatform`, because nothing re-parents it. It moves with the platform anyway.
   That is the mechanism: the platform moves the player, it does not adopt it.

## Done when (this step)
- [ ] Standing on the moving platform → the player travels with it, in both directions, without sliding,
      juddering, or creeping ahead of the platform in its travel direction.
- [ ] Walking or jumping off → the player stops travelling and the platform continues.
- [ ] In the Hierarchy during Play Mode, `Player` **stays a root object** at all times — it is never
      re-parented under `MovingPlatform`.
- [ ] Jumping while riding still works, and the jump goes straight up relative to the level rather than being
      dragged sideways beyond the platform's own speed.
- [ ] The Console shows **no red entries** — in particular, no `Cannot set the parent … while activating`
      error; the project compiles.

## Suggested commit
```
feat(level): carry riders by moving them with the platform, no re-parenting
```

## If it breaks
- **The Console shows `Cannot set the parent of the GameObject 'Player' while activating or deactivating the
  parent GameObject`** → you are running an older `SetParent`-based version of this script. The script above
  does not re-parent anything; replace the whole file with it. (Re-parenting from a collision callback fails
  because contacts can fire while the platform is still being activated on scene load.)
- **The player is carried while jumping past the platform's edge** → `standingToleranceUnits` (the detection
  strip's height) is far too large; the strip reaches up to where the player passes. `0.1` is right; `1` would
  grab anything within a unit of the top.
- **The player is never carried** → **Rider Layers** is `Nothing`, or the `Player` is not on `Default`; or the
  script is on the child `Visual` object, whose collider does not exist, so `platformCollider.bounds` is the
  wrong box. It belongs on the `MovingPlatform` parent that carries the `Box Collider 2D`.
- **The rider creeps a hair ahead of the platform, in the travel direction** → the surface is still dragging it
  by friction on top of the delta. A `Material` was assigned to the platform's `Box Collider 2D`, so the
  carrier's `sharedMaterial == null` guard skipped installing the frictionless one. Set that material's
  **Friction** to `0`, or clear the field so the carrier installs its own.
- **Riding is jittery** → the platform's `Rigidbody 2D` **Interpolate** is `None`, or the platform is being
  moved from `Update` instead of `FixedUpdate`.

---
> Nav: [← A platform that travels](02_moving-platform.md) · [Overview](00_overview.md) · [Verify →](04_verify.md)
