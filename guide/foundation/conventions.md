<!-- Foundation doc. Section headings and table column keys stay English by design (they are the schema
     the GuideForge skills look things up by). Prose language: English. -->

# Conventions — Cavern Dash

> The rules every step in this guide follows. If a step seems to contradict one of these, the convention
> wins — fix the step.

## Voice
- Address the reader as **you**; never "the Human"/"the user"/"the reader".

## Writing language
- Prose language: **English**. Every sentence and every heading the reader sees is written in it.
- Code language: **English**. Applies to identifiers, comments and user-facing strings in the code this guide
  writes. Never translated either way: language keywords, standard-library and framework API names,
  framework-mandated identifiers (lifecycle methods, config keys, route/DI names), package names, file names.
  Rule 3.6 (self-describing identifiers) applies **in the code language**.
- Untranslated in every guide, whatever the two settings say: file and folder names, the `foundation/` docs'
  own section headings and table column keys, commands, paths, doc URLs, the `·` separator, the `—` bare-prev
  dash, and the `[ ]` / `[x]` marks.

### Heading map

| Canonical (what the skills call it) | As written in this guide |
|-------------------------------------|--------------------------|
| `## Before you continue — corrections` | `## Before you continue — corrections` |
| `## Glossary for this step`            | `## Glossary for this step` |
| `## Why / design`                      | `## Why / design` |
| `## Do this`                           | `## Do this` |
| `## Code`                              | `## Code` |
| `## Done when (this step)`             | `## Done when (this step)` |
| `## Suggested commit`                  | `## Suggested commit` |
| `## If it breaks`                      | `## If it breaks` |
| `## Goal`                              | `## Goal` |
| `## Prerequisite`                      | `## Prerequisite` |
| `## Steps at a glance`                 | `## Steps at a glance` |
| `## Design / decisions folded in`      | `## Design / decisions folded in` |
| `Sitting <N> — <name>`                 | `Sitting <N> — <name>` |
| `## Done-when gate (the real test — check every box by hand)` | `## Done-when gate (the real test — check every box by hand)` |
| `## Files after this milestone (the checkpoint)` | `## Files after this milestone (the checkpoint)` |
| `### Pre-existing files modified`      | `### Pre-existing files modified` |
| `### Unchanged this milestone`         | `### Unchanged this milestone` |
| `## Troubleshooting`                   | `## Troubleshooting` |
| `## Handoff`                           | `## Handoff` |
| `## Objective`                         | `## Objective` |
| `## Stack (summary)`                   | `## Stack (summary)` |
| `## Key decisions`                     | `## Key decisions` |
| `## Updates`                           | `## Updates` |
| `## How a step is built`               | `## How a step is built` |
| `## Following this guide`              | `## Following this guide` |
| `New here:`                            | `New here:` |
| `New concept —`                        | `New concept —` |
| `Build vs borrow —`                    | `Build vs borrow —` |
| `**Corrected when:**`                  | `**Corrected when:**` |
| `**Suggested commit:**`                | `**Suggested commit:**` |
| `⚠️ **Superseded <YYYY-MM-DD>**`        | `⚠️ **Superseded <YYYY-MM-DD>**` |
| `Nav:`                                 | `Nav:` |
| `Overview`                             | `Overview` |
| `prev:` / `next:` / `start:`           | `prev:` / `next:` / `start:` |
| `milestone K of N`                     | `milestone K of N` |
| `start: — not drafted yet`             | `start: — not drafted yet` |

**This guide adds one section of its own**, because most of its work happens in the Editor rather than in a
file: `### Editor checkpoint` — the GameObject → Component → Field → value table each `NN_verify.md` carries,
collecting every Inspector value that milestone set. Inside a step, Editor values are given in the numbered
action that sets them, not under a heading of their own.

## Repository layout
- **One repository holds both.** `guide/` and the Unity project `cavern-dash/` are **sibling folders** at the
  repository root. M1 never runs `git init`; a repository nested inside `cavern-dash/` would hide the project
  from the outer one.
- **Unity's `.gitignore` and `.gitattributes` live inside `cavern-dash/`**, never at the root. Unity's official
  ignore list uses anchored patterns (`/[Ll]ibrary/`, `/[Bb]uilds/`), and a leading slash resolves against the
  folder holding the file — at the root they would match nothing and let `Library/` into history.
