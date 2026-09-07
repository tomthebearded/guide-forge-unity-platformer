<!-- Foundation doc. Section headings stay English by design. Prose language: English.
     Append-only; supersede rather than delete. Every build-vs-borrow row from the plan is an entry here. -->

# Decision log — Cavern Dash

> Why the guide is the way it is. Each entry: the decision, the reasoning, and what it rules out.

## D1 — Pin Unity 6.3 LTS (`6000.3.x`)
- **Date:** 2026-08-22
- **Source:** the Phase 0.5 web check (unity.com release/support pages).
- **Decision:** every step targets **Unity 6.3 LTS**, the newest LTS line.
- **Why:** 6.3 LTS is supported until **December 2027**, giving the guide the longest runway available. Unity
  **6.0 LTS support ends October 2026** — two months after this guide was planned — so the older LTS was the
  real risk, not the newer one. The tech-stream releases (6.4, 6.5) are not LTS and move too fast for a guide.
- **Rules out / trade-off:** features added after 6.3 cannot be used, and a reader already on 6.0 must install
  a second Editor version (the Hub makes this routine).
- **Revisit if:** a newer LTS ships, or 6.3 approaches its December 2027 end of support — run `/update-stack`.

## D2 — The player is a Dynamic `Rigidbody2D` driven by velocity
- **Date:** 2026-08-22
- **Source:** the audience interview (the physics question), resolved against the Phase 0.5 check.
- **Decision:** the player carries a **Dynamic `Rigidbody2D`**; the engine integrates gravity and resolves
  collisions, and your code writes **`linearVelocity` in `FixedUpdate`**. No `transform.position` writes.
- **Why:** it is the idiom the rest of the ladder is built on — Tilemap composite colliders, effectors, moving
  platforms, enemy contacts and Cinemachine all assume it. A hand-rolled kinematic controller (the literal
  translation of the *Shape Jumper* approach) would have to re-solve every one of those, doubling the work of
  each later milestone while teaching physics the reader is not here for.
- **Rules out / trade-off:** you inherit the engine's collision response, including its edge cases; precise
  frame-perfect control is harder than with a fully hand-written controller.
- **Revisit if:** the movement starts fighting the solver (jitter that damping cannot fix, or a need for
  deterministic replays) — then a Kinematic body with hand-written resolution becomes the right trade.

## D3 — Drop the gravity flip inherited from *Shape Jumper*
- **Date:** 2026-08-22
- **Source:** the audience interview.
- **Decision:** the random gravity flip — the source guide's headline twist — is **not** part of this guide.
- **Why:** it was asked to be removed, and the expressive weight moves instead to the moveset (dash,
  wall-jump) and to real level content. It also sits badly with D2: keeping every vertical calculation
  sign-agnostic is cheap in a hand-written integrator and awkward once the engine owns gravity.
- **Rules out / trade-off:** the guide loses its most distinctive single mechanic and gains a more
  conventional — but far deeper — platformer.
- **Revisit if:** you want it back: it becomes a `gravityScale` sign flip plus an "up is down" ground check,
  and it is an amendment (`/amend-guide`), not an edit, once anyone is following the guide.

## D4 — Art and audio come from Kenney's CC0 packs
- **Date:** 2026-08-22
- **Source:** the audience interview; licence verified on kenney.nl 2026-08-22.
- **Decision:** the reader downloads **Kenney's Pixel Platformer** (18×18 tiles) and three CC0 sound packs;
  the finished project in this repository **commits the exact files it uses**, with a credits file.
- **Why:** CC0 is a public-domain dedication — *"you're free to use them, even in commercial projects"*,
  attribution not required — so a public repository can redistribute them. Committing them means the guide
  never depends on a live download that may move or be re-versioned.
- **Rules out / trade-off:** the guide is not teaching you to make art, and the 18×18 tile size is
  load-bearing (it fixes Pixels Per Unit at 18).
- **Revisit if:** you swap art packs — then PPU and every tile-derived measurement in the guide change with it.

## D5 — Git + Git LFS from Milestone 1
- **Date:** 2026-08-22
- **Source:** the plan's hard constraints (the deliverable is a GitHub repository).
- **Decision:** version control is set up in **M1**, before any code exists: a Unity `.gitignore`, `.meta`
  files tracked, and `.gitattributes` routing binary types through **Git LFS**.
