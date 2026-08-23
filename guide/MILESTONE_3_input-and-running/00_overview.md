# Milestone 3 — Input & running (keyboard and gamepad)
> Part 2 — The character · milestone 3 of 13 · prev: [First script & frame-rate-independent motion](../MILESTONE_2_first-script-and-motion/00_overview.md) · next: [Gravity, jumping & the ground check](../MILESTONE_4_gravity-and-jumping/00_overview.md) · start: [Meet the Input System](01_meet-the-input-system.md)

## Goal
By the end of this milestone the square obeys you. Holding **A**/**D** or the arrow keys runs it left and
right at exactly 7 units per second, a gamepad's left stick does the same without a line of extra code, and
releasing everything stops it. It is also a physics body now: it falls onto a floor and stays there instead of
sliding through it.

## Prerequisite
M2's gate passed: `ConstantMover` moves the `Player` at a tunable, frame-rate-independent speed, and VSync is
off.

## Steps at a glance

**Sitting 1 — Intent (01–02)**
1. [Meet the Input System](01_meet-the-input-system.md)
2. [Read the Move action](02_input-reader.md)

**Sitting 2 — A body in a world (03–05)**
3. [Give the world a floor](03_ground-platform.md)
4. [Give the player a body](04_rigidbody-and-collider.md)
5. [Move the body with velocity](05_move-with-velocity.md)

6. [Verify](06_verify.md)

## Design / decisions folded in
- The Input System's action/binding model, and why one action serves two devices — taught in [step 01](01_meet-the-input-system.md); recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d8--build-vs-borrow-input-handling-and-device-bindings).
- Dynamic `Rigidbody2D`: the engine owns the transform, your code owns the intent — taught in [step 04](04_rigidbody-and-collider.md); recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d2--the-player-is-a-dynamic-rigidbody2d-driven-by-velocity).
- Input in `Update`, physics in `FixedUpdate` — taught in [step 05](05_move-with-velocity.md); recorded in [../foundation/conventions.md](../foundation/conventions.md).
- One component, one responsibility: `PlayerInputReader` answers *what is being asked for*, `PlayerMotor` decides *what happens* — taught in [steps 02](02_input-reader.md) and [05](05_move-with-velocity.md).

---
> Part 2 — The character · milestone 3 of 13 · prev: [First script & frame-rate-independent motion](../MILESTONE_2_first-script-and-motion/00_overview.md) · next: [Gravity, jumping & the ground check](../MILESTONE_4_gravity-and-jumping/00_overview.md) · start: [Meet the Input System](01_meet-the-input-system.md)
