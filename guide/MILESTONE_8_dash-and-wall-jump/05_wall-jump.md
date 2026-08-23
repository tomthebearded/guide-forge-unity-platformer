# M8 · Step 05 of 06 — Jump off the wall
> Nav: [← Cling to a wall](04_wall-slide.md) · [Overview](00_overview.md) · [Verify →](06_verify.md)

**Before you start:** [step 04](04_wall-slide.md) finished — the player slides down a wall while pressing into
it, and stops when it stops pressing.

## Why / design
The jump itself is one line: leave the wall with a velocity of **(9, 13)** — nine units per second away from
the wall, thirteen up. Away means `-WallDirection`, which is the whole reason the wall was recorded as a
direction rather than as two booleans.

The interesting part is what happens on the very next physics step. The player is still holding the direction
*into* the wall — that is how they got into the slide — so `TickNormalState` immediately accelerates them back
towards it, and at 35 units per second squared of air acceleration the outward velocity is cancelled in about
a quarter of a second. The player would slide back down the wall they just left. It looks like the wall-jump
did not fire, and it is one of the classic wall-jump bugs.

The fix is a **control lock**: for **0.15 seconds** after a wall-jump, horizontal input does not steer. Long
enough to get clear of the wall, short enough that nobody notices it. It is the smallest amount of "the game
ignores you" that a good wall-jump requires, and knowing it exists is what will save you an afternoon when you
build one somewhere else.

## Do this

1. In `Assets/_Project/Scripts/PlayerMotor.cs`, **ADD** these fields below the existing
   `wallSlideSpeedUnitsPerSecond` line:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — below the wallSlideSpeedUnitsPerSecond field
   [SerializeField] private Vector2 wallJumpVelocity = new Vector2(9f, 13f);
   [SerializeField] private float wallJumpControlLockSeconds = 0.15f;

   // Until this moment passes, horizontal input does not steer.
   private float horizontalControlLockedUntilTimeSeconds;
   ```

2. **ADD** the wall-jump to `TickWallSlidingState`, directly below the two lines that clamp the fall speed —
   so it is the last thing the state does:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — end of TickWallSlidingState()
   bool jumpIsBuffered = input.TimeSinceJumpPressedSeconds < jumpBufferSeconds;

   if (jumpIsBuffered)
   {
       // Away from the wall and up. -WallDirection is "the other way".
       body.linearVelocity = new Vector2(-WallDirection * wallJumpVelocity.x, wallJumpVelocity.y);

       horizontalControlLockedUntilTimeSeconds = Time.time + wallJumpControlLockSeconds;
       input.ConsumeJumpRequest();

       state = PlayerMovementState.Normal;
       body.gravityScale = baseGravityScale;
   }
   ```

   The jump buffer from M5 works here unchanged, so a press made a moment before touching the wall still
   fires — the same forgiveness, for free, because it lives in the input reader rather than in the ground
   jump.

3. **REPLACE** the horizontal-movement block at the top of `TickNormalState` — the three statements computing
   `desiredHorizontalSpeed`, `accelerationThisStep` and `newHorizontalSpeed`, plus the
   `body.linearVelocity = new Vector2(newHorizontalSpeed, …)` line that follows them — with this:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — replacing the horizontal block in TickNormalState()
   bool horizontalControlLocked = Time.time < horizontalControlLockedUntilTimeSeconds;

   if (!horizontalControlLocked)
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
   }
   ```

   Everything else in `TickNormalState` — the facing tracker and dash start above it, the jump and the gravity
   multipliers below it — stays exactly as it is.

4. **ADD** one guard to the wall-slide transition at the end of `TickNormalState`, so a wall-jump cannot be
   undone by re-entering the slide during the lock. Replace the condition line
   `if (!IsGrounded && IsPressingIntoWall() && body.linearVelocity.y < 0f)` with:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — the wall-slide transition condition in TickNormalState()
   if (!IsGrounded && !horizontalControlLocked && IsPressingIntoWall() && body.linearVelocity.y < 0f)
   ```

5. Save, let Unity compile, and confirm `Player Motor (Script)` shows **Wall Jump Velocity** `9, 13` and
   **Wall Jump Control Lock Seconds** `0.15`.

6. Save the scene and press **Play**. Jump at a wall, hold into it to start the slide, and press **Space**:
   the player leaves the wall in a clean arc away from it — even though you are still holding towards it.
   Land, jump back, and do it again from the other side: it mirrors.

7. Prove the lock is what does it, on a difference you can see rather than time. Wall-jump and immediately
   hold the direction back *into* the wall: the player leaves anyway, and only gets pulled back once the
   window closes. Now stop, set **Wall Jump Control Lock Seconds** to `0`, and repeat: the player is dragged
   straight back onto the wall and the arc collapses. Restore `0.15`. Then press dash inside the window: it
   fires — the lock governs steering, not the moveset.

## Done when (this step)
- [ ] Sliding on a wall and pressing **Space** → the player leaves the wall upward and outward, in an arc,
      while you are still holding the direction into the wall.
- [ ] The same works on walls on both sides, mirrored.
- [ ] Holding the direction back into the wall right after the wall-jump does not stop the player leaving;
      with **Wall Jump Control Lock Seconds** at `0` the same press drags it straight back. Restore `0.15`.
- [ ] A jump pressed a fraction *before* reaching the wall still fires on contact — the M5 buffer applies.
- [ ] The ground jump, coyote time, the dash, the moving platform and the one-way ledge all still behave.
- [ ] The player cannot chain wall-jumps up a single flat wall without re-entering the slide first — each
      jump needs the player to be pressing into the wall and falling again.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(player): add a wall-jump with a brief horizontal control lock
```

## If it breaks
- **The player leaves the wall and immediately slams back into it** → the control lock is not being applied.
  Check action 3 replaced the block (not just added to it) and that `horizontalControlLocked` is read from the
  field set in the wall-jump.
- **The wall-jump pushes the player *into* the wall** → the minus sign in front of `WallDirection` is missing.
- **Nothing happens on Space while sliding** → `TickWallSlidingState` returns before reaching the jump; the
  jump block must be after the clamp, not after the early `return`.
- **Control never comes back** → `wallJumpControlLockSeconds` is far too large, or the comparison uses `>`
  instead of `<`.
- **The player can climb a wall by mashing jump** → the wall-slide transition is missing the
  `!horizontalControlLocked` guard from action 4.

---
> Nav: [← Cling to a wall](04_wall-slide.md) · [Overview](00_overview.md) · [Verify →](06_verify.md)
