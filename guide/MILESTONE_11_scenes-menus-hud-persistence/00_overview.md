# Milestone 11 — Scenes, menus, HUD & persistence
> Part 4 — The game · milestone 11 of 13 · prev: [Animation, camera & audio](../MILESTONE_10_animation-camera-audio/00_overview.md) · next: [Options: volume, display & key rebinding](../MILESTONE_12_options-and-rebinding/00_overview.md) · start: [The HUD](01_hud.md)

## Goal
By the end of this milestone *Cavern Dash* is a game rather than a level. A title menu leads into Level 01,
finishing it leads into Level 02, and finishing that leads to a win screen showing your time and your best
time — which is still there after you quit and relaunch. **Esc** pauses properly, and running out of lives
ends the run instead of quietly resetting it.

## Prerequisite
M10's gate passed: the character animates, the camera follows and confines, the background parallaxes, and
the five sound effects play through the mixer.

## Steps at a glance

**Sitting 1 — Numbers on screen (01)**
1. [The HUD](01_hud.md)

**Sitting 2 — More than one scene (02–03)**
2. [More scenes](02_more-scenes.md)
3. [Carry the run across scenes](03_game-session.md)

**Sitting 3 — Stopping the world (04)**
4. [Pause](04_pause.md)

**Sitting 4 — Time, and losing (05–06)**
5. [The timer and the best time](05_timer-and-best-time.md)
6. [Game over](06_game-over.md)

7. [Verify](07_verify.md)

## Design / decisions folded in
- TextMeshPro and the Canvas — taught in [step 01](01_hud.md); recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d19--build-vs-borrow-screen-text-and-the-hud).
- `SceneManager` and the build scene list — taught in [step 02](02_more-scenes.md); recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d20--build-vs-borrow-loading-levels-and-moving-between-scenes).
- What dies on a scene load, and the one object that does not — taught in [step 03](03_game-session.md); recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d21--build-vs-borrow-carrying-score-lives-and-time-across-scenes).
- `Time.timeScale`, and switching action maps so menus and gameplay do not fight — taught in [step 04](04_pause.md).
- `PlayerPrefs` for a handful of scalars, and where its limits are — taught in [step 05](05_timer-and-best-time.md); recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d22--build-vs-borrow-persisting-the-best-time-and-the-options).

---
> Part 4 — The game · milestone 11 of 13 · prev: [Animation, camera & audio](../MILESTONE_10_animation-camera-audio/00_overview.md) · next: [Options: volume, display & key rebinding](../MILESTONE_12_options-and-rebinding/00_overview.md) · start: [The HUD](01_hud.md)
