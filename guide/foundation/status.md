<!-- Foundation doc. Section headings and table column keys stay English by design (they are the schema
     the GuideForge skills look things up by). Prose language: English. -->

# STATUS — Cavern Dash

> _Generated with **GuideForge v1.18.0** on 2026-08-22._
> _Last updated with **GuideForge v1.16.0** on 2026-08-27._

## Frontier
- **Current frontier:** M2 — in progress (steps 01–04 executed; `05_verify.md` gate not yet confirmed).
- **Executed through:** M2 / 04_frame-rate-independence.md (2026-08-27).

## Source inputs
| Input file | Used for (stack / scope / decisions) | Provided on | Re-checked on |
|------------|--------------------------------------|-------------|---------------|
| `guide-forge-web-platformer` (the *Shape Jumper* guide, GuideForge v1.1.0) | Lineage: the milestone spine (loop → motion → gravity/jump → solid ground → collectibles → goal → persisted best time), the reality-check-gate placement, and the repository layout this guide mirrors | 2026-08-22 | — |

## Milestone status
| Milestone | Status | Verified on | Notes |
|-----------|--------|-------------|-------|
| M1 — Project, Editor & version control | ✅ | 2026-08-24 | Amended 2026-08-24 (D30): the project is a folder in this repository, not a repository of its own. Executed and gate passed 2026-08-24. |
| M2 — First script & frame-rate-independent motion | ⏳ | — | Steps 01–04 executed 2026-08-27; `05_verify.md` gate not yet confirmed. |
| M3 — Input & running (keyboard and gamepad) | ❌ | — | |
| M4 — Gravity, jumping & the ground check | ❌ | — | |
| M5 — Game feel | ❌ | — | Reality-check gate: stop and play for five minutes |
| M6 — The level as a Tilemap | ❌ | — | |
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

## Session log
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
