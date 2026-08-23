# M3 · Step 04 of 06 — Give the player a body
> Nav: [← Give the world a floor](03_ground-platform.md) · [Overview](00_overview.md) · [Move the body with velocity →](05_move-with-velocity.md)

**Before you start:** the `Ground` strip from [step 03](03_ground-platform.md) exists with its collider, and
Play Mode is stopped.

## Glossary for this step
> New here: **[Rigidbody2D](../foundation/glossary.md#rigidbody2d)** (defined in *Why / design*) ·
> **[body type](../foundation/glossary.md#body-type)** (defined in *Do this*, action 3).

## Why / design
This is the decision the whole guide is built on, so it is worth thirty seconds of your attention.

Adding a **`Rigidbody2D`** hands the object to the physics engine. From that moment **the engine owns the
transform**: it integrates gravity, resolves collisions, and writes the position every physics step. Your code
stops writing `transform.position` — the two would fight, and the engine would win half a frame later, which
looks like jitter and is impossible to debug.

> New concept — **`Rigidbody2D`**: the component that puts a GameObject under 2D physics simulation. Once it
> is there, you steer by setting **velocity** and let the engine place the object. Documentation:
> <https://docs.unity3d.com/6000.3/Documentation/Manual/2d-physics/rigidbody/introduction-to-rigidbody-2d.html>

That is why the first thing this step does is delete `ConstantMover`. It writes `transform.position` every
frame, and leaving it attached would produce exactly the fight described above.

> The engine gives you gravity and collision resolution for free; what it does not give you is anything that
> feels good. Everything from M5 onward is you shaping the *intent* — how fast, how high, how forgiving —
> while the engine keeps doing the arithmetic.

## Do this

1. Select `Player` in the **Hierarchy**. On the `Constant Mover (Script)` component, click the **⋮** menu at
   its top right and choose **Remove Component**.

2. In the **Project** panel, right-click `Assets/_Project/Scripts/ConstantMover.cs` and choose **Delete**,
   then confirm. Its job is done and dead scripts rot: it is in your Git history if you ever want to read it
   again.

3. With `Player` selected, click **Add Component**, type `Rigidbody 2D`, and press Enter. Set these fields —
   **and leave every field not listed here at its default**:

   | Field | Value | Why |
   |---|---|---|
   | **Body Type** | `Dynamic` (already the default) | The engine moves it and collisions affect it. |
   | **Gravity Scale** | `1` (leave it) | M4 tunes this to `4`; the default is enough to prove the body falls. |
   | **Collision Detection** | `Continuous` | Checks the *path* between two physics steps, not just the endpoints — a fast-moving box can otherwise pass through a thin floor entirely. |
   | **Interpolate** | `Interpolate` | Physics runs 50 times a second, your screen redraws more often than that; interpolation smooths the visual between steps instead of showing you the steps. |
   | **Constraints > Freeze Rotation Z** | ticked | Without it the box tips over the first time a corner catches, and a platformer character that topples is a bug in every game ever made. |

   > New concept — **body type**: `Dynamic` (the engine moves it, gravity and collisions apply),
   > `Kinematic` (only your code moves it, but it still collides), `Static` (never moves — the cheapest kind,
   > right for scenery). The player is `Dynamic`.

4. Still on `Player`, click **Add Component** and add a **`Box Collider 2D`**. Set **Size** to `1`, `1` and
   **Offset** to `0`, `0`; leave **Is Trigger** unticked and everything else at its default.

   The `Player`'s `Transform` scale is `1, 1, 1`, so this is a one-unit square in the world — the same size as
   the sprite you are looking at. Check that in the Scene view: the green outline traces the orange square.
   (If [step 03](03_ground-platform.md) needed the scale correction, apply it to the `Transform` here too, and
   leave the collider `Size` at `1, 1`.)

5. Save the scene, then press **Play**. The square falls, lands on the dark strip, and stops. It does not
   rotate, does not sink, and does not drift sideways — nothing is driving it horizontally any more.

6. While still in Play Mode, select `Player` and read its `Transform` **Position Y** in the Inspector: it
   settles at **−3.0**, give or take a hundredth. That is arithmetic you can check — the floor's surface is at
   `−4 + 0.5 = −3.5`, and the player's centre rests half its height above that, at `−3.0`. Two-dimensional
   physics also keeps a hair of separation between touching colliders, so `−2.99` is a pass.

## Done when (this step)
- [ ] `Player`'s Inspector lists `Transform`, `Sprite Renderer`, `Player Input Reader (Script)`,
      `Rigidbody 2D` and `Box Collider 2D` — and **no** `Constant Mover`.
- [ ] `Assets/_Project/Scripts/ConstantMover.cs` no longer exists in the Project panel.
- [ ] Pressing **Play** → the square falls straight down and comes to rest on top of the strip, upright.
- [ ] During Play Mode, `Player`'s `Transform` **Position Y** reads between `-3.00` and `-2.98`, and
      **Rotation Z** stays `0`.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(player): replace ConstantMover with a dynamic Rigidbody2D body
```

## If it breaks
- **The square falls straight through the floor** → the `Ground` has no `Box Collider 2D`, or the player's
  collider is ticked as **Is Trigger**. A trigger detects overlap and stops nothing.
- **The square lands and then slowly tips over** → **Freeze Rotation Z** is unticked.
- **The square jitters or vibrates while resting** → `Interpolate` is set to `None`, or two colliders overlap
  on the same object. Check `Player` has exactly one `Box Collider 2D`.
- **`The referenced script on this Behaviour is missing`** → you deleted `ConstantMover.cs` before removing
  the component, leaving the scene pointing at nothing. Select `Player`, remove the broken component entry
  with its **⋮ > Remove Component**, and save.

---
> Nav: [← Give the world a floor](03_ground-platform.md) · [Overview](00_overview.md) · [Move the body with velocity →](05_move-with-velocity.md)
