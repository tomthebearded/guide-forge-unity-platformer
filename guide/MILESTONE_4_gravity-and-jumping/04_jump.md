# M4 · Step 04 of 06 — Jump once per press
> Nav: [← Ask whether the player is grounded](03_ground-check.md) · [Overview](00_overview.md) · [Measure the jump →](05_measure-the-jump.md)

**Before you start:** [step 03](03_ground-check.md) finished — `PlayerMotor` has a working ground check and
its **Ground Layers** mask reads `Ground`.

## Why / design
A jump is one line of physics — set the vertical velocity to something positive and let gravity do the rest —
wrapped in two questions that are less obvious than they look.

**"Was jump pressed?" is asked on the wrong clock.** Input is polled in `Update`, once per rendered frame;
physics runs in `FixedUpdate`, on its own 0.02-second beat. At 60 frames per second those two are not in step:
some frames get no physics tick at all, and a press read during such a frame would be gone by the time the
next tick arrives. So `PlayerInputReader` **latches** the press into a flag, and `PlayerMotor` **consumes** it
— reads it and clears it. This latch is the seed of M5's jump buffer, which is the same idea with a stopwatch
attached.

**"Should it be allowed?" is the ground check's job.** Jump only when `IsGrounded`. That one condition is the
entire difference between a jump and a double jump, and it is why [step 03](03_ground-check.md) came first.

The value is **`14` units per second upward**, against the gravity of `39.24` you set in
[step 01](01_tune-gravity.md).

## Do this

1. In `Assets/_Project/Scripts/PlayerInputReader.cs`, **ADD** this field and this property below the existing
   `private InputAction moveAction;` line:

   ```csharp
   // Assets/_Project/Scripts/PlayerInputReader.cs — below the moveAction field
   private InputAction jumpAction;

   // Set the frame Jump is pressed; stays set until the physics step consumes it.
   public bool JumpRequested { get; private set; }
   ```

2. **ADD** the lookup to `Awake`, directly below the line that assigns `moveAction`:

   ```csharp
   // Assets/_Project/Scripts/PlayerInputReader.cs — in Awake(), below the moveAction assignment
   jumpAction = InputSystem.actions.FindAction("Player/Jump");
   ```

   `Jump` is already bound to the space bar and the gamepad's south button in the project-wide asset — you saw
   it in [M3 step 01](../MILESTONE_3_input-and-running/01_meet-the-input-system.md). No asset editing needed.

3. **ADD** the latch to `Update`, directly below the line that assigns `HorizontalInput`:

   ```csharp
   // Assets/_Project/Scripts/PlayerInputReader.cs — in Update(), below the HorizontalInput assignment
   // WasPressedThisFrame() is true only on the frame the button goes down —
   // holding the key does not keep it true.
   if (jumpAction.WasPressedThisFrame())
   {
       JumpRequested = true;
   }
   ```

4. **ADD** this method at the end of the same class, after `Update`. Only the code that acts on a request may
   clear it, which is why this is a method rather than a public setter:

   ```csharp
   // Assets/_Project/Scripts/PlayerInputReader.cs — after Update()
   // Called by PlayerMotor once it has dealt with the request.
   public void ConsumeJumpRequest()
   {
       JumpRequested = false;
   }
   ```

5. In `Assets/_Project/Scripts/PlayerMotor.cs`, **ADD** this field below the existing `groundLayers` line:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — below the groundLayers field
   [Header("Jump")]
   [SerializeField] private float jumpVelocityUnitsPerSecond = 14f;
   ```

6. **ADD** the jump itself inside `FixedUpdate`, directly below the line that assigns `body.linearVelocity`:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — in FixedUpdate(), below the linearVelocity assignment
   if (input.JumpRequested && IsGrounded)
   {
       // Replace the vertical velocity outright rather than adding to it, so a jump
       // always reaches the same height however the player was already moving.
       body.linearVelocity = new Vector2(body.linearVelocity.x, jumpVelocityUnitsPerSecond);
   }

   // Consume it either way: an unusable request must not wait around and fire late.
   input.ConsumeJumpRequest();
   ```

7. Save both files, let Unity compile, then select `Player` and confirm `Player Motor (Script)` shows a
   **Jump** header with **Jump Velocity Units Per Second** = `14`.

8. Save the scene and press **Play**. Press **Space** (or the gamepad's south button): the square launches,
   arcs over, and lands. Press it again while in the air: nothing happens. Hold it down: it jumps once, and
   holding does not jump again — `WasPressedThisFrame` is true for exactly one frame.

## Done when (this step)
- [ ] Pressing **Space** while resting on the strip → the square rises well above its own height and falls
      back onto the strip.
- [ ] Pressing **Space** repeatedly while airborne → nothing happens; there is no second jump.
- [ ] Holding **Space** down → exactly one jump, not a stream of them.
- [ ] Running off the end of the strip and pressing **Space** in mid-air → nothing happens.
- [ ] Jumping while running keeps the horizontal speed: the square travels in an arc, not straight up.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(player): add a grounded-only jump driven by the Jump action
```

## If it breaks
- **Nothing happens when you press Space** → three usual causes, in order: **Ground Layers** is unset (the
  ground check never returns true), the `Jump` action is missing from the project-wide asset, or the Game view
  does not have focus.
- **The player jumps in mid-air** → the `IsGrounded` condition is missing, or `UpdateGroundedState()` is not
  the first line of `FixedUpdate`, so it is reading last step's answer.
- **Holding Space bounces the player repeatedly** → you used `IsPressed()` instead of
  `WasPressedThisFrame()`.
- **The jump feels inconsistent — sometimes lower** → expected at this stage, and it is not your bug: a press
  landing between two physics steps is served on the next one. M5 fixes the whole class of problem.
- **The player shoots off sideways when jumping** → the X component was overwritten. Read
  `body.linearVelocity.x` back in, as the code above does.

---
> Nav: [← Ask whether the player is grounded](03_ground-check.md) · [Overview](00_overview.md) · [Measure the jump →](05_measure-the-jump.md)