- **Why:** every step in this guide ends with a suggested commit, which is meaningless without a repository
  from step one. Doing it first also means `Library/` never enters the history — the mistake that is expensive
  to undo and free to prevent.
- **Rules out / trade-off:** a small amount of setup before anything visible happens; LFS's free tier
  (1 GB storage, 1 GB/month bandwidth) is a ceiling to know about, though this project's assets are kilobytes.
- **Revisit if:** you move to a different host or to Unity Version Control (out of scope here).

## D6 — Options and key rebinding are core scope; CI and automated tests are not
- **Date:** 2026-08-22
- **Source:** the audience interview (scope revision after the first pass).
- **Decision:** an options screen with volume, display settings and **full interactive rebinding** is a
  milestone of its own (**M12**). **No GitHub Actions / CI pipeline** and **no Unity Test Framework tests** —
  these are excluded outright, not deferred.
- **Why:** rebinding is the feature the modern Input System exists for, and it closes the loop on the guide's
  three cross-cutting systems at once (input, UI, persistence). CI and automated tests would each add a
  milestone about tooling rather than about the game.
- **Rules out / trade-off:** every gate in this guide is verified by a person watching the game run — there
  is no regression suite behind you. (Builds were originally made by hand in an M13; that milestone was
  removed on 2026-09-07 — see [D33](#d33--the-guide-stops-at-development-no-build-and-ship-milestone).)
- **Revisit if:** the project grows past what one person can re-verify by hand.

## D7 — Steps show fragments; verify files hold whole files and whole settings
- **Date:** 2026-08-22
- **Source:** the plan's conventions (§ Code presentation).
- **Decision:** a step creating a file shows it whole; a step editing one shows only the fragment plus a
  unique anchor. Each `NN_verify.md` carries the **complete** contents of every file the milestone touched —
  and, because this is Unity, an **Editor checkpoint** table of every Inspector value that milestone set.
- **Why:** re-pasting a whole file into an edit step invites the reader to overwrite their own work; but a
  guide with only fragments leaves nothing authoritative to diff against. The settings table exists because
  half of a Unity project's state is not in any file the reader edits by hand.
- **Rules out / trade-off:** verify files are long.
- **Revisit if:** never for the code rule; the settings table can be trimmed if a milestone touches no
  Inspector state.

## D8 — Build vs borrow: Input handling and device bindings
- **Date:** 2026-08-22
- **Source:** the plan's build-vs-borrow table (posture: **balanced**), options verified 2026-08-22. Lands in **M3**.
- **Decision:** **borrow `com.unity.inputsystem` 1.19** (https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/).
- **Why:** it is plumbing on the way to the lesson, and it is what makes the gamepad a first-class device rather than a second implementation. Unity 6 installs it by default and ships the `InputSystem_Actions` asset the guide edits.
- **Rules out / trade-off:** an asset-based model to learn, and a settings page you have to know exists.
- **Revisit if:** you need input the package cannot express, or Unity replaces it again. Swapping it on a guide someone is already following is an amendment, not an edit — `/amend-guide`.

## D9 — Build vs borrow: Gravity, collision detection and resolution
- **Date:** 2026-08-22
- **Source:** the plan's build-vs-borrow table (posture: **balanced**), options verified 2026-08-22. Lands in **M4**.
- **Decision:** **borrow the engine's `Rigidbody2D` + `Collider2D`** (https://docs.unity3d.com/6000.3/Documentation/Manual/2d-physics/rigidbody/introduction-to-rigidbody-2d.html).
- **Why:** writing swept collision from scratch is a guide of its own, and a hand-rolled controller fights every other package on this ladder (tilemap colliders, effectors, Cinemachine). See D2.
- **Rules out / trade-off:** the engine's collision response and its edge cases are yours to live with.
- **Revisit if:** you need deterministic, frame-perfect control — see D2's revisit-if. Swapping it on a guide someone is already following is an amendment, not an edit — `/amend-guide`.

## D10 — Build vs borrow: The feel layer: coyote time, jump buffering, variable jump height, acceleration
- **Date:** 2026-08-22
- **Source:** the plan's build-vs-borrow table (posture: **balanced**), options verified 2026-08-22. Lands in **M5**.
- **Decision:** **build by hand** — no maintained free 2D-controller package cleared the Phase 0.5 bar.
- **Why:** this is precisely what the guide set out to teach, and it is the clearest possible demonstration of why input is captured in `Update` and consumed in `FixedUpdate`.
- **Rules out / trade-off:** roughly a hundred lines you own and must tune yourself.
- **Revisit if:** a package appears that is maintained, free, and does not hide the mechanism. Swapping it on a guide someone is already following is an amendment, not an edit — `/amend-guide`.

## D11 — Build vs borrow: Level geometry and its collider
- **Date:** 2026-08-22
- **Source:** the plan's build-vs-borrow table (posture: **balanced**), options verified 2026-08-22. Lands in **M6**.
- **Decision:** **borrow Tilemap + `CompositeCollider2D`** (https://docs.unity3d.com/6000.3/Documentation/Manual/class-Tilemap.html).
- **Why:** hand-placing hundreds of box colliders is not a lesson, and the composite is what removes the seams a player would otherwise catch on.
- **Rules out / trade-off:** the grid and palette workflow to learn before you can paint anything.
- **Revisit if:** the level stops being hand-authored content. Swapping it on a guide someone is already following is an amendment, not an edit — `/amend-guide`.

## D12 — Build vs borrow: Auto-tiling (correct edge and corner tiles while painting)
- **Date:** 2026-08-22
- **Source:** the plan's build-vs-borrow table (posture: **balanced**), options verified 2026-08-22. Lands in **M6**.
- **Decision:** **borrow `com.unity.2d.tilemap.extras` — Rule Tile** (https://docs.unity3d.com/Packages/com.unity.2d.tilemap.extras@latest/).
- **Why:** neighbour matching is real work that is entirely off this guide's subject.
- **Rules out / trade-off:** one extra package and a rule editor to learn.
- **Revisit if:** you need tiling rules the Rule Tile cannot express. Swapping it on a guide someone is already following is an amendment, not an edit — `/amend-guide`.

## D13 — Build vs borrow: One-way (pass-through-from-below) platforms
- **Date:** 2026-08-22
- **Source:** the plan's build-vs-borrow table (posture: **balanced**), options verified 2026-08-22. Lands in **M7**.
- **Decision:** **borrow `PlatformEffector2D`** (https://docs.unity3d.com/6000.3/Documentation/Manual/class-PlatformEffector2D.html).
- **Why:** per-contact normal filtering and collider toggling is a classic bug farm, and the engine already ships the correct version behind two Inspector fields.
- **Rules out / trade-off:** a surface-arc concept to understand, and behaviour you cannot step through in your own code.
- **Revisit if:** you need pass-through rules the effector cannot express (a drop-through input, say). Swapping it on a guide someone is already following is an amendment, not an edit — `/amend-guide`.

## D14 — Build vs borrow: Carrying a rider on a moving platform
- **Date:** 2026-08-22
- **Source:** the plan's build-vs-borrow table (posture: **balanced**), options verified 2026-08-22. Lands in **M7**.
- **Decision:** **build by hand** — the engine has no rider solution for Dynamic bodies.
- **Why:** there is nothing to borrow, and the reason a rider slides off is worth understanding: it is the difference between a platform that moves the world and one that moves itself.
- **Rules out / trade-off:** you own how the platform carries its rider, and its edge cases.
- **Approach — corrected 2026-08-31 (field report):** originally taught **re-parenting** (make the rider a child of the platform). That was wrong twice over: `transform.SetParent(...)` called from a physics collision callback throws `Cannot set the parent … while activating or deactivating` when the first contacts fire during scene activation, and re-parenting a Dynamic `Rigidbody2D` is fragile anyway (it fights world-space physics and inherits the parent's scale). Superseded by **movement inheritance**: each physics step the platform measures its own position delta and shifts every rider on its top surface by the same amount, applied to the rider's `Rigidbody2D.position`. Riders are detected with a per-step `Physics2D.OverlapBox` strip on the platform's top edge rather than `OnCollisionEnter/Exit` bookkeeping, because a kinematic body sliding into a resting one makes those callbacks fire unreliably. The carrier also installs a frictionless `PhysicsMaterial2D` on its surface (in `Awake`, only if none was assigned) so `MovePosition`'s residual friction drag does not add to the delta and push the rider ahead of the platform. This keeps the player's movement code (dash, wall-jump in M8) ignorant of platforms — the coupling the original feared from "velocity inheritance" lives entirely on the platform. See [M7/03](../MILESTONE_7_moving-and-one-way-platforms/03_carry-the-rider.md).
- **Revisit if:** Unity ships a rider solution for 2D Dynamic bodies. Swapping it on a guide someone is already following is an amendment, not an edit — `/amend-guide`.

## D15 — Build vs borrow: The movement state machine (grounded / airborne / dashing / wall-sliding)
- **Date:** 2026-08-22
- **Source:** the plan's build-vs-borrow table (posture: **balanced**), options verified 2026-08-22. Lands in **M8**.
- **Decision:** **build by hand** — Animator state machines drive animation, not physics, and no free C# FSM package cleared the bar.
- **Why:** it is the single best C# lesson on the ladder: exclusive states with explicit entry and exit, and a first-hand demonstration of why four booleans stop working at four states.
- **Rules out / trade-off:** a small amount of structure you maintain yourself.
- **Revisit if:** the state count grows past what a hand-written switch stays readable at. Swapping it on a guide someone is already following is an amendment, not an edit — `/amend-guide`.

## D16 — Build vs borrow: Camera follow, look-ahead and bounds
- **Date:** 2026-08-22
- **Source:** the plan's build-vs-borrow table (posture: **balanced**), options verified 2026-08-22. Lands in **M10**.
- **Decision:** **borrow `com.unity.cinemachine` 3.1** (https://docs.unity3d.com/Packages/com.unity.cinemachine@3.1/).
- **Why:** a `LateUpdate` lerp is ten lines that immediately need damping, look-ahead and a confiner bolted onto them — all of which Cinemachine already has.
- **Rules out / trade-off:** a package and its brain/camera split to learn, plus the confusion of a web full of Cinemachine 2 tutorials naming a class that no longer exists.
- **Revisit if:** the camera needs behaviour Cinemachine does not model. Swapping it on a guide someone is already following is an amendment, not an edit — `/amend-guide`.

## D17 — Build vs borrow: Sprite animation and transitions
- **Date:** 2026-08-22
- **Source:** the plan's build-vs-borrow table (posture: **balanced**), options verified 2026-08-22. Lands in **M10**.
- **Decision:** **borrow the Animator / Animation window** (https://docs.unity3d.com/6000.3/Documentation/Manual/AnimationSection.html).
- **Why:** manual sprite swapping on a timer is fine for two clips and unmanageable at six; the Animator is also how every other Unity project you will meet does it.
- **Rules out / trade-off:** a state-machine editor and parameter plumbing to learn.
- **Revisit if:** the animation logic becomes simpler in code than in the graph. Swapping it on a guide someone is already following is an amendment, not an edit — `/amend-guide`.

## D18 — Build vs borrow: Volume control and audio routing
- **Date:** 2026-08-22
- **Source:** the plan's build-vs-borrow table (posture: **balanced**), options verified 2026-08-22. Lands in **M10**.
- **Decision:** **borrow `AudioMixer`** (https://docs.unity3d.com/6000.3/Documentation/Manual/AudioMixer.html).
- **Why:** multiplying volumes by hand across every `AudioSource` is the wrong shape, and the mixer is what M12's sliders will talk to.
- **Rules out / trade-off:** one asset, exposed parameters, and the decibel-versus-linear conversion trap.
- **Revisit if:** never, for this project. Swapping it on a guide someone is already following is an amendment, not an edit — `/amend-guide`.

## D19 — Build vs borrow: Screen text and the HUD
- **Date:** 2026-08-22
- **Source:** the plan's build-vs-borrow table (posture: **balanced**), options verified 2026-08-22. Lands in **M11**.
- **Decision:** **borrow TextMeshPro** via `com.unity.ugui` (https://docs.unity3d.com/Packages/com.unity.ugui@2.0/manual/TextMeshPro/index.html).
- **Why:** there is nothing to learn from hand-rendering text, and legacy `Text` is worse in every dimension.
- **Rules out / trade-off:** importing TMP Essential Resources once, and remembering that the package moved.
- **Revisit if:** never, for this project. Swapping it on a guide someone is already following is an amendment, not an edit — `/amend-guide`.

## D20 — Build vs borrow: Loading levels and moving between scenes
- **Date:** 2026-08-22
- **Source:** the plan's build-vs-borrow table (posture: **balanced**), options verified 2026-08-22. Lands in **M11**.
- **Decision:** **borrow `SceneManager`** (https://docs.unity3d.com/6000.3/Documentation/Manual/scene-management.html).
- **Why:** it is the platform's own API; there is no alternative worth writing.
- **Rules out / trade-off:** learning what survives a scene load and what does not.
- **Revisit if:** never, for this project. Swapping it on a guide someone is already following is an amendment, not an edit — `/amend-guide`.

## D21 — Build vs borrow: Carrying score, lives and time across scenes
- **Date:** 2026-08-22
- **Source:** the plan's build-vs-borrow table (posture: **balanced**), options verified 2026-08-22. Lands in **M11**.
- **Decision:** **build by hand** on `DontDestroyOnLoad` — the engine offers a mechanism, not a design.
- **Why:** the real lesson is what dies on a scene load; one deliberate long-lived object, named and justified, teaches it better than any package.
- **Rules out / trade-off:** one global-ish object (`GameSession`) that must be kept small or it becomes the place everything hides.
- **Revisit if:** the session state outgrows a single object — then a persisted save file replaces it. Swapping it on a guide someone is already following is an amendment, not an edit — `/amend-guide`.

## D22 — Build vs borrow: Persisting the best time and the options
- **Date:** 2026-08-22
- **Source:** the plan's build-vs-borrow table (posture: **balanced**), options verified 2026-08-22. Lands in **M11**.
- **Decision:** **borrow `PlayerPrefs`** (https://docs.unity3d.com/6000.3/Documentation/ScriptReference/PlayerPrefs.html).
- **Why:** a handful of scalar values is exactly what `PlayerPrefs` is for; a JSON save file is named in a callout as the next step up, not built.
- **Rules out / trade-off:** no schema, no versioning, and a store that is not meant for large or structured saves.
- **Revisit if:** the save data becomes structured or large — then a JSON file under `Application.persistentDataPath`. Swapping it on a guide someone is already following is an amendment, not an edit — `/amend-guide`.

## D23 — Build vs borrow: Interactive key and button rebinding
- **Date:** 2026-08-22
- **Source:** the plan's build-vs-borrow table (posture: **balanced**), options verified 2026-08-22. Lands in **M12**.
- **Decision:** **borrow `PerformInteractiveRebinding`** (https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/api/UnityEngine.InputSystem.InputActionRebindingExtensions.html).
- **Why:** polling every device for "the next thing pressed" is exactly the code the package already got right, down to excluding pointer deltas so a mouse twitch cannot bind itself.
- **Rules out / trade-off:** a rebinding operation with its own lifecycle — cancel key, excluded controls, disposal — to learn.
- **Revisit if:** you need a rebinding flow the operation cannot express. Swapping it on a guide someone is already following is an amendment, not an edit — `/amend-guide`.

## D24 — Build vs borrow: Persisting rebinds across launches
- **Date:** 2026-08-22
- **Source:** the plan's build-vs-borrow table (posture: **balanced**), options verified 2026-08-22. Lands in **M12**.
- **Decision:** **borrow `SaveBindingOverridesAsJson` / `LoadBindingOverridesFromJson`** with `PlayerPrefs`.
- **Why:** it is two calls, and a hand-written override serializer would silently diverge from the asset it is describing.
- **Rules out / trade-off:** nothing meaningful.
- **Revisit if:** never, for this project. Swapping it on a guide someone is already following is an amendment, not an edit — `/amend-guide`.

## D25 — Build vs borrow: The options UI itself (sliders, the rebind button's states)
- **Date:** 2026-08-22
- **Source:** the plan's build-vs-borrow table (posture: **balanced**), options verified 2026-08-22. Lands in **M12**.
- **Decision:** **build by hand** — this is ordinary UI, with nothing to borrow.
- **Why:** wiring UI to systems is the lesson: a slider that both reads current state and writes it, and a button that changes meaning while an operation is listening.
- **Rules out / trade-off:** the usual UI plumbing, which you maintain.
- **Revisit if:** never, for this project. Swapping it on a guide someone is already following is an amendment, not an edit — `/amend-guide`.

## D26 — Build vs borrow: Producing the executable — **withdrawn 2026-09-07**
- **Date:** 2026-08-22 · **withdrawn** 2026-09-07
- **Source:** the plan's build-vs-borrow table (posture: **balanced**), options verified 2026-08-22. Landed in **M13**.
- **Decision (withdrawn):** **borrow Build Profiles** (https://docs.unity3d.com/6000.3/Documentation/Manual/create-build-profile.html).
- **Why it is withdrawn:** the guide no longer produces an executable at all — M13 was removed and the guide
  stops at development ([D33](#d33--the-guide-stops-at-development-no-build-and-ship-milestone)). The row is
  kept because the capability was weighed, not because it is still in scope; a reader who wants a build after
  finishing the guide should use Build Profiles, and Unity's manual is the place for it.

## D27 — The jump buffer clamps its age at zero, because `Time.time` is two clocks
- **Date:** 2026-08-23
- **Source:** audit finding (GuideForge v1.18.0). Lands in **M5**, carried by **M8** and **M10**.
- **Decision:** `PlayerInputReader.TimeSinceJumpPressedSeconds` returns `Mathf.Max(0f, Time.time - lastJumpPressedTimeSeconds)`, and `PlayerMotor` tests it with `<` rather than `<=`.
- **Why:** the press is stamped in `Update` (frame clock) and the age is read from `FixedUpdate`, where Unity substitutes the physics clock. The two can be a physics step apart, so the raw subtraction can go negative — which made a *correct* build fire a jump with **Jump Buffer Seconds** set to `0`, and so made M5's own break recipe and milestone gate unreliable. Clamping at zero plus a strict `<` makes a window of zero mean zero.
- **Rules out / trade-off:** one line the reader has to be told about rather than deduce; in exchange the gate is exact instead of probabilistic.
- **Revisit if:** the buffer becomes a countdown drained in `FixedUpdate` (the coyote-timer shape), which sidesteps the two clocks entirely at the cost of the timestamp lesson.

## D28 — Volume settings flush to disk on panel close, not on every slider frame
- **Date:** 2026-08-23
- **Source:** audit finding (GuideForge v1.18.0). Lands in **M12**.
- **Decision:** `AudioOptions.Set` calls `PlayerPrefs.SetFloat` only; `PlayerPrefs.Save()` moves to `OnDisable`.
- **Why:** `Slider.onValueChanged` fires on every frame of a drag, so the original code performed a disk write per frame. The rebind path keeps its immediate `Save()` because a rebind is a single rare event — the two cases differ, and the guide now says why.
- **Rules out / trade-off:** a hard crash mid-drag loses that visit's volume changes. Acceptable for a setting the player can re-drag in two seconds.
- **Revisit if:** the options panel stops being a panel that closes (an always-on HUD slider would need a debounce instead).

## D29 — The options component is `AudioOptions`, not `AudioSettings`
- **Date:** 2026-08-23
- **Source:** audit finding (GuideForge v1.18.0). Lands in **M12**.
- **Decision:** name the volume component **`AudioOptions`**.
- **Why:** `UnityEngine.AudioSettings` already exists. A user class of the same name in the global namespace silently wins over it everywhere in the project, leaving the engine's type reachable only fully qualified. Renaming costs nothing and removes a trap the reader would meet much later.
- **Rules out / trade-off:** none.
- **Revisit if:** never.

## D30 — The Unity project is a folder in the guide's repository, not a repository of its own
- **Date:** 2026-08-24
- **Source:** amendment request — the project must live beside the guide folder. Lands in **M1**; it is also what `PLAN.md` §8 already described.
- **Decision:** one repository holds both. `cavern-dash/` is created as a sibling of `guide/`, M1 never runs `git init`, Unity's `.gitignore` and `.gitattributes` are written **inside `cavern-dash/`**, and the repository's own `README.md`, `LICENSE`, `CREDITS.md` and `docs/screenshot.png` belong at the **root** (they were written there by M13 until that milestone was removed on 2026-09-07 — [D33](#d33--the-guide-stops-at-development-no-build-and-ship-milestone); the guide no longer authors them). Supersedes the M1 steps' earlier assumption of a standalone project repository.
- **Why:** two facts settle the placement. A leading slash makes a gitignore pattern *"relative to the directory level of the particular `.gitignore` file itself"* (<https://git-scm.com/docs/gitignore>), so Unity's official list — `/[Ll]ibrary/`, `/[Bb]uilds/` — matches nothing from the repository root and would let `Library/` into history; inside `cavern-dash/` it works unedited, which also keeps it re-checkable against upstream. And `git lfs track` prefixes every pattern with the directory of the `.gitattributes` that declared it (git-lfs `git/gitattr/files.go`), so the gates read `cavern-dash/*.png (cavern-dash/.gitattributes)` — the prefix is the reader's proof the rules are scoped to the project. Beyond Git: the guide and the game it builds are one deliverable, and a visitor landing on the root should meet both.
- **Rules out / trade-off:** the project cannot be cloned on its own — anyone wanting only the game clones the guide with it. Every Git path in the guide gains a `cavern-dash/` prefix, and `git status --porcelain` needs `-uall` before the first commit, because Git collapses a wholly untracked folder to one line. A reader who does want a standalone game repository runs `git init` inside `cavern-dash/` themselves and reads the M1 gates without the prefix.
- **Revisit if:** the guide is published separately from the game, or the repository grows a second Unity project.

## D31 — The coyote refill is guarded against re-arming while rising
- **Date:** 2026-08-28
- **Source:** field report (reader: "when i keep pressing jump sometime it jumps in midair"), fixed via `/report-issue`. Lands in **M5**, carried by **M8** and **M10**.
- **Decision:** refill the coyote window only while grounded **and not moving upward** — `if (IsGrounded && body.linearVelocity.y <= 0f) coyoteTimeRemainingSeconds = coyoteTimeSeconds; else if (!IsGrounded) coyoteTimeRemainingSeconds -= Time.fixedDeltaTime;`. While grounded and rising the window is neither refilled nor drained (it stays spent).
- **Why:** the `OverlapBox` ground check still reports grounded for a physics step after take-off. An unconditional refill re-armed the window mid-rise, so — once the buffer ([D27](#d27--the-jump-buffer-clamps-its-age-at-zero-because-timetime-is-two-clocks)) let a fresh press qualify — a mashed jump fired a second time in the air, contradicting M5's "one press, one jump" gate. Spending the window on a jump (already present) is not enough on its own, because the refill overwrites the spend the very next step.
- **Rules out / trade-off:** chose the velocity guard over a `hasJumped`/"must land again" flag. The guard is one condition and needs no new state, but it assumes a resting player reports `linearVelocity.y <= 0` (true on a static floor); a mechanism that pushes the player up *while grounded* (a future upward-moving platform) would need the flag instead.
- **Revisit if:** a grounded upward force is added (moving platforms in M7 travel horizontally, so this holds through M7), or the motor stops deriving "grounded" from a box that overlaps the floor after take-off.

## D32 — Opening reads happen in `Start`, not in `Awake` or `OnEnable`
- **Date:** 2026-09-07
- **Source:** field report (reader: the HUD showed `Lives: 0` from the first frame), fixed via `/report-issue`. Lands in **M11** and **M10**.
- **Decision:** a script that must *display or seed itself from* another component's state reads it in `Start`. Event subscriptions stay in `OnEnable`/`OnDisable`, where they belong. Applies to `HudView` (`ShowCoins`/`ShowLives` opening calls) and `PlayerAudio` (`livesLastSeen`), and is the rule for any future component that opens on someone else's number.
- **Why:** Unity brings a scene up one object at a time — `Awake`, then `OnEnable` — and defines no order **between** objects, nor between two components on one object ([execution order](https://docs.unity3d.com/6000.3/Documentation/Manual/execution-order.html)). `PlayerHealth` fills `LivesRemaining` in its own `Awake`, so a read from another object's `OnEnable` is a coin toss; a headless PlayMode trace caught it losing (`OnEnable frame=11 lives=0`, then `PlayerHealth.Awake frame=11 lives=3`). Because `LivesChanged` only fires on a *change*, the wrong value then stands for the whole run. `Start` runs only once every `Awake` in the scene has, which makes the read deterministic without adding any coordination.
- **Rules out / trade-off:** chose `Start` over the two alternatives. **Script Execution Order** (Project Settings) would work but pins a global ordering the reader must remember and cannot see from the code — a project-wide setting to fix a two-line problem. **Having `PlayerHealth` raise `LivesChanged` from its own `Awake`** would push the value instead of pulling it, but a subscriber that has not run its `OnEnable` yet misses the event, so it trades one ordering bug for another. The cost of `Start` is one extra hook and the discipline of remembering which of the three a read belongs in.
- **Revisit if:** a component needs the value *before* the first frame (nothing does today: both readers only draw), or the project adopts a scene-loading scheme where `Start` no longer follows every `Awake` — additive loads, for instance, run their own `Awake`/`Start` pass per scene.

## D33 — The guide stops at development: no build-and-ship milestone
- **Date:** 2026-09-07
- **Source:** scope change requested by the author — the guide must cover development only. Removes **M13**; touches **M1**, **M3**, **M9**, **M11** and **M12**.
- **Decision:** the guide's last milestone is **M12**. `MILESTONE_13_build-and-ship/` (player settings, the build, testing the build, the repository, and its gate) is deleted, and no step packages an executable, writes the repository's `README.md`/`LICENSE`/`CREDITS.md`, or observes anything outside the Unity Editor. The finished state of the guide is a complete game playable end to end in Play Mode.
- **Why:** the subject being taught is how a 2D platformer is built — physics, feel, level, moveset, game loop, options. Producing a build teaches Unity's build window, and writing the repository's front door teaches neither; both are one-off distribution chores that belong to whoever publishes the project, not to a reader learning the engine. Removing them also removes the guide's only gates that could not be observed where the reader is already working.
- **Rules out / trade-off:** every gate is now an **Editor** gate, so three things the old M13 gate settled go unchecked by the guide: the fullscreen toggle never actually moves a window (M12/03 says so plainly), the game is never proven to run without the Editor (missing scenes in the build list, a `PlayerPrefs` store in a different place, a window that will not close), and the two gamepad boxes that a reader without a pad could defer to the build now have no later gate to fall to — they close whenever a pad is to hand. [D26](#d26--build-vs-borrow-producing-the-executable--withdrawn-2026-09-07) is withdrawn with the milestone.
- **Revisit if:** the guide is extended to distribution — then it is an `/amend-guide` adding a milestone after M12, not a restoration of the deleted files, because the removed steps assumed a repository the guide no longer authors.

## D33 — A stomp is decided against the enemy's centre, not its head
- **Date:** 2026-09-07
- **Source:** field report (reader: "si è rotta l'eliminazione dei nemici dall'alto"), fixed via `/report-issue`. Lands in **M9**.
- **Decision:** `EnemyContact` treats an overlap as a stomp when the player is moving downward **and** its feet are at or above `ownCollider.bounds.center.y`. The `stompToleranceUnits` field is deleted; there is nothing left to tune.
- **Why:** a 2D trigger callback runs *after* the physics step that produced the overlap, so the geometry your code reads is already stale by as much as `|velocity.y| × 0.02`. Measured headless in this project: a contact at `5.41` u/s read `feet=-0.178 head=-0.050 tol=0.1 -> stomp=False` — `0.128` below the head — and the player took the damage; free fall reaches `23.4` u/s over six units and `31.9` over eight, which is `0.47` and `0.64` of sink per step. No tolerance can fix that: one wide enough for a fast landing would also accept a walk into the enemy's flank. The enemy's centre sits `0.4` below its head (it is `0.8` tall), which is room rather than margin — the test cannot miss while the sink stays under `0.4`, i.e. up to a `20` u/s landing, and above that it degrades to *likely* instead of *certain* rather than failing outright. Measured after the fix: drops from one, four and eight units (impacts `10.7`, `22.0`, `31.9` u/s) all stomped, a synthetic one-step-deep contact stomps in both levels, and a side contact still costs a life.
- **Rules out / trade-off:** chose the centre over the two alternatives, and the measurements say what that costs. **Reconstructing the pre-step position** — `other.bounds.min.y + |velocity.y| * Time.fixedDeltaTime >= ownCollider.bounds.max.y`, "the feet were above the head before this step" — is exact at every speed and still one line; it was rejected only because it asks the reader to reason about a frame that has already happened, in the first hour of the guide. **Scaling the tolerance with speed** is the same arithmetic wearing a magic number. The centre's two costs are now known: the upper half is a generous target (brushing the top corner while descending counts), and a landing faster than `20` u/s — a fall of roughly six units or more, straight down onto a head — is likely but not guaranteed to register.
- **Revisit if:** a level puts an enemy under a long drop and players report the odd missed stomp — the pre-step reconstruction above is the upgrade, and it is a two-line change. Also revisit if enemies become much taller than they are wide (the upper half stops reading as "landed on it"), or if one must be stompable only on a small head area.
