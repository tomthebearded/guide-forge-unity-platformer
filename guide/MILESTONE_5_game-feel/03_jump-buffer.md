# M5 · Step 03 of 06 — Jump buffering: jump just before landing
> Nav: [← Coyote time: jump just after the ledge](02_coyote-time.md) · [Overview](00_overview.md) · [Variable jump height →](04_variable-jump-height.md)

**Before you start:** [step 02](02_coyote-time.md) finished — coyote time works and does not permit a second
jump.

## Glossary for this step
> New here: **[jump buffering](../foundation/glossary.md#jump-buffering)** (defined in *Why / design*).

## Why / design
Coyote time forgives a press that is slightly **late**. This step forgives one that is slightly **early** —
and early presses are far more common, because the natural rhythm of a platformer is to press jump as you come
down, not after you have landed.

> New concept — **jump buffering**: remembering a jump press for a short window so that a press made while
> still falling fires the moment the ground arrives, instead of being discarded. This guide uses **0.12
> seconds**.

M4's latch is nearly there — it already holds the press until a physics step consumes it — but it forgets
*when* the press happened, and it is cleared by the very next step whether or not the jump was usable. A
buffer needs both facts, so the latch becomes a **timestamp**: you record when Jump was last pressed, and
the motor asks how long ago that was.

This is a change to a public member that `PlayerMotor` already calls, so both files change in this one step —
leaving them out of step would mean a project that does not compile.

## Do this

1. In `Assets/_Project/Scripts/PlayerInputReader.cs`, **REPLACE** the `JumpRequested` property declaration —
   the line reading `public bool JumpRequested { get; private set; }` together with its comment — with this:

   ```csharp
   // Assets/_Project/Scripts/PlayerInputReader.cs — replacing the JumpRequested property
   // When Jump was last pressed, on the same clock as Time.time.
   // Starts far in the past so that nothing is buffered at the first step.
   private float lastJumpPressedTimeSeconds = float.NegativeInfinity;

   // How long ago Jump was pressed. Large means "not recently".
   public float TimeSinceJumpPressedSeconds => Time.time - lastJumpPressedTimeSeconds;
   ```

   `=>` on a property is C#'s expression-bodied syntax: the value is recomputed each time it is read, rather
   than stored.

2. **REPLACE** the `if` block inside `Update` that sets `JumpRequested = true;` with this:

   ```csharp
   // Assets/_Project/Scripts/PlayerInputReader.cs — replacing the JumpRequested assignment in Update()
   if (jumpAction.WasPressedThisFrame())
   {
       lastJumpPressedTimeSeconds = Time.time;
   }
   ```

3. **REPLACE** the body of `ConsumeJumpRequest` — the line reading `JumpRequested = false;` — with this:

   ```csharp
   // Assets/_Project/Scripts/PlayerInputReader.cs — replacing the body of ConsumeJumpRequest()
   // Push the timestamp far into the past so the same press cannot be used twice.
   lastJumpPressedTimeSeconds = float.NegativeInfinity;
   ```

4. In `Assets/_Project/Scripts/PlayerMotor.cs`, **ADD** this field directly below the existing
   `coyoteTimeSeconds` line:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — below the coyoteTimeSeconds field
   [SerializeField] private float jumpBufferSeconds = 0.12f;
   ```

5. **REPLACE** the jump block in `FixedUpdate` — the one that begins `if (input.JumpRequested && …)` — and
   the `input.ConsumeJumpRequest();` line that follows it, with this. Consuming now happens **only when the
   jump actually fires**, which is the whole point: an unusable press must survive long enough to become
   usable.

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — replacing the jump block and the ConsumeJumpRequest() line
   bool jumpIsBuffered = input.TimeSinceJumpPressedSeconds <= jumpBufferSeconds;

   if (jumpIsBuffered && coyoteTimeRemainingSeconds > 0f)
   {
       // Replace the vertical velocity outright rather than adding to it, so a jump
       // always reaches the same height however the player was already moving.
       body.linearVelocity = new Vector2(body.linearVelocity.x, jumpVelocityUnitsPerSecond);

       // Spend both windows, so one press produces exactly one jump.
       coyoteTimeRemainingSeconds = 0f;
       input.ConsumeJumpRequest();
   }
   ```

6. Save both files and let Unity compile. `PlayerMotor` no longer mentions `JumpRequested`, and nothing else
   in the project referenced it, so the project builds clean. Confirm `Player Motor (Script)` shows **Jump
   Buffer Seconds** = `0.12`.

7. Press **Play** and jump repeatedly, pressing **Space** *while still falling* rather than waiting for the
   landing. The square now takes off the instant it touches down — the press waited for the ground.

8. Prove the window is doing it. Stop, set **Jump Buffer Seconds** to `0`, play, and press Space slightly
   early again: the press is discarded and the square lands and stays put, exactly as in M4. Stop, restore
   `0.12`, save the scene.

9. Check the failure this could have introduced: press **Space** once, land, and wait. The square must **not**
   bounce a second time — the press was consumed by the jump that used it.

## Done when (this step)
- [ ] Pressing **Space** shortly before landing → the square jumps immediately on contact, with no visible
      pause on the ground.
- [ ] Pressing **Space** a long way above the ground → nothing is remembered; the square lands and stays.
- [ ] With **Jump Buffer Seconds** at `0`, an early press is discarded; restoring `0.12` makes it work again.
- [ ] One press never produces two jumps, however early it is made.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(player): buffer jump presses for 0.12s before landing
```

## If it breaks
- **The player bounces repeatedly after one press** → `input.ConsumeJumpRequest();` is outside the `if`
  block, or missing, so the same timestamp keeps satisfying the buffer on every step.
- **`The name 'JumpRequested' does not exist`** → the `PlayerMotor` edit in action 5 was skipped or partly
  applied. Both files change in this step; that is deliberate.
- **Jumps fire at strange moments long after a press** → `jumpBufferSeconds` is far too large (a value like
  `1.2` instead of `0.12`). The buffer must be shorter than the time it takes a player to change their mind.
- **The very first jump of a session fires without a press** → `lastJumpPressedTimeSeconds` was initialised to
  `0` rather than `float.NegativeInfinity`, and `Time.time` starts near zero.

---
> Nav: [← Coyote time: jump just after the ledge](02_coyote-time.md) · [Overview](00_overview.md) · [Variable jump height →](04_variable-jump-height.md)
