# M3 · Step 03 of 06 — Give the world a floor
> Nav: [← Read the Move action](02_input-reader.md) · [Overview](00_overview.md) · [Give the player a body →](04_rigidbody-and-collider.md)

**Before you start:** `PlayerInputReader` is attached to `Player` and printing values
([step 02](02_input-reader.md)). Play Mode is **stopped** — everything below is scene editing, and scene edits
made while playing are discarded.

## Glossary for this step
> New here: **[collider](../foundation/glossary.md#collider)** (defined in *Why / design*).

## Why / design
Physics needs something to collide *with*, so the floor comes before the body that lands on it. A floor in
Unity is two things bolted to one GameObject: a **sprite** so you can see it, and a **collider** so the physics
engine knows it is solid. They are independent — a collider with no sprite is an invisible wall, a sprite with
no collider is a painted backdrop — and keeping that distinction straight is most of 2D physics.

> New concept — **collider**: the shape the physics engine uses for an object. It has nothing to do with what
> is drawn: `BoxCollider2D` is a rectangle described in **world units**, positioned by its `Offset` and sized
> by its `Size`, and physics only ever sees that rectangle.

This floor is temporary scaffolding with a job to do: it lets M3, M4 and M5 test movement, gravity and jump
feel against something solid. M6 replaces it with a painted Tilemap.

## Do this

1. In the **Hierarchy**, right-click empty space and choose **2D Object > Sprites > Square**. Rename the new
   object to **`Ground`** (cosmetic — no code refers to it by name).

2. In its `Transform`, set **Position** to `X 0`, `Y -4`, `Z 0`, and **Scale** to `X 30`, `Y 1`, `Z 1`. That
   makes a strip 30 units wide and 1 unit tall, four units below the origin — wide enough to run on for
   several seconds at the speed you set in [step 05](05_move-with-velocity.md).

   Confirm the numbers against the Scene view's grid, where **one cell is one unit**: the strip should span
   thirty cells across and one down. If it spans a different number, your Unity version's built-in square is
   not one unit at scale 1 — divide both scale numbers by however many units it does measure, and carry that
   same correction into [step 04](04_rigidbody-and-collider.md).

3. In the `Sprite Renderer`, set **Color** to a dark slate, hex `39424E`. Cosmetic: any colour that is not the
   player's orange will do.

4. With `Ground` still selected, click **Add Component** at the bottom of the Inspector, type `Box Collider
   2D`, and press Enter. A green outline appears around the strip in the Scene view — that outline is the
   collider, and from now on it is what "the floor" means to physics.

5. Check the collider's numbers in the Inspector. **Size** must read `1` by `1` and **Offset** `0`, `0`.
   Those are *local* units: the `Transform`'s scale of `30, 1` stretches the same rectangle to 30 × 1 in the
   world. Leave **Is Trigger** unticked and every other field at its default — a trigger detects overlap
   without stopping anything, which is the opposite of what a floor is for.

6. Save the scene with **Ctrl+S** / **Cmd+S**.

## Done when (this step)
- [ ] The **Game** view shows a dark horizontal strip across the lower part of the frame.
- [ ] Selecting `Ground` shows a green collider outline in the **Scene** view that traces the visible strip
      exactly — not a small square in its middle, not a rectangle spilling past its ends.
- [ ] `Ground`'s Inspector reads: Position `0, -4, 0`, Scale `30, 1, 1`, `Box Collider 2D` with Size `1, 1`,
      Offset `0, 0`, **Is Trigger** unticked.
- [ ] Pressing Play changes nothing about the floor: it just sits there. The `Player` still drifts right and
      passes straight through it — it has no body yet, which is [step 04](04_rigidbody-and-collider.md).

## Suggested commit
```
feat(level): add a temporary ground strip with a box collider
```

## If it breaks
- **The green outline is a 1 × 1 square in the middle of the strip** → you edited the collider's `Size` instead
  of the `Transform`'s `Scale`. Set `Size` back to `1, 1` and put the 30 in the Transform: scaling the
  transform keeps the sprite and the collider in step with each other, editing the collider does not.
- **The strip hides the player** → both sprites are at `Z = 0` and their draw order is undefined. Set the
  `Ground`'s `Sprite Renderer` → **Order in Layer** to `-1` so it draws behind.
- **There is no `Box Collider 2D` in the Add Component list, only `Box Collider`** → you picked the 3D
  collider. The 2D components all end in `2D`; type the `2D` in the search box to filter them.

---
> Nav: [← Read the Move action](02_input-reader.md) · [Overview](00_overview.md) · [Give the player a body →](04_rigidbody-and-collider.md)
