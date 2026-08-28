# M8 · Step 02 of 06 — Turn the motor into a state machine
> Nav: [← Add the Dash action](01_add-the-dash-action.md) · [Overview](00_overview.md) · [Dash →](03_dash.md)

**Before you start:** [step 01](01_add-the-dash-action.md) finished — the `Dash` action exists and the asset
is saved. Nothing reads it yet.

## Glossary for this step
> New here: **[state machine](../foundation/glossary.md#state-machine)** (defined in *Why / design*).

## Why / design
This step adds no feature. It changes the shape of `PlayerMotor` so that the next two steps can add one each
without turning it into a maze — and doing it *before* the features, rather than after, is the whole lesson.

Think about what happens if you add a dash the obvious way: a `bool isDashing`, and a condition on the
acceleration code so it does not fight the dash. Then a wall slide: `bool isWallSliding`, and now conditions on
the acceleration code, on the gravity code, and on the dash. Then a wall-jump, which must not happen while
dashing. Four booleans describe sixteen combinations, of which perhaps four are legal, and the other twelve
are bugs waiting for a player to find them.

> New concept — **state machine**: a structure in which an object is in exactly **one** named state at a time,
> with explicit transitions between them. The illegal combinations stop being possible rather than being
> guarded against.

> Build vs borrow — **build by hand.** Unity ships a state machine, but the Animator's one drives *animation*,
> not physics, and no free C# FSM package cleared this guide's bar (maintained, free, and not hiding the
> mechanism). A `switch` over an `enum` is three lines of structure you can read in one go; reach for a
> package only when the states grow transition guards and entry/exit hooks. Recorded in
> [the decision log](../foundation/decision-log.md#d15--build-vs-borrow-the-movement-state-machine-grounded--airborne--dashing--wall-sliding).

Here the states are `Normal`, `Dashing` and `WallSliding`. A player who is dashing is *not* also wall-sliding,
because the field can only hold one value. Each state gets its own method, and a transition is one assignment.

You will end this step with exactly the behaviour you started with — the milestone gate for it is *nothing
changed* — and with somewhere for [step 03](03_dash.md) and [step 04](04_wall-slide.md) to go.

## Do this

1. In the **Project** panel, create a new C# script in `Assets/_Project/Scripts` — right-click >
   **Create > Scripting > Empty C# Script File** — named **`PlayerMovementState`**, and replace its contents
   with this. It is a plain `enum`, not a component, so it never gets attached to anything:

   ```csharp
   // Assets/_Project/Scripts/PlayerMovementState.cs — the whole file
   // What the player is doing right now. Exactly one of these is true at a time,
   // which is the point: the illegal combinations cannot be represented.
   public enum PlayerMovementState
   {
       Normal,        // running, falling, jumping — everything from M3 to M7
       Dashing,       // step 03
       WallSliding    // step 04
   }
   ```

2. In `Assets/_Project/Scripts/PlayerMotor.cs`, **ADD** this field directly below the private
   `coyoteTimeRemainingSeconds` field:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — below the coyoteTimeRemainingSeconds field
   // The one state the player is in. Every transition in this class is an
   // assignment to this field, so they are easy to find.
   private PlayerMovementState state = PlayerMovementState.Normal;
   ```

3. **REPLACE** the whole `FixedUpdate` method — from its `// FixedUpdate runs on the physics clock` comment
   down to its closing brace — with these three methods. Everything that was inside it is still here, moved
   into `TickNormalState`, unchanged apart from its indentation:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — replacing the whole FixedUpdate() method
   // FixedUpdate runs on the physics clock — 50 times a second by default.
   private void FixedUpdate()
   {
       // Facts first: every state is entitled to know these before it decides anything.
       UpdateGroundedState();
       UpdateCoyoteTimer();

       // Then exactly one behaviour runs.
       switch (state)
       {
           case PlayerMovementState.Normal:
               TickNormalState();
               break;
       }
   }

   private void UpdateCoyoteTimer()
   {
       // Refill only while grounded AND not rising, so the ground check can't re-arm the
       // window for a step after takeoff and let a mashed press double-jump in mid-air.
       if (IsGrounded && body.linearVelocity.y <= 0f)
       {
           coyoteTimeRemainingSeconds = coyoteTimeSeconds;
       }
       else if (!IsGrounded)
       {
           coyoteTimeRemainingSeconds -= Time.fixedDeltaTime;
       }
   }

   private void TickNormalState()
   {
       float desiredHorizontalSpeed = input.HorizontalInput * moveSpeedUnitsPerSecond;
       float accelerationThisStep = IsGrounded
           ? groundAccelerationUnitsPerSecondSquared
           : airAccelerationUnitsPerSecondSquared;

       // MoveTowards walks the current value towards the target by at most the third
       // argument — never overshooting it.
       float newHorizontalSpeed = Mathf.MoveTowards(
           body.linearVelocity.x,
           desiredHorizontalSpeed,
           accelerationThisStep * Time.fixedDeltaTime);

       body.linearVelocity = new Vector2(newHorizontalSpeed, body.linearVelocity.y);

       bool jumpIsBuffered = input.TimeSinceJumpPressedSeconds < jumpBufferSeconds;

       if (jumpIsBuffered && coyoteTimeRemainingSeconds > 0f)
       {
           // Replace the vertical velocity outright rather than adding to it, so a jump
           // always reaches the same height however the player was already moving.
           body.linearVelocity = new Vector2(body.linearVelocity.x, jumpVelocityUnitsPerSecond);

           // Spend both windows, so one press produces exactly one jump.
           coyoteTimeRemainingSeconds = 0f;
           input.ConsumeJumpRequest();
       }

       ApplyJumpGravityMultipliers();
   }
   ```

   The `switch` has one case today. C# is happy with that, and by the end of the milestone it has three.

4. Save and let Unity compile. The Console must be clean: this is a pure restructure, and a compile error here
   means a brace landed in the wrong place rather than anything conceptual.

5. Press **Play** and walk through M5's and M7's behaviour once more. Running accelerates. Coyote time
   forgives a late jump. The buffer forgives an early one. Tap and hold give different heights. The moving
   platform still carries you.

   If any of those changed, the extraction dropped a line. Compare `TickNormalState` against the code you had
   before — it should be identical from `desiredHorizontalSpeed` to `ApplyJumpGravityMultipliers()`.

## Done when (this step)
- [ ] `Assets/_Project/Scripts/PlayerMovementState.cs` exists and holds a three-value `enum`.
- [ ] `PlayerMotor` has a `state` field, a `FixedUpdate` that reads facts and switches, and the movement code
      inside `TickNormalState`.
- [ ] **Nothing about the game changed**: acceleration, coyote time, jump buffering, variable jump height,
      the one-way ledge and the moving platform all behave exactly as their own gates described.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
refactor(player): move movement into an explicit state machine
```

## If it breaks
- **`The name 'PlayerMovementState' does not exist`** → the file name and the `enum` name differ, or the file
  landed outside `Assets/`. Unity compiles what is under `Assets/`.
- **`Not all code paths return a value` or a stray brace error** → the replacement in action 3 swallowed or
  left behind a closing brace. `FixedUpdate`, `UpdateCoyoteTimer` and `TickNormalState` are three sibling
  methods; none is nested inside another.
- **The player no longer jumps** → `UpdateCoyoteTimer()` is missing from `FixedUpdate`, so the window never
  refills.
- **The player accelerates but ignores gravity multipliers** → `ApplyJumpGravityMultipliers()` was left out of
  `TickNormalState`. It is the last line of that method.

---
> Nav: [← Add the Dash action](01_add-the-dash-action.md) · [Overview](00_overview.md) · [Dash →](03_dash.md)
