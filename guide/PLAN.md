# PLAN — Cavern Dash: a 2D platformer in Unity 6.3 LTS

> **Stage-1 deliverable** (GuideForge `/plan-guide`). This is the plan the guide will be drafted from — the
> whole guide, in one drafting pass, after approval. It is **not** the guide. Nothing is scaffolded yet
> except this file.
>
> **Status: awaiting approval.** On approval → `/scaffold-guide` stamps the skeleton, then `/draft-milestone`
> drafts the whole guide (M1→M13 in one pass).
>
> **Lineage.** This guide is the Unity counterpart of
> [`guide-forge-web-platformer`](https://github.com/tomthebearded/guide-forge-web-platformer) (*Shape Jumper*,
> vanilla JS + Canvas). It keeps that guide's spine — loop, motion, gravity, jump, solid ground, collectibles,
> a goal, a persisted best time — and re-teaches it in the idiom of a real engine, then expands well past it.
> **Deliberately dropped from the source guide: the random gravity flip.** It was the JS guide's headline
> twist; here the expressive weight moves to the moveset (dash, wall-jump) and to real level content.

---

## 1. Brief & audience model

### What we're building

**Cavern Dash** — a single-player 2D pixel platformer built in **Unity 6.3 LTS** with **C#**, shipped as a
**desktop standalone executable** for Windows and macOS.

The reader builds, in order: a project under version control, a script that moves a box frame-rate
independently, a physics-driven character that runs from keyboard *and* gamepad, gravity and a single jump,
then the **game-feel layer** that separates a toy from a game (coyote time, jump buffering, variable jump
height). On that base they paint a real level with a **Tilemap**, add **moving and one-way platforms**, extend
the moveset with **dash and wall-jump**, then populate the world with **coins, hazards, a patrolling enemy,
damage, lives, checkpoints and respawn**. Finally they make it look and sound like a game (**sprite animation,
Cinemachine camera, parallax, audio**), wrap it in a real **game structure** (menu → two levels → win screen,
pause, HUD, a per-level best time that survives a restart), give it an **options screen with volume, display
settings and full key/button rebinding that persists**, and **build and publish it**.

**Observable end state:** double-click `CavernDash.exe` (or `CavernDash.app`) → a title menu appears → press
Play → Level 1 loads → run, jump, dash and wall-jump through a tilemap cavern, ride a moving platform, hop up
through a one-way ledge, stomp an enemy, take a hit and respawn at a checkpoint, collect every coin, reach the
exit → Level 2 → the win screen shows this run's time and the best time → quit, relaunch, and the best time is
still there.

**Art and audio** come from **Kenney's CC0 packs** (verified: public domain, no attribution required,
commercial use allowed). The guide has the reader download them; the finished project in this repository
**ships the exact files it uses**, which CC0 permits, so the guide never depends on a live download.

### Audience model (per-topic expertise → explanation-depth policy)

The reader **can already program** (loops, functions, classes, types are not new) but has **never opened
Unity** and has never built a game.

| Topic | Level | Depth policy applied in the guide |
|-------|-------|-----------------------------------|
| C# the language (types, classes, methods, properties, `enum`, generics) | **Beginner** | Assumed known; only *Unity-flavoured* C# is taught — `MonoBehaviour`, `[SerializeField]`, the serialization rules, coroutines, `struct` vs `class` where physics cares. Doc link, brief why, no fundamentals. |
| The Unity Editor (Hub, Project/Hierarchy/Scene/Game/Inspector, prefabs, scenes, play mode) | **New** | Fullest tier: define + doc link + deep-dive callout + failure notes. Every panel named the first time the reader must find it; every menu path spelled out. |
| The MonoBehaviour lifecycle (`Awake`/`Start`/`Update`/`FixedUpdate`/`LateUpdate`, execution order) | **New — deepest tier** | The load-bearing mental model of the whole guide. Taught fully in M2 and re-stated at every point where the *choice* of callback matters. |
| 2D physics (`Rigidbody2D`, `Collider2D`, body types, `gravityScale`, layers, triggers, `OverlapBox`) | **New — deepest tier** | Every concept derived, not asserted. The "who moves the transform — you or the engine?" model is repeated wherever it recurs. |
| The Input System package (actions, action maps, bindings, `InputAction.CallbackContext`, devices, **interactive rebinding + binding overrides**) | **New** | Define the action/binding model from zero + doc link + the "project-wide actions asset" gotcha; gamepad is a first-class case, not a footnote. Rebinding is taught as what it really is — an *override layer* on top of the asset, serialized to JSON. |
| Tilemap & Tile Palette (grid, tile assets, `TilemapCollider2D`, `CompositeCollider2D`, Rule Tiles) | **New** | Full definitions + doc links; the pixels-per-unit / 1 tile = 1 unit relationship is taught before a single tile is painted. |
| Sprite import & animation (PPU, Filter Mode, Sprite Editor, Animator, states, parameters, transitions) | **New** | Full tier; the pixel-art import settings are exact values, not advice. |
| Cinemachine 3 (`CinemachineCamera`, Follow, Position Composer, Confiner) | **New** | Define the brain/camera split + doc link + the "this is Cinemachine 3, not 2 — the class was renamed" trap. |
| UI & TextMeshPro (Canvas, scaling, anchors, `TextMeshProUGUI`, TMP Essential Resources) | **New** | Full tier for the Canvas mental model; TMP usage taught at the level the HUD needs, no typography deep-dive. |
| Scene management & game flow (`SceneManager`, additive vs single, `Time.timeScale`, persistence across scenes) | **New** | Full tier; the "what dies on scene load" trap is taught before the reader's score dies on scene load. |
| Persistence (`PlayerPrefs`, `JsonUtility`) | **Beginner** | Define on first use + doc link + brief why; the reader knows what serialization is, so only Unity's take on it is taught. |
| Audio (`AudioSource`, `AudioClip`, `AudioMixer`, groups, exposed parameters) | **New** | Full tier for the mixer graph; SFX usage taught at HUD depth. |
| Git for a Unity project (`.gitignore`, `.meta` files, Git LFS, what must never be committed) | **Beginner** | The reader knows Git; only the *Unity-specific* rules are taught — meta files are mandatory, `Library/` never, binaries via LFS. |
| Building & shipping (Build Profiles, Player Settings, testing the build) | **New** | Full tier: Unity 6 renamed Build Settings to **Build Profiles**, and the reader has never seen either. |

