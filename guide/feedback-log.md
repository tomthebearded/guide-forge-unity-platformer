# Feedback log — Cavern Dash

> Append-only field log of friction readers hit while following this guide. Its purpose is to improve the
> guide **and** the GuideForge method over time — so entries are captured even when the guide is not (yet)
> changed. This is a **record, not a to-do**: logging an entry here does not by itself change the guide. To
> actually fix the guide from a report, run `/report-issue` (it fixes the root cause and sweeps for siblings).
>
> Written by `/log-feedback` (capture) and, when a fix ships, by `/report-issue`. Newest entries on top; one
> entry per distinct piece of friction. Use absolute dates (`2026-08-22`), never "today".

## 2026-09-07 — M9 · Jumping onto an enemy hurts the player instead of killing it

- **Where:** M9 — Coins, enemies, damage, lives & checkpoints, [`04_stomp-and-damage.md`](MILESTONE_9_coins-enemies-lives-checkpoints/04_stomp-and-damage.md), action 2 (the `EnemyContact` listing); breaks its own third Done-when box and the `06_verify.md` stomp gate.
- **Reader:** the guide's target reader, playing the finished M11 build and jumping on enemies.
- **What happened:** Landing on an enemy from a jump costs a life instead of destroying it. **Expected** (M9/04): "Landing on an enemy from above → the enemy is destroyed and the player bounces upward." Root cause: the stomp test asks whether the player's feet are within `0.1` units of the enemy's *head*, but a trigger callback runs after the physics step that produced the overlap — the feet sink up to `|velocity.y| × 0.02` first. Traced headless: a contact at `5.41` u/s read `feet=-0.178 head=-0.050 tol=0.1 -> stomp=False`, 0.128 below the head, and the life was lost; a six-unit fall reaches 23.4 u/s, or 0.47 of sink per step. Only a slow approach ever passed, which is how the milestone gate came to be ticked. After the fix, measured drops from one, four and eight units all stomp. Not a regression from M11 — the `Player` and `Enemy` objects are byte-identical to commit `26f15d53` and `EnemyContact.cs` has not changed since M9 — but M11/06 made it loud, because the third failed stomp now ends the run instead of silently handing back three lives.
- **Suspected class:** `unknown` — a correctness defect in the guide's own code; the fixed vocabulary has no value for that.
- **Severity:** `blocked` — a core mechanic the milestone gate asserts does not work at the speeds the game is played at.
- **Tags:** M9, M11, stomp, EnemyContact, physics, trigger-callback, fixed-timestep, tolerance, correctness, verify-gate
- **Status:** fixed via /report-issue (2026-09-07) — Route B; root fix in M9/04, swept to M9/06's checkpoint, gate box and troubleshooting; retrofit in M11/`07_verify.md` corrections; see [decision-log D33](foundation/decision-log.md#d33--a-stomp-is-decided-against-the-enemys-centre-not-its-head)
- **Quote:** "si è rotta l'eliminazione dei nemici dall'alto"

## 2026-09-07 — M11 · The HUD reads `Lives: 0` from the first frame and never changes

