# M10 · Step 03 of 07 — The camera follows
> Nav: [← Animate the player](02_animate-the-player.md) · [Overview](00_overview.md) · [A background with depth →](04_parallax.md)

**Before you start:** [step 02](02_animate-the-player.md) finished — the character animates from its movement
parameters.

## Glossary for this step
> New here: **[Cinemachine brain](../foundation/glossary.md#cinemachine-brain)** (defined in *Why / design*).

## Why / design
The camera has not moved since M1, which is why the level has had to fit on one screen. A follow camera is ten
lines of `LateUpdate` — and then it needs damping so it does not snap, then look-ahead so you can see where
you are going, then bounds so it stops at the edge of the level, and by then it is not ten lines any more.

Cinemachine ships all of that. You add a **`CinemachineCamera`**, point its `Follow` field at the player, and
tune the rest in the Inspector.

> New concept — **Cinemachine brain**: the component that goes on the real `Camera` and drives it towards
> whichever `CinemachineCamera` is currently active. Cinemachine cameras do not render anything themselves —
> they are targets, and the brain does the moving.

**One warning that will save you an hour.** Cinemachine 3 renamed nearly everything, and the internet is full
of Cinemachine 2 tutorials. `CinemachineVirtualCamera` is now **`CinemachineCamera`**; the Framing Transposer
is now the **Position Composer**; the namespace is `Unity.Cinemachine`. If a tutorial tells you to add a
component that does not exist in your Add Component menu, that is why.

## Do this

1. Open **Window > Package Manager**, switch to **Unity Registry**, find **Cinemachine**, and press
   **Install**. Confirm afterwards that the installed version is **3.x** — the *In Project* list shows the
   number. This edits `Packages/manifest.json`, which is tracked.

2. In the **Hierarchy**, right-click and choose **Cinemachine > 2D Camera**. Unity creates a
   `CinemachineCamera` object and, at the same time, adds a **`CinemachineBrain`** component to your
   `Main Camera`.

3. Select the new camera object. Rename it **`FollowCamera`**. In its `Cinemachine Camera` component, set
   **Follow** to the **`Player`** object by dragging `Player` from the Hierarchy into the field.

4. Below that, the component list shows a **Position Control** section holding a
   **`Cinemachine Position Composer`**. Set these, leaving everything else at its default:

   | Field | Value | Why |
   |---|---|---|
   | **Damping** | `1` on X, `1` on Y | How long the camera takes to catch up, in seconds. `0` snaps and reads as jitter; `1` is calm without feeling late. |
   | **Lookahead Time** | `0.3` | The camera drifts ahead of where the player is going, so you can see what you are running into. |
   | **Lookahead Smoothing** | `5` | Stops the look-ahead darting about when you change direction. |
   | **Dead Zone Width / Height** | `0.15` / `0.2` | A small region in the middle where the player can move without the camera reacting at all. This is what stops a jump from making the whole screen bounce. |

5. Stop the camera showing the void. Select the `Grid` object in the Hierarchy and **Add Component >
   Polygon Collider 2D**. Tick **Is Trigger** on it — it exists to describe an area, not to block anything —
   and use the **Edit Collider** button to drag its points into a rectangle that covers the whole painted
   cavern, plus a couple of units of margin.

6. Select `FollowCamera` again, press **Add Extension** at the bottom of the `Cinemachine Camera` component,
   and choose **Cinemachine Confiner 2D**. Drag the `Grid` object into its **Bounding Shape 2D** field, and
   press **Bake** if the component offers it.

7. Save the scene and press **Play**. Run: the camera follows smoothly, drifting slightly ahead of you. Jump:
   the camera barely moves vertically, because of the dead zone. Run to either end of the level: the camera
   stops at the boundary while the player carries on to the wall.

## Done when (this step)
- [ ] **Window > Package Manager > In Project** lists **Cinemachine** at version `3.x`.
- [ ] `Main Camera` carries a `Cinemachine Brain` component, and `FollowCamera` carries a
      `Cinemachine Camera` whose **Follow** is the `Player`.
- [ ] Running moves the camera smoothly, and it settles slightly **ahead** of the player rather than centred
      on them.
- [ ] Jumping straight up does **not** make the camera jump with you.
- [ ] At both ends of the level the camera stops and shows no empty space beyond the painted cavern.
- [ ] Everything from M9 still behaves.
- [ ] The Console shows no red entries.

## Suggested commit
```
feat(camera): follow the player and confine to the level
```

## If it breaks
- **The Add Component menu has no `CinemachineCamera`** → you are following a Cinemachine 2 tutorial, or the
  package did not finish installing. In Cinemachine 3 the menu entry is **Cinemachine > 2D Camera**.
- **The camera does not move at all** → `Main Camera` has no `Cinemachine Brain`, or **Follow** is empty.
- **The camera judders** → **Damping** is `0`, or the player's `Rigidbody 2D` **Interpolate** was turned off.
  Both must be on for smooth following.
- **The camera shows black beyond the level** → the confiner's bounding shape is smaller than the visible
  area, or the extension was not added. Enlarge the polygon.
- **`Bounding Shape 2D` refuses the Grid object** → the `Polygon Collider 2D` landed on `GroundTilemap`
  instead of `Grid`, or on an object with no collider at all.
- **The camera is confined but stutters at the edge** → the polygon has too many points; a simple rectangle
  confines best.

---
> Nav: [← Animate the player](02_animate-the-player.md) · [Overview](00_overview.md) · [A background with depth →](04_parallax.md)