**Depth-policy legend** — **Expert** → name it, nothing more · **Intermediate** → one-line reminder + doc link
· **Beginner** → define on first use + doc link + brief why · **New** → define + doc link + a short deep-dive
callout + extra failure-mode notes.

### Granularity

**Standard.** Atomic steps, but sub-actions are grouped once a workflow has been taught: the first time the
reader creates a GameObject the menu path is spelled out in full; the tenth time it is one line. Editor work
is always exact about *where* — panel, menu path, and the field's label as it appears in the Inspector —
because the reader cannot search for a thing they cannot name.

### Accepted feature additions (from the advise-back)

| Addition | Why it was accepted | Where it lands |
|---|---|---|
| Moving platforms + one-way platforms | The two elements that make a level read as a real platformer; they teach the two classic problems (carrying a rider, `PlatformEffector2D`) | **M7** |
| Dash + wall-jump | Turns the moveset into something expressive and forces an honest movement **state machine** — the best C#-teaching moment in the guide | **M8** |
| Explicit gamepad support | Chosen over a passing mention: one step maps and *tests* every action on keyboard and gamepad, which is the whole point of the Input System | **M3**, re-checked at **M13** |
| **Options screen: volume, display settings, and full key/button rebinding** | Requested as core scope, not as a "later". It closes the loop on all three of the guide's cross-cutting systems at once — Input System, UI, and persistence — and interactive rebinding is the single feature the modern Input System exists for | **M12** |

### Scope boundaries (out of scope — deliberately)

- **No multiplayer**, no networking, no leaderboards.
- **No mobile or touch input**, no console platforms. Keyboard + gamepad, desktop only.
- **No DOTS/ECS**, no Burst, no Job System.
- **No custom shaders**, no 2D lighting, no post-processing. (The project uses the Universal 2D template, but
  URP is scenery here, not a subject.)
- **No procedural level generation** — levels are hand-painted content.
- **No localization** and no accessibility settings beyond the options screen's own controls.
- **No store publishing** (Steam/itch pages, achievements, cloud saves).
- **No CI pipeline** — no GitHub Actions, no automated build or release workflow. Builds are made from the
  Editor, by hand, in M13.
- **No automated tests** — no Unity Test Framework, no Play Mode or Edit Mode test assemblies. Every gate in
  this guide is verified by a person observing the running game.
- **No Unity Version Control** — the guide teaches Git, because the deliverable is a GitHub repository.
- **No `UnityEngine.LowLevelPhysics2D`** (the Box2D v3 API new in 6.3). See the risk table below.
- **No 3D**, at any point.

### Hard constraints

- **Unity 6.3 LTS (`6000.3.x`)**, the current LTS, supported until December 2027.
- **Free tier only.** No paid Asset Store packages, no Unity Pro features.
- **All art and audio CC0** (Kenney), so the finished repository can be public, cloned, and reused with no
  licence friction.
- **The repository is the deliverable**: `guide/` + the finished Unity project + a root `README.md`, publishable
  on GitHub as-is, mirroring the layout of `guide-forge-web-platformer`.
- **Prose language: English. Code language: English** (identifiers, comments, strings, commit messages).
- **Commit convention:** Conventional Commits — `<type>(<scope>): <subject>`, imperative, ≤72 chars.

### Acknowledged long-run risks (logged so the *why* survives → `decision-log.md`)

| Choice | The future problem | Mitigation / cheaper alternative |
|---|---|---|
| Pin **Unity 6.3 LTS** | Any LTS eventually ages out; 6.3's support ends December 2027 | It is the *newest* LTS and the longest runway available today. **Unity 6.0 LTS ends October 2026 — two months from this check** — so pinning 6.0 was the real risk, and it is refused here. `/update-stack` re-pins later. |
| Teach **`Rigidbody2D`** rather than the new low-level 2D physics | Unity 6.3 shipped `UnityEngine.LowLevelPhysics2D` (Box2D v3) and states it *"currently runs alongside the existing API but will eventually replace it"* | `Rigidbody2D` is still the supported component workflow, and it is what Tilemap colliders, effectors, Cinemachine and every doc page integrate with. The low-level API has no component workflow to teach a newcomer. Logged; revisit when Unity marks `Rigidbody2D` deprecated in an LTS. |
| **Cinemachine 3** | Nearly every tutorial online is Cinemachine 2 — the reader will find `CinemachineVirtualCamera` everywhere and it no longer exists | The class is now `CinemachineCamera` and the namespace `Unity.Cinemachine`. The step that introduces it names the rename explicitly so search results stop being confusing. |
| **Kenney packs downloaded by the reader** | A pack can be re-versioned or moved; the guide would rot | The exact pixel dimensions the guide depends on (18×18 tiles) are stated as values, and the finished project **commits** the files it uses — CC0 allows it. If the link dies, the repo is still the source of truth. |
| **A 13-milestone guide** | Reader attrition; the ladder is a multi-week course, not a weekend | The reality-check gate at **M5** is placed exactly where the thing first becomes *fun*, so a reader who stops there still has something they enjoy. Milestones are cut into sittings, and M1–M5 form a coherent "part one". |
| **Git + Unity binaries** | Sprite/audio binaries bloat history; GitHub LFS free tier is 1 GB storage / 1 GB bandwidth per month | `.gitattributes` tracks binary types in LFS from **M1**, before any binary exists. The Kenney subset used is small (kilobytes), so the free tier is never a real constraint here — but the reader learns the rule before it can bite. |
| **Standard granularity for a New-to-Unity reader** | Editor work is where beginners get lost, and Standard risks under-serving them | Mitigated by policy, not by dial: Editor topics sit at **New** depth in the matrix, so every Editor action still spells out panel + menu path + field label, even though prose elsewhere stays terse. |

