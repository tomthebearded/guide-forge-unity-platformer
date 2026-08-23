# Milestone 2 — First script & frame-rate-independent motion
> Part 1 — Foundations · milestone 2 of 13 · prev: [Project, Editor & version control](../MILESTONE_1_project-and-version-control/00_overview.md) · next: [Input & running (keyboard and gamepad)](../MILESTONE_3_input-and-running/00_overview.md) · start: [Add the player sprite](01_player-sprite.md)

## Goal
By the end of this milestone a white square glides across the Game view at a speed you set in the Inspector,
driven by the first C# script you wrote. It covers the same distance every second whether the game runs at 60
frames per second or 15 — and you will have proved that rather than assumed it.

## Prerequisite
M1's gate passed: `cavern-dash` opens on `6000.3.x`, Play Mode runs clean, and `git status --porcelain` is
silent.

## Steps at a glance

**Sitting 1 — Something on screen that moves (01–03)**
1. [Add the player sprite](01_player-sprite.md)
2. [Write your first MonoBehaviour](02_first-script.md)
3. [Tune it while it runs](03_tune-in-inspector.md)

**Sitting 2 — Prove it (04)**
4. [Prove the motion is frame-rate independent](04_frame-rate-independence.md)

5. [Verify](05_verify.md)

## Design / decisions folded in
- The MonoBehaviour lifecycle (`Awake`, `Update`) — taught in [step 02](02_first-script.md); recorded in [../foundation/glossary.md](../foundation/glossary.md#monobehaviour).
- Delta time, the mental model the whole guide rests on — taught in [step 02](02_first-script.md); recorded in [../foundation/glossary.md](../foundation/glossary.md#delta-time).
- Tuning through `[SerializeField]` rather than magic numbers — taught in [step 02](02_first-script.md); recorded in [../foundation/conventions.md](../foundation/conventions.md).
- Play Mode discards what you change while it runs — taught in [step 03](03_tune-in-inspector.md).

---
> Part 1 — Foundations · milestone 2 of 13 · prev: [Project, Editor & version control](../MILESTONE_1_project-and-version-control/00_overview.md) · next: [Input & running (keyboard and gamepad)](../MILESTONE_3_input-and-running/00_overview.md) · start: [Add the player sprite](01_player-sprite.md)
