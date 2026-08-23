# M5 · Step 02 of 06 — Coyote time: jump just after the ledge
> Nav: [← Give movement weight](01_acceleration.md) · [Overview](00_overview.md) · [Jump buffering: jump just before landing →](03_jump-buffer.md)

**Before you start:** [step 01](01_acceleration.md) finished — movement accelerates, and the jump still works
exactly as M4 left it.

## Glossary for this step
> New here: **[coyote time](../foundation/glossary.md#coyote-time)** (defined in *Why / design*).

## Why / design
Run off the end of the strip and press jump as fast as you can. Nothing happens — `IsGrounded` went false
several physics steps before your finger arrived, and the rule says no jump in the air.

That rule is correct and the result is wrong. Players do not press jump when they leave the ground; they press
it when they *see* they have left the ground, and seeing takes a frame or two, and pressing takes another.
Every platformer you have enjoyed quietly forgives that gap.

> New concept — **coyote time**: a short window after leaving the ground during which a jump still counts as
> a ground jump — named after the cartoon coyote who hangs in the air before he falls. This guide uses
> **0.10 seconds**: five physics steps, generous enough to catch a normal reaction and far too short to read
> as a double jump.

The implementation is a countdown, not a flag. While grounded it is topped back up; while airborne it drains;
the jump asks whether it is still positive. **And a jump spends it** — setting it to zero — because otherwise
the player could jump, still be inside the window, and jump again.

## Do this

1. In `Assets/_Project/Scripts/PlayerMotor.cs`, **ADD** this field and this counter directly below the
   existing `jumpVelocityUnitsPerSecond` line:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — below the jumpVelocityUnitsPerSecond field
   [SerializeField] private float coyoteTimeSeconds = 0.10f;

   // Counts down while airborne; refilled while grounded; spent by a jump.
   private float coyoteTimeRemainingSeconds;
   ```

2. **ADD** this block inside `FixedUpdate`, directly below the `UpdateGroundedState();` call, so the counter
   is current before the jump reads it:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — in FixedUpdate(), below UpdateGroundedState()
   if (IsGrounded)
   {
       coyoteTimeRemainingSeconds = coyoteTimeSeconds;
   }
   else
   {
       coyoteTimeRemainingSeconds -= Time.fixedDeltaTime;
   }
   ```

3. **REPLACE** the jump block further down in `FixedUpdate` — the one that currently begins
   `if (input.JumpRequested && IsGrounded)` — with this. The condition now asks the counter rather than the
   ground check, and spends it on the way out:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — replacing the existing jump block in FixedUpdate()
   if (input.JumpRequested && coyoteTimeRemainingSeconds > 0f)
   {
       // Replace the vertical velocity outright rather than adding to it, so a jump
       // always reaches the same height however the player was already moving.
       body.linearVelocity = new Vector2(body.linearVelocity.x, jumpVelocityUnitsPerSecond);

       // Spend the window: without this, one jump could be followed by a second
       // one a few steps later, still inside the same window.
       coyoteTimeRemainingSeconds = 0f;
   }
   ```

   `IsGrounded` is still used — by the acceleration choice in [step 01](01_acceleration.md) and by the
   countdown above — so nothing else in the file needs changing.

4. Save, let Unity compile, and confirm `Player Motor (Script)` shows **Coyote Time Seconds** = `0.1`.

5. Press **Play**. Run off the end of the strip and press **Space** the instant the square starts to drop: it
   jumps. Wait half a second in the air and press: nothing. There is now a window, and it closes.

6. Prove it is the window doing the work rather than a coincidence. Stop, set **Coyote Time Seconds** to `0`,
   play again, and repeat the same run-off-and-press: the jump **no longer fires**, exactly as in M4. Stop,
   set it back to `0.1`, and save the scene.

7. Check the thing that would make this a bug rather than a feature: jump normally from the middle of the
   strip and press **Space** again immediately while rising. Nothing happens — the window was spent by the
   first jump.

## Done when (this step)
- [ ] Running off the strip and pressing **Space** within about a tenth of a second → the square jumps.
- [ ] Doing the same after a clear pause in the air → nothing happens.
- [ ] With **Coyote Time Seconds** set to `0`, the run-off-and-press produces no jump at all; restoring `0.1`
      brings it back.
- [ ] Jumping from flat ground and immediately pressing **Space** again → still exactly one jump.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(player): forgive late jumps with a 0.10s coyote window
```

## If it breaks
- **The player can now double-jump** → `coyoteTimeRemainingSeconds = 0f;` is missing from inside the jump
  block, so the window survives the jump that used it.
- **Coyote time seems to last for ever** → the `else` branch is missing, or the countdown is subtracting
  `Time.deltaTime` outside `FixedUpdate`. It must drain on the physics clock, in the same method.
- **The window feels far longer than a tenth of a second** → check the field really reads `0.1` and not `1`.
  The Inspector shows `0.1`; a stray missing decimal point is a very long coyote.
- **Nothing changed at all** → the jump block still tests `IsGrounded`. Re-read action 3: the condition is
  the counter now.

---
> Nav: [← Give movement weight](01_acceleration.md) · [Overview](00_overview.md) · [Jump buffering: jump just before landing →](03_jump-buffer.md)
