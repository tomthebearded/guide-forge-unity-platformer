# M7 · Step 02 of 04 — A platform that travels
> Nav: [← A ledge you can jump up through](01_one-way-platform.md) · [Overview](00_overview.md) · [Carry the rider →](03_carry-the-rider.md)

**Before you start:** [step 01](01_one-way-platform.md) finished — the one-way ledge works and **Ground
Layers** includes `Ground` and `OneWay`.

## Why / design
Three decisions are packed into this platform, and each of them prevents a specific bug.

**It is `Kinematic`, not `Dynamic`.** A dynamic body is moved *by* physics — gravity pulls it, collisions
shove it, and a player landing on it would push it out of its path. A kinematic body is moved only by your
code, but still collides with everything. That is exactly a moving platform.

**It is moved with `MovePosition`, not by writing `transform.position`.** Both change where the object is;
only `MovePosition` tells the physics engine it is *moving*, so contacts are resolved along the way instead of
the object teleporting into whatever is standing on it. It is also the reason the platform does not shove the
player through the floor at the end of its travel.

**Its root object has a scale of `1, 1, 1`, and the sprite lives on a child.** This looks like fussiness and
is not: the root is the object that carries the `Box Collider 2D`, and a collider is **stretched by its
object's transform scale**. Size the collider in units on a scale-`1` object and the physics box matches the
numbers you typed; give the root a scale of `4, 0.25, 1` instead and the same collider is silently squashed to
a different shape than the sprite (non-uniform scale on a collider is a classic source of "it looks right but
collides wrong"). So the root carries the collider — sized in units, since there is no scale to stretch it —
and a child carries the stretched sprite.

## Do this

1. In the **Hierarchy**, right-click and choose **Create Empty**. Rename it **`MovingPlatform`**. Set its
   `Transform` **Position** to a clear stretch of your cavern — this guide uses `X 8`, `Y -2`, `Z 0` — and
   **leave Scale at `1, 1, 1`**. That last part is load-bearing: it keeps the collider you size next undistorted.

2. Right-click `MovingPlatform` and choose **2D Object > Sprites > Square** to create it as a **child**.
   Rename the child **`Visual`**, set its `Transform` **Position** to `0, 0, 0` and its **Scale** to
   `4, 0.5, 1`, and give its `Sprite Renderer` a colour that stands out — this guide uses `C56C86`.

3. Select `MovingPlatform` (the parent) and **Add Component > Box Collider 2D**. Set **Size** to `4, 0.5` and
   **Offset** to `0, 0`. This is where the units go, since the parent has no scale to stretch anything.

4. Still on the parent, **Add Component > Rigidbody 2D**. Set **Body Type** to **`Kinematic`**, and
   **Interpolate** to **`Interpolate`** so the movement is smooth between physics steps. Leave everything else
   at its default.

5. Set `MovingPlatform`'s **Layer** to **`Ground`** — it is something you stand on and jump from, and the
   ground check reads that layer. When Unity asks whether to change the children too, choose **Yes, change
   children**: the `Visual` child has no collider, so its layer is irrelevant, and keeping them consistent
   avoids confusion later.

6. Create a new MonoBehaviour script in `Assets/_Project/Scripts` named **`MovingPlatform`** and replace its
   contents with this:

   ```csharp
   // Assets/_Project/Scripts/MovingPlatform.cs — the whole file
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

7. Save, let Unity compile, and drag `MovingPlatform.cs` onto the **`MovingPlatform`** object (the parent).
   Confirm the Inspector shows **Travel Offset Units** `6, 0` and **Speed Units Per Second** `2`.

8. With the object selected, look at the **Scene** view: a cyan line runs from the platform to the far end of
   its path, with a small sphere at the end. Drag the platform around and the path follows it.

9. Save the scene and press **Play**. The platform slides right, stops, slides back, and repeats. Jump onto
   it: you land, and it moves out from under you — the player stays put while the platform leaves. That is the
   rider problem, and [step 03](03_carry-the-rider.md) is entirely about it.

## Done when (this step)
- [ ] `MovingPlatform` (parent) has `Transform` Scale `1, 1, 1`, a `Box Collider 2D` of Size `4, 0.5`, a
      `Rigidbody 2D` set to **Kinematic** and **Interpolate**, Layer `Ground`, and the `Moving Platform
      (Script)`.
- [ ] Its child `Visual` carries the `Sprite Renderer` and the Scale `4, 0.5, 1`.
- [ ] Selecting the platform shows a cyan path line in the Scene view ending in a small sphere.
- [ ] Pressing Play → the platform travels six units, reverses, and repeats, smoothly and for ever.
- [ ] Standing on it → the player is **not** carried: the platform slides away underneath. (Step 03 fixes
      this; nothing here is broken.)
- [ ] Jumping while standing on the platform works — the ground check sees its `Ground` layer.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(level): add a kinematic moving platform on a ping-pong path
```

## If it breaks
- **The platform falls out of the level when you press Play** → its `Rigidbody 2D` is `Dynamic`. Kinematic
  bodies ignore gravity; dynamic ones do not.
- **The platform judders** → **Interpolate** is `None`, or the movement is being written from `Update`
  instead of `FixedUpdate`.
- **The platform shoves the player through the floor at the end of its travel** → `transform.position` is
  being written instead of `MovePosition`.
- **The platform does not move at all** → `Travel Offset Units` is `0, 0`, which makes the path length zero
  and the division in `FixedUpdate` produce `NaN`. Give it a non-zero offset.
- **The collider does not match the sprite** → the parent has a scale other than `1, 1, 1`, stretching the
  unit-sized `Box Collider 2D` into a different shape. Move the scale onto the `Visual` child and leave the
  parent at `1, 1, 1`.

---
> Nav: [← A ledge you can jump up through](01_one-way-platform.md) · [Overview](00_overview.md) · [Carry the rider →](03_carry-the-rider.md)
