<!-- Foundation doc. Section headings, table column keys and marks stay English by design (they are the
     schema the GuideForge skills look things up by). Prose language: English. -->

# PROGRESS — Cavern Dash

> _What has actually been **executed**, step by step. The guide describes intent, [`status.md`](status.md)
> states the guide's state — this file states **where you are in it**._
>
> Tick a step the moment you finish it, not at the end of a sitting: every maintenance skill that must avoid
> rewriting work you have already done reads this file to find the boundary. An unticked step is treated as
> **not done**, and an unticked guide is a guide those skills cannot safely amend.

## Legend
| Mark | Meaning |
|------|---------|
| `[ ]` | not executed yet |
| `[x]` | executed, and the step's **Done when** was observed |
| `[~]` | executed, but the **Done when** did not pass (or was skipped) — the row says what is outstanding |
| `[!]` | executed, then **invalidated** by a later change to the guide — the row names the retrofit that repairs it |

## Current position
- **Last executed:** M1 / 07_verify.md (milestone gate passed).
- **Next up:** M2 / 01_player-sprite.md.

## MILESTONE_1 — Project, Editor & version control
- [x] `01_install-unity.md` — Install Unity Hub and Unity 6.3 LTS — 2026-08-24
- [x] `02_create-project.md` — Create the Universal 2D project — 2026-08-24
- [x] `03_find-your-way-around.md` — Find your way around the Editor — 2026-08-24
- [x] `04_git-init.md` — Put the project under Git — 2026-08-24
- [x] `05_git-lfs.md` — Track binary assets with Git LFS — 2026-08-24
- [x] `06_project-folders.md` — Lay out the project folders and name the scene — 2026-08-24
- [x] `07_verify.md` — milestone gate — 2026-08-24

## MILESTONE_2 — First script & frame-rate-independent motion
- [ ] `01_player-sprite.md` — Add the player sprite
- [ ] `02_first-script.md` — Write your first MonoBehaviour
- [ ] `03_tune-in-inspector.md` — Tune it while it runs
- [ ] `04_frame-rate-independence.md` — Prove the motion is frame-rate independent
- [ ] `05_verify.md` — milestone gate

## MILESTONE_3 — Input & running (keyboard and gamepad)
- [ ] `01_meet-the-input-system.md` — Meet the Input System
- [ ] `02_input-reader.md` — Read the Move action
- [ ] `03_ground-platform.md` — Give the world a floor
- [ ] `04_rigidbody-and-collider.md` — Give the player a body
- [ ] `05_move-with-velocity.md` — Move the body with velocity
- [ ] `06_verify.md` — milestone gate

## MILESTONE_4 — Gravity, jumping & the ground check
- [ ] `01_tune-gravity.md` — Give gravity some weight
- [ ] `02_ground-layer.md` — Put the ground on its own layer
- [ ] `03_ground-check.md` — Ask whether the player is grounded
- [ ] `04_jump.md` — Jump once per press
- [ ] `05_measure-the-jump.md` — Measure the jump
- [ ] `06_verify.md` — milestone gate

## MILESTONE_5 — Game feel
- [ ] `01_acceleration.md` — Give movement weight
- [ ] `02_coyote-time.md` — Coyote time: jump just after the ledge
- [ ] `03_jump-buffer.md` — Jump buffering: jump just before landing
- [ ] `04_variable-jump-height.md` — Variable jump height
- [ ] `05_reality-check.md` — Stop and play it
- [ ] `06_verify.md` — milestone gate

## MILESTONE_6 — The level as a Tilemap
- [ ] `01_import-the-art.md` — Import the art
- [ ] `02_tile-palette.md` — Build a tile palette
- [ ] `03_rule-tile.md` — Make a Rule Tile
- [ ] `04_paint-the-level.md` — Paint the cavern
- [ ] `05_composite-collider.md` — One collider for the whole level
- [ ] `06_verify.md` — milestone gate

## MILESTONE_7 — Moving & one-way platforms
- [ ] `01_one-way-platform.md` — A ledge you can jump up through
- [ ] `02_moving-platform.md` — A platform that travels
- [ ] `03_carry-the-rider.md` — Carry the rider
- [ ] `04_verify.md` — milestone gate

## MILESTONE_8 — Dash & wall-jump (a movement state machine)
- [ ] `01_add-the-dash-action.md` — Add the Dash action
- [ ] `02_movement-state-machine.md` — Turn the motor into a state machine
- [ ] `03_dash.md` — Dash
- [ ] `04_wall-slide.md` — Cling to a wall
- [ ] `05_wall-jump.md` — Jump off the wall
- [ ] `06_verify.md` — milestone gate

## MILESTONE_9 — Coins, enemies, damage, lives & checkpoints
- [ ] `01_coin-prefab.md` — Make a coin
- [ ] `02_collect-coins.md` — Collect coins
- [ ] `03_enemy.md` — An enemy that patrols
- [ ] `04_stomp-and-damage.md` — Stomp it, or lose a life
- [ ] `05_checkpoints-and-respawn.md` — Checkpoints and respawn
- [ ] `06_verify.md` — milestone gate

## MILESTONE_10 — Animation, camera & audio
- [ ] `01_dress-the-player.md` — Dress the player
- [ ] `02_animate-the-player.md` — Animate the player
- [ ] `03_cinemachine-camera.md` — The camera follows
- [ ] `04_parallax.md` — A background with depth
- [ ] `05_sound-effects.md` — Sound effects
- [ ] `06_audio-mixer.md` — The audio mixer
- [ ] `07_verify.md` — milestone gate

## MILESTONE_11 — Scenes, menus, HUD & persistence
- [ ] `01_hud.md` — The HUD
- [ ] `02_more-scenes.md` — More scenes
- [ ] `03_game-session.md` — Carry the run across scenes
- [ ] `04_pause.md` — Pause
- [ ] `05_timer-and-best-time.md` — The timer and the best time
- [ ] `06_game-over.md` — Game over
- [ ] `07_verify.md` — milestone gate

## MILESTONE_12 — Options: volume, display & key rebinding
- [ ] `01_options-panel.md` — The options panel
- [ ] `02_volume-sliders.md` — Volume sliders
- [ ] `03_display-settings.md` — Display settings
- [ ] `04_rebinding.md` — Rebind a key
- [ ] `05_persist-rebinds.md` — Make rebinds stick
- [ ] `06_verify.md` — milestone gate

## MILESTONE_13 — Build & ship
- [ ] `01_player-settings.md` — Player settings
- [ ] `02_build-it.md` — Build it
- [ ] `03_test-the-build.md` — Test the build
- [ ] `04_the-repository.md` — The repository
- [ ] `05_verify.md` — milestone gate
