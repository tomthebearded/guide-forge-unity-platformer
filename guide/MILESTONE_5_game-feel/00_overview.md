# Milestone 5 — Game feel
> Part 2 — The character · milestone 5 of 13 · prev: [Gravity, jumping & the ground check](../MILESTONE_4_gravity-and-jumping/00_overview.md) · next: [The level as a Tilemap](../MILESTONE_6_tilemap-level/00_overview.md) · start: [Give movement weight](01_acceleration.md)

## Goal
By the end of this milestone the jump stops being arithmetic and starts being a game. Movement builds up and
bleeds off instead of snapping; a jump pressed a fraction *before* landing still fires; a jump pressed a
fraction *after* walking off a ledge still fires; and a tapped button gives a visibly lower jump than a held
one. Nothing new appears on screen — everything here is felt rather than seen, which is exactly why this
milestone ends by making you play it.

## Prerequisite
M4's gate passed: gravity scale `4`, a working ground check, one jump per press, and a measured apex you
wrote down.

## Steps at a glance

**Sitting 1 — Weight and forgiveness (01–03)**
1. [Give movement weight](01_acceleration.md)
2. [Coyote time: jump just after the ledge](02_coyote-time.md)
3. [Jump buffering: jump just before landing](03_jump-buffer.md)

**Sitting 2 — Height you control, then play it (04–05)**
4. [Variable jump height](04_variable-jump-height.md)
5. [Stop and play it](05_reality-check.md) ⭐ *reality-check gate*

6. [Verify](06_verify.md)

## Design / decisions folded in
- Acceleration through `Mathf.MoveTowards`, and separate ground and air rates — taught in [step 01](01_acceleration.md); values in [../foundation/conventions.md](../foundation/conventions.md).
- Coyote time — taught in [step 02](02_coyote-time.md); recorded in [../foundation/glossary.md](../foundation/glossary.md#coyote-time).
- Jump buffering, and why the M4 latch becomes a timestamp — taught in [step 03](03_jump-buffer.md); recorded in [../foundation/glossary.md](../foundation/glossary.md#jump-buffering).
- `Time.time` is the frame clock in `Update` and the physics clock in `FixedUpdate`, which is why the buffer clamps its age — taught in [step 03](03_jump-buffer.md); recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d27--the-jump-buffer-clamps-its-age-at-zero-because-timetime-is-two-clocks).
- Variable jump height as a gravity multiplier rather than a velocity cut — taught in [step 04](04_variable-jump-height.md); recorded in [../foundation/glossary.md](../foundation/glossary.md#variable-jump-height).
- The whole feel layer is hand-written, not borrowed — recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d10--build-vs-borrow-the-feel-layer-coyote-time-jump-buffering-variable-jump-height-acceleration).

---
> Part 2 — The character · milestone 5 of 13 · prev: [Gravity, jumping & the ground check](../MILESTONE_4_gravity-and-jumping/00_overview.md) · next: [The level as a Tilemap](../MILESTONE_6_tilemap-level/00_overview.md) · start: [Give movement weight](01_acceleration.md)