---

## 2. Verified stack  *(Phase 0.5 — checked 2026-08-22)*

| Tool / package | Pinned version | Latest stable (as of 2026-08-22) | Official docs | Notes |
|---|---|---|---|---|
| **Unity Editor** | **6.3 LTS — `6000.3.x`** (use the newest `6000.3` patch Unity Hub offers) | Unity 6.3 LTS is the current LTS; 6.4/6.5 are non-LTS tech-stream releases | https://unity.com/releases/unity-6 | Supported until **December 2027**. **Unity 6.0 LTS support ends October 2026** — do not pin it. Docs for this line: https://docs.unity3d.com/6000.3/Documentation/Manual/ |
| **Unity Hub** | latest | latest | https://unity.com/unity-hub | The installer/version manager; the guide installs the Editor through it. |
| **C#** | **C# 9.0** | C# 9.0 is the language version Unity 6.3 supports | https://docs.unity3d.com/6000.3/Documentation/Manual/csharp-compiler.html | No C# 10+ syntax anywhere in the guide (no file-scoped namespaces, no `global using`). |
| **Universal 2D template (URP 17)** | bundled with 6.3 | URP 17 is the Unity 6 line | https://docs.unity3d.com/6000.3/Documentation/Manual/urp/Setup.html | The Hub's **Universal 2D** template. URP is used as-is: no custom shaders, no 2D lights. |
| **Input System** | **`com.unity.inputsystem` 1.19.x** | 1.19.0 released for Unity 6000.4/6000.6; 6.3 resolves the matching 1.19 build | https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/ | Installed by default in Unity 6 projects. Unity 6 ships a **project-wide actions asset** named `InputSystem_Actions` with `Player` (Move, Jump, …) and `UI` maps already present — the guide *edits* it rather than creating one. **Rebinding APIs used:** `PerformInteractiveRebinding`, `SaveBindingOverridesAsJson`, `LoadBindingOverridesFromJson` — https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/api/UnityEngine.InputSystem.InputActionRebindingExtensions.html |
| **Cinemachine** | **`com.unity.cinemachine` 3.1.x** | 3.1.6 / 3.1.7 for the 6000.3 line | https://docs.unity3d.com/Packages/com.unity.cinemachine@3.1/ | **Breaking rename:** `CinemachineVirtualCamera` → **`CinemachineCamera`**, namespace → `Unity.Cinemachine`, `m_` field prefixes dropped. Upgrade notes: https://docs.unity3d.com/Packages/com.unity.cinemachine@3.1/manual/CinemachineUpgradeFrom2.html |
| **2D Tilemap** | bundled with the 2D template | — | https://docs.unity3d.com/6000.3/Documentation/Manual/class-Tilemap.html | Grid, Tilemap, Tile Palette, `TilemapCollider2D`. |
| **2D Tilemap Extras** | **`com.unity.2d.tilemap.extras`** (install via Package Manager) | 6.0.x doc line published | https://docs.unity3d.com/Packages/com.unity.2d.tilemap.extras@latest/ | Only for the **Rule Tile** (auto-tiling). Docs: https://docs.unity3d.com/Manual/com.unity.2d.tilemap.extras.html |
| **TextMeshPro** | via **`com.unity.ugui` 2.x** | — | https://docs.unity3d.com/Packages/com.unity.ugui@2.0/manual/TextMeshPro/index.html | TMP was **merged into the uGUI package** and ships with the Editor — nothing to install, but **TMP Essential Resources must be imported once** (`Window > TextMeshPro > Import TMP Essential Resources`). The old `com.unity.textmeshpro` package is deprecated. |
| **Build Profiles** | built in | — | https://docs.unity3d.com/6000.3/Documentation/Manual/create-build-profile.html | Unity 6 **replaced the Build Settings window with Build Profiles** (`File > Build Profiles`). Every build instruction in the guide uses the new name. |
| **Git** | latest | — | https://git-scm.com/doc | The reader already knows Git; only Unity-specific rules are taught. |
| **Git LFS** | latest | — | https://git-lfs.com | Tracks `*.png *.wav *.ogg *.psd` from M1. GitHub's free tier: 1 GB storage + 1 GB/month bandwidth. |
| **Art — Kenney "Pixel Platformer"** | pack v1.2 | CC0 | https://kenney.nl/assets/pixel-platformer | **18 × 18 px tiles**, ~200 files, tiles + characters + backgrounds. **Pixels Per Unit = 18** is a load-bearing value in this guide. |
| **Audio — Kenney "Impact Sounds", "Interface Sounds", "Music Jingles"** | current | CC0 | https://kenney.nl/assets/impact-sounds · https://kenney.nl/assets/interface-sounds · https://kenney.nl/assets/music-jingles | Jump/coin/hit SFX, menu clicks, a win jingle. |
| **Licence of all Kenney assets** | **CC0 (public domain)** | — | https://kenney.nl/support | *"All game assets on the asset pages are public domain licensed (CC0). You're free to use them, even in commercial projects."* Attribution **not required**; the Kenney **logo** may not be reused. This is what makes committing them to a public repo legal. |

### Install (the exact commands, at the pinned versions)

```text
1. Install Unity Hub            → https://unity.com/unity-hub
2. In the Hub: Installs > Install Editor > Unity 6.3 LTS (6000.3.x)
   Modules to tick: Windows Build Support (IL2CPP) on Windows,
                    Mac Build Support (IL2CPP) on macOS,
                    Documentation (optional), Visual Studio / Rider integration.
3. git --version        (any recent Git)
4. git lfs install      (once per machine)
```

