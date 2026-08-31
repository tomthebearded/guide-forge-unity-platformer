<!-- Foundation doc. Section headings and table column keys stay English by design (they are the schema
     the GuideForge skills look things up by). Prose language: English. -->

# STATUS — Cavern Dash

> _Generated with **GuideForge v1.18.0** on 2026-08-22._
> _Last updated with **GuideForge v1.16.0** on 2026-08-31._

## Frontier
- **Current frontier:** M7 — not started. M1–M6 complete and verified.
- **Executed through:** M6 / 06_verify.md (2026-08-31) — gate confirmed by the reader.
- **Note:** M6/05–06 carry a 2026-08-31 correction (Used By Composite → **Composite Operation** = `Merge`, Unity 6.3). It was wording-only and the reader verified against the corrected steps.

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
| M7 — Moving & one-way platforms | ❌ | — | |
| M8 — Dash & wall-jump (a movement state machine) | ❌ | — | |
| M9 — Coins, enemies, damage, lives & checkpoints | ❌ | — | |
| M10 — Animation, camera & audio | ❌ | — | |
| M11 — Scenes, menus, HUD & persistence | ❌ | — | |
| M12 — Options: volume, display & key rebinding | ❌ | — | |
| M13 — Build & ship | ❌ | — | Gate is observed in the built player, not the Editor. Amended 2026-08-24 (D30): README, licence and credits move to the repository root. |

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
- 2026-08-22 — Plan approved (13 milestones). Scaffold stamped with GuideForge v1.18.0: README, foundation docs, and 13 placeholder milestone overviews.
- 2026-08-22 — Audit run (GuideForge v1.18.0): 1 BLOCKER, 11 WARNINGs. Every WARNING fixed — three over-long commit subjects, a README row and a conventions promise the content did not keep, a dead `WinScene` constant, two `[Mn]` markers naming the wrong milestone, one third-person reference, two bare built-ins (`Vector2.Lerp`, `SetIsOnWithoutNotify`), an over-long M9 goal, and a gate reading git's human summary instead of `--porcelain`. **The BLOCKER stands:** the eight break recipes (M2/04, M4/05, M5/02-04, M6/05, M10/04, M12/05) describe failures derived from the code, never observed — they need a real Unity 6.3 session, and so do the seven engine-derived gate numbers.
- 2026-08-22 — Whole guide drafted with GuideForge v1.18.0: 13 milestones, 77 step files including 13 verify gates. `progress.md` filled with one row per step. Structural self-audit run: nav lines, Done-when gates, handoffs, glossary and decision-log anchors, and every relative link check clean. **No gate has been executed by a person yet** — every milestone is ❌ below until someone follows the guide in the Editor.
