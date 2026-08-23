# M6 · Step 02 of 06 — Build a tile palette
> Nav: [← Import the art](01_import-the-art.md) · [Overview](00_overview.md) · [Make a Rule Tile →](03_rule-tile.md)

**Before you start:** [step 01](01_import-the-art.md) finished — the tiles are in
`Assets/ThirdParty/Kenney/PixelPlatformer` at PPU `18` with Point filtering.

## Glossary for this step
> New here: **[tile asset](../foundation/glossary.md#tile-asset)** (defined in *Why / design*) ·
> **[tile palette](../foundation/glossary.md#tile-palette)** (defined in *Why / design*).

## Why / design
A sprite is a picture. A **tile asset** is a picture Unity can stamp onto a grid — it holds the sprite plus
what colour to tint it and how it collides. A **tile palette** is the tray of tiles you paint from.

Dragging sprites into a palette window creates the tile assets for you, in a folder you choose. They are real
files in your project, and the palette is a real asset too, which is why both go into version control.

> New concept — **tile asset**: a small asset pairing a sprite with tilemap-specific settings. It is what a
> Tilemap actually stores in each cell.
>
> New concept — **tile palette**: the Editor window and asset holding the set of tiles you paint with — the
> tray beside the canvas.

Keeping these under `Assets/_Project/Art` rather than beside the imported PNGs is the boundary from
[`../foundation/conventions.md`](../foundation/conventions.md): `ThirdParty` holds what you downloaded,
`_Project` holds what you made — and tile assets, though generated, are yours.

## Do this

1. In the **Project** panel, create two folders inside `Assets/_Project/Art`: **`Tiles`** and
   **`Palettes`**. Right-click the folder > **Create > Folder** for each.

2. Open the palette window: **Window > 2D > Tile Palette**. Dock it somewhere you can see alongside the Scene
   view — it is going to be open for the rest of this milestone.

3. In that window, open the palette dropdown at the top left and choose **Create New Palette**. Set:
   - **Name**: `CavernPalette`
   - **Grid**: `Rectangle`
   - **Cell Size**: `Automatic`

   Press **Create**, and when Unity asks where to save it, choose `Assets/_Project/Art/Palettes`.

4. In the **Project** panel, select the tile sprites you want to build the cavern from — the solid ground
   blocks are the ones that matter; a dozen is plenty — and **drag them into the Tile Palette window**.

   Unity asks where to save the generated tile assets. Choose **`Assets/_Project/Art/Tiles`**. It writes one
   `.asset` per sprite and lays them out in the palette.

5. Click a tile in the palette to select it, then look at the row of tool buttons at the top of the window:
   the brush (**B**), the eraser (**D**), the rectangle fill, the picker. You will use the brush and the
   eraser in [step 04](04_paint-the-level.md); there is nothing to paint on yet.

6. Save the project (**Ctrl+S** / **Cmd+S** saves the scene; **File > Save Project** flushes the assets) and
   commit.

## Done when (this step)
- [ ] `Assets/_Project/Art/Palettes/CavernPalette.prefab` exists (Unity stores a palette as a prefab asset)
      and `Assets/_Project/Art/Tiles` contains one `.asset` file per tile you dragged in.
- [ ] The **Tile Palette** window shows your tiles as a grid of thumbnails, sharp rather than blurry.
- [ ] Clicking a tile highlights it and the brush tool becomes usable.
- [ ] `git status --short` lists the new `.asset` files **and** a `.meta` for each.

## Suggested commit
```
feat(level): add the CavernPalette tile palette and its tile assets
```

## If it breaks
- **The palette window says "No valid palette"** → the palette asset was saved outside `Assets/`, or creation
  was cancelled. Create it again and save it under `Assets/_Project/Art/Palettes`.
- **Dropping sprites does nothing** → you dropped them onto the palette *dropdown* rather than into the grid
  area below it, or the dragged files are textures rather than sprites (see step 01's **Texture Type**).
- **The tiles look blurry in the palette** → the palette shows what the import settings produce.
  **Filter Mode** must be `Point (no filter)`.
- **The tiles overlap or leave gaps in the palette grid** → the palette's **Cell Size** is not matching the
  sprites. Delete the palette asset and create it again with **Cell Size: Automatic**.

---
> Nav: [← Import the art](01_import-the-art.md) · [Overview](00_overview.md) · [Make a Rule Tile →](03_rule-tile.md)
