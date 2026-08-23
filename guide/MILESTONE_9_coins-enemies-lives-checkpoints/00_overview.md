# Milestone 9 — Coins, enemies, damage, lives & checkpoints
> Part 4 — The game · milestone 9 of 13 · prev: [Dash & wall-jump (a movement state machine)](../MILESTONE_8_dash-and-wall-jump/00_overview.md) · next: [Animation, camera & audio](../MILESTONE_10_animation-camera-audio/00_overview.md) · start: [Make a coin](01_coin-prefab.md)

## Goal
By the end of this milestone the cavern is a game you can win and lose: coins vanish when you touch them and
a count goes up, and an enemy patrols a ledge that you can land on to kill or walk into to lose a life. Losing
one costs you a second of invulnerability and sends you back to the last checkpoint you touched; running out
of lives, or falling off the world, ends the run.

## Prerequisite
M8's gate passed: dash, wall slide and wall-jump all behave, and the movement state machine keeps them
exclusive.

## Steps at a glance

**Sitting 1 — Something to collect (01–02)**
1. [Make a coin](01_coin-prefab.md)
2. [Collect coins](02_collect-coins.md)

**Sitting 2 — Something to avoid (03–04)**
3. [An enemy that patrols](03_enemy.md)
4. [Stomp it, or lose a life](04_stomp-and-damage.md)

**Sitting 3 — Somewhere to come back to (05)**
5. [Checkpoints and respawn](05_checkpoints-and-respawn.md)

6. [Verify](06_verify.md)

## Design / decisions folded in
- Triggers versus solid colliders, and why one side needs a `Rigidbody2D` — taught in [step 01](01_coin-prefab.md); recorded in [../foundation/glossary.md](../foundation/glossary.md#trigger).
- Prefabs: author once, place many, edit in one place — taught in [step 01](01_coin-prefab.md); recorded in [../foundation/glossary.md](../foundation/glossary.md#prefab).
- C# events as the way components talk without knowing each other — taught in [step 02](02_collect-coins.md); recorded in [../foundation/conventions.md](../foundation/conventions.md).
- Deciding a stomp by comparing collider bounds rather than by contact normals — taught in [step 04](04_stomp-and-damage.md).
- Invulnerability frames — taught in [step 04](04_stomp-and-damage.md); recorded in [../foundation/glossary.md](../foundation/glossary.md#i-frames-invulnerability-frames).

---
> Part 4 — The game · milestone 9 of 13 · prev: [Dash & wall-jump (a movement state machine)](../MILESTONE_8_dash-and-wall-jump/00_overview.md) · next: [Animation, camera & audio](../MILESTONE_10_animation-camera-audio/00_overview.md) · start: [Make a coin](01_coin-prefab.md)
