# Milestone 7 — Moving & one-way platforms
> Part 3 — The world · milestone 7 of 13 · prev: [The level as a Tilemap](../MILESTONE_6_tilemap-level/00_overview.md) · next: [Dash & wall-jump (a movement state machine)](../MILESTONE_8_dash-and-wall-jump/00_overview.md) · start: [A ledge you can jump up through](01_one-way-platform.md)

## Goal
By the end of this milestone the cavern stops being furniture. A ledge lets you jump up *through* it from
below and land on top of it; a platform travels back and forth on a path you set; and standing on that
platform carries you with it, without sliding off and without jitter.

## Prerequisite
M6's gate passed: the painted tilemap has one composite collider on the `Ground` layer and the player runs its
full length without catching.

## Steps at a glance

**Sitting 1 — Through from below (01)**
1. [A ledge you can jump up through](01_one-way-platform.md)

**Sitting 2 — Geometry that travels (02–03)**
2. [A platform that travels](02_moving-platform.md)
3. [Carry the rider](03_carry-the-rider.md)

4. [Verify](04_verify.md)

## Design / decisions folded in
- `PlatformEffector2D` instead of hand-written normal filtering — taught in [step 01](01_one-way-platform.md); recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d13--build-vs-borrow-one-way-pass-through-from-below-platforms).
- A `Kinematic` body moved with `MovePosition`, and why not a `Dynamic` one — taught in [step 02](02_moving-platform.md).
- The rider problem, and moving the rider with the platform (movement inheritance, **not** re-parenting) as the chosen fix — taught in [step 03](03_carry-the-rider.md); recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d14--build-vs-borrow-carrying-a-rider-on-a-moving-platform).
- Why the platform's collider lives on a scale-`1, 1, 1` parent with the stretched sprite on a child — taught in [step 02](02_moving-platform.md).

---
> Part 3 — The world · milestone 7 of 13 · prev: [The level as a Tilemap](../MILESTONE_6_tilemap-level/00_overview.md) · next: [Dash & wall-jump (a movement state machine)](../MILESTONE_8_dash-and-wall-jump/00_overview.md) · start: [A ledge you can jump up through](01_one-way-platform.md)
