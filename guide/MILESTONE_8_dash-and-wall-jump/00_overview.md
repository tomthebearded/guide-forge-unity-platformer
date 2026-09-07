# Milestone 8 — Dash & wall-jump (a movement state machine)
> Part 3 — The world · milestone 8 of 12 · prev: [Moving & one-way platforms](../MILESTONE_7_moving-and-one-way-platforms/00_overview.md) · next: [Coins, enemies, damage, lives & checkpoints](../MILESTONE_9_coins-enemies-lives-checkpoints/00_overview.md) · start: [Add the Dash action](01_add-the-dash-action.md)

## Goal
By the end of this milestone the player has a moveset rather than a move. A dash covers a fixed distance in a
fixed time and cannot be spammed; sliding down a wall is slow and controlled; and a wall-jump pushes you off
it. All three are governed by an explicit state machine in which the player is doing exactly one thing at a
time — which is the point of the milestone at least as much as the moves are.

## Prerequisite
M7's gate passed: the one-way ledge and the moving platform both behave, and the player still runs the painted
floor without catching.

## Steps at a glance

**Sitting 1 — Structure before features (01–02)**
1. [Add the Dash action](01_add-the-dash-action.md)
2. [Turn the motor into a state machine](02_movement-state-machine.md)

**Sitting 2 — The moves (03–05)**
3. [Dash](03_dash.md)
4. [Cling to a wall](04_wall-slide.md)
5. [Jump off the wall](05_wall-jump.md)

6. [Verify](06_verify.md)

## Design / decisions folded in
- Editing the project-wide actions asset for the first time — taught in [step 01](01_add-the-dash-action.md).
- An explicit `enum` state machine instead of a pile of booleans — taught in [step 02](02_movement-state-machine.md); recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d15--build-vs-borrow-the-movement-state-machine-grounded--airborne--dashing--wall-sliding).
- A dash defined by *distance and duration*, not by a force — taught in [step 03](03_dash.md); values in [../foundation/conventions.md](../foundation/conventions.md).
- Detecting a wall with the same overlap trick as the ground check — taught in [step 04](04_wall-slide.md).
- Why a wall-jump needs a brief control lock — taught in [step 05](05_wall-jump.md).

---
> Part 3 — The world · milestone 8 of 12 · prev: [Moving & one-way platforms](../MILESTONE_7_moving-and-one-way-platforms/00_overview.md) · next: [Coins, enemies, damage, lives & checkpoints](../MILESTONE_9_coins-enemies-lives-checkpoints/00_overview.md) · start: [Add the Dash action](01_add-the-dash-action.md)