- **Where:** M11 — Scenes, menus, HUD & persistence, [`01_hud.md`](MILESTONE_11_scenes-menus-hud-persistence/01_hud.md), action 6 (the `HudView` listing); breaks its own second Done-when box.
- **Reader:** the guide's target reader — a developer who had just executed M11/01–03 and ran the game to check the flow.
- **What happened:** Pressing Play in `Level01` showed `Coins: 0` and `Lives: 0`, and the lives label never moved until damage was taken. **Expected** (M11/01 Done-when): `Coins: 0` and `Lives: 3` from the first frame, without touching anything. Root cause: the taught `HudView` draws its opening values in `OnEnable`, and Unity defines no `Awake`/`OnEnable` order between two objects — the `Canvas` was initialized before the `Player`, so `health.LivesRemaining` was still `0`. A headless PlayMode trace pinned it: `OnEnable frame=11 lives=0`, then `PlayerHealth.Awake frame=11 lives=3`. Because `LivesChanged` only fires on a change, the `0` then stood for the whole run. The sweep found one sibling: `PlayerAudio` seeds `livesLastSeen` from its own `Awake` (M10/05) — latent, and only saved by the component happening to sit after `PlayerHealth` in the Player's component list. Separately, and not a guide defect, `HudView`'s `Stats` and `Health` fields had been wired crossed over in both level scenes, which threw `NullReferenceException` on every load until it was fixed.
- **Suspected class:** `unknown` — a correctness defect in the guide's own code; the fixed vocabulary has no value for that, so `unknown` is the honest fit.
- **Severity:** `slowed-down` — the game runs, but the step's gate cannot go green on a correct build.
- **Tags:** M11, M10, HUD, HudView, PlayerAudio, lifecycle, Awake, OnEnable, Start, execution-order, correctness, verify-gate
- **Status:** fixed via /report-issue (2026-09-07) — Route B; root fix in M11/01, swept to M10/05, M10/07 and M11/07; retrofit in M11/04 corrections; see [decision-log D32](foundation/decision-log.md#d32--opening-reads-happen-in-start-not-in-awake-or-onenable)
- **Quote:** "fai un check che funzioni"

## 2026-08-31 — M7 · Carrying the rider errors on Play, then the player still slides off

- **Where:** M7 — Moving & one-way platforms, [`03_carry-the-rider.md`](MILESTONE_7_moving-and-one-way-platforms/03_carry-the-rider.md); gate in [`04_verify.md`](MILESTONE_7_moving-and-one-way-platforms/04_verify.md).
- **Reader:** the guide's target reader — a developer following the M7 build and testing the moving platform in Play Mode.
- **What happened:** On pressing Play, the Console threw `Cannot set the parent of the GameObject 'Player' while activating or deactivating the parent GameObject` (from `PlatformRiderCarrier.OnCollisionEnter2D` → `transform.SetParent`). **Expected** (M7/03): stand on the platform and travel with it. Root cause: `SetParent` from a physics collision callback is forbidden while the parent is activating, which happens as the first contacts fire on scene load — and re-parenting a Dynamic `Rigidbody2D` is fragile regardless (inherits the platform's scale, fights world-space physics). A first replacement (delta-carry with `OnCollisionEnter/Exit` rider tracking) removed the error but the player **still slid off**, because a kinematic platform sliding into a resting body fires those callbacks unreliably; the fix that held detects riders with a per-step `Physics2D.OverlapBox` on the platform's top edge.
- **Suspected class:** `unknown` — the guide taught code that both errors and (in its first fix) fails to carry; the fixed vocabulary has no value for "defect in the guide's own approach", so `unknown` is the honest fit.
- **Severity:** `blocked` — the step errors on Play and its core outcome (the rider is carried) does not happen.
- **Tags:** M7, moving-platform, rider, SetParent, physics-callback, OverlapBox, correctness, verify-gate, PlatformRiderCarrier
- **Status:** fixed via /report-issue (2026-08-31) — all ahead of the frontier, rewritten in place; root fix in M7/03, swept to M7/02, M7/04, M7/00, PLAN.md; see [decision-log D14](foundation/decision-log.md#d14--build-vs-borrow-carrying-a-rider-on-a-moving-platform)
- **Quote:** "ho questo errore … sta scivolando ancora"

## 2026-08-28 — M5 · Mashing the jump button produces a second jump in mid-air

- **Where:** M5 — Game feel. Coyote logic introduced in [`02_coyote-time.md`](MILESTONE_5_game-feel/02_coyote-time.md), combined with the buffer in [`03_jump-buffer.md`](MILESTONE_5_game-feel/03_jump-buffer.md); contradicts the gate box in [`06_verify.md`](MILESTONE_5_game-feel/06_verify.md).
- **Reader:** the guide's target reader — a developer following the M5 build and testing jump feel by hand.
- **What happened:** Repeatedly mashing the jump button while taking off *sometimes* produces a second jump a fraction of a unit above the ground. **Expected** (M5/06 gate): "one press, one jump — pressing again in mid-air does nothing." **Actually:** an intermittent extra mid-air jump. Root cause understood: the taught `PlayerMotor` refills the coyote window on *every* grounded frame (`if (IsGrounded) coyoteTimeRemainingSeconds = coyoteTimeSeconds;`), the ground-check `OverlapBox` can still report grounded for a frame after takeoff, so coyote is re-armed, and a fresh (mashed) buffered press then satisfies `jumpIsBuffered && coyoteTimeRemainingSeconds > 0f` a second time. Timing-dependent, hence "sometime".
- **Suspected class:** `unknown` — the underlying cause is diagnosed (a correctness defect in the guide's taught code), but the fixed vocabulary has no value for "bug in the guide's own code"; `unknown` is the honest fit.
- **Severity:** `slowed-down` — the game still runs, but the behaviour is wrong and directly contradicts the M5 verify gate's "one press, one jump" box (audit-BLOCKER-worthy for the guide when fixed).
- **Tags:** M5, jump, coyote-time, jump-buffer, game-feel, correctness, verify-gate, PlayerMotor
- **Status:** fixed via /report-issue (2026-08-28) — Route B; root fix in M5/02, swept to M5/03, M5/06, M8/02, M8/06, M10/07; retrofit in M6/01 corrections; see [decision-log D31](foundation/decision-log.md#d31--the-coyote-refill-is-guarded-against-re-arming-while-rising)
- **Quote:** "when i keep pressing jump sometime it jumps in midair"