### Load-bearing API facts (steps must honor these exact spellings)

- Physics runs in **`FixedUpdate`**; input is read in **`Update`**. Never move a `Rigidbody2D` from `Update`.
- Move a Dynamic body by writing **`rb.linearVelocity`** — `Rigidbody2D.velocity` was renamed; the guide uses
  `linearVelocity` throughout.
- The Input System's project-wide asset is **`InputSystem_Actions`**, with the **`Player`** action map.
- Cinemachine's camera component is **`CinemachineCamera`** (`Unity.Cinemachine`), *not*
  `CinemachineVirtualCamera`.
- TMP text on a Canvas is **`TextMeshProUGUI`**; **TMP Essential Resources must be imported once**.
- Rebinding is an **override layer**, never an edit of the asset: start it with
  **`action.PerformInteractiveRebinding(bindingIndex)`**, persist it with
  **`SaveBindingOverridesAsJson()`**, restore it on launch with **`LoadBindingOverridesFromJson(json)`**, and
  clear it with **`RemoveAllBindingOverrides()`**.
- Mixer volume is set in **decibels** via an **exposed parameter** (`SetFloat`), while a slider is linear
  **0–1** — the conversion is `Mathf.Log10(value01) * 20`, and `0` must be special-cased to `-80 dB`.
- Builds are made from **`File > Build Profiles`**, not "Build Settings".
- Kenney Pixel Platformer tiles are **18 × 18 px** → **Pixels Per Unit = 18**, Filter Mode **Point (no
  filter)**, Compression **None**.

---

## 3. Foundation docs (the cross-cutting layer — `/scaffold-guide` stamps these)

- **`guide/README.md`** — the guide's front door: the observable end state, a one-line stack summary, the
  headline decisions (Rigidbody2D Dynamic; Input System; CC0 art; Git+LFS from step one), an **Updates** log,
  a **"How a step is built"** table, and a short **"Following this guide"** note asking the reader to *type the
  code rather than paste it*. It links to the detailed docs instead of duplicating them; `status.md` still owns
  progress.
- **`foundation/stack.md`** — the Phase 0.5 table above, verbatim, with the check date.
- **`foundation/audience.md`** — the per-topic matrix + the granularity setting, as the guide's north star.
- **`foundation/conventions.md`** — the rules below.
- **`foundation/glossary.md`** — growing list of terms (GameObject, component, prefab, serialization, body
  type, trigger, layer mask, coyote time, jump buffer, i-frames, tilemap, composite collider, effector,
  Animator parameter, Cinemachine brain, mixer group, build profile…), one plain-language sentence each.
- **`foundation/status.md`** — the single source of truth for what is drafted vs actually verified.
- **`foundation/progress.md`** — the execution ledger (`/mark-progress` writes it; `/amend-guide` reads it to
  know which steps must not be rewritten).
- **`foundation/decision-log.md`** — every non-obvious call with its *why* and a revisit-if: the physics
  choice, the dropped gravity flip, every build-vs-borrow row, the excluded non-goals (CI, automated tests),
  and each acknowledged risk above.

### Conventions the code will follow (`conventions.md`)

**Project layout.** All authored content lives under **`Assets/_Project/`** (leading underscore so it sorts to
the top), split into `Scripts/`, `Scenes/`, `Prefabs/`, `Art/`, `Audio/`, `Animation/`, `Settings/`.
Third-party imports (Kenney) stay in **`Assets/ThirdParty/Kenney/`**, untouched, with the pack's licence file
beside them.

**Units.** **1 tile = 1 Unity unit**, Pixels Per Unit **18**. Every distance in the guide is quoted in **units**
and every speed in **units per second**; no value is ever quoted in pixels except import settings.

**Naming.**
- Load-bearing (must match exactly): layer names **`Ground`**, **`Player`**, **`OneWay`**, **`Hazard`**;
  the input action names **`Move`**, **`Jump`**, **`Dash`** in the **`Player`** map of `InputSystem_Actions`;
  the `PlayerPrefs` keys **`cavernDash.level01.bestTimeMs`** / **`cavernDash.level02.bestTimeMs`** /
  **`cavernDash.bindingOverrides`** / **`cavernDash.volume.master01`** / **`cavernDash.volume.music01`** /
  **`cavernDash.volume.sfx01`** / **`cavernDash.display.fullscreen`**; the AudioMixer exposed parameters
  **`MasterVolumeDb`**, **`MusicVolumeDb`**, **`SfxVolumeDb`**; scene names
  **`Menu`**, **`Level01`**, **`Level02`**, **`Win`**; the Animator parameters **`Speed`**, **`IsGrounded`**,
  **`VerticalVelocity`**, **`IsDashing`**.
- Cosmetic (rename freely): colours, tile choices, level shape, coin count, window title, sprite selection.
- C# style: `PascalCase` types and methods, `camelCase` locals and private fields, `[SerializeField] private`
  over `public` for Inspector values, one class per file, file name == class name (Unity requires it for
  `MonoBehaviour`).

**Architecture.**
- **The engine owns the transform; you own the intent.** The player is a **Dynamic `Rigidbody2D`**: Unity
  integrates gravity and resolves collisions, the reader's code writes `linearVelocity` in `FixedUpdate`.
  No `transform.position` writes on anything that has a `Rigidbody2D`.
- **Input in `Update`, physics in `FixedUpdate`, camera in `LateUpdate`.** Buffered inputs (jump, dash) are
  captured in `Update` and consumed in the next `FixedUpdate` — that is *why* jump buffering exists at all.
- **Tuning lives in `[SerializeField]` fields with unit-bearing names** (`moveSpeedUnitsPerSecond`,
  `coyoteTimeSeconds`), tuned in the Inspector, never as magic numbers in the body of a method.
- **The level is content, not code.** Geometry is a Tilemap; coins, enemies, checkpoints and the exit are
  **prefabs** placed in the scene. Adding a coin is placing a prefab, never editing a script.
