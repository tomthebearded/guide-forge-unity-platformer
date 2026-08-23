# M10 · Step 01 of 07 — Dress the player
> Nav: — · [Overview](00_overview.md) · [Animate the player →](02_animate-the-player.md)

**Before you start:** M9's gate passed. You need the Kenney character sprites in the project — if you only
copied the tiles in [M6 step 01](../MILESTONE_6_tilemap-level/01_import-the-art.md), go back to the unzipped
pack now and copy the character images into `Assets/ThirdParty/Kenney/PixelPlatformer`, then apply the same
import settings: **Pixels Per Unit `18`**, **Filter Mode `Point (no filter)`**, **Compression `None`**.

## Glossary for this step
> New here: **[sorting layer](../foundation/glossary.md#sorting-layer)** (defined in *Do this*, action 3).

## Why / design
The square has done its job. It was the right shape for eight milestones because it made the *movement*
legible: when a jump feels wrong you want to see a rectangle's arc, not a character's animation.

Two things change when a real sprite arrives, and both are worth doing deliberately.

**The collider stays exactly as it is.** One unit wide, one unit tall, and it is what physics sees. If your
chosen character is a little taller or narrower than that, leave the collider alone: a sprite that is slightly
larger than its collider is completely normal — it is how platformers stop the player catching on corners that
look like they should fit.

**Draw order stops being an accident.** Until now everything sat at `Z = 0` and Unity drew it in whatever order
it liked. Once there is a background (in [step 04](04_parallax.md)) that has to be *behind* and a foreground
that has to be *in front*, you need to say so.

## Do this

1. Select the `Player` in the **Hierarchy**. In its `Sprite Renderer`, click the small circle at the right of
   the **Sprite** field and pick one of the Kenney character sprites from the picker — a standing pose.

2. Set the `Sprite Renderer` **Color** back to pure white, hex `FFFFFF`. Anything else tints the artwork; the
   orange was only ever a stand-in.

3. Set the draw order properly. In the `Sprite Renderer`, open the **Sorting Layer** dropdown and choose
   **Add Sorting Layer…**. Add three, in this order: **`Background`**, **`Level`**, **`Player`**.

   > New concept — **sorting layer**: a named draw order for 2D renderers. Layers are drawn in the order they
   > appear in the list, so anything on `Background` is painted before — and therefore behind — anything on
   > `Level`. Within one layer, **Order in Layer** breaks the tie. It has nothing to do with the physics
   > layers from M4; the two lists are independent and unfortunately similarly named.

4. Come back to the `Player`'s `Sprite Renderer` and set its **Sorting Layer** to **`Player`**.

5. Select **`GroundTilemap`** in the Hierarchy. Its `Tilemap Renderer` has the same two fields: set
   **Sorting Layer** to **`Level`**.

6. Set the same on the objects that belong with the level: `OneWayLedge`, `MovingPlatform/Visual`, and the
   `Coin`, `Enemy` and `Checkpoint` **prefabs** (open each prefab, set its `Sprite Renderer`'s Sorting Layer to
   `Level`, leave Prefab Mode). Editing the prefabs is what makes every placed instance follow.

7. Save the scene and press **Play**. The character now runs, jumps, dashes and dies exactly as the square did
   — because nothing about the physics changed. It just does not animate yet.

## Done when (this step)
- [ ] The `Player` renders as a Kenney character sprite, untinted, at roughly one tile tall.
- [ ] **Project Settings > Tags and Layers > Sorting Layers** lists `Background`, `Level`, `Player` in that
      order (after the built-in `Default`).
- [ ] `Player`'s Sorting Layer is `Player`; `GroundTilemap`, `OneWayLedge`, the platform visual and the three
      prefabs are on `Level`.
- [ ] The player is drawn **in front of** the tiles when it stands against a wall.
- [ ] Everything from M9 still behaves: coins, enemies, stomping, lives, checkpoints.
- [ ] The Console shows no red entries.

## Suggested commit
```
feat(player): replace the placeholder square with the character sprite
```

## If it breaks
- **The character is enormous or tiny** → its import settings differ from the tiles'. It needs the same
  **Pixels Per Unit `18`**.
- **The character is blurry while the tiles are sharp** → **Filter Mode** is `Bilinear` on the character.
- **The player disappears behind the tilemap** → its Sorting Layer is `Level` or `Default` rather than
  `Player`, or the sorting layer list is in the wrong order. Drag the entries in Project Settings to reorder.
- **The sprite hangs oddly on the collider** → expected if the art is not square. Do not resize the collider
  to match: `1, 1` is what M4's and M8's tuning assumes, and changing it silently changes the jump.

---
> Nav: — · [Overview](00_overview.md) · [Animate the player →](02_animate-the-player.md)
