# M6 · Step 01 of 06 — Import the art
> Nav: — · [Overview](00_overview.md) · [Build a tile palette →](02_tile-palette.md)

**Before you start:** M5's gate passed. You need a browser and a few minutes; the download is small. Git LFS
has been armed for `*.png` since
[M1 step 05](../MILESTONE_1_project-and-version-control/05_git-lfs.md), which is why this step can commit
images without a second thought.

## Glossary for this step
> New here: **[pixels per unit (PPU)](../foundation/glossary.md#pixels-per-unit-ppu)** (defined in *Why / design*) ·
> **[CC0](../foundation/glossary.md#cc0)** (defined in *Do this*, action 1).

## Why / design
Two import settings decide whether pixel art looks like pixel art, and both default to the wrong value for it.

**Pixels Per Unit** is the exchange rate between the artist's pixels and Unity's world units. Kenney's Pixel
Platformer tiles are **18 × 18 pixels**, so **PPU 18** makes one tile exactly **one world unit** — and the
whole guide's vocabulary (a jump of 2.5 units, a dash of 5 units, a player 1 unit tall) suddenly reads as
tiles. Leave it at Unity's default of 100 and one tile becomes 0.18 units, at which point every number you
tuned in M5 is wrong by a factor of five.

> New concept — **pixels per unit (PPU)**: how many pixels of a sprite make one Unity world unit. With 18 × 18
> tiles and PPU 18, one tile is one unit, which is the convention this whole guide is quoted in.

**Filter Mode** decides what happens between pixels. The default, `Bilinear`, blends them — which is right for
photographs and disastrous for pixel art, where it turns crisp edges into mud. **Point (no filter)** keeps
every pixel square.

## Do this

1. Open <https://kenney.nl/assets/pixel-platformer> in a browser and press **Download**. The pack is **CC0**.

   > New concept — **CC0**: a public-domain dedication. Kenney's terms say *"all game assets on the asset
   > pages are public domain licensed (CC0). You're free to use them, even in commercial projects"*, and
   > attribution is not required. That is what makes it safe to commit these files to a public repository —
   > which you are about to do. The one restriction is Kenney's **logo**, which is not yours to reuse.
   > Terms: <https://kenney.nl/support>

2. Unzip it. Inside you will find a licence file and folders of PNGs — the tiles, some characters, and
   backgrounds. Create the folder `Assets/ThirdParty/Kenney/PixelPlatformer` inside your project (in the
   **Project** panel: right-click `ThirdParty` > **Create > Folder**, and so on), and copy into it the pack's
   **licence file** and its **tile images**.

   You need the tiles for this milestone. Copying the characters too is free and saves a trip in M10 — do it
   now if you like.

3. Select every imported tile image in the **Project** panel — click the first, shift-click the last — so the
   Inspector shows the import settings for all of them at once. Set exactly these fields, and leave every
   other one at its default:

   | Field | Value | Why |
   |---|---|---|
   | **Texture Type** | `Sprite (2D and UI)` | Usually already correct for a 2D project. |
   | **Pixels Per Unit** | `18` | One 18 × 18 tile becomes one world unit. **Load-bearing.** |
   | **Filter Mode** | `Point (no filter)` | Keeps pixels square instead of blurring them. |
   | **Compression** | `None` | Compression artefacts are visible on flat pixel art. |

   Press **Apply** at the bottom of the Inspector. Unity re-imports every selected file.

   *If your download contains one large packed spritesheet rather than separate tiles*, the settings above are
   the same, plus one more: set **Sprite Mode** to `Multiple`, press **Apply**, then open the **Sprite
   Editor**, choose **Slice > Grid By Cell Size** with a cell size of `18` by `18`, press **Slice**, and
   **Apply**. That cuts the sheet into individual sprites; everything after this step is identical.

4. Check the result at the pixel level rather than by eye. Click a single tile in the Project panel and look
   at the preview at the bottom of the Inspector: it reads `18x18` and the edges are hard. Then drag one tile
   into the **Scene** view temporarily and compare it with the grid — it covers exactly one cell. Delete it
   again (select it in the Hierarchy, press **Delete**); it was a measurement, not a decision.

5. Save the scene and commit. Because `.gitattributes` routes `*.png` to Git LFS, these files go in as LFS
   pointers — you can confirm that after committing with `git lfs ls-files`, which lists them.

## Done when (this step)
- [ ] `Assets/ThirdParty/Kenney/PixelPlatformer` contains the pack's licence file and its tile PNGs.
- [ ] Selecting any imported tile shows **Pixels Per Unit** `18`, **Filter Mode** `Point (no filter)`,
      **Compression** `None`.
- [ ] A single tile dropped into the Scene view covers exactly **one** grid cell — not a fifth of one.
- [ ] After committing, `git lfs ls-files` lists the imported PNGs (each line is a hash, then the path).
- [ ] The Console shows no red entries.

## Suggested commit
```
feat(assets): import the Kenney Pixel Platformer tiles at 18 PPU
```

## If it breaks
- **The tiles look blurry in the Scene view** → **Filter Mode** is still `Bilinear`, or you set it on one file
  rather than the whole selection. Re-select all of them and re-apply.
- **A tile covers a fifth of a grid cell** → PPU is still `100`. This is the setting that silently invalidates
  every tuned number in M5, so fix it now rather than compensating with scale.
- **`git lfs ls-files` prints nothing after committing** → the images were added before LFS was configured, or
  `.gitattributes` is missing. Check `git lfs track` lists `cavern-dash/*.png`; if the files went in as plain
  blobs, the
  simplest fix at this stage is to remove them, commit, then re-add them.
- **Unity shows the images but the Project panel says "0 sprites"** → **Texture Type** is `Default`, not
  `Sprite (2D and UI)`.

---
> Nav: — · [Overview](00_overview.md) · [Build a tile palette →](02_tile-palette.md)
