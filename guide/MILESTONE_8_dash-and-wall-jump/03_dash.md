# M8 · Step 03 of 06 — Dash
> Nav: [← Turn the motor into a state machine](02_movement-state-machine.md) · [Overview](00_overview.md) · [Cling to a wall →](04_wall-slide.md)

**Before you start:** [step 02](02_movement-state-machine.md) finished — `PlayerMotor` has a `state` field and
a `switch`, and the game behaves exactly as it did in M7.

## Why / design
A dash is defined by **how far** and **how long**, not by how hard you push. Those two numbers are what a
level designer thinks in — *this gap is five units wide, can the player cross it?* — and the speed falls out of
them: `5 units ÷ 0.15 seconds = 33.3 units per second`. Define it as a force instead and the distance becomes
an emergent property of mass, drag and gravity that changes every time you tune something else.

Three details make it feel like a dash rather than a shove:

- **Gravity is off while dashing.** A dash that droops is a lunge. The player travels in a straight line and
  gravity resumes at the end.
- **The state holds the velocity, and nothing else writes it.** This is what the state machine buys: the
  acceleration code in `TickNormalState` simply does not run, so there is nothing to fight.
- **The exit clamps the speed back to running speed.** Without that, the dash ends at 33 units per second and
  the player rockets away.

The **cooldown is measured from the start of the dash**, not the end: 0.6 seconds total, of which the first
0.15 is the dash itself. Measuring from the start means the rhythm stays the same whatever you tune the
duration to.

## Do this

1. In `Assets/_Project/Scripts/PlayerInputReader.cs`, **ADD** this field and property below the existing
   `IsJumpHeld` property:

   ```csharp
   // Assets/_Project/Scripts/PlayerInputReader.cs — below the IsJumpHeld property
   private InputAction dashAction;

   // Set the frame Dash is pressed; cleared when the motor acts on it.
   public bool DashRequested { get; private set; }
   ```

2. **ADD** the lookup to `Awake`, below the `jumpAction` line:

   ```csharp
   // Assets/_Project/Scripts/PlayerInputReader.cs — in Awake(), below the jumpAction assignment
   dashAction = InputSystem.actions.FindAction("Player/Dash");
   ```

3. **ADD** the latch to `Update`, below the `IsJumpHeld` assignment:

   ```csharp
   // Assets/_Project/Scripts/PlayerInputReader.cs — in Update(), below the IsJumpHeld assignment
   if (dashAction.WasPressedThisFrame())
   {
       DashRequested = true;
   }
   ```

4. **ADD** this method at the end of the class, below `ConsumeJumpRequest`:

   ```csharp
   // Assets/_Project/Scripts/PlayerInputReader.cs — below ConsumeJumpRequest()
   public void ConsumeDashRequest()
   {
       DashRequested = false;
   }
   ```

5. In `Assets/_Project/Scripts/PlayerMotor.cs`, **ADD** these fields below the existing
   `lowJumpGravityMultiplier` line:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — below the lowJumpGravityMultiplier field
   [Header("Dash")]
   [SerializeField] private float dashDistanceUnits = 5f;
   [SerializeField] private float dashDurationSeconds = 0.15f;
   [SerializeField] private float dashCooldownSeconds = 0.60f;

   private float dashEndTimeSeconds;
   private float nextDashAllowedTimeSeconds;

   // +1 while facing right, -1 while facing left. A dash with no input uses this.
   private int facingDirection = 1;
   ```

6. **ADD** these two methods below the existing `TickNormalState` method:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — below TickNormalState()
   private void StartDash()
   {
       state = PlayerMovementState.Dashing;
       dashEndTimeSeconds = Time.time + dashDurationSeconds;
       nextDashAllowedTimeSeconds = Time.time + dashCooldownSeconds;
       input.ConsumeDashRequest();

       // No gravity for the duration: a dash is a straight line.
       body.gravityScale = 0f;

       // Distance over duration is the speed the dash must hold.
       float dashSpeedUnitsPerSecond = dashDistanceUnits / dashDurationSeconds;
       body.linearVelocity = new Vector2(facingDirection * dashSpeedUnitsPerSecond, 0f);
   }

   private void TickDashingState()
   {
       // Nothing to do while it runs: the velocity set at the start is the dash.
       if (Time.time < dashEndTimeSeconds)
       {
           return;
       }

       state = PlayerMovementState.Normal;
       body.gravityScale = baseGravityScale;

       // Come out at running speed rather than at dash speed.
       float exitSpeed = Mathf.Clamp(body.linearVelocity.x, -moveSpeedUnitsPerSecond, moveSpeedUnitsPerSecond);
       body.linearVelocity = new Vector2(exitSpeed, body.linearVelocity.y);
   }
   ```

