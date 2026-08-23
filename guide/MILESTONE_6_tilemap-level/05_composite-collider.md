# M6 · Step 05 of 06 — One collider for the whole level
> Nav: [← Paint the cavern](04_paint-the-level.md) · [Overview](00_overview.md) · [Verify →](06_verify.md)

**Before you start:** [step 04](04_paint-the-level.md) finished — the cavern is painted, the old `Ground`
strip is gone, and the player falls straight through the tiles.

## Glossary for this step
> New here: **[composite collider](../foundation/glossary.md#composite-collider)** (defined in *Why / design*).

## Why / design
`Tilemap Collider 2D` gives every painted cell its own box. That works, and it has one nasty property: a
character sliding across two adjacent boxes can catch on the invisible seam where they meet. The floor is
geometrically flat, and the player trips on nothing. It is the single most reported bug in hand-rolled 2D
platformers, and the cause is never in your movement code.

A **composite collider** merges all those boxes into one outline. Two hundred rectangles become a handful of
edges, seams stop existing because the surfaces they divided are now one surface, and physics gets faster
into the bargain.

> New concept — **composite collider**: a collider that merges the shapes of the colliders set to feed it into
> a single outline. On a Tilemap it removes every internal seam.
> Documentation: <https://docs.unity3d.com/6000.3/Documentation/Manual/2d-physics/collider/composite-collider/composite-collider-2d-reference.html>

One wrinkle: a `Composite Collider 2D` needs a `Rigidbody 2D` beside it, because compositing happens on a
body. Level geometry never moves, so that body's **Body Type** is **`Static`** — the cheapest kind, and the
one that tells the engine this outline can be baked and forgotten.

## Do this

1. Select **`GroundTilemap`** in the Hierarchy. Click **Add Component**, type `Tilemap Collider 2D`, and press
   Enter. The Scene view fills with green outlines — one per painted cell. That is the problem, drawn.

2. With `GroundTilemap` still selected, **Add Component > Composite Collider 2D**. Unity adds a
   **`Rigidbody 2D`** at the same time, because the composite requires one. If for any reason it did not, add
   a `Rigidbody 2D` yourself.

3. On that **`Rigidbody 2D`**, set **Body Type** to **`Static`**. Leave everything else at its default —
   a static body ignores gravity, mass and interpolation entirely.

4. On the **`Tilemap Collider 2D`**, tick **Used By Composite**. The per-cell outlines collapse into a single
   outline tracing the silhouette of the whole cavern. That tick is the entire mechanism.

5. On the **`Composite Collider 2D`**, leave **Geometry Type** at **`Outlines`** and every other field at its
   default.

6. Set the layer. With `GroundTilemap` selected, open the **Layer** dropdown at the top right of the Inspector
   and choose **`Ground`**. If Unity asks about child objects, either answer is fine — there are none.

   **This is load-bearing**: `PlayerMotor`'s ground check only looks at the `Ground` layer, so a tilemap left
   on `Default` is a floor the player stands on but can never jump from.

7. Save the scene and press **Play**. The player lands on the painted floor, runs along it, and jumps between
   the ledges.

8. Now run the experiment that proves the composite is doing something. In Play Mode or out of it, **untick
   Used By Composite** on the `Tilemap Collider 2D` and run the length of the floor at full speed a few times:
   the player catches or stutters on cell boundaries — most noticeably after landing, and worst where a ledge
   meets the floor. **Tick it again** and the same run is smooth. Leave it ticked, and save the scene.

## Done when (this step)
- [ ] `GroundTilemap` carries `Tilemap Collider 2D` (**Used By Composite** ticked), `Rigidbody 2D`
      (**Body Type** `Static`) and `Composite Collider 2D` (**Geometry Type** `Outlines`).
- [ ] The Scene view shows **one** green outline around the painted shape, not one box per cell.
- [ ] `GroundTilemap`'s Layer reads **`Ground`**.
- [ ] Pressing **Play** → the player lands on the painted floor and can run its full length at speed **without
      catching, stuttering or stopping** on any cell boundary.
- [ ] The player can jump from the floor onto every ledge you painted, and back down.
- [ ] With **Used By Composite** unticked, the same full-speed run visibly catches on seams; re-ticking it
      restores the smooth run.
- [ ] The Console shows no red entries.

## Suggested commit
```
feat(level): merge the tilemap colliders into one composite outline
```

## If it breaks
- **The player still falls through** → the `Tilemap Collider 2D` is on the `Grid` parent instead of on
  `GroundTilemap`. It belongs on the object that holds the tiles.
- **The player lands but cannot jump** → the tilemap's layer is not `Ground`, so the ground check finds
  nothing. This is the same failure as an unset **Ground Layers** mask in M4, arriving from the other side.
- **Everything falls apart the moment you press Play, tiles included** → the `Rigidbody 2D` on
  `GroundTilemap` is `Dynamic`. Set it to `Static`.
- **The outline traces the cells but the player passes through anyway** → the composite's **Geometry Type** is
  `Outlines` and the shape is open, or the player's collider is a trigger. Check the player's
  `Box Collider 2D` has **Is Trigger** unticked.
- **The green outline does not update after painting more tiles** → composites regenerate on change; if one
  goes stale, toggling **Used By Composite** off and on forces a rebuild.

---
> Nav: [← Paint the cavern](04_paint-the-level.md) · [Overview](00_overview.md) · [Verify →](06_verify.md)