- **One responsibility per component.** `PlayerMotor` (physics), `PlayerInputReader` (input),
  `PlayerStateMachine` (dash/wall states), `Health`, `Collectible`, `Checkpoint`, `LevelTimer`,
  `GameHud` — small components, composed on GameObjects.
- **Cross-object communication via C# `event`/`UnityEvent`**, not `GameObject.Find` and not singletons —
  except one deliberate, named exception (`GameSession`, which survives scene loads) introduced with its
  trade-off stated.

**Canonical tuning values** (rule 3.5 — these exact numbers appear in the code, the prose, the gates and the
glossary, and nowhere is a different number used for the same thing):

| Value | Number | First set in |
|---|---|---|
| `moveSpeedUnitsPerSecond` | **7** | M3 |
| `groundAccelerationUnitsPerSecondSquared` | **60** | M5 |
| `airAccelerationUnitsPerSecondSquared` | **35** | M5 |
| `Physics2D` gravity Y | **−9.81** (project default, untouched) | M4 |
| player `gravityScale` | **4** | M4 |
| `jumpVelocityUnitsPerSecond` | **14** → apex ≈ **2.50 units** | M4 |
| `fallGravityMultiplier` | **1.8** | M5 |
| `lowJumpGravityMultiplier` | **2.2** | M5 |
| `coyoteTimeSeconds` | **0.10** | M5 |
| `jumpBufferSeconds` | **0.12** | M5 |
| `dashDistanceUnits` / `dashDurationSeconds` / `dashCooldownSeconds` | **5** / **0.15** / **0.60** | M8 |
| `wallSlideSpeedUnitsPerSecond` | **2.5** | M8 |
| `wallJumpVelocity` | **(9, 13)** | M8 |
| `invulnerabilitySeconds` | **1.0** | M9 |
| starting lives | **3** | M9 |

**Commit messages.** Conventional Commits, English, imperative, ≤72 chars: `feat(player): add coyote time to
the jump`. Every step that changes anything tracked ends with exactly one such message.

**Code presentation.** A step that creates a new file shows the complete file. A step that edits an existing
file shows only the fragment plus a **unique** placement anchor ("ADD this method below `HandleJump`"). The
milestone's `NN_verify.md` holds the **complete current contents of every file the milestone touched**, so the
reader always has something authoritative to diff against.

**Editor work is code too.** Inspector values, layer assignments, physics settings and import settings are
recorded in the verify checkpoint as an explicit **settings table** (component → field → exact value), because
they are as load-bearing as the C# and cannot be diffed from a code block.

---

## 4. Milestone ladder

| # | Milestone | Proves (end state) | Depends on | Done-when (one line) |
|---|---|---|---|---|
| **M1** | Project, Editor & version control | A Unity 6.3 project exists, opens, runs, and is a clean Git repository | — | The Universal 2D project opens on `6000.3.x`, Play Mode runs an empty scene, and `git status` is clean with `Library/` ignored, `.meta` files tracked and LFS active |
| **M2** | First script & frame-rate-independent motion | A `MonoBehaviour` the reader wrote moves a sprite at a speed set in the Inspector | M1 | A white square glides right at exactly **3 units/s**, measured against tiles; changing the Inspector field changes the speed **without** re-entering Play Mode; the same distance is covered per second regardless of frame rate |
| **M3** | Input & running (keyboard **and** gamepad) | A physics body runs left/right from either device and collides with solid ground | M2 | Holding A/D **and** pushing a gamepad stick both move the player at **7 units/s**; releasing stops it; the player rests on a ground collider instead of falling through |
| **M4** | Gravity, jumping & the ground check | The player falls, lands, and jumps exactly once per press | M3 | The player falls at `gravityScale` **4**, lands on ground, and Space/gamepad-South launches it to an apex of **≈2.50 units**; a second press while airborne does nothing |
| **M5** ⭐ *reality-check gate* | Game feel | The jump stops being technically correct and starts feeling good | M4 | A jump pressed up to **0.12 s before** landing still fires; a jump pressed up to **0.10 s after** walking off a ledge still fires; a tapped jump peaks **lower** than a held jump (two measured, different apex values); **then stop and actually play it for five minutes** |
| **M6** | The level as a Tilemap | A hand-painted cavern the player runs through, with one collider | M5 | Kenney tiles import at PPU **18** with Point filtering; a painted Tilemap has a single `CompositeCollider2D`; the player runs the whole level without catching on seams between tiles |
| **M7** | Moving & one-way platforms | Level geometry that moves and that you can pass through from below | M6 | The player jumps **up through** a one-way platform and lands on top of it; standing on a moving platform, the player travels with it and does not slide or jitter |
| **M8** | Dash & wall-jump (a movement state machine) | An expressive moveset governed by explicit states | M7 | Dash covers **5 units in 0.15 s** and cannot re-fire for **0.60 s**; against a wall the player slides at **2.5 units/s** and wall-jumps away at **(9, 13)**; states are mutually exclusive (no dashing while wall-jumping) |
| **M9** | Coins, enemies, damage, lives & checkpoints | A world that can be won and lost | M8 | Touching a coin removes it and increments a counter; stomping the enemy kills it, walking into it costs a life and grants **1.0 s** of invulnerability; dying respawns at the last checkpoint touched; **0 lives** ends the run |
| **M10** | Animation, camera & audio | It reads and sounds like a game | M9 | Idle/run/jump/fall animations switch on the Animator parameters; the Cinemachine camera follows with look-ahead and never shows outside the level bounds; jump, coin, hit and land each play a distinct sound through the mixer |
| **M11** | Scenes, menus, HUD & persistence | A complete game loop, not a single scene | M10 | Menu → Level01 → Level02 → Win runs end to end; Esc pauses (`Time.timeScale` 0) and resumes; the HUD shows coins, lives and a running timer; the Win screen shows this run's time and the best time, and the best time **survives quitting and relaunching** |
| **M12** | Options: volume, display & key rebinding | The player can change how the game sounds, looks and controls — and it sticks | M11 | Each of the three volume sliders changes what is audible and the value **survives a relaunch**; the fullscreen toggle applies immediately; **rebinding `Jump` to another key takes effect on the next jump**, the same screen rebinds the **gamepad** button, both survive a relaunch, and **Reset to defaults** restores every original binding |
| **M13** | Build & ship | A real executable, and a repository someone else can use | M12 | A standalone build produced from **`File > Build Profiles`** launches by double-click **outside the Editor** and plays the full loop — including the options screen and a rebind — with gamepad; the repository has a README, a licence, the CC0 credit file, and no `Library/` in its history |

