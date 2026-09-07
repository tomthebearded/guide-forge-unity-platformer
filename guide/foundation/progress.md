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
- **Last executed:** M11 / 07_verify.md — **gate passed 2026-09-07**. M11 is complete: steps 01–06 executed
  and its Done-when gate observed by hand.
- **Next up:** M12 / 01_options-panel.md.
- **Outstanding gates:** two, both re-verifications rather than new work. **M9's** `06_verify.md` has not been
  re-run since the 2026-09-07 stomp fix (`04_stomp-and-damage.md` is `[!]`, `06_verify.md` is `[~]`), and
  **M10's** `07_verify.md` has not been re-run since the 2026-09-07 lifecycle-order fix (`07_verify.md` is
  `[~]`). Both retrofits are applied to the project; what is missing is a person observing the two gates.
- **Note:** the guide is mid-restructure from 13 milestones to 12 — `MILESTONE_13/` is deleted in the working
  tree. This ledger still carries its rows; they need reconciling once that change lands.

## MILESTONE_1 — Project, Editor & version control
- [x] `01_install-unity.md` — Install Unity Hub and Unity 6.3 LTS — 2026-08-24
- [x] `02_create-project.md` — Create the Universal 2D project — 2026-08-24
- [x] `03_find-your-way-around.md` — Find your way around the Editor — 2026-08-24
- [x] `04_git-init.md` — Put the project under Git — 2026-08-24
- [x] `05_git-lfs.md` — Track binary assets with Git LFS — 2026-08-24
- [x] `06_project-folders.md` — Lay out the project folders and name the scene — 2026-08-24
- [x] `07_verify.md` — milestone gate — 2026-08-24

## MILESTONE_2 — First script & frame-rate-independent motion
- [x] `01_player-sprite.md` — Add the player sprite — 2026-08-27
- [x] `02_first-script.md` — Write your first MonoBehaviour — 2026-08-27
- [x] `03_tune-in-inspector.md` — Tune it while it runs — 2026-08-27
- [x] `04_frame-rate-independence.md` — Prove the motion is frame-rate independent — 2026-08-27
- [x] `05_verify.md` — milestone gate — 2026-08-28

## MILESTONE_3 — Input & running (keyboard and gamepad)
- [x] `01_meet-the-input-system.md` — Meet the Input System — 2026-08-28
- [x] `02_input-reader.md` — Read the Move action — 2026-08-28
- [x] `03_ground-platform.md` — Give the world a floor — 2026-08-28
- [x] `04_rigidbody-and-collider.md` — Give the player a body — 2026-08-28
- [x] `05_move-with-velocity.md` — Move the body with velocity — 2026-08-28
- [x] `06_verify.md` — milestone gate — 2026-08-28

## MILESTONE_4 — Gravity, jumping & the ground check
- [x] `01_tune-gravity.md` — Give gravity some weight — 2026-08-28
- [x] `02_ground-layer.md` — Put the ground on its own layer — 2026-08-28
- [x] `03_ground-check.md` — Ask whether the player is grounded — 2026-08-28
- [x] `04_jump.md` — Jump once per press — 2026-08-28
- [x] `05_measure-the-jump.md` — Measure the jump — 2026-08-28
- [x] `06_verify.md` — milestone gate — 2026-08-28

## MILESTONE_5 — Game feel
- [x] `01_acceleration.md` — Give movement weight — 2026-08-28
- [x] `02_coyote-time.md` — Coyote time: jump just after the ledge — executed 2026-08-28; **retrofit applied 2026-08-31** (coyote refill guarded against mid-rise re-arm in `PlayerMotor.cs`)
- [x] `03_jump-buffer.md` — Jump buffering: jump just before landing — executed 2026-08-28; **retrofit applied 2026-08-31** (same guard; gate re-tightened to check mashing)
- [x] `04_variable-jump-height.md` — Variable jump height — 2026-08-28
- [x] `05_reality-check.md` — Stop and play it — 2026-08-28 (played; `JumpApexProbe.cs` deleted — confirmed gone from disk)
- [x] `06_verify.md` — milestone gate — re-run and passed 2026-08-31 (mash-jump test confirmed after the coyote retrofit; first passed 2026-08-28, then corrected)

## MILESTONE_6 — The level as a Tilemap
- [x] `01_import-the-art.md` — Import the art — 2026-08-31 (Kenney tiles at 18 PPU; carried the *Before you continue — corrections* coyote retrofit)
- [x] `02_tile-palette.md` — Build a tile palette — 2026-08-31 (`CavernPalette` + 11 tile assets)
- [x] `03_rule-tile.md` — Make a Rule Tile — 2026-08-31 (`GroundRuleTile`: Default Sprite `tile_0122`, 5 tiling rules, collider = Sprite)
- [x] `04_paint-the-level.md` — Paint the cavern — 2026-08-31
- [x] `05_composite-collider.md` — One collider for the whole level — 2026-08-31 (Composite Operation = `Merge`; step corrected 2026-08-31 from the stale "Used By Composite" label)
- [x] `06_verify.md` — milestone gate — passed 2026-08-31 (verified against the corrected M6/05–06 wording)

