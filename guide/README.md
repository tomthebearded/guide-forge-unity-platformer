# Cavern Dash — build a 2D platformer in Unity 6.3 LTS

> The front door to this guide. Skim this, then follow the milestones. **Progress lives in
> [foundation/status.md](foundation/status.md) and [foundation/progress.md](foundation/progress.md), not
> here** — this page describes intent; those two state reality (what the guide has verified, and which steps
> you have actually run).

> _Generated with **GuideForge v1.18.0** on 2026-08-22._

## Objective

You will build **Cavern Dash**, a single-player 2D pixel platformer, and finish holding a **desktop
executable** you can double-click. Launch it and you get a title menu; press Play and Level 01 loads; you run,
jump, dash and wall-jump through a tilemap cavern, ride a moving platform, hop up through a one-way ledge,
stomp an enemy, take a hit and respawn at a checkpoint, and collect every coin on your way to the exit. Level
02 follows, then a win screen showing this run's time and your best time. Quit the game, launch it again, and
the best time — along with your volume settings and any key you rebound — is still there.

## Stack (summary)

Unity **6.3 LTS** (`6000.3.x`) · C# **9.0** · Input System **1.19** · Cinemachine **3.1** · art and audio from
Kenney's **CC0** packs — full verified table + check date: **[foundation/stack.md](foundation/stack.md)**.

## Key decisions

- **The player is a Dynamic `Rigidbody2D` you steer by velocity** — the engine integrates gravity and resolves
  collisions; your code writes `linearVelocity` in `FixedUpdate`. → [foundation/decision-log.md](foundation/decision-log.md#d2--the-player-is-a-dynamic-rigidbody2d-driven-by-velocity)
- **Unity 6.3 LTS, not 6.0 LTS** — 6.0's support ends October 2026; 6.3's runs to December 2027. → [foundation/decision-log.md](foundation/decision-log.md#d1--pin-unity-63-lts-60003x)
- **All art and audio are CC0** (Kenney), so everything you build here is yours to publish. → [foundation/decision-log.md](foundation/decision-log.md#d4--art-and-audio-come-from-kenneys-cc0-packs)
- **Git from step one**, with `.meta` files tracked and Git LFS configured before the first binary exists. → [foundation/decision-log.md](foundation/decision-log.md#d5--git--git-lfs-from-milestone-1)

## Updates

- 2026-08-28 — fixed: mashing jump could produce a second jump in mid-air; the coyote window is now guarded
  against re-arming while rising (M5/02, /03, /06; swept M8/02, /06 and M10/07; retrofit in M6/01). See
  [decision-log D31](foundation/decision-log.md#d31--the-coyote-refill-is-guarded-against-re-arming-while-rising).
- 2026-08-24 — audited after the amendment: 4 blockers and 5 warnings, all fixed — the `git status --short`
  path claim, two Unix-only pipes in a gate, M13/04's command order, and seven gates that never said which
  folder to run from.
- 2026-08-24 — amended: the Unity project is a folder in this repository beside `guide/`, not a repository of
  its own — no `git init`, Unity's `.gitignore` and `.gitattributes` inside `cavern-dash/`, README and licence
  at the root (M1, M13).
- 2026-08-23 — Second audit: 3 blockers and 14 warnings, all fixed. The jump buffer, M5's milestone gate and
  two "break it and watch it fail" recipes were the blockers. Still open: the gate numbers derived from the
  physics rather than measured in the Editor.
- 2026-08-22 — Audited and fixed: 11 findings closed. One open blocker — the eight "break it and watch
  it fail" recipes have not been run in the Editor yet, and neither have the measured gate values.
- 2026-08-22 — Whole guide drafted: 13 milestones, 64 steps and 13 verify gates (77 step files).
- 2026-08-22 — Guide created with GuideForge v1.18.0.

## How a step is built

Every step file has the same shape, so you always know where to look:

| Section | What it gives you |
|---|---|
| `> Nav:` | where you are and how to move — at the top **and** the bottom of every file |
| `## Glossary for this step` | the terms this step introduces and where on the page each is explained — only when it introduces any |
| `## Why / design` | what this step accomplishes and why it comes now. Read it before you type |
| `## Do this` | the numbered actions, each with its code block directly underneath and a note of where the code goes |
| `## Code` | reserved for a step whose code is a single small piece. This guide never needs it: every code block sits under the numbered action that introduces it |
| `## Done when (this step)` | the gate: an action paired with the exact result you should see. Don't move on until every box is true |
| `## Suggested commit` | a ready-made commit message for what this step changed — absent when the step changes no files |
| `## If it breaks` | the failure you're most likely to hit, and the first thing to check |

Two you'll meet less often: a **`## Before you continue — corrections`** section at the top of a step — a
repair to apply *before* the step itself, and only if you executed earlier steps before the date it names —
and a **`⚠️ Superseded`** banner under the nav line, which means the step below it is out of date and points
at the file carrying its correction.

Each milestone ends in a **`NN_verify.md`** with a different shape: the milestone's one Done-when gate, the
complete contents of every file it created or changed (your authoritative copy to diff against), a
troubleshooting table, and the handoff to the next milestone.

Because much of Unity's work happens in the Editor rather than in a file, the verify files also carry a
**settings checkpoint** — a table of GameObject → Component → Field → exact value — so Inspector state is as
diffable as the C#.

## Following this guide

1. Read **[foundation/status.md](foundation/status.md)** first — the single source of truth for what's done and verified.
2. **Work inside a clone of this repository, not a download.** The Unity project you build lives in it, as
   `cavern-dash/` beside `guide/` — one repository holding the guide and the game it builds. `git clone` it
   before you start.
3. Start at **[Milestone 1](MILESTONE_1_project-and-version-control/00_overview.md)**; do the milestones in order (each builds on the last).
   Tick each step in **[foundation/progress.md](foundation/progress.md)** as you finish it — that ledger is
   what lets the guide be changed later without disturbing the work you've already done.
4. **Type the code — don't paste it.** The complete files are included so you always have an authoritative
   copy to diff against, *not* so you can paste blindly. You'll learn far more by typing each file, reading it
   as you go, and predicting a step's expected output *before* you run it. Reach for paste only to unstick
   yourself when something won't work.
5. If you're following this a while after it was written, run the **review-before-follow** gate first —
   re-check the stack's "Latest stable" column and reconcile any drift before executing (reality wins).