**Reality-check gate: M5.** It is placed exactly where the game first becomes *fun* rather than merely correct.
The reader is told to stop, play for five minutes, and decide whether to continue — before committing to seven
more milestones.

### Sittings (natural pause points inside the larger milestones)

- **M1** — (a) install & create the project · (b) tour the Editor · (c) Git, LFS and the first commit.
- **M6** — (a) import and configure the Kenney art · (b) build the Tile Palette and Rule Tile · (c) paint the
  level and give it a composite collider.
- **M9** — (a) coins & the counter · (b) the patrolling enemy and stomp-vs-hit · (c) health, lives,
  invulnerability · (d) checkpoints & respawn.
- **M10** — (a) sprite animation and the Animator · (b) Cinemachine and parallax · (c) audio and the mixer.
- **M11** — (a) the HUD · (b) scenes and the flow between them · (c) pause · (d) the timer and the persisted
  best time.
- **M12** — (a) the options panel and the volume sliders · (b) display settings · (c) interactive rebinding ·
  (d) persisting and resetting the overrides.
- **M13** — (a) player settings and the build · (b) testing the build · (c) the repository, README and release.

**Estimated size:** 13 milestones, ~78 steps, 13 verify gates — two more rungs than the 10–11 sketched during
the interview. The first is M2: keeping the first script separate from M3 (input and physics) makes the first
C# a rung of its own rather than a footnote to a physics milestone. The second is M12, which was added when
the options screen and rebinding moved into core scope; it sits after M11 because rebinding needs a menu to
live in and a persistence pattern to reuse, and before M13 because the build is the last thing that happens.

---

## 5. Build vs borrow

The ladder above is cut on the **Recommended** column. Flipping a row before approval re-cuts that milestone —
cheap now, expensive after drafting. Posture: **Balanced** — build what the guide set out to teach, borrow
everything else. Every row's option was verified in Phase 0.5.

