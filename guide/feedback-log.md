# Feedback log — Cavern Dash

> Append-only field log of friction readers hit while following this guide. Its purpose is to improve the
> guide **and** the GuideForge method over time — so entries are captured even when the guide is not (yet)
> changed. This is a **record, not a to-do**: logging an entry here does not by itself change the guide. To
> actually fix the guide from a report, run `/report-issue` (it fixes the root cause and sweeps for siblings).
>
> Written by `/log-feedback` (capture) and, when a fix ships, by `/report-issue`. Newest entries on top; one
> entry per distinct piece of friction. Use absolute dates (`2026-08-22`), never "today".

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
