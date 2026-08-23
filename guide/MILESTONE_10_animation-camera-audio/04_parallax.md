# M10 · Step 04 of 07 — A background with depth
> Nav: [← The camera follows](03_cinemachine-camera.md) · [Overview](00_overview.md) · [Sound effects →](05_sound-effects.md)

**Before you start:** [step 03](03_cinemachine-camera.md) finished — the camera follows the player and stops
at the level bounds.

## Why / design
Now that the camera moves, the flat colour behind the cavern is conspicuously flat. **Parallax** fixes it with
one observation: things far away appear to move less. Shift a background layer by a *fraction* of the camera's
movement and the eye reads it as distant.

The fraction is the only parameter. `0` means the layer is nailed to the camera — infinitely far away, like a
sky. `1` means it moves exactly with the world, which is what the level itself does. Anything between is a
distance. Two layers at `0.2` and `0.5` are enough to sell it.

> Build vs borrow — **nothing to borrow**: this is six lines of arithmetic and one `LateUpdate`. Adding a
> dependency for it would be worse than writing it, which is the test from
> [`../foundation/decision-log.md`](../foundation/decision-log.md).

It goes in `LateUpdate` for the same reason the camera does: it must run **after** the camera has moved this
frame, or the background lags one frame behind and shimmers.

## Do this

1. Create a new MonoBehaviour script in `Assets/_Project/Scripts` named **`ParallaxLayer`**:

   ```csharp
   // Assets/_Project/Scripts/ParallaxLayer.cs — the whole file
   using UnityEngine;

   // Moves this layer by a fraction of the camera's movement, so it reads as distant.
   // 0 = pinned to the camera (infinitely far), 1 = moves with the world (not parallax).
   public class ParallaxLayer : MonoBehaviour
   {
       [SerializeField, Range(0f, 1f)] private float parallaxFactor = 0.3f;

       private Transform cameraTransform;
       private Vector3 previousCameraPosition;

       private void Start()
       {
           // Camera.main finds the camera tagged MainCamera. Once, in Start —
           // it is a scene search, not something to do every frame.
           cameraTransform = Camera.main.transform;
           previousCameraPosition = cameraTransform.position;
       }

       // LateUpdate runs after every Update — and, crucially, after Cinemachine has
       // moved the camera this frame.
       private void LateUpdate()
       {
           Vector3 cameraMovement = cameraTransform.position - previousCameraPosition;

           // Move with the camera by the missing fraction: a factor of 0.2 means the
           // layer keeps 80% of the camera's movement, so it appears to lag behind.
           transform.position += new Vector3(cameraMovement.x, cameraMovement.y, 0f) * (1f - parallaxFactor);

           previousCameraPosition = cameraTransform.position;
       }
   }
   ```

2. Build the far layer. In the **Hierarchy**, create a **2D Object > Sprites > Square**, rename it
   **`BackgroundFar`**, and set:
   - `Transform` **Position** `0, 0, 10` — a positive Z puts it behind everything the camera sees.
   - `Transform` **Scale** `60, 30, 1` — deliberately much larger than the level, so its edges never come
     into view.
   - `Sprite Renderer` **Color**: a dark blue, hex `1B2A3A`.
   - `Sprite Renderer` **Sorting Layer**: **`Background`** (added in [step 01](01_dress-the-player.md)),
     **Order in Layer** `0`.

3. Build the near layer the same way: **`BackgroundNear`**, Position `0, -2, 9`, Scale `50, 16, 1`, Color a
   lighter slate `24384D`, Sorting Layer `Background`, **Order in Layer** `1` — so it draws in front of
   `BackgroundFar`.

   If your Kenney download included background images, use one of those as the sprite instead of a flat
   colour; the layering and the factors below are identical either way.

4. Drag `ParallaxLayer.cs` onto both objects. Set **Parallax Factor** to `0.2` on `BackgroundFar` and `0.5` on
   `BackgroundNear`.

5. Save the scene and press **Play**. Run from one end of the level to the other: the far layer barely moves,
   the near layer moves a little, and the cavern moves fully. Stand still and jump — the same effect
   vertically, more subtly.

6. Prove the factor is what does it. Stop, set both factors to `1`, and play again: the background is nailed
   to the level and the depth disappears completely. Set them back to `0.2` and `0.5`, and save.

## Done when (this step)
- [ ] Running the length of the level moves `BackgroundFar` visibly less than `BackgroundNear`, and both less
      than the cavern.
- [ ] Neither background's edge ever comes into view, at either end of the level or at the top of a jump.
- [ ] Both layers stay **behind** the tilemap, the player, the coins and the enemies at all times.
- [ ] With both **Parallax Factor** values set to `1`, the depth effect disappears; restoring `0.2` and `0.5`
      brings it back.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(level): add two parallax background layers
```

## If it breaks
- **`NullReferenceException` in `Start`** → `Camera.main` found nothing, which means `Main Camera` lost its
  **MainCamera** tag. Set the tag back at the top of its Inspector.
- **The background drifts away over time** → the factor is above `1`, or `previousCameraPosition` is not being
  updated at the end of `LateUpdate`.
- **The background shimmers or lags one frame** → the movement is in `Update` rather than `LateUpdate`, so it
  runs before Cinemachine moves the camera.
- **The background covers the level** → its Sorting Layer is not `Background`, or its Z is negative. Both must
  be right: sorting layers decide 2D draw order, and Z decides what the camera can see.
- **The background edges come into view** → make the layers bigger. There is no cost to a background that is
  twice the size it needs to be.

---
> Nav: [← The camera follows](03_cinemachine-camera.md) · [Overview](00_overview.md) · [Sound effects →](05_sound-effects.md)