| Capability | Where | Verified off-the-shelf option | What borrowing costs | What building teaches | Recommended | Your call |
|---|---|---|---|---|---|---|
| Device input, bindings, gamepad support | M3 | **Input System 1.19** (https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/) | An asset-based model to learn; a settings page the reader must find | Polling raw keys — and re-solving gamepads, dead zones and rebinding badly | **borrow** | |
| Gravity, collision detection & resolution | M4 | **`Rigidbody2D` + `Collider2D`** (engine) (https://docs.unity3d.com/6000.3/Documentation/Manual/2d-physics/rigidbody/introduction-to-rigidbody-2d.html) | Handing the transform to the engine; you steer with velocity, not position | Swept AABB collision from scratch — a guide of its own, and it fights every other package in the ladder | **borrow** | |
| The **feel** layer: coyote time, jump buffering, variable jump height, acceleration | M5 | *(none verified — no maintained free 2D-controller package clears the bar)* | — | Exactly what the guide set out to teach; also the clearest use of buffered input across `Update`/`FixedUpdate` | **build** | |
| Level geometry & its collider | M6 | **Tilemap + `CompositeCollider2D`** (engine) | Learning the grid/palette workflow | Hand-placing hundreds of box colliders, and the seam-catching bug that follows | **borrow** | |
| Auto-tiling (correct edge/corner tiles while painting) | M6 | **2D Tilemap Extras — Rule Tile** (https://docs.unity3d.com/Packages/com.unity.2d.tilemap.extras@latest/) | One extra package; a rule editor to learn | Neighbour-matching logic — real, but off the guide's subject | **borrow** | |
| One-way (pass-through-from-below) platforms | M7 | **`PlatformEffector2D`** (engine) (https://docs.unity3d.com/6000.3/Documentation/Manual/class-PlatformEffector2D.html) | Two Inspector fields and a surface-arc concept | Per-contact normal filtering and collider toggling — a classic bug farm | **borrow** | |
| Carrying a rider on a moving platform | M7 | *(none — the engine has no rider solution for Dynamic bodies)* | — | Why a rider slides off, and the two honest fixes (re-parenting vs velocity inheritance) | **build** | |
| The movement state machine (grounded / airborne / dashing / wall-sliding) | M8 | *(Animator state machines exist but drive animation, not physics; no verified free C# FSM package)* | — | The single best C# lesson in the guide: exclusive states, entry/exit, and why `bool` soup fails at four states | **build** | |
| Camera follow, look-ahead, bounds | M10 | **Cinemachine 3.1** (https://docs.unity3d.com/Packages/com.unity.cinemachine@3.1/) | A package and its brain/camera split; the 2.x→3 rename confusion | A `LateUpdate` lerp — 10 lines that then need damping, look-ahead and a confiner bolted on | **borrow** | |
| Sprite animation & transitions | M10 | **Animator / Animation window** (engine) | The state-machine editor and parameter plumbing | Manual sprite swapping on a timer — fine for two clips, unmanageable at six | **borrow** | |
| Volume control & audio routing | M10 | **`AudioMixer`** (engine) (https://docs.unity3d.com/6000.3/Documentation/Manual/AudioMixer.html) | One asset, exposed parameters, a dB-vs-linear gotcha | Multiplying volumes by hand across every `AudioSource` | **borrow** | |
| Screen text / HUD | M11 | **TextMeshPro** via `com.unity.ugui` (https://docs.unity3d.com/Packages/com.unity.ugui@2.0/manual/TextMeshPro/index.html) | Importing TMP Essential Resources once | Nothing — legacy `Text` is worse in every dimension | **borrow** | |
| Loading levels and moving between scenes | M11 | **`SceneManager`** (engine) (https://docs.unity3d.com/6000.3/Documentation/Manual/scene-management.html) | Learning what survives a load and what does not | — | **borrow** | |
| Carrying score/lives/time **across** scenes | M11 | *(engine offers `DontDestroyOnLoad`, not a design)* | — | The real lesson: what dies on scene load, and one deliberate long-lived object with its trade-off stated | **build** (on `DontDestroyOnLoad`) | |
| Persisting the best time | M11 | **`PlayerPrefs`** (https://docs.unity3d.com/6000.3/Documentation/ScriptReference/PlayerPrefs.html) | A key/value store with no schema; not for large saves | A JSON file writer — taught as the *next* step, in one callout, not built | **borrow** | |
| Interactive key/button rebinding | M12 | **`PerformInteractiveRebinding`** (https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/api/UnityEngine.InputSystem.InputActionRebindingExtensions.html) | A rebinding operation with its own lifecycle to learn (cancel key, excluded controls, disposal) | Polling every device for "the next thing pressed" — which is exactly the code the package already got right, including the mouse-delta exclusions | **borrow** | |
| Persisting rebinds across launches | M12 | **`SaveBindingOverridesAsJson` / `LoadBindingOverridesFromJson`** + `PlayerPrefs` | Nothing meaningful — it is two calls | Serializing an override layer by hand, which will silently diverge from the asset | **borrow** | |
| The options UI itself (sliders, the rebind button's three states, "listening…") | M12 | *(none — this is ordinary UI)* | — | Wiring UI to systems: a slider that both reads current state and writes it, and a button that changes meaning while an operation is running | **build** | |
| Producing the executable | M13 | **Build Profiles** (https://docs.unity3d.com/6000.3/Documentation/Manual/create-build-profile.html) | — | — | **borrow** | |

Rows that did **not** clear the bar and are therefore just code in a step: the coin counter, the patrolling
enemy's turn-at-edge check, the parallax offset, the timer format.

---

## 6. Templates

### Per-step template (Standard granularity)

```
# <Milestone> · Step NN of <TOTAL> — <single action title>
> Nav: [← prev](<prev>.md) · [Overview](00_overview.md) · [next →](<next>.md)

## Before you start        (rule 7.1 — what must already be true: package installed, scene open, step NN done)
## Glossary for this step  (an INDEX of terms this step introduces — link + where they're taught below; omit if none)
## Why / design            (the rationale; omit only if pure mechanics)
## Do this                 (numbered actions; each says WHERE + WHAT + WHY; code interleaved under its action)
## Code                    (single-block steps only; the whole file lives in NN_verify.md)
## Done when (this step)   (the sub-slice of the milestone gate, with the EXACT observable result)
## If it breaks            (the most likely error + the first thing to check)
## Suggested commit        (one Conventional Commits message; omitted only if nothing tracked changed)
```

**Editor-work steps** additionally carry a **settings block**: a table of *GameObject → Component → Field →
exact value*, and an explicit "leave everything else at its default" line (rule 3.3), because a half-specified
Inspector is the single most common way a Unity tutorial fails.

### Milestone-overview template (`00_overview.md`)

```
# <Milestone N> — <title>
**Goal:** <one sentence — what will be true at the end>
**Prerequisite:** <the previous milestone's gate, passed>
**Steps at a glance:** <numbered list, grouped into sittings>
**Design decisions folded in:** <compact index, linking to decision-log entries — not an essay>
```

The milestone's Done-when gate does **not** appear here; it lives once, in `NN_verify.md`.

### Verify template (`NN_verify.md`)

```
# <Milestone N> — Verify
## Done when (the milestone gate)   (aggregated from the per-step gates, each with its expected observable)
## Checkpoint — the files           (complete current contents of every file this milestone touched)
## Checkpoint — the Editor          (settings table: object → component → field → value; import settings; layers)
## Troubleshooting                  (the traps of this milestone, each with its fix)
## Handoff                          (what exists so far · what is still open · the next milestone and what it proves)
```

---

## 7. Writing contract

### Pedagogical rules (every step must satisfy these)

**P1 — Explain what's new.** 1.1 Every concept is explained on first use at the depth its topic's row in the
audience matrix demands — which here means Unity concepts get the full treatment and C# concepts do not.
Definitions go inline or in a "New concept" callout directly above the line they land on; the step's
`## Glossary for this step` block only *indexes* them. Functions get an inline code comment, never a glossary
entry. 1.2 The mental models are re-stated where they recur: **"the engine owns the transform, you own the
intent"**, **"input in `Update`, physics in `FixedUpdate`"**, and **"the level is content, not code"**.

**P2 — Anchor every action.** 2.1 Every action says WHERE — the panel, the menu path, the file, the exact
Inspector field label. 2.2 Every action says WHAT it does and WHY, so the reader finishes understanding the
mechanism rather than having clicked correctly.

**P3 — Leave nothing ambiguous.** 3.1 Exact values, never ranges (see the canonical tuning table). 3.2
MANDATORY vs ILLUSTRATIVE is marked — the level's shape is illustrative, the layer names are mandatory. 3.3
Every step that touches the Inspector states which fields to change **and** that everything else stays at its
default. 3.4 Load-bearing names are flagged before the reader types them. 3.5 A recurring number is identical
in the code, the prose, the gate and the glossary. 3.6 Identifiers are self-describing and carry their unit
(`coyoteTimeSeconds`, `moveSpeedUnitsPerSecond`). 3.7 Every **build** row from §5 carries a `Build vs borrow`
callout naming the package it replaces; every **borrow** row says in one clause what the package is doing.

**P4 — Structure steps & code.** 4.1 Sequences are numbered lists, never arrow-chains — Unity menu paths are
the one allowed arrow, inside a single action. 4.2 Each code block sits directly under the instruction it
implements. 4.3 An existing file is edited by fragment + a unique anchor, never re-pasted whole. 4.4 **Every
step ends on a green compile** — Unity recompiles on save, so a step that leaves a red console is a mis-cut
step; the step that changes a signature fixes every call site it breaks. 4.5 Every step that changes anything
tracked ends with one Conventional Commits message.

**P5 — Anticipate failure.** 5.1 Each step names its most likely failure and the first thing to check. The
recurring Unity ones are planned for by name: *the script is not on the GameObject*, *the field is `private`
without `[SerializeField]` so it never appears*, *the layer was never assigned*, *the collider is a trigger*,
*`Rigidbody2D` is Kinematic instead of Dynamic*, *the sprite imported at the wrong PPU*, *TMP Essential
Resources were never imported*, *the scene was never added to the Build Profile*, *the change was made in Play
Mode and lost on exit*.

**P7 — Declare the starting state.** 7.1 Every step opens with what must already be true. A prerequisite that
no earlier step established gets its own step; it is never buried in a preamble.

### Verification design (Phase 5)

- **Every gate is observed in Play Mode or in the built player, and names which** (rule 6.2). Where a value
  cannot be judged by eye — apex height, dash distance, coyote window — the gate is read as a **number** the
  reader's own code prints to the Console or displays in the HUD, never as "it looks about right" (rules
  6.1, 6.3, 6.4).
- **Measured, not asserted.** The apex (≈2.50 units), the dash distance (5 units) and the coyote window
  (0.10 s) are values the drafting pass must **measure in the Editor** and then quote identically everywhere.
- **Gates proven by breaking them** (rule 6.5), planned here with their blast radius: **M5** — set
  `coyoteTimeSeconds` to `0` and confirm the ledge jump stops working, then restore it; **M6** — remove the
  `CompositeCollider2D` and confirm the player catches on tile seams; **M9** — disable the checkpoint component
  and confirm death sends the player back to the level start; **M12** — delete the
  `cavernDash.bindingOverrides` key from `PlayerPrefs` and confirm the next launch comes up on the default
  bindings, then rebind and relaunch to confirm it comes back.
- **Where a gate names a set, it sweeps the set** (rule 6.6): "every animation transition fires" enumerates
  idle→run→jump→fall→dash; "both devices work" is tested on keyboard **and** gamepad; "the level plays" means
  every coin in the level is reachable, checked one by one at the M12 gate.
- **The M13 gate runs in the built player, not the Editor** — the last milestone's whole point is that a build
  behaves differently (missing scenes, missing input, a window that will not close, and rebinds written to a
  `PlayerPrefs` store that lives somewhere else than the Editor's).
- **Consistency check before shipping:** every command and code block uses the pinned versions; every
  load-bearing name is spelled identically wherever it recurs.
- **Reconcile-before-follow:** if the guide is followed against a Unity version that has drifted, **reality
  wins** — patch the guide and log the drift in `status.md`.
- **Troubleshooting sheet** at the guide root gathers the cross-milestone traps: Library corruption after a
  crash, `.meta` conflicts in Git, the Editor stealing gamepad focus, Play Mode edits being discarded.

---

## 8. Folder / file layout

```
guide-forge-unity-platformer/
├── README.md                      # repo front door: what this is, how to run the game, how to follow the guide
├── LICENSE                        # MIT (the guide + code); Kenney assets are CC0, credited separately
├── CREDITS.md                     # the CC0 asset packs used, with links
├── .gitignore                     # Unity-specific
├── .gitattributes                 # Git LFS rules
├── guide/
│   ├── README.md                  # the guide's front door
│   ├── PLAN.md                    # this file
│   ├── feedback-log.md
│   ├── foundation/
│   │   ├── stack.md
│   │   ├── audience.md
│   │   ├── conventions.md
│   │   ├── glossary.md
│   │   ├── status.md
│   │   ├── progress.md
│   │   └── decision-log.md
│   ├── MILESTONE_1_project-and-version-control/
│   │   ├── 00_overview.md · 01_… · NN_verify.md
│   ├── MILESTONE_2_first-script-and-motion/
│   ├── MILESTONE_3_input-and-running/
│   ├── MILESTONE_4_gravity-and-jumping/
│   ├── MILESTONE_5_game-feel/
│   ├── MILESTONE_6_tilemap-level/
│   ├── MILESTONE_7_moving-and-one-way-platforms/
│   ├── MILESTONE_8_dash-and-wall-jump/
│   ├── MILESTONE_9_coins-enemies-lives-checkpoints/
│   ├── MILESTONE_10_animation-camera-audio/
│   ├── MILESTONE_11_scenes-menus-hud-persistence/
│   ├── MILESTONE_12_options-and-rebinding/
│   └── MILESTONE_13_build-and-ship/
└── cavern-dash/                   # the finished Unity project — the end state of M13
    ├── Assets/_Project/{Scripts,Scenes,Prefabs,Art,Audio,Animation,Settings}
    ├── Assets/ThirdParty/Kenney/  # the CC0 files actually used, with their licence
    ├── Packages/manifest.json
    └── ProjectSettings/
```

---

## 9. First move

Once this plan is approved:

1. **`/scaffold-guide`** stamps the skeleton above and pre-fills the seven foundation docs from this plan.
2. **`/draft-milestone`** drafts the **whole** guide — M1 → M13 — in one pass, honoring the ladder, the
   audience matrix, the conventions and the pedagogy contract.
3. **`/audit-guide`** QAs the result against the GuideForge contract before anything ships.
4. The reader then builds *Cavern Dash* against the finished guide, verifying each Done-when gate as they go,
   and **`/mark-progress`** records what was actually executed.
