# M6 · Verify — The level as a Tilemap
> Nav: [← One collider for the whole level](05_composite-collider.md) · [Overview](00_overview.md) · [Moving & one-way platforms →](../MILESTONE_7_moving-and-one-way-platforms/00_overview.md)

## Done-when gate (the real test — check every box by hand)

Observed in **Play Mode in the Editor**, `Level01` open, Game view focused.

- [ ] **The art imports as pixel art at the guide's scale.** Any imported tile shows **Pixels Per Unit** `18`,
      **Filter Mode** `Point (no filter)`, **Compression** `None`, and one tile covers exactly one grid cell.
- [ ] **The palette exists and paints.** `CavernPalette` is in `Assets/_Project/Art/Palettes`, its tile assets
      are in `Assets/_Project/Art/Tiles`, and `GroundRuleTile` sits in the palette showing its default sprite.
- [ ] **The Rule Tile chooses sprites by neighbour.** Painting a new block of cells produces a surface sprite
      on the top row and interior sprites below, with no hand-picking; extending a platform re-tiles its old
      end.
- [ ] **The level is painted and reachable.** The cavern has a floor at least ten cells long, end walls, and
      two or three ledges — **none of them more than two cells above** what you jump from. From a standing
      start you can reach every ledge you painted, and get back down.
- [ ] **One collider, not hundreds.** `GroundTilemap` carries `Tilemap Collider 2D` with **Used By Composite**
      ticked, a **Static** `Rigidbody 2D`, and a `Composite Collider 2D` — and the Scene view shows a single
      outline around the whole shape.
- [ ] **No seams.** Running the full length of the floor at top speed, several times, the player never
      catches, stutters or stops on a cell boundary.
- [ ] **The composite is what does it.** Unticking **Used By Composite** makes the same run visibly catch on
      seams; re-ticking it restores the smooth run.
- [ ] **The ground check sees the tilemap.** `GroundTilemap`'s Layer reads `Ground`, and the player can jump
      from anywhere on the painted floor — including from the top of every ledge.
- [ ] **The temporary scaffolding is gone.** There is no `Ground` object in the Hierarchy.
- [ ] **The project is clean.** No red Console entries; after committing, `git status --porcelain` prints
      nothing, and `git lfs ls-files` lists the imported PNGs.

## Files after this milestone (the checkpoint)

_This milestone authored no C#. Everything it produced is assets and Editor state, so the checkpoint below is
the settings table and the asset inventory rather than code blocks. Files not listed were not touched this
milestone._

### Assets created this milestone

| Path | What it is |
|---|---|
| `Assets/ThirdParty/Kenney/PixelPlatformer/` | The CC0 tile PNGs and the pack's licence file, imported at PPU 18 |
| `Assets/_Project/Art/Palettes/CavernPalette.prefab` | The tile palette |
| `Assets/_Project/Art/Tiles/*.asset` | One tile asset per sprite dragged into the palette |
| `Assets/_Project/Art/Tiles/GroundRuleTile.asset` | The five-rule auto-tiling tile |

### Editor checkpoint

| GameObject / where | Component | Field | Exact value |
|---|---|---|---|
| every imported tile | Texture importer | Pixels Per Unit | `18` |
| every imported tile | Texture importer | Filter Mode | `Point (no filter)` |
| every imported tile | Texture importer | Compression | `None` |
| `Grid` | `Grid` | Cell Size | `1, 1, 0` |
| `GroundTilemap` | GameObject | Layer | `Ground` |
| `GroundTilemap` | `Tilemap Collider 2D` | Used By Composite | ticked |
| `GroundTilemap` | `Rigidbody 2D` | Body Type | `Static` |
| `GroundTilemap` | `Composite Collider 2D` | Geometry Type | `Outlines` |
| `Player` | `Transform` | Position | just above your painted floor (e.g. `-10, -2, 0`) |
| `GroundRuleTile` | Rule Tile | Tiling Rules | five rules, catch-all last |

### Pre-existing files modified
- `Assets/_Project/Scenes/Level01.unity` — `Grid`/`GroundTilemap` added with their colliders and layer; the
  `Ground` strip deleted; the `Player`'s start position moved. Edited through the Editor.
- `Packages/manifest.json` — **2D Tilemap Extras** added in [step 03](03_rule-tile.md).

### Unchanged this milestone
- `Assets/_Project/Scripts/PlayerInputReader.cs`, `Assets/_Project/Scripts/PlayerMotor.cs` — unchanged since
  M5. The movement code did not need one line changing to run on a real level, which is the quiet result of
  this milestone.
- `Assets/InputSystem_Actions.inputactions`, `ProjectSettings/TagManager.asset`,
  `ProjectSettings/QualitySettings.asset` — unchanged since M4/M2.

## Troubleshooting

| Symptom | Likely cause → fix |
|---|---|
| Tiles look blurry | **Filter Mode** is `Bilinear`; set `Point (no filter)` on the whole selection and Apply. |
| One tile covers a fraction of a cell | PPU is `100`, not `18`. Fix the import, not the scale. |
| Painting does nothing | The Tile Palette's **Active Tilemap** points elsewhere, or no tile is selected. |
| The Rule Tile always draws the same sprite | The catch-all rule (all boxes empty) is not last in the list. |
| The player falls through the painted floor | The `Tilemap Collider 2D` is on `Grid` instead of `GroundTilemap`. |
| The player lands but cannot jump | `GroundTilemap`'s layer is not `Ground`. |
| The player catches on invisible bumps | **Used By Composite** is unticked — the seam problem this milestone exists to remove. |
| The whole level falls when you press Play | The tilemap's `Rigidbody 2D` is `Dynamic`; it must be `Static`. |
| A ledge is unreachable | It is more than two cells above its approach. Your jump clears about 2.4 units and one cell is one unit. |

## Handoff
- **You now have:** the M1 project and clean repository; a player that runs, accelerates, and jumps with
  coyote time, buffering and variable height; and a real level — CC0 pixel-art tiles imported at 18 PPU,
  painted onto a `GroundTilemap` through a `CavernPalette` and a five-rule `GroundRuleTile`, made solid by a
  single composite collider on the `Ground` layer. The temporary grey strip is gone and the movement code was
  not touched to make any of it work.
- **Open / deferred:** every surface in the cavern is static and solid from all sides — nothing moves, and
  nothing can be jumped up through. The player is still an orange square; M10 replaces it with an animated
  character.
- **Next:** **[M7 — Moving & one-way platforms](../MILESTONE_7_moving-and-one-way-platforms/00_overview.md)** —
  geometry that travels, and ledges you can pass through from below and land on from above.

---
> Nav: [← One collider for the whole level](05_composite-collider.md) · [Overview](00_overview.md) · [Moving & one-way platforms →](../MILESTONE_7_moving-and-one-way-platforms/00_overview.md)
