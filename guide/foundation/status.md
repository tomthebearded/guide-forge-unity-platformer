<!-- Foundation doc. Section headings and table column keys stay English by design (they are the schema
     the GuideForge skills look things up by). Prose language: English. -->

# STATUS — Cavern Dash

> _Generated with **GuideForge v1.18.0** on 2026-08-22._
> _Last updated with **GuideForge v1.18.0** on 2026-09-07._

## Frontier
- **Current frontier:** M12 — not started. **M11 is complete and verified** (gate passed 2026-09-07).
- **Executed through:** M11 / 07_verify.md (2026-09-07) — the milestone gate confirmed by the reader, observed
  against corrected code: both retrofits M11 carried (the lifecycle-order fix in `04_pause.md`, the stomp fix
  in `07_verify.md`) were applied to the project first.
- **Outstanding gates:** **M9's `06_verify.md`** (not re-run since the 2026-09-07 stomp fix) and **M10's
  `07_verify.md`** (not re-run since the 2026-09-07 lifecycle-order fix). Both milestones stay `⏳` until a
  person observes them; the code repairs themselves are already in the project. M11's gate box *"nothing
  earlier regressed"* was observed against the corrected stomp, which is evidence toward M9 but is not M9's
  own gate.
- **Note:** M7/03 (rider carry) was corrected 2026-08-31 via /report-issue — re-parenting replaced by movement inheritance (OverlapBox detection + frictionless surface). All of M7 was unexecuted when it was fixed, so the reader followed the corrected steps and observed the gate against them.

## Source inputs
| Input file | Used for (stack / scope / decisions) | Provided on | Re-checked on |
|------------|--------------------------------------|-------------|---------------|
| `guide-forge-web-platformer` (the *Shape Jumper* guide, GuideForge v1.1.0) | Lineage: the milestone spine (loop → motion → gravity/jump → solid ground → collectibles → goal → persisted best time), the reality-check-gate placement, and the repository layout this guide mirrors | 2026-08-22 | — |

## Milestone status
| Milestone | Status | Verified on | Notes |
|-----------|--------|-------------|-------|
| M1 — Project, Editor & version control | ✅ | 2026-08-24 | Amended 2026-08-24 (D30): the project is a folder in this repository, not a repository of its own. Executed and gate passed 2026-08-24. |
| M2 — First script & frame-rate-independent motion | ✅ | 2026-08-28 | Steps 01–04 executed 2026-08-27; `05_verify.md` gate passed 2026-08-28. |
| M3 — Input & running (keyboard and gamepad) | ✅ | 2026-08-28 | Steps 01–05 executed 2026-08-28; `06_verify.md` gate passed 2026-08-28. |
| M4 — Gravity, jumping & the ground check | ✅ | 2026-08-28 | Steps 01–05 executed 2026-08-28; `06_verify.md` gate passed 2026-08-28. |
| M5 — Game feel | ✅ | 2026-08-31 | Gate first passed 2026-08-28, then corrected (coyote double-jump fix). Retrofit applied 2026-08-31 in `PlayerMotor.cs`; `06_verify.md` re-run and passed 2026-08-31 (mash-jump test confirmed). |
| M6 — The level as a Tilemap | ✅ | 2026-08-31 | All six steps executed 2026-08-31; `06_verify.md` gate confirmed by the reader. M6/05–06 were corrected 2026-08-31 (Used By Composite → **Composite Operation** = `Merge`, Unity 6.3) — a wording-only fix; the reader applied `Merge` and verified against the corrected steps, so no separate re-verification is outstanding. |
| M7 — Moving & one-way platforms | ✅ | 2026-08-31 | Steps 01–04 executed 2026-08-31; `04_verify.md` gate confirmed by the reader. M7/03 was corrected 2026-08-31 (/report-issue): rider carry re-cast from re-parenting to movement inheritance (per-step delta + `Physics2D.OverlapBox` detection + a frictionless surface). All of M7 was unexecuted at fix time, so the reader followed the corrected steps and observed the gate against them. |
| M8 — Dash & wall-jump (a movement state machine) | ✅ | 2026-08-31 | Steps 01–06 executed 2026-08-31; `06_verify.md` gate confirmed by the reader. |
| M9 — Coins, enemies, damage, lives & checkpoints | ⏳ | — | Steps 01–06 executed 2026-09-06 and the gate passed. **Sent back for re-verification 2026-09-07:** `04_stomp-and-damage.md` decided a stomp against the enemy's head inside a 0.1-unit tolerance, which a falling player crosses in one physics step, so a stomp from a real jump never landed. Corrected to measure against the centre; the retrofit is in M11/`07_verify.md`. Re-run the gate, stomping from a full jump. Build steps corroborated by commits `6cf544b`, `82af780`, `313efaf`, `a51331f`, `f4a055d`. |
| M10 — Animation, camera & audio | ⏳ | — | Steps 01–07 executed 2026-09-07 and the gate passed, then `05_sound-effects.md` and its `07_verify.md` checkpoint were corrected the same day for the lifecycle-order defect. The retrofit is applied to the project (commit `3b663dd1`); **the gate has not been re-observed** — `07_verify.md` sits at `[~]`. |
| M11 — Scenes, menus, HUD & persistence | ✅ | 2026-09-07 | Steps 01–06 executed 2026-09-07 (commits `32459dbf`, `b44d7b97`, `db7874d5`, `bdc6345a`, `35516dac`, `fd43767a`) and `07_verify.md` gate confirmed by the reader the same day: the whole `Menu` → `Level01` → `Level02` → `Win` loop, the HUD's opening and live numbers, the run accumulating across the load, one `GameSession`, pause and its menu exit, the persisted best time, and the run ending in `GAME OVER` on the third life. Observed against corrected code — `01_hud.md`'s lifecycle retrofit and M9/04's stomp retrofit were both applied first. |
| M12 — Options: volume, display & key rebinding | ❌ | — | |

