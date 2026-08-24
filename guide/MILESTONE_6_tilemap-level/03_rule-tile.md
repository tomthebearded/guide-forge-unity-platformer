# M6 · Step 03 of 06 — Make a Rule Tile
> Nav: [← Build a tile palette](02_tile-palette.md) · [Overview](00_overview.md) · [Paint the cavern →](04_paint-the-level.md)

**Before you start:** [step 02](02_tile-palette.md) finished — `CavernPalette` exists with tile assets in
`Assets/_Project/Art/Tiles`.

## Glossary for this step
> New here: **[Rule Tile](../foundation/glossary.md#rule-tile)** (defined in *Why / design*).

## Why / design
Paint a platform with plain tiles and you will spend the afternoon choosing tiles: a grass-topped one here, a
left corner there, a solid interior in the middle — and re-choosing every one of them the moment you extend
the platform by a cell.

A **Rule Tile** does that choosing. It is a single tile you paint with, carrying a list of rules of the form
*"if my neighbours look like this, draw that sprite"*. Paint a blob of them and the edges, corners and
interiors sort themselves out, including when you edit later.

> New concept — **Rule Tile**: a tile asset that picks its sprite from its neighbours, using a 3 × 3 pattern
> per rule where each neighbouring cell is marked *must be the same tile*, *must not be*, or *don't care*.
> Documentation: <https://docs.unity3d.com/Packages/com.unity.2d.tilemap.extras@latest/>

> One clause on where this comes from: Rule Tiles live in the **2D Tilemap Extras** package rather than in
> Unity itself, so this step installs it. Writing neighbour-matching by hand is real work, entirely beside
> this guide's subject — see [the decision](../foundation/decision-log.md#d12--build-vs-borrow-auto-tiling-correct-edge-and-corner-tiles-while-painting).

## Do this

1. Open **Window > Package Manager**. Switch the dropdown at the top left from *In Project* to **Unity
   Registry**, find **2D Tilemap Extras** in the list, and press **Install**. Wait for the reimport to
   finish — the package adds asset types, so Unity recompiles.

   This edits `Packages/manifest.json`, which is tracked, so it is part of this step's commit.

2. In the **Project** panel, right-click `Assets/_Project/Art/Tiles` and choose **Create > 2D > Tiles > Rule
   Tile**. Name it **`GroundRuleTile`**.

3. Select it. The Inspector shows a **Default Sprite** field and an empty **Tiling Rules** list. Set
   **Default Sprite** to your plain solid-interior tile — the one that looks like the middle of a block. A
   Rule Tile with no default sprite is invisible in the palette, which is a confusing five minutes.

4. Press the **+** at the bottom right of the **Tiling Rules** list five times. Each rule shows a 3 × 3 grid
   of neighbour boxes with a sprite field beside it. Clicking a box cycles it through three states:
   - **empty** — don't care,
   - **green arrow** — this neighbour **must** be the same Rule Tile,
   - **red cross** — this neighbour must **not** be.

   The centre box is the tile itself and is not editable.

   Fill in the five rules exactly like this, top to bottom. "Above" means the box directly over the centre:

   | Rule | Set these boxes | Sprite to use |
   |---|---|---|
   | 1 | **Above** = red cross | your grass/top-surface tile |
   | 2 | **Above** = green arrow, **Left** = red cross | your left-edge tile |
   | 3 | **Above** = green arrow, **Right** = red cross | your right-edge tile |
   | 4 | **Above** = green arrow, **Below** = red cross | your bottom-edge tile |
   | 5 | *(leave every box empty)* | your solid interior tile |

   Order matters: Unity uses the **first** rule that matches, so the specific ones must sit above the
   catch-all. Rule 5 has no conditions at all, which is why it is last — it is the "none of the above" case.

   If your chosen tileset has no distinct left, right or bottom edge sprite, point those rules at the interior
   tile. The mechanism is the same and the cavern still reads correctly.

5. Drag `GroundRuleTile` from the Project panel into the **Tile Palette** window, into an empty cell. It now
   sits in the tray beside the plain tiles, and it is the one you will paint with.

6. Save the project and commit.

## Done when (this step)
- [ ] **Window > Package Manager > In Project** lists **2D Tilemap Extras**.
- [ ] `Assets/_Project/Art/Tiles/GroundRuleTile.asset` exists, its **Default Sprite** is set, and its
      **Tiling Rules** list has five entries.
- [ ] `GroundRuleTile` appears in the `CavernPalette` window showing its default sprite — not a blank cell.
- [ ] `git status --short`, run from `cavern-dash`, lists `Packages/manifest.json` as modified and the new
      `.asset` plus `.meta`.
- [ ] The Console shows no red entries.

## Suggested commit
```
feat(level): add a five-rule GroundRuleTile for auto-tiling
```

## If it breaks
- **There is no "Rule Tile" under Create > 2D > Tiles** → the package is not installed or Unity has not
  finished importing it. Check the Package Manager's *In Project* list.
- **The Rule Tile is an empty square in the palette** → **Default Sprite** is unset.
- **Every cell you paint uses the same sprite** → the rules are all empty conditions, or the catch-all rule
  sits first in the list. Drag rule 5 to the bottom using the handle at its left.
- **The top surface appears in the middle of a block** → rule 1's box is set on the wrong side. "Above" is the
  box directly over the centre, not the top-left corner.

---
> Nav: [← Build a tile palette](02_tile-palette.md) · [Overview](00_overview.md) · [Paint the cavern →](04_paint-the-level.md)
