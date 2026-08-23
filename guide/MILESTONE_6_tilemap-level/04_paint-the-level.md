# M6 · Step 04 of 06 — Paint the cavern
> Nav: [← Make a Rule Tile](03_rule-tile.md) · [Overview](00_overview.md) · [One collider for the whole level →](05_composite-collider.md)

**Before you start:** [step 03](03_rule-tile.md) finished — `GroundRuleTile` is in the palette and shows its
default sprite. Play Mode is stopped.

## Glossary for this step
> New here: **[Tilemap](../foundation/glossary.md#tilemap)** (defined in *Why / design*) ·
> **[Grid](../foundation/glossary.md#grid)** (defined in *Do this*, action 1).

## Why / design
A **Tilemap** stores which tile sits in which cell of a grid. That is the whole idea, and it is why a level of
two thousand blocks costs almost nothing: there are not two thousand GameObjects, there is one component with
a very efficient dictionary and a renderer that batches the lot.

> New concept — **Tilemap**: the component that stores tiles by grid position and draws them. Painting into it
> edits data, not objects.

It also changes what "editing the level" means. Adding a ledge is three brush strokes, not three GameObjects
with three colliders to align. This is the *level is content, not code* rule from
[`../foundation/conventions.md`](../foundation/conventions.md), and M9 leans on it again: coins and enemies are
prefabs you drop on top of the painted world.

**One design constraint governs everything you paint**: your jump clears about **2.4 units**, measured back in
M4 — and one unit is one tile. So a step up of **two tiles** is comfortable, three is a jump you will
sometimes miss, and four is impossible. Build the cavern out of one- and two-tile steps.

## Do this

1. In the **Hierarchy**, right-click empty space and choose **2D Object > Tilemap > Rectangular**. Unity
   creates a **`Grid`** object with a **`Tilemap`** child.

   > New concept — **Grid**: the parent component defining the cell size and layout that its child Tilemaps
   > share. Leave its **Cell Size** at `1, 1, 0` — one cell, one unit, one tile, which is exactly what the
   > PPU of 18 from [step 01](01_import-the-art.md) bought you.

2. Rename the child `Tilemap` object to **`GroundTilemap`**. Leave the parent named `Grid`.

3. In the **Tile Palette** window, make sure **`GroundTilemap`** is selected in the window's **Active Tilemap**
   dropdown at the top — this is where paint lands, and painting into the wrong tilemap is the most common
   confusion of the milestone.

4. Select `GroundRuleTile` in the palette, pick the **brush** tool (**B**), and paint in the **Scene** view:

   - A **floor** running from about `x = -12` to `x = 26` at `y = -4`, two or three cells thick. Drag to paint
     a run of cells.
   - Two or three **ledges** above it — each **one or two cells** higher than what you would jump from, and
     three to six cells long. Space them a few cells apart horizontally so a jump has somewhere to land.
   - A **wall** three or four cells tall at each end, so the player cannot run off into nothing.

   Exact positions are **illustrative** — this is your level. The mandatory parts are: never a vertical step
   greater than two cells, and a floor long enough to build up to full speed on (ten cells or more).

   Watch the Rule Tile work as you paint: the top row picks up the surface sprite, the sides pick up edges,
   and extending a platform re-tiles its old end automatically.

5. Fix mistakes with the **eraser** (**D**) — the same drag, holding the eraser tool.

6. Move the `Player` to a sensible starting point: select it and set its `Transform` **Position** to a spot
   just above your floor, for example `X -10`, `Y -2`, `Z 0`. It falls onto the floor when you press Play.

7. The old grey strip is now in the way and does nothing useful. Select the **`Ground`** object in the
   Hierarchy and press **Delete**.

8. Press **Play**. The player falls onto… nothing. It drops straight through the painted tiles and out of the
   frame — the tilemap is a picture until [step 05](05_composite-collider.md) gives it a collider. That is
   expected, and it is one step away.

9. Save the scene and commit.

## Done when (this step)
- [ ] The Hierarchy shows `Grid` with a child named exactly `GroundTilemap`, and no `Ground` object.
- [ ] The Scene view shows a painted cavern: a floor at least ten cells long, two or three ledges no more
      than two cells above their approach, and end walls.
- [ ] The tiles line up exactly with the grid — no gaps, no overlaps, and the top row shows the surface
      sprite while the rows below show the interior.
- [ ] Pressing **Play** → the player falls through the painted floor (no collider yet).
- [ ] The Console shows no red entries.

## Suggested commit
```
feat(level): paint the cavern tilemap and remove the temporary ground
```

## If it breaks
- **Nothing appears when you paint** → the **Active Tilemap** dropdown points at another tilemap, or no tile
  is selected in the palette. Both live at the top of the Tile Palette window.
- **The tiles are half a cell out of alignment** → the `Grid`'s **Cell Size** is not `1, 1, 0`, or the tiles
  were imported at a PPU other than `18`.
- **Every cell shows the same sprite regardless of position** → the Rule Tile's rules did not apply. Check
  step 03's rule order: the empty catch-all must be last.
- **Painting in the Scene view moves objects instead** → the Scene view is in the Move tool rather than the
  brush. Click the brush in the Tile Palette window again; the Scene view switches to painting.
- **The player now falls for ever** → correct at this point in the milestone. Step 05 is the collider.

---
> Nav: [← Make a Rule Tile](03_rule-tile.md) · [Overview](00_overview.md) · [One collider for the whole level →](05_composite-collider.md)