<!-- Status key: ✅ verified (Done-when passed by hand) · ⏳ in progress · ❌ not started -->

## Drift log
| Date | Where | Guide said | Reality is | Action taken |
|------|-------|-----------|-----------|--------------|
| 2026-08-24 | M1/04 | `git init -b main` inside `cavern-dash` | The repository already exists — `guide/` lives in it and the project is its sibling | `git init` removed; the step now verifies the repository root and the branch instead |
| 2026-08-24 | M1/05, M1/07 | `git lfs track` prints `*.png (.gitattributes)` | git-lfs prefixes each pattern with the directory of the `.gitattributes` that declared it, so it prints `cavern-dash/*.png (cavern-dash/.gitattributes)` | Both gates rewritten to the prefixed form, with the prefix explained as the proof of scoping |
| 2026-08-24 | M1/04, M1/07 | `git status --porcelain` lists `?? Assets/` and siblings | Porcelain paths are root-relative, and a wholly untracked folder collapses to one `?? cavern-dash/` line | Gates now use `git status --porcelain -uall .` and read `cavern-dash/…` paths |
| 2026-08-24 | PLAN.md §8 | Unity's `.gitignore` / `.gitattributes` at the repository root | Anchored patterns (`/[Ll]ibrary/`) resolve against the folder holding the file, so at the root they match nothing | Layout corrected: both files live inside `cavern-dash/`; the root keeps a `.DS_Store`-only `.gitignore` |
| 2026-08-24 | M13/04 | README, licence, credits and screenshot inside `cavern-dash/` | The repository root is the front door, and it holds guide and game together | Moved to the root; `git check-ignore Builds Library` now stated as run from `cavern-dash` |
| 2026-08-24 | M1/05, `conventions.md` | The amendment claimed `git status --short` prints root-relative paths | `--short` honours `status.relativePaths` (default on) and prints them relative to the reader's folder; only `--porcelain` and `git lfs track` are root-relative | Both corrected, and the two renderings written into `conventions.md` § *Repository layout* as a rule |
| 2026-08-24 | M1/04 | The gate piped `git status` into `head` and `grep` | `stack.md` targets PowerShell as well as zsh/bash, where neither exists | Replaced with pipe-free `git status --porcelain -uall <pathspec>`, and the both-shells rule added to `conventions.md` |
| 2026-08-24 | M13/04 | Action 5 numbered its commands "first / second / third" against a different order than it listed them in | The reader would run `git check-ignore` from the repository root, where it prints nothing and the gate fails | Commands named instead of numbered, each with the folder it runs from |
| 2026-08-24 | M1/01, guide `README.md` | Nothing told the reader to clone the repository, yet M1/02 sets the project's location by it | A reader working from a download has no repository to put the project in | Clone stated in M1/01 *Before you start* and in *Following this guide*; M1/04's troubleshooting now covers the download case |
| 2026-08-28 | M5/02, M5/03, M5/06 (swept M8/02, M8/06, M10/07) | Coyote window refilled on *every* grounded frame (`if (IsGrounded) coyoteTimeRemainingSeconds = coyoteTimeSeconds;`); gates tested only a single press | The ground check still reads grounded for a step after take-off, so the refill re-armed the window mid-rise and a mashed press double-jumped in mid-air — contradicting M5's "one press, one jump" gate | **Route B.** Guarded the refill (`&& body.linearVelocity.y <= 0f`, `else if (!IsGrounded)`) in all six places; M5/02/03/06 corrected in place + superseded banners; retrofit (one code change) collected in M6/01 *Before you continue — corrections*; M5 gates re-tightened to test mashing. Re-apply checklist for the reader lives in that corrections section. |
| 2026-08-31 | M6/05 (step 4, seam experiment, two Done-when boxes, "If it breaks"); swept M6/06 (gate box, checkpoint table, troubleshooting) and PLAN.md §gates | Tick **Used By Composite** on the `Tilemap Collider 2D` to feed the composite | Unity 6.3 removed the `usedByComposite` checkbox; a collider now feeds a `CompositeCollider2D` via the **Composite Operation** dropdown — `Merge` (Boolean OR) is the old "ticked", `None` is "unticked" ([Unity 6.3 docs](https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Collider2D.CompositeOperation.html)) | **All ahead of frontier — rewritten in place** (no route decision). Every reference changed to Composite Operation = `Merge`/`None`; a 5.1 failure note added at M6/05 for "there is no Used By Composite checkbox". Verified online against the Unity 6.3 ScriptReference. |
| 2026-08-31 | M7/03 (design, code, Do-this, Done-when, If-it-breaks); swept M7/02 (scale rationale + troubleshooting), M7/04 (two gate boxes, checkpoint code, troubleshooting, handoff), M7/00, PLAN.md, decision-log D14 | Carry the rider by `transform.SetParent()` in `OnCollisionEnter2D`/`Exit2D` — re-parent the player under the platform | `SetParent` from a physics collision callback throws `Cannot set the parent of the GameObject 'Player' while activating or deactivating the parent GameObject` when the first contacts fire during scene activation, and re-parenting a Dynamic `Rigidbody2D` is fragile anyway (inherits the platform's scale, fights world-space physics). Carry instead by measuring the platform's per-step position delta and shifting every rider on its top surface by it | **All ahead of frontier — rewritten in place** (M7 unexecuted; no route decision, no banners). Corrected in three passes the same day: (1) delta-carry with `OnCollisionEnter/Exit` rider tracking still slid (a kinematic platform sliding into a resting body fires those callbacks unreliably) → (2) per-step `Physics2D.OverlapBox` detection of what is on the top edge → (3) the rider then crept a hair ahead, because `MovePosition` still drags a resting body by friction on top of the delta; the carrier now installs a frictionless `PhysicsMaterial2D` on its surface in `Awake` (guarded on `sharedMaterial == null`). |
| 2026-09-07 | M11/01 (design, code, Done-when, If-it-breaks); swept M10/05 + its M10/07 checkpoint, M11/07 checkpoint, `glossary.md` | `HudView` drew its opening `Coins`/`Lives` from `OnEnable`, and `PlayerAudio` seeded `livesLastSeen` from its own `Awake` | Unity promises no `Awake`/`OnEnable` order between two objects, nor between two components on one object. Proven at run time with a headless PlayMode test: `OnEnable frame=11 lives=0` then `PlayerHealth.Awake frame=11 lives=3` — the HUD stuck at `Lives: 0`, because `LivesChanged` only fires on a change. `Start` runs only once every `Awake` has ([Unity 6.3 execution order](https://docs.unity3d.com/6000.3/Documentation/Manual/execution-order.html)) | **Route B.** Opening reads moved to `Start` in M11/01 and M10/05 (subscriptions stay in `OnEnable`); both verify checkpoints realigned and both milestone gates strengthened to exercise the defect (M11/07 opening HUD values, M10/07 first-hit hurt sound); superseded banners on M11/01, M10/05 and M10/07; one retrofit — HudView, the crossed `Stats`/`Health` fields, and PlayerAudio — collected in M11/04 *Before you continue — corrections*, which is the reader's re-apply checklist. New glossary term *script lifecycle order*. M10 → ⏳. |
| 2026-09-07 | M9/04 (design, code, action 4, Done-when, If-it-breaks); swept its M9/06 checkpoint, gate box and troubleshooting row | A stomp is `other.bounds.min.y >= ownCollider.bounds.max.y - stompToleranceUnits` with the tolerance at `0.1` — the feet must be within 10 cm of the enemy's head | A 2D trigger callback runs **after** the physics step that produced the overlap, so the feet sink up to `|velocity.y| × 0.02` before the code looks. Traced headless: `feet=-0.178 head=-0.050 tol=0.1 -> stomp=False` — `0.128` below the head against a `0.1` window, life lost. Free fall reaches `23.4` u/s over six units and `31.9` over eight (`0.47` and `0.64` of sink per step), so no usable tolerance exists: one wide enough would also accept a side collision. Only a slow approach ever passed, which is how the gate came to be ticked | **Route B.** The test now measures against `ownCollider.bounds.center.y`, which gives `0.4` units of room and cannot miss below a `20` u/s landing (above that it is likely, not certain — recorded in D33 with the exact upgrade if it ever bites); `stompToleranceUnits` is deleted. Verified after the fix: drops from one, four and eight units (impacts `10.7`, `22.0`, `31.9` u/s) all stomp, and a side contact still costs a life. M9/04 and the M9/06 checkpoint corrected and bannered; both gates now say **from a full jump**, because a gentle approach passes on a broken build; the backwards failure note replaced by the symptom actually reported. Retrofit in M11/`07_verify.md` *Before you continue — corrections*. M9 → ⏳. **Not** a regression from M11: the `Player` and `Enemy` objects are byte-identical to commit `26f15d53`; M11/06 only made the failure loud, by replacing the silent lives-reset with GAME OVER. |

- 2026-09-07 — **Scope reduced: M13 — Build & ship removed; the guide is now 12 milestones.** The guide
  teaches development, not distribution, so packaging the game into a standalone executable and authoring
  the repository's own README/licence/credits are out of scope. `MILESTONE_13_build-and-ship/` deleted (4
  steps + its gate); **M12 is now the last milestone**, its handoff rewritten to close the guide and its nav
  `next` cleared. Every forward reference to the M13 gate was re-homed rather than dropped: the two
  gamepad boxes deferred to it (M3/02, M3/06) now close whenever a pad is to hand, M9/01's coin placement is
  justified by the later gates instead of a build, M11/05's persistence note explains the Editor equivalent,
  M12/03's fullscreen toggle states plainly that the window is a build-only effect this guide never checks,
  and M1/01 keeps the IL2CPP build module as a recommendation rather than a requirement. `PLAN.md` §4/§5/§8,
  `conventions.md` § *Repository layout* and D6/D26/D30 updated; [D33](decision-log.md#d33--the-guide-stops-at-development-no-build-and-ship-milestone)
  records the decision. **Earlier dated entries — in the drift log above and further
  down this log — still mention M13; they are history, and the files they name no longer exist.**
- 2026-09-07 — **M10 executed and completed; M10 → ✅.** Reader confirmed steps 01–07 run and the
  `07_verify.md` gate passed (the character sprite is dressed and flips with input, the Animator drives
  idle/run/jump/fall from movement parameters, the Cinemachine camera follows and stays confined to the
  level, two parallax layers give the background depth, jump/land/dash play their sound effects, and every
  sound routes through the mixer's Music and SFX groups). Rows 01–07 marked `[x]`; **M10 → ✅** (verified
  2026-09-07). Frontier advanced to M11 (not started). The six build steps are each backed by a commit
  (`6bec19a` character sprite, `fbb7f6a` animation driver, `3818162` camera follow + confiner, `db3308d`
  parallax layers, `b852e3e` jump/land/dash SFX, `32d9ce8` audio mixer with Music and SFX groups).
- 2026-09-06 — **M9 executed and completed; M9 → ✅.** Reader confirmed steps 01–06 run and the
  `06_verify.md` gate passed (coins collect and count, enemies patrol and turn at ledges and walls, a stomp
  kills and bounces, a touch costs a life behind one-second invulnerability, the run resets at zero,
  checkpoints arm once and respawn works, the kill zone below the level costs a life). Rows 01–06 marked
  `[x]`; **M9 → ✅** (verified 2026-09-06). Frontier advanced to M10 (not started). The five build steps are
  each backed by a commit (`6cf544b` coin prefab, `82af780` collection through the `PlayerStats` event,
  `313efaf` patrolling enemy, `a51331f` lives + i-frames + stomp-or-be-hurt, `f4a055d` checkpoints, respawn
  and kill zone).
- 2026-08-31 — **M8 executed and completed; M8 → ✅.** Reader confirmed steps 01–06 run and the
  `06_verify.md` gate passed (dash and wall-jump behave as a movement state machine). Rows 01–06 marked
  `[x]`; **M8 → ✅** (verified 2026-08-31). Frontier advanced to M9 (not started).
- 2026-08-31 — **M7 executed and completed; M7 → ✅.** Reader confirmed steps 01–04 run and the
  `04_verify.md` gate passed (rider travels with the platform, no re-parent, no creep; one-way ledge and
  platform travel behave). Rows 01–04 marked `[x]`; **M7 → ✅** (verified 2026-08-31). Frontier advanced to
  M8 (not started). M7/03 was the /report-issue fix applied and tested the same day (movement inheritance +
  OverlapBox + frictionless surface); the guide fix and the game script were committed separately.
- 2026-08-31 — **M7/03 rider carry fixed (/report-issue).** Field report: on entering Play the Console threw
  `Cannot set the parent of the GameObject 'Player' while activating or deactivating the parent GameObject`,
  from `PlatformRiderCarrier` calling `transform.SetParent(...)` inside `OnCollisionEnter2D`. Root cause:
  re-parenting from a physics callback during scene activation is forbidden, and the re-parenting approach
  itself is fragile for a Dynamic body (scale inheritance, transform-vs-physics fighting). Re-cast the step to
  **movement inheritance**: the platform measures its own per-step position delta and shifts each rider by it,
  applied to `Rigidbody2D.position`, so the player's movement code stays platform-agnostic. Rider detection
  went through two forms the same day — an `OnCollisionEnter/Exit` set that **still slid** (a kinematic
  platform sliding into a resting body fires those callbacks unreliably), then a per-step
  `Physics2D.OverlapBox` strip on the top edge, which is deterministic. A third pass fixed the rider then
  **creeping a hair ahead** of the platform: `MovePosition` drags a resting body by friction on top of the
  delta, so the carrier now installs a frictionless `PhysicsMaterial2D` on its surface in `Awake` (guarded so
  an intentionally-assigned material is respected). All hits **ahead of the frontier**
  (M7 unexecuted), so rewritten in place — no route decision, no banners. Swept M7/02 (scale rationale +
  troubleshooting), M7/04 (gate boxes + checkpoint code + troubleshooting + handoff), M7/00, PLAN.md; D14
  records the superseded premise. `feedback-log.md` entry added (Status: fixed via /report-issue).
- 2026-08-31 — **M6 fixed (Used By Composite → Composite Operation) and completed; M6 → ✅.** Field report:
  the reader had no **Used By Composite** checkbox on the `Tilemap Collider 2D` (Unity 6.3 replaced it with
  the **Composite Operation** dropdown). Root cause: stale API label — `usedByComposite` was removed; a
  collider now feeds a `CompositeCollider2D` via `compositeOperation`, where `Merge` = old ticked and `None`
  = old unticked (verified against the Unity 6.3 ScriptReference). All hits were **ahead of the frontier**
  (M6/04–06 unexecuted), so rewritten in place — no route decision, no superseded banners. Swept M6/05 (6
  spots), M6/06 (4), PLAN.md (1); a 5.1 failure note added at M6/05. Then, on the reader's confirmation
  ("tutto verificato"), M6/04–06 marked `[x]` and **M6 → ✅** (verified 2026-08-31): the correction was
  wording-only and the reader applied `Merge` and observed the `06_verify.md` gate against the corrected
  steps. Frontier advanced to M7. `feedback-log.md` created with the report (Status: fixed via /report-issue).
- 2026-08-31 — **M5 gate re-run and passed; M5 → ✅.** Reader confirmed re-running `M5/06_verify.md` in Play
  Mode after the coyote retrofit — mashing jump in mid-air no longer double-jumps. `06_verify.md` moved
  `[~]` → `[x]`; M5 marked ✅ (verified 2026-08-31). Frontier stays at M6/04.
- 2026-08-31 — **M6 steps 01–03 executed; M5 retrofit applied.** Reader marked through M6/03, all valid.
  Verified on disk: art imported at 18 PPU (commit `cac8620`); `CavernPalette` + 11 tile assets (commit
  `ac0315a`); `GroundRuleTile.asset` created (untracked) with Default Sprite `tile_0122`, 5 tiling rules,
  collider = Sprite. The M5 coyote retrofit is present in `PlayerMotor.cs` (refill guarded:
  `IsGrounded && body.linearVelocity.y <= 0f` / `else if (!IsGrounded)`) and `JumpApexProbe` is gone from
  disk — so M5/02 and M5/03 moved `[!]` → `[x]`. **M6 → ⏳** (04–06 pending); frontier at M6/04. **M5 stays
  ⏳:** the reader did not confirm re-running `M5/06_verify.md` after the correction, so its gate is `[~]`,
  not `[x]`. (Still open, unchanged: no `.gitattributes` exists, so `*.png` is not actually LFS-tracked.)
- 2026-08-28 — **M5 coyote double-jump fixed (/report-issue, Route B).** Field report: mashing jump sometimes
  produced a second jump in mid-air. Root cause: the coyote window was refilled on every grounded frame, and
  the ground check still reads grounded for a physics step after take-off, so a freshly pressed (mashed) jump
  caught the re-armed window. Fixed by guarding the refill against re-arming while rising, in M5/02, M5/03
  (gate), M5/06 (checkpoint + gate) and swept into M8/02, M8/06, M10/07. Executed steps 02/03/06 corrected in
  place with superseded banners; retrofit collected in M6/01's corrections section; rows `[!]`. **M5 → ⏳,
  needs re-verification.** (Separately noted, not fixed here: M1/05 is marked done but no `.gitattributes`
  exists, so `*.png` is **not** actually LFS-tracked — a distinct open defect.)
- 2026-08-28 — **M5 gate confirmed passed; milestone complete.** Reader confirmed step 05 (played it) and the
  `06_verify.md` gate, and deleted the apex probe — `JumpApexProbe*` confirmed gone from disk, so the gate's
  "measuring tool is gone" box is genuinely satisfied. Rows 05 and 06 marked `[x]`; M5 marked ✅ (verified
  2026-08-28). Frontier advanced to M6 (not started).
- 2026-08-28 — **M5 marked against the codebase (claimed "complete").** Reconciled the "M5 complete" claim
  with the working tree. Build steps 01–04 (01_acceleration → 04_variable-jump-height) are demonstrably in
  `PlayerMotor.cs` (acceleration via `Mathf.MoveTowards`, `coyoteTimeSeconds`, `jumpBufferSeconds` +
  `TimeSinceJumpPressedSeconds`, fall/low-jump gravity multipliers + `IsJumpHeld`), each backed by a commit —
  marked `[x]`. **Step 05 (reality-check) is NOT done:** it deletes `JumpApexProbe.cs`, but that file is still
  on disk, so its `[ ]` stands. The `06_verify.md` gate is a hand-observed Play Mode test and would currently
  fail its "measuring tool is gone" box while the probe exists — left `[ ]`. **M5 stays ⏳, not ✅.** Frontier
  advanced to M5 / 04. Awaiting confirmation the reader played it, deleted the probe, and the gate passed.
- 2026-08-28 — **M4 gate confirmed passed.** Reader confirmed the `06_verify.md` (M4) gate passed. Verify row
  marked `[x]`; M4 marked ✅ (verified 2026-08-28). Frontier advanced to M5 (not started).
- 2026-08-28 — **M4 build steps executed.** Reader reported "M4 done"; steps 01–05
  (01_tune-gravity → 05_measure-the-jump) marked `[x]`. The `06_verify.md` milestone gate was not mentioned,
  so M4 stays ⏳ and its verify row unticked pending confirmation the gate passed. Frontier advanced to M4 / 05.
- 2026-08-28 — **M2 and M3 gates confirmed passed.** Reader confirmed both `05_verify.md` (M2) and
  `06_verify.md` (M3) gates passed. Both verify rows marked `[x]`; M2 and M3 both marked ✅ (verified 2026-08-28).
  Frontier advanced to M4 (not started).
- 2026-08-28 — **M3 build steps executed.** Reader reported "M3 done"; steps 01–05
  (01_meet-the-input-system → 05_move-with-velocity) marked `[x]`. The `06_verify.md` milestone gate was not
  mentioned, so M3 stayed ⏳ pending confirmation the gate passed (confirmed later same day). Frontier advanced
  to M3 / 05.
- 2026-08-27 — **M2 build steps executed.** Reader reported "milestone 2 completed"; steps 01–04
  (01_player-sprite → 04_frame-rate-independence) marked `[x]`. The `05_verify.md` milestone gate was not
  mentioned, so M2 stays ⏳ and its verify row unticked pending confirmation the gate passed. Frontier advanced
  to M2 / 04.
- 2026-08-24 — **M1 executed and completed.** All seven steps (01_install-unity → 07_verify) run in order; the
  `07_verify.md` milestone gate passed. Frontier advanced to M2; M1 marked ✅ (verified 2026-08-24).
- 2026-08-24 — Audit of M1 and M13 after the amendment (GuideForge v1.18.0): **4 BLOCKERs, 5 WARNINGs, all
  fixed.** The blockers were the `git status --short` path claim (wrong in M1/05 and promoted to a rule in
  `conventions.md`), two Unix-only pipes in M1/04's gate on a guide that targets PowerShell too, and M13/04's
  command ordinals contradicting the order the commands were listed in. Warnings: M1/06's stale "one commit
  exists", six later gates whose `git status --short` output is correct only from `cavern-dash` and never said
  so (M2/01, M4/01, M4/02, M6/02, M6/03, M8/01, M13/01), M6/01's un-prefixed `git lfs track` pattern, and the
  repository never being established as a clone. **Still unverified:** `git lfs track`'s directory-prefixed
  output is read from the git-lfs source, not from a run — settle it on a real clone with Git LFS installed.
- 2026-08-24 — Amended (GuideForge v1.18.0): the Unity project is a folder in this repository beside `guide/`,
  not a repository of its own ([D30](decision-log.md#d30--the-unity-project-is-a-folder-in-the-guides-repository-not-a-repository-of-its-own)).
  Nothing had been executed, so the whole guide was ahead of the frontier — no superseded banners and no
  corrections section were needed. Rewrote M1/02, M1/04, M1/05, M1/07, M1/00, M13/02, M13/04, M13/05; corrected
  `PLAN.md` §1 and §8; added a *Repository layout* section to `conventions.md`. The amendment also closed two
  gates that would have failed as written: `git lfs track`'s output is directory-prefixed, and
  `git status --porcelain` collapses an untracked folder to a single line. **M1 and M13 stay ❌** — not started,
  and their gates are now unrun in their new form.
- 2026-08-23 — Second audit run (GuideForge v1.18.0): 3 BLOCKERs, 14 WARNINGs. **All 17 fixed.** The three
  blockers were: M5's milestone gate asked for an apex number after step 05 deletes the probe that prints it
  (the gate now reads back step 04's recorded figures); the jump buffer stamped its press on the frame clock
  and read the age on the physics clock, so a correct build could still jump with the window set to `0`
  (clamped at zero, tested with `<` — [D27](decision-log.md#d27--the-jump-buffer-clamps-its-age-at-zero-because-timetime-is-two-clocks)); and two
  break recipes asserted failures that a correct build may not show (M2/04's "first line reads low", which
  the arithmetic contradicts, and M6/05's seam stutter, which is machine-dependent — both now gate on
  something deterministic). Warnings closed: M8's step counters, M4's apex tolerance contradicting its own
  troubleshooting, M12/05's "three call sites" that were two, three component names in `conventions.md` that
  no script uses, the `moveSpeedUnitsPerSecond` collision between M2 and M3, three missing `Build vs borrow`
  callouts (M8/02, M11/03, M12/01), `AudioSettings` shadowing `UnityEngine.AudioSettings`
  ([D29](decision-log.md#d29--the-options-component-is-audiooptions-not-audiosettings)), a `PlayerPrefs`
  disk write per slider frame ([D28](decision-log.md#d28--volume-settings-flush-to-disk-on-panel-close-not-on-every-slider-frame)),
  a `Died` comment that contradicted its code, two M8 gate boxes that quoted numbers with no instrument,
  M1/04's undeclared same-commit bundle, a README file count, a third-person glossary entry, and a
  mis-indented line in M9's checkpoint. **Still open:** the remaining engine-derived gate numbers — the apex
  range `2.20`–`2.60`, the dash `5.0`–`5.5`, the `17`/`67` ms frame times — are derived from the code and the
  physics, not observed in a Unity 6.3 session. They stay unverified until someone runs the guide.
- 2026-09-07 — **M11 verified; milestone → ✅.** The reader ran `07_verify.md` by hand and confirmed the gate, with both retrofits already applied to the project, so the run was observed against corrected code. `07_verify.md` marked `[x]`; frontier advanced to M12 (not started). **M9 and M10 remain ⏳** — neither gate has been re-run since the day's two fixes, and neither was claimed.
- 2026-09-07 — **Second field report acted on (`/report-issue`): stomping an enemy from a jump hurt the player instead of killing it.** Root cause: the stomp compared the feet with the enemy's head inside a 0.1-unit tolerance, smaller than the sink of a single physics step at any landing speed (`|velocity.y| × 0.02`; measured 0.128 on a gentle 5.41 u/s contact, 0.47 in a six-unit fall). Fixed by measuring against the enemy's centre, which gives 0.4 units of room; the tolerance field is gone. Reader chose **route B**: M9/04 and the M9/06 checkpoint corrected and bannered, retrofit consolidated in M11/`07_verify.md`, M9 sent back to ⏳. Swept the same class — a geometric threshold read after a body has moved — across the guide: M7/03's `standingToleranceUnits` has the same shape but tests bodies at rest and is sound, and the ground and wall checks use an `OverlapBox` volume rather than a threshold. Recorded that this was **not** a regression from M11; M11/06 only made a long-standing defect visible by ending the run instead of quietly resetting the lives.
- 2026-09-07 — **M11 steps 01–06 executed and recorded; ledger reconciled.** The six steps had all been run and committed while `progress.md` still read "M11 not started"; every row is now `[x]` with its commit, and the frontier moved from M10/07 to M11/06. M10/05's `[!]` cleared to `[x]` — its retrofit is applied (commit `3b663dd1`) — while M10/07 went to `[~]`, because applying the retrofit is not the same as re-observing the gate. **Neither milestone is `✅`:** M11's gate has never been run and M10's has not been re-run. What does exist is an automated Unity PlayMode suite written alongside the work (28 tests, all green) covering the scene chain, the HUD, both level exits, pause, the timer, the best time and game over — evidence that stands beside the by-hand gates, not in place of them.
- 2026-09-07 — **Field report acted on (`/report-issue`): the HUD showed `Lives: 0`.** Root cause: a step taught an opening read from `OnEnable`, which Unity may run before the `Awake` that fills the value in — confirmed by a headless PlayMode trace, and the `Start`-based fix confirmed the same way. One sibling found by the sweep (`PlayerAudio.livesLastSeen`, latent: it survives only because the component happens to sit after `PlayerHealth` in the Player's component list). Reader chose **route B**: M11/01, M10/05 and M10/07 corrected and bannered, the repair consolidated in M11/04. A second, unrelated defect surfaced in the same session and needed no guide change — `HudView`'s `Stats` and `Health` were crossed over in both level scenes (a wiring slip, not a guide instruction); it is covered by a new failure note and by action 2 of the corrections section. M10 sent back to ⏳.
- 2026-08-22 — Plan approved (13 milestones). Scaffold stamped with GuideForge v1.18.0: README, foundation docs, and 13 placeholder milestone overviews.
- 2026-08-22 — Audit run (GuideForge v1.18.0): 1 BLOCKER, 11 WARNINGs. Every WARNING fixed — three over-long commit subjects, a README row and a conventions promise the content did not keep, a dead `WinScene` constant, two `[Mn]` markers naming the wrong milestone, one third-person reference, two bare built-ins (`Vector2.Lerp`, `SetIsOnWithoutNotify`), an over-long M9 goal, and a gate reading git's human summary instead of `--porcelain`. **The BLOCKER stands:** the eight break recipes (M2/04, M4/05, M5/02-04, M6/05, M10/04, M12/05) describe failures derived from the code, never observed — they need a real Unity 6.3 session, and so do the seven engine-derived gate numbers.
- 2026-08-22 — Whole guide drafted with GuideForge v1.18.0: 13 milestones, 77 step files including 13 verify gates. `progress.md` filled with one row per step. Structural self-audit run: nav lines, Done-when gates, handoffs, glossary and decision-log anchors, and every relative link check clean. **No gate has been executed by a person yet** — every milestone is ❌ below until someone follows the guide in the Editor.