- **The repository's own documents live at the root**: `README.md`, `LICENSE`, `CREDITS.md`,
  `docs/screenshot.png`, plus a root `.gitignore` carrying nothing but `.DS_Store`. The guide does **not**
  author them — it stops at development ([D33](decision-log.md#d33--the-guide-stops-at-development-no-build-and-ship-milestone));
  they belong to whoever publishes the repository.
- **Every terminal command in the guide runs from `cavern-dash/`**, and a step that quotes Git output says so.
  Git renders paths two different ways from that one place, and a gate must quote the right one:
  - **From the repository root** — `cavern-dash/Assets/…`: `git status --porcelain` (which ignores
    `status.relativePaths`) and `git lfs track` (which prefixes each pattern with the folder of the
    `.gitattributes` that declared it).
  - **Relative to the folder the reader is standing in** — `Assets/…`: `git status --short`, and
    `git check-ignore`, which echoes each path exactly as it was typed.
  Before the project's first commit, `--porcelain` also needs **`-uall`**: Git collapses a wholly untracked
  folder into a single `?? cavern-dash/` line otherwise.
- **Every command must run on PowerShell and on zsh/bash alike** (`stack.md` § *Target OS / shell(s)*). Use
  plain `git` with a pathspec instead of piping into `grep`, `head`, `wc` or `findstr` — those differ between
  the two shells, and a gate the reader cannot run is not a gate.

## Commit messages
- Format: **`<type>(<scope>): <subject>`** (Conventional Commits).
- Types: `feat`, `fix`, `refactor`, `perf`, `style`, `docs`, `test`, `build`, `chore`, `ci`.
- Scope: the part of the project the step touched — `player`, `input`, `level`, `enemy`, `ui`, `audio`,
  `camera`, `scenes`, `options`, `project-settings`, `assets`, `build`, `unity` (the project folder itself),
  `repo` (the repository's own documents).
- Subject: imperative mood, no trailing period, ≤72 characters, says **what** changed, in the code language.
- One step, one commit — including a step that only changes settings, assets or config.

## Naming
- **Load-bearing** (must match exactly — the game breaks otherwise; flagged at first use in each step):
  - layer names **`Ground`**, **`Player`**, **`OneWay`**, **`Hazard`**;
  - the input action names **`Move`**, **`Jump`**, **`Dash`** in the **`Player`** action map of
    **`InputSystem_Actions`**;
  - the `PlayerPrefs` keys **`cavernDash.run.bestTimeMs`** (one best time for the whole run, set in M11),
    **`cavernDash.bindingOverrides`**, **`cavernDash.volume.master01`**, **`cavernDash.volume.music01`**,
    **`cavernDash.volume.sfx01`**, **`cavernDash.display.fullscreen`**;
  - the AudioMixer exposed parameters **`MasterVolumeDb`**, **`MusicVolumeDb`**, **`SfxVolumeDb`**;
  - the scene names **`Menu`**, **`Level01`**, **`Level02`**, **`Win`**;
  - the Animator parameters **`Speed`**, **`IsGrounded`**, **`VerticalVelocity`**, **`IsDashing`**.
- **Cosmetic** (rename or retune freely): colours, tile choices, the shape of each level, the number of coins,
  the window title, which Kenney sprite you pick for the player.
- C# style: `PascalCase` types and methods, `camelCase` locals and private fields, `[SerializeField] private`
  over `public` for Inspector values, one class per file, **file name == class name** (Unity requires it for a
  `MonoBehaviour`).

## Structure / architecture
- **Project layout.** Everything you author lives under **`Assets/_Project/`** (the leading underscore sorts it
  to the top), split into `Scripts/`, `Scenes/`, `Prefabs/`, `Art/`, `Audio/`, `Animation/`, `Settings/`.
  Third-party imports stay in **`Assets/ThirdParty/Kenney/`**, untouched, with the pack's licence file beside
  them.
- **Units.** **1 tile = 1 Unity unit**, Pixels Per Unit **18**. Every distance is quoted in **units** and every
  speed in **units per second**; pixels appear only in import settings.
- **The engine owns the transform; you own the intent.** The player is a **Dynamic `Rigidbody2D`**: Unity
  integrates gravity and resolves collisions, your code writes `linearVelocity` in `FixedUpdate`. No
  `transform.position` writes on anything that has a `Rigidbody2D`.
- **Input in `Update`, physics in `FixedUpdate`, camera in `LateUpdate`.** Buffered inputs (jump, dash) are
  captured in `Update` and consumed in the next `FixedUpdate` — which is *why* jump buffering exists at all.
- **Tuning lives in `[SerializeField]` fields with unit-bearing names** (`moveSpeedUnitsPerSecond`,
  `coyoteTimeSeconds`), tuned in the Inspector, never as a magic number in the body of a method.
- **One responsibility per component**: `PlayerMotor` (physics, including the `PlayerMovementState` machine
  that owns the dash and wall states), `PlayerInputReader` (input), `PlayerStats` (coins), `PlayerHealth`
  (lives and i-frames), `Collectible`, `Checkpoint`, `LevelTimer`, `HudView`, `GameSession` (run totals
  across scenes), `AudioOptions` / `DisplaySettings` / `RebindButton` (the options screen).
- **Objects talk through C# `event`/`UnityEvent`**, not `GameObject.Find` and not singletons — with one
  deliberate, named exception (`GameSession`, which survives scene loads) introduced with its trade-off stated.

### Canonical tuning values
These exact numbers appear in the code, the prose, the gates and the glossary; no two places use a different
number for the same thing (rule 3.5).

| Value | Number | First set in |
|---|---|---|
| `moveSpeedUnitsPerSecond` | **7** | M3 |
| `moveSpeedUnitsPerSecond` on M2's throwaway `ConstantMover` | **3** (a different component, deleted in M3 — not the value above) | M2 |
| `groundAccelerationUnitsPerSecondSquared` | **60** | M5 |
| `airAccelerationUnitsPerSecondSquared` | **35** | M5 |
| `Physics2D` gravity Y | **−9.81** (project default, untouched) | M4 |
| player `gravityScale` | **4** | M4 |
| `jumpVelocityUnitsPerSecond` | **14** → apex **2.50 units** in continuous maths, **≈2.3–2.4 measured** (fixed 0.02 s steps sample just under the peak) | M4 |
| `fallGravityMultiplier` | **1.8** | M5 |
| `lowJumpGravityMultiplier` | **2.2** | M5 |
| `coyoteTimeSeconds` | **0.10** | M5 |
| `jumpBufferSeconds` | **0.12** | M5 |
| `dashDistanceUnits` / `dashDurationSeconds` / `dashCooldownSeconds` | **5** / **0.15** / **0.60** | M8 |
| `wallSlideSpeedUnitsPerSecond` | **2.5** | M8 |
| `wallJumpVelocity` | **(9, 13)** | M8 |
| `wallJumpControlLockSeconds` | **0.15** | M8 |
| `dashSpeedUnitsPerSecond` (derived: distance ÷ duration) | **33.3** | M8 |
| `invulnerabilitySeconds` | **1.0** | M9 |
| starting lives | **3** | M9 |
| `stompBounceVelocityUnitsPerSecond` | **10** | M9 |
| enemy `speedUnitsPerSecond` | **2** | M9 |

## Data vs code
- **The level is content, not code.** Geometry is a Tilemap; coins, enemies, checkpoints and the exit are
  **prefabs** placed in the scene. Adding a coin means placing a prefab — never editing a script.
- **Rebinds are an override layer**, never an edit of `InputSystem_Actions`: they are produced by
  `PerformInteractiveRebinding`, serialized with `SaveBindingOverridesAsJson`, and restored on launch with
  `LoadBindingOverridesFromJson`.

## Language / framework specifics
- **C# 9.0** only — no file-scoped namespaces, no `global using`, nothing newer than Unity 6.3 compiles.
- Move a Dynamic body by writing **`rb.linearVelocity`** (`Rigidbody2D.velocity` was renamed).
- Cinemachine's camera component is **`CinemachineCamera`** in namespace `Unity.Cinemachine` — *not*
  `CinemachineVirtualCamera`, which is Cinemachine 2 and no longer exists.
- Canvas text is **`TextMeshProUGUI`**; **TMP Essential Resources** must be imported once per project.
- Mixer volume is set in **decibels** through an exposed parameter, while a slider is linear **0–1**:
  convert with `Mathf.Log10(value01) * 20`, and special-case `0` to `-80 dB`.
- Builds are produced from **`File > Build Profiles`** (Unity 6 replaced the Build Settings window).

## Commands / shells (cross-platform)
The guide targets **Windows (PowerShell)** and **macOS (zsh/bash)**. Almost all work happens inside the Unity
Editor, which is identical on both; the only commands are `git` and `git lfs`, which are too. Any command
added later must run on both shells, or ship a variant per shell.

## Code presentation
- A step that **creates a new file** shows that file's complete contents. A step that **edits a file created
  earlier** shows only the fragment, with a **unique** placement anchor ("ADD this method below
  `HandleJump`") — never the whole file re-pasted.
- The milestone's **`NN_verify.md` holds the complete current contents** of every file that milestone touched,
  so you always have an authoritative copy to diff against.
- **Editor work is code too.** Inspector values, layer assignments, physics settings and import settings are
  recorded in the verify checkpoint as a **settings table** (object → component → field → exact value),
  because they are as load-bearing as the C# and cannot be diffed from a code block.

## Testing / verification
- Every Done-when gate is an **observable result in Play Mode or in the built player**, and says which of the
  two you are watching.
- A value you cannot judge by eye — apex height, dash distance, the coyote window — is gated on a **number**
  your own code prints to the Console or shows in the HUD, never on "it looks about right".
- Where a gate names a set ("every animation transition", "both devices"), it **sweeps the set** rather than
  sampling one member of it.
