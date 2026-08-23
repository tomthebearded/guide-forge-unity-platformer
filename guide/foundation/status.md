<!-- Foundation doc. Section headings and table column keys stay English by design (they are the schema
     the GuideForge skills look things up by). Prose language: English. -->

# STATUS — Cavern Dash

> _Generated with **GuideForge v1.18.0** on 2026-08-22._
> _Last updated with **GuideForge v1.18.0** on 2026-08-22._

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
- 2026-08-22 — Plan approved (13 milestones). Scaffold stamped with GuideForge v1.18.0: README, foundation docs, and 13 placeholder milestone overviews.
- 2026-08-22 — Audit run (GuideForge v1.18.0): 1 BLOCKER, 11 WARNINGs. Every WARNING fixed — three over-long commit subjects, a README row and a conventions promise the content did not keep, a dead `WinScene` constant, two `[Mn]` markers naming the wrong milestone, one third-person reference, two bare built-ins (`Vector2.Lerp`, `SetIsOnWithoutNotify`), an over-long M9 goal, and a gate reading git's human summary instead of `--porcelain`. **The BLOCKER stands:** the eight break recipes (M2/04, M4/05, M5/02-04, M6/05, M10/04, M12/05) describe failures derived from the code, never observed — they need a real Unity 6.3 session, and so do the seven engine-derived gate numbers.
- 2026-08-22 — Whole guide drafted with GuideForge v1.18.0: 13 milestones, 77 step files including 13 verify gates. `progress.md` filled with one row per step. Structural self-audit run: nav lines, Done-when gates, handoffs, glossary and decision-log anchors, and every relative link check clean. **No gate has been executed by a person yet** — every milestone is ❌ below until someone follows the guide in the Editor.
