<!-- Foundation doc. Section headings and table column keys stay English by design (they are the schema
     the GuideForge skills look things up by). Prose language: English. -->

# STATUS — Cavern Dash

> _Generated with **GuideForge v1.18.0** on 2026-08-22._
> _Last updated with **GuideForge v1.18.0** on 2026-08-23._

## Frontier
- **Current frontier:** M1 — not started (guide fully drafted, nothing executed).
- **Executed through:** nothing yet.

## Source inputs
| Input file | Used for (stack / scope / decisions) | Provided on | Re-checked on |
|------------|--------------------------------------|-------------|---------------|
| `guide-forge-web-platformer` (the *Shape Jumper* guide, GuideForge v1.1.0) | Lineage: the milestone spine (loop → motion → gravity/jump → solid ground → collectibles → goal → persisted best time), the reality-check-gate placement, and the repository layout this guide mirrors | 2026-08-22 | — |

## Milestone status
| Milestone | Status | Verified on | Notes |
|-----------|--------|-------------|-------|
| M1 — Project, Editor & version control | ❌ | — | |
| M2 — First script & frame-rate-independent motion | ❌ | — | |
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
| M13 — Build & ship | ❌ | — | Gate is observed in the built player, not the Editor |

<!-- Status key: ✅ verified (Done-when passed by hand) · ⏳ in progress · ❌ not started -->

## Drift log
| Date | Where | Guide said | Reality is | Action taken |
|------|-------|-----------|-----------|--------------|
| | | | | |

## Session log
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
