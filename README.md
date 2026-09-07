# guide-forge-unity-platformer

**A sample guide produced with [GuideForge](https://github.com/tomthebearded/guide-forge) `v1.18.0` — plus
the Unity game it builds.**

This repository exists as a worked example: it shows what a *learn-as-you-go* build guide looks like when it
comes out of the GuideForge toolkit end to end (plan → scaffold → draft → audit), and what the reader ends up
with if they follow it. Like the [VS Code extension example](https://github.com/tomthebearded/guide-forge-vscode-extension)
and unlike the [web-platformer one](https://github.com/tomthebearded/guide-forge-web-platformer), this guide
is also being **executed** — so it exercises the maintenance half of the toolkit too (mark-progress →
report-issue → amend-guide → audit-guide), and the record of that is kept in the open, defects included.

> **Built with GuideForge v1.18.0**, the pedagogy contract this guide was stamped against; it was planned,
> drafted and audited on **2026-08-22** and has been maintained with that same release ever since, most
> recently on **2026-09-07** — see the session log in
> [`guide/foundation/status.md`](guide/foundation/status.md). The toolkit has moved on since; a guide
> generated with a newer GuideForge will differ in structure and conventions.

The subject is **Cavern Dash**, a single-player 2D pixel platformer built in **Unity 6.3 LTS** (`6000.3.x`)
and C#. You run, jump, dash and wall-jump through a tilemap cavern, ride a moving platform, hop up through a
one-way ledge, stomp an enemy, take a hit and respawn at a checkpoint, and collect coins on the way to the
exit; Level 02 follows, then a win screen with this run's time and your best. The physics is a Dynamic
`Rigidbody2D` steered by velocity, the camera is Cinemachine 3.1, input goes through the Input System 1.19
(keyboard **and** gamepad, rebindable), and every asset is Kenney **CC0**. The finish line is a
double-clickable **desktop executable** whose best time, volume settings and rebound keys survive a relaunch.

---

## What's in here

| Path | What it is |
|------|------------|
| **[`guide/`](guide/)** | The guide itself — the actual artifact this repo is an example of. 13 milestones, 64 atomic teaching steps and 13 verify gates, plus the GuideForge foundation docs (`stack`, `status`, `progress`, `glossary`, `conventions`, `decision-log`, `audience`) and a feedback log. Start at [`guide/README.md`](guide/README.md). |
| **[`cavern-dash/`](cavern-dash/)** | The Unity project as actually built by following the guide. **It is mid-guide, not finished** — the frontier is M11 (see [`guide/foundation/progress.md`](guide/foundation/progress.md)), so the run timer, the options menu, key rebinding and the packaged build aren't in it yet. |
| [`guide/PLAN.md`](guide/PLAN.md) | The stage-1 plan the whole guide was drafted from (audience model, milestone ladder, scope boundaries). |

---

## Play it

There is no build yet — M13 is where the guide makes one — so you play it in the Editor.

1. Clone this repository. The Kenney art and audio are committed
   ([`cavern-dash/Assets/ThirdParty/`](cavern-dash/Assets/ThirdParty/)), so there is nothing to download.
2. Install **Unity 6.3 LTS** through [Unity Hub](https://unity.com/unity-hub). The project was last opened
   with `6000.3.21f1`; any `6000.3.x` patch will do.
3. In the Hub, **Add** the **`cavern-dash/`** folder (not the repository root) and open it. `Library/` isn't
   committed, so the first import takes a few minutes.
4. Open **`Assets/_Project/Scenes/Menu.unity`** and press **Play**.

**Controls**

| Input | Action |
|-------|--------|
| `A` / `D`, `←` / `→`, or the left stick | Run left / right |
| `Space` or gamepad **south** | Jump — hold for height, tap for a short hop; press again while wall-sliding to wall-jump |
| `Left Shift` or gamepad **west** | Dash |
| `Esc` | Pause |

Stomp an enemy from above to kill it; touch one from the side and you lose one of three lives behind a second
of invulnerability. Falling off the level costs a life too. Checkpoints arm as you pass them, and the exit
carries the run into the next level.

> **It's mid-guide, and it shows.** Milestone 11 is the frontier: the HUD, the four scenes and the run that
> survives a scene load are in; pause is the step being executed right now, and the timer, the best time, the
> game-over screen, the options panel, key rebinding and the packaged build are drafted but not built. What
> is actually on disk is stated by [`guide/foundation/progress.md`](guide/foundation/progress.md) — not by
> this page.

---

## Follow the guide instead

If you'd rather build it yourself — which is the point of the guide:

1. Read [`guide/foundation/status.md`](guide/foundation/status.md) first: it's the single source of truth for
   what's drafted and what's verified, and it names the guide's open defects.
2. **Work inside a clone of this repository, not a download** — the Unity project you build lives in it, as
   `cavern-dash/` beside `guide/`, one repository holding the guide and the game it builds.
3. Start at [Milestone 1](guide/MILESTONE_1_project-and-version-control/00_overview.md) and do the milestones
   in order; tick each step in [`guide/foundation/progress.md`](guide/foundation/progress.md) as you finish
   it — that ledger is what lets the guide be amended later without disturbing work you've already done.
4. **Type the code, don't paste it.** Every milestone ends in a `NN_verify.md` carrying the complete contents
   of every file it touched *and* a settings checkpoint (GameObject → Component → Field → value), so Inspector
   state is as diffable as the C#. They're there to diff against, not to paste blindly.
5. Use [`cavern-dash/`](cavern-dash/) as the reference implementation when you get stuck — remembering it
   only goes as far as the guide has been executed.

The guide assumes you can read code but not that you know Unity — every Editor window, component, C# API and
physics concept is defined on first use.

> **Verification status:** M1–M9 were executed and their Done-when gates passed by hand in the Editor. M10 is
> ⏳ — it passed, then went back for re-verification on 2026-09-07 when a *lifecycle-order* defect was found
> in its audio step (an opening read from `OnEnable` that Unity may run before the `Awake` filling the value
> in). M11 steps 01–03 are executed but not yet ticked in the ledger; M11/04 opens with a *Before you
> continue — corrections* section carrying the repair for that defect. M12–M13 are drafted but not run. Two
> defects are still open: `.gitattributes` was never created, so the binaries are **not** actually
> LFS-tracked despite M1/05 being ticked, and a handful of gate numbers (jump apex, dash distance, frame
> times) are derived from the physics rather than measured in a session. See
> [`guide/foundation/status.md`](guide/foundation/status.md).

---

## Licence

The Kenney art and audio under [`cavern-dash/Assets/ThirdParty/`](cavern-dash/Assets/ThirdParty/) are **CC0**
(public domain) — free to reuse, attribution not required. The code is meant to be MIT, but the `LICENSE`
file is written by the guide itself, in
[M13 step 04](guide/MILESTONE_13_build-and-ship/04_the-repository.md), which hasn't been executed yet.
