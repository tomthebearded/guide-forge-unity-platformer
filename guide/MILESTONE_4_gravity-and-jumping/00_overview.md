# Milestone 4 — Gravity, jumping & the ground check
> Part 2 — The character · milestone 4 of 13 · prev: [Input & running (keyboard and gamepad)](../MILESTONE_3_input-and-running/00_overview.md) · next: [Game feel](../MILESTONE_5_game-feel/00_overview.md) · start: [Give gravity some weight](01_tune-gravity.md)

## Goal
By the end of this milestone the player falls at a weight you chose rather than the engine's default, and
jumps — exactly once per press, to a height you can measure. It knows whether it is standing on something,
because you wrote the test that answers that question, and pressing jump in mid-air does nothing at all.

## Prerequisite
M3's gate passed: the player runs left and right at 7 units per second on both devices and rests on the
`Ground` strip.

## Steps at a glance

**Sitting 1 — Weight and ground (01–03)**
1. [Give gravity some weight](01_tune-gravity.md)
2. [Put the ground on its own layer](02_ground-layer.md)
3. [Ask whether the player is grounded](03_ground-check.md)

**Sitting 2 — Jump, and measure it (04–05)**
4. [Jump once per press](04_jump.md)
5. [Measure the jump](05_measure-the-jump.md)

6. [Verify](06_verify.md)

## Design / decisions folded in
- Gravity as `Physics2D.gravity` × `gravityScale`, and why the default feels floaty — taught in [step 01](01_tune-gravity.md); the resulting numbers are recorded in [../foundation/conventions.md](../foundation/conventions.md).
- Layers and layer masks as the way physics queries are filtered — taught in [step 02](02_ground-layer.md); the layer names are load-bearing, see [../foundation/conventions.md](../foundation/conventions.md).
- The ground check as an overlap query you own, not a collision callback — taught in [step 03](03_ground-check.md).
- Latching a press in `Update` and consuming it in `FixedUpdate` — taught in [step 04](04_jump.md); it becomes the jump buffer in [M5](../MILESTONE_5_game-feel/00_overview.md).

---
> Part 2 — The character · milestone 4 of 13 · prev: [Input & running (keyboard and gamepad)](../MILESTONE_3_input-and-running/00_overview.md) · next: [Game feel](../MILESTONE_5_game-feel/00_overview.md) · start: [Give gravity some weight](01_tune-gravity.md)
