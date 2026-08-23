# M7 · Step 03 of 04 — Carry the rider
> Nav: [← A platform that travels](02_moving-platform.md) · [Overview](00_overview.md) · [Verify →](04_verify.md)

**Before you start:** [step 02](02_moving-platform.md) finished — the platform travels, the player can stand
and jump on it, and it slides out from underneath.

## Why / design
Stand on the platform and it leaves without you. Nothing is broken: the platform moves its own body, the
player's body is not touching anything that pushes it sideways, and friction between two colliders is not a
transport system. The player is *resting* on the platform, not *attached* to it.

There are two honest fixes.

**Inherit the velocity**: each step, add the platform's movement to the player's own. Precise, but every piece
of movement code then has to know about a "platform velocity" term, including the dash and wall-jump you write
in M8.

**Re-parent**: make the player a child of the platform while it rides, and a root object again when it leaves.
A child moves with its parent for free — no term, no coupling, and every future movement feature keeps
working without knowing platforms exist. That is the one this guide takes, and it is why
[step 02](02_moving-platform.md) insisted the platform's own transform stays at scale `1, 1, 1`: a child
inherits scale too.

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

   The position test is deliberate. Asking the collision for its contact normals works too, but the direction
   those normals point depends on which collider Unity reports first, and getting it backwards produces a
   platform that carries you only when you hit it from underneath. Comparing two boxes' world coordinates is
   unambiguous.

2. Save, let Unity compile, and drag `PlatformRiderCarrier.cs` onto the **`MovingPlatform`** parent object.

3. In the Inspector, set **Rider Layers** to **`Default`** — the layer the `Player` sits on, unchanged since
   [M4 step 02](../MILESTONE_4_gravity-and-jumping/02_ground-layer.md). Leave **Standing Tolerance Units** at
   `0.1`.

4. Save the scene and press **Play**. Jump onto the platform: you travel with it, both ways, without sliding.
   Walk off the end: you fall, and the platform carries on without you.

5. Check the Hierarchy while the game runs — this is the mechanism made visible. Standing on the platform,
   `Player` appears **indented under `MovingPlatform`**. Step off, and it jumps back to the root of the list.

6. Check the thing that would have broken. Look at the `Player`'s `Transform` **Scale** while riding: it still
   reads `1, 1, 1`. If the platform's own scale were not `1, 1, 1`, this is where the player would visibly
   stretch.

## Done when (this step)
- [ ] Standing on the moving platform → the player travels with it, in both directions, without sliding or
      juddering.
- [ ] Walking or jumping off → the player stops travelling and the platform continues.
- [ ] In the Hierarchy during Play Mode, `Player` is a child of `MovingPlatform` while riding and a root
      object again after leaving.
- [ ] The player's `Transform` Scale reads `1, 1, 1` at all times, riding or not.
- [ ] Jumping while riding still works, and the jump goes straight up relative to the level rather than being
      dragged sideways beyond the platform's own speed.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(level): carry riders by re-parenting them to the moving platform
```

## If it breaks
- **The player is carried when it touches the platform's side or underside** → `IsStandingOnTop` is not being
  consulted, or `standingToleranceUnits` is far too large. `0.1` is right; `1` would adopt anything nearby.
- **The player stretches or shrinks when it steps on** → the platform parent's scale is not `1, 1, 1`. Move
  the scale to the `Visual` child, as [step 02](02_moving-platform.md) describes.
- **The player is never adopted** → **Rider Layers** is `Nothing`, or the `Player` is not on `Default`. This
  is the same class of mistake as an unset **Ground Layers** mask.
- **The player stays a child after jumping off** → `OnCollisionExit2D` is not firing because the script is on
  the child `Visual` object rather than on the parent that owns the collider.
- **Riding is jittery** → the platform's `Rigidbody 2D` **Interpolate** is `None`, or the platform is being
  moved from `Update`.

---
> Nav: [← A platform that travels](02_moving-platform.md) · [Overview](00_overview.md) · [Verify →](04_verify.md)
