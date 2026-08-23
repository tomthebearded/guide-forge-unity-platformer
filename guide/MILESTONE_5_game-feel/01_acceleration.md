# M5 · Step 01 of 06 — Give movement weight
> Nav: — · [Overview](00_overview.md) · [Coyote time: jump just after the ledge →](02_coyote-time.md)

**Before you start:** M4's gate passed, and you have your measured apex written down.

## Why / design
Right now the player's horizontal speed is whatever the input says, instantly. Press right and you are at 7
units per second in the same frame; release and you are at zero in the same frame. It is precise, it is
predictable, and it feels like sliding a chess piece.

Real character movement **approaches** its target speed. The difference is one number — how many units per
second the speed may change per second of time — and it is the difference between moving a value and moving a
character.

Two rates, not one:

- **On the ground: 60 units per second squared.** Reaching 7 units per second takes `7 / 60 ≈ 0.12` seconds —
  about six physics steps. Quick enough to feel responsive, slow enough to feel like mass.
- **In the air: 35 units per second squared.** Roughly half. Air control that matches ground control makes a
  jump feel like flying; too little makes a mistimed jump unrecoverable. Half is the usual compromise, and it
  is the number you will most want to argue with in [step 05](05_reality-check.md) — which is the point of
  that step.

> Build vs borrow — **there is nothing to borrow here.** No maintained, free Unity package supplies a 2D
> character-feel layer, and this is the capability the guide exists to teach. You are writing the mechanism
> that the commercial controller assets on the Asset Store package up.

## Do this

1. In `Assets/_Project/Scripts/PlayerMotor.cs`, **ADD** these fields directly below the existing
   `moveSpeedUnitsPerSecond` line:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — below the moveSpeedUnitsPerSecond field
   [SerializeField] private float groundAccelerationUnitsPerSecondSquared = 60f;
   [SerializeField] private float airAccelerationUnitsPerSecondSquared = 35f;
   ```

2. **REPLACE** the two lines inside `FixedUpdate` that compute `desiredHorizontalSpeed` and assign
   `body.linearVelocity` — the pair that currently reads:

   ```csharp
   float desiredHorizontalSpeed = input.HorizontalInput * moveSpeedUnitsPerSecond;
   body.linearVelocity = new Vector2(desiredHorizontalSpeed, body.linearVelocity.y);
   ```

   with this. The rest of `FixedUpdate` — the ground check above it, the jump below it — stays exactly as it
   is:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — replacing the two lines described above
   float desiredHorizontalSpeed = input.HorizontalInput * moveSpeedUnitsPerSecond;
   float accelerationThisStep = IsGrounded
       ? groundAccelerationUnitsPerSecondSquared
       : airAccelerationUnitsPerSecondSquared;

   // MoveTowards walks the current value towards the target by at most the third
   // argument — never overshooting it. Multiplying by fixedDeltaTime turns
   // "units per second squared" into "units per second, this step".
   float newHorizontalSpeed = Mathf.MoveTowards(
       body.linearVelocity.x,
       desiredHorizontalSpeed,
       accelerationThisStep * Time.fixedDeltaTime);

   body.linearVelocity = new Vector2(newHorizontalSpeed, body.linearVelocity.y);
   ```

   The same expression handles speeding up, slowing down and turning around, because all three are "move the
   current speed towards the desired speed". Releasing the key makes the target `0`; pressing the opposite
   direction makes it `-7`. No branch needed for any of them.

3. Save, let Unity compile, and confirm `Player Motor (Script)` now shows **Ground Acceleration Units Per
   Second Squared** = `60` and **Air Acceleration Units Per Second Squared** = `35`.

4. Press **Play** and hold **D**. The square now leans into the run rather than teleporting into it, and
   coasts briefly when you release. Tap **A** while running right: it slows, stops, and turns — a real turn,
   taking about a quarter of a second, instead of an instant reversal.

5. Feel the air rate. Jump while running and change direction in mid-air: you can still steer, but with
   noticeably less authority than on the ground.

## Done when (this step)
- [ ] Holding **D** from a standstill → the square visibly builds up speed over roughly an eighth of a second
      rather than starting at full speed.
- [ ] Releasing everything at full speed → it coasts to a stop over a similar interval instead of stopping in
      one frame.
- [ ] Pressing the opposite direction at full speed → it decelerates through zero and accelerates the other
      way, without ever snapping.
- [ ] `Rigidbody 2D` → **Info** → **Speed** still tops out at `7` while a direction is held — acceleration
      changes how you get there, not where you arrive.
- [ ] Steering in mid-air works but responds visibly more slowly than on the ground.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(player): accelerate towards the target speed instead of snapping
```

## If it breaks
- **The square never reaches full speed** → `accelerationThisStep` is being multiplied by `Time.deltaTime`
  inside `FixedUpdate` when it should be `Time.fixedDeltaTime`; they are the same number here, so if speed is
  wrong, look instead for a missing `moveSpeedUnitsPerSecond` factor in the target.
- **The square drifts on for ever after you release** → the target is not `0` when input is `0`. Check
  `desiredHorizontalSpeed` is computed from `input.HorizontalInput`, not from the last non-zero value.
- **Movement feels mushy and slow to respond** → the acceleration is too low for the speed. `60` against a top
  speed of `7` is the tuned pair; if you raised `moveSpeedUnitsPerSecond`, raise acceleration with it.
- **The jump now moves the player sideways oddly** → the jump block must still assign
  `body.linearVelocity.x` back unchanged; check you replaced only the two lines named above.

---
> Nav: — · [Overview](00_overview.md) · [Coyote time: jump just after the ledge →](02_coyote-time.md)
