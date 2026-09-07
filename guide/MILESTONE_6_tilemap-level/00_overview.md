# Milestone 6 — The level as a Tilemap
> Part 3 — The world · milestone 6 of 12 · prev: [Game feel](../MILESTONE_5_game-feel/00_overview.md) · next: [Moving & one-way platforms](../MILESTONE_7_moving-and-one-way-platforms/00_overview.md) · start: [Import the art](01_import-the-art.md)

## Goal
By the end of this milestone the grey strip is gone and the player runs through a cavern you painted: real
pixel-art tiles on a grid, drawn with a brush, backed by a **single** collider that the player can cross at
full speed without catching on a single seam.

## Prerequisite
M5's gate passed, including the five minutes of play — the movement is something you are happy to build eight
more milestones on top of.

## Steps at a glance

**Sitting 1 — Art in the project (01–02)**
1. [Import the art](01_import-the-art.md)
2. [Build a tile palette](02_tile-palette.md)

**Sitting 2 — A tile that knows its neighbours (03)**
3. [Make a Rule Tile](03_rule-tile.md)

**Sitting 3 — Paint it and make it solid (04–05)**
4. [Paint the cavern](04_paint-the-level.md)
5. [One collider for the whole level](05_composite-collider.md)

6. [Verify](06_verify.md)

## Design / decisions folded in
- Pixels Per Unit `18`, Point filtering, no compression — taught in [step 01](01_import-the-art.md); recorded in [../foundation/conventions.md](../foundation/conventions.md).
- Tilemap and Tile Palette instead of hand-placed objects — taught in [steps 02](02_tile-palette.md) and [04](04_paint-the-level.md); recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d11--build-vs-borrow-level-geometry-and-its-collider).
- Rule Tiles for auto-tiling — taught in [step 03](03_rule-tile.md); recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d12--build-vs-borrow-auto-tiling-correct-edge-and-corner-tiles-while-painting).
- Why one composite collider beats hundreds of box colliders — taught in [step 05](05_composite-collider.md).

---
> Part 3 — The world · milestone 6 of 12 · prev: [Game feel](../MILESTONE_5_game-feel/00_overview.md) · next: [Moving & one-way platforms](../MILESTONE_7_moving-and-one-way-platforms/00_overview.md) · start: [Import the art](01_import-the-art.md)