## MILESTONE_7 — Moving & one-way platforms
- [x] `01_one-way-platform.md` — A ledge you can jump up through — 2026-08-31
- [x] `02_moving-platform.md` — A platform that travels — 2026-08-31
- [x] `03_carry-the-rider.md` — Carry the rider — 2026-08-31 (carried by movement inheritance after the /report-issue fix: OverlapBox detection + frictionless surface, replacing re-parenting)
- [x] `04_verify.md` — milestone gate — passed 2026-08-31

## MILESTONE_8 — Dash & wall-jump (a movement state machine)
- [x] `01_add-the-dash-action.md` — Add the Dash action — 2026-08-31
- [x] `02_movement-state-machine.md` — Turn the motor into a state machine — 2026-08-31
- [x] `03_dash.md` — Dash — 2026-08-31
- [x] `04_wall-slide.md` — Cling to a wall — 2026-08-31
- [x] `05_wall-jump.md` — Jump off the wall — 2026-08-31
- [x] `06_verify.md` — milestone gate — passed 2026-08-31

## MILESTONE_9 — Coins, enemies, damage, lives & checkpoints
- [x] `01_coin-prefab.md` — Make a coin — 2026-09-06
- [x] `02_collect-coins.md` — Collect coins — 2026-09-06
- [x] `03_enemy.md` — An enemy that patrols — 2026-09-06
- [!] `04_stomp-and-damage.md` — Stomp it, or lose a life — 2026-09-06 — **invalidated 2026-09-07** by the
  stomp fix (the test now measures against the enemy's centre, and `stompToleranceUnits` is deleted);
  repaired by *Before you continue — corrections* in
  `MILESTONE_11_scenes-menus-hud-persistence/07_verify.md`
- [x] `05_checkpoints-and-respawn.md` — Checkpoints and respawn — 2026-09-06
- [~] `06_verify.md` — milestone gate — passed 2026-09-06 — **invalidated 2026-09-07**: its `EnemyContact`
  checkpoint and its stomp gate box both changed. Outstanding: re-run the gate after applying the
  corrections in `MILESTONE_11_scenes-menus-hud-persistence/07_verify.md`, stomping **from a full jump**

## MILESTONE_10 — Animation, camera & audio
- [x] `01_dress-the-player.md` — Dress the player — 2026-09-07 (commit `6bec19a`)
- [x] `02_animate-the-player.md` — Animate the player — 2026-09-07 (commit `fbb7f6a`)
- [x] `03_cinemachine-camera.md` — The camera follows — 2026-09-07 (commit `3818162`)
- [x] `04_parallax.md` — A background with depth — 2026-09-07 (commit `db3308d`)
- [x] `05_sound-effects.md` — Sound effects — 2026-09-07 (commit `b852e3e`); **retrofit applied 2026-09-07**
  (`livesLastSeen` moved from `Awake` to `Start` via the *Before you continue — corrections* section in
  `MILESTONE_11_scenes-menus-hud-persistence/04_pause.md`, commit `3b663dd1`)
- [x] `06_audio-mixer.md` — The audio mixer — 2026-09-07 (commit `32d9ce8`)
- [~] `07_verify.md` — milestone gate — first passed 2026-09-07, then **invalidated** the same day when the
  `PlayerAudio` checkpoint changed. The retrofit is applied to the project (commit `3b663dd1`), but the gate
  itself has **not been re-observed** — outstanding: re-run it, in particular the box on the hurt sound
  playing on the *first* hit of a run

## MILESTONE_11 — Scenes, menus, HUD & persistence
- [x] `01_hud.md` — The HUD — 2026-09-07 (commit `32459dbf`); **retrofit applied 2026-09-07** (the opening
  `ShowCoins`/`ShowLives` moved from `OnEnable` to `Start` via the corrections section in `04_pause.md`,
  commit `3b663dd1`, which also straightened the crossed `Stats`/`Health` fields in both level scenes)
- [x] `02_more-scenes.md` — More scenes — 2026-09-07 (commit `b44d7b97`)
- [x] `03_game-session.md` — Carry the run across scenes — 2026-09-07 (commit `db7874d5`)
- [x] `04_pause.md` — Pause — 2026-09-07 (commit `bdc6345a`)
- [x] `05_timer-and-best-time.md` — The timer and the best time — 2026-09-07 (commit `35516dac`)
- [x] `06_game-over.md` — Game over — 2026-09-07 (commit `fd43767a`)
- [x] `07_verify.md` — milestone gate — passed 2026-09-07 (observed by the reader, against the corrected
  code: both retrofits — the lifecycle-order one in `04_pause.md` and the stomp one in this file — were
  already applied to the project)

## MILESTONE_12 — Options: volume, display & key rebinding
- [ ] `01_options-panel.md` — The options panel
- [ ] `02_volume-sliders.md` — Volume sliders
- [ ] `03_display-settings.md` — Display settings
- [ ] `04_rebinding.md` — Rebind a key
- [ ] `05_persist-rebinds.md` — Make rebinds stick
- [ ] `06_verify.md` — milestone gate