7. **ADD** the new case to the `switch` inside `FixedUpdate`, below the `Normal` case:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — in FixedUpdate()'s switch, below the Normal case
   case PlayerMovementState.Dashing:
       TickDashingState();
       break;
   ```

8. **ADD** the facing tracker and the transition at the **top** of `TickNormalState`, directly above the
   `float desiredHorizontalSpeed` line:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — first lines inside TickNormalState()
   if (!Mathf.Approximately(input.HorizontalInput, 0f))
   {
       facingDirection = input.HorizontalInput > 0f ? 1 : -1;
   }

   if (input.DashRequested && Time.time >= nextDashAllowedTimeSeconds)
   {
       StartDash();
       return;   // the rest of this state does not run on the step the dash begins
   }

   // Drop a request that arrived during the cooldown, so it cannot fire late.
   input.ConsumeDashRequest();
   ```

9. Save both files, let Unity compile, and confirm `Player Motor (Script)` shows a **Dash** header with
   **Dash Distance Units** `5`, **Dash Duration Seconds** `0.15`, **Dash Cooldown Seconds** `0.6`.

10. Save the scene and press **Play**. Run right and press **Left Shift**: the player shoots forward in a
    straight line and drops back into a normal run. Press it again immediately: nothing, for a little over
    half a second. Dash off a ledge: the player crosses in a flat line and only starts falling when the dash
    ends.

11. Measure it. Stop, note the player's Position X, press Play, and from a standstill press Left Shift once
    without holding a direction. Read Position X again: it has moved about **5 units** in the direction the
    player was facing. You will read something between `5.0` and `5.5` — the dash ends on a physics step
    boundary, and 0.15 seconds is seven and a half steps of 0.02, so the last one lands slightly long.

## Done when (this step)
- [ ] Pressing **Left Shift** (or the gamepad's west button) → the player crosses about **5 units** in a
      straight, flat line, in the direction it faces.
- [ ] The measured displacement from a standstill reads between `5.0` and `5.5` units.
- [ ] Pressing dash again immediately → nothing happens for the remainder of the 0.6-second cooldown, then it
      works again.
- [ ] Dashing off a ledge → the player does not fall during the dash, and falls normally the moment it ends.
- [ ] The player exits the dash at running speed, not flying.
- [ ] Everything from M5 and M7 still behaves: acceleration, coyote time, buffering, the platform ride.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(player): add a fixed-distance dash with a cooldown
```

## If it breaks
- **Nothing happens on Left Shift** → the `Dash` action is not saved in the asset
  ([step 01](01_add-the-dash-action.md)'s **Save Asset**), or `FindAction("Player/Dash")` is misspelled.
- **The player dashes for ever** → `TickDashingState` is not in the `switch`, so nothing ever ends the dash.
  Check action 7.
- **The player keeps flying after the dash** → the exit clamp is missing.
- **The player falls during the dash** → `body.gravityScale = 0f;` is missing from `StartDash`, or
  `ApplyJumpGravityMultipliers` is being called while dashing (it lives in `TickNormalState` and must stay
  there).
- **Gravity is permanently zero after one dash** → `TickDashingState` never restores `baseGravityScale`; check
  the assignment sits after the early `return`, not before it.
- **The dash always goes right** → `facingDirection` is not being updated, or the tracker was added below the
  `return` in action 8 instead of above it.

---
> Nav: [← Turn the motor into a state machine](02_movement-state-machine.md) · [Overview](00_overview.md) · [Cling to a wall →](04_wall-slide.md)
