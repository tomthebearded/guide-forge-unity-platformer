# M5 · Step 04 of 06 — Variable jump height
> Nav: [← Jump buffering: jump just before landing](03_jump-buffer.md) · [Overview](00_overview.md) · [Stop and play it →](05_reality-check.md)

**Before you start:** [step 03](03_jump-buffer.md) finished — coyote time and jump buffering both work, and
one press still produces exactly one jump.

## Glossary for this step
> New here: **[variable jump height](../foundation/glossary.md#variable-jump-height)** (defined in *Why / design*).

## Why / design
Every jump you have made so far is the same jump. Tap the button, hold it down for a second — identical arc.
That means the only way to clear a low obstacle without launching over it is to not jump at all, and it is why
the movement still reads as a machine rather than a character.

> New concept — **variable jump height**: letting a tapped button produce a lower jump than a held one, so
> the height is something the player chooses in the moment rather than a constant.

The naive implementation — cut the upward velocity the instant the button is released — works and feels
terrible: the character stops dead in mid-air like a video paused. The technique that feels right is to change
**gravity** instead of velocity, in two situations:

- **Rising with the button already released** → gravity × **2.2**. The rise slows quickly but smoothly.
- **Falling** → gravity × **1.8**, always, held or not. This is the trick that separates a floaty platformer
  from a crisp one: the character goes up on a gentle curve and comes down on a steep one, which reads as
  decisive and keeps hang time short. Nearly every 2D platformer you can name does this.

Two consequences worth predicting before you see them. Hang time drops noticeably, because the descent is
faster than the ascent. And your measured apex from M4 does **not** change for a held jump — the fall
multiplier only touches the way down.

## Do this

1. In `Assets/_Project/Scripts/PlayerInputReader.cs`, **ADD** this property below the existing
   `TimeSinceJumpPressedSeconds` property:

   ```csharp
   // Assets/_Project/Scripts/PlayerInputReader.cs — below TimeSinceJumpPressedSeconds
   // True for as long as the button is down — unlike the press timestamp above,
   // which records a single moment.
   public bool IsJumpHeld { get; private set; }
   ```

2. **ADD** this line inside `Update`, directly below the `if (jumpAction.WasPressedThisFrame())` block:

   ```csharp
   // Assets/_Project/Scripts/PlayerInputReader.cs — in Update(), below the WasPressedThisFrame block
   // IsPressed() reports the button's current state, every frame.
   IsJumpHeld = jumpAction.IsPressed();
   ```

3. In `Assets/_Project/Scripts/PlayerMotor.cs`, **ADD** these fields directly below the existing
   `jumpBufferSeconds` line:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — below the jumpBufferSeconds field
   [SerializeField] private float fallGravityMultiplier = 1.8f;
   [SerializeField] private float lowJumpGravityMultiplier = 2.2f;

   // The Inspector's Gravity Scale, captured once so the multipliers below
   // always scale the original value rather than compounding on themselves.
   private float baseGravityScale;
   ```

4. **ADD** this line to `Awake`, below the two `GetComponent` lines:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — in Awake(), below the GetComponent lines
   baseGravityScale = body.gravityScale;
   ```

5. **ADD** this method at the end of the class, directly above `OnDrawGizmosSelected`:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — above OnDrawGizmosSelected()
   private void ApplyJumpGravityMultipliers()
   {
       if (body.linearVelocity.y < 0f)
       {
           // Falling: come down faster than you went up.
           body.gravityScale = baseGravityScale * fallGravityMultiplier;
       }
       else if (body.linearVelocity.y > 0f && !input.IsJumpHeld)
       {
           // Rising, but the button is already released: cut the climb short.
           body.gravityScale = baseGravityScale * lowJumpGravityMultiplier;
       }
       else
       {
           // Rising with the button held, or standing still.
           body.gravityScale = baseGravityScale;
       }
   }
   ```

6. **ADD** the call as the **last** line inside `FixedUpdate`, below the jump block, so it reacts to the
   velocity this step just produced:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — last line inside FixedUpdate()
   ApplyJumpGravityMultipliers();
   ```

7. Save both files, let Unity compile, and confirm `Player Motor (Script)` shows **Fall Gravity Multiplier** =
   `1.8` and **Low Jump Gravity Multiplier** = `2.2`.

8. Press **Play** and use the probe from [M4 step 05](../MILESTONE_4_gravity-and-jumping/05_measure-the-jump.md),
   which is still attached:
   - **Hold** Space through the whole jump → the Console prints your M4 baseline apex, unchanged.
   - **Tap** Space as briefly as you can → the printed apex is clearly lower, typically between 40% and 70%
     of the baseline depending on how fast you let go.
   - Either way, the fall back down is visibly quicker than the rise.

9. **Write both numbers down**, beside your M4 baseline: the held-jump apex and the tapped-jump apex. The
   probe is deleted at the end of [step 05](05_reality-check.md), and the milestone gate reads these two
   figures back rather than asking you to measure them again.

## Done when (this step)
- [ ] A held jump prints the same apex you measured in M4, to within a few hundredths.
- [ ] A quick tap prints an apex **clearly lower** than the baseline — under 70% of it — and tapping faster
      lowers it further.
- [ ] Both figures are written down somewhere outside the Console.
- [ ] The descent is visibly faster than the ascent on every jump.
- [ ] Setting **Low Jump Gravity Multiplier** to `1` makes tap and hold produce the same height again;
      restoring `2.2` restores the difference.
- [ ] Coyote time and jump buffering from steps 02 and 03 still behave exactly as their gates described.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(player): make jump height depend on how long jump is held
```

## If it breaks
- **The player falls unbearably fast after a few jumps** → `baseGravityScale` is being read inside
  `ApplyJumpGravityMultipliers` instead of captured once in `Awake`, so each step multiplies the already
  multiplied value.
- **Tap and hold give the same height** → `IsJumpHeld` is never true, or the middle branch's `!` is missing.
  Check the Input System's `IsPressed()` call is in `Update`, not `FixedUpdate`.
- **The jump feels like it hits a ceiling** → `lowJumpGravityMultiplier` is far too high. `2.2` is a firm but
  smooth cut; values above `4` read as a hard stop.
- **The player now sinks slowly through the floor** → nothing to do with this step: check the ground check
  box size, which the acceleration change in step 01 does not touch.

---
> Nav: [← Jump buffering: jump just before landing](03_jump-buffer.md) · [Overview](00_overview.md) · [Stop and play it →](05_reality-check.md)
