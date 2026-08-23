# M4 · Step 05 of 06 — Measure the jump
> Nav: [← Jump once per press](04_jump.md) · [Overview](00_overview.md) · [Verify →](06_verify.md)

**Before you start:** [step 04](04_jump.md) finished — Space jumps once per press, only from the ground.

## Why / design
You are about to spend all of M5 changing how the jump feels. Every one of those changes is a comparison —
*higher than what?*, *more forgiving than what?* — and a comparison needs a baseline you actually measured.
Watching the square and saying "looks about two units" is not one.

So this step adds a **tuning tool**: a component that watches the player leave the ground, tracks the highest
point it reaches, and prints the difference when it lands. It is not gameplay, it is instrumentation, and it
is deliberately a separate file so that deleting it at the end of M5 removes it completely.

**What number should you expect?** The textbook answer is height = v² / (2g) = 14² / (2 × 39.24) = **2.50
units**. What the probe prints will be a little *lower* — somewhere around **2.3 to 2.4** — and that gap is
worth understanding rather than explaining away: physics advances in fixed 0.02-second steps, so the player is
never sampled exactly at the top of the arc, and the peak that gets recorded is the last step before gravity
wins. Continuous maths describes a curve; the engine walks it in stairs.

> Build vs borrow — nothing to borrow here: this is a dozen lines of instrumentation, well under the bar for a
> dependency. What matters is that it is *separate* from `PlayerMotor`, so gameplay code never grows a
> measurement habit.

**Write down the number your project prints.** M5's gate compares against it, not against the figure in this
page.

## Do this

1. In the **Project** panel, create a new MonoBehaviour script in `Assets/_Project/Scripts` named
   **`JumpApexProbe`**, and replace its contents with this:

   ```csharp
   // Assets/_Project/Scripts/JumpApexProbe.cs — the whole file
   using UnityEngine;

   // A tuning tool, not gameplay: measures how high the player rises between leaving
   // the ground and landing again, and prints it. Deleted at the end of M5.
   [RequireComponent(typeof(PlayerMotor))]
   public class JumpApexProbe : MonoBehaviour
   {
       private PlayerMotor motor;
       private bool wasGroundedLastStep = true;
       private float heightWhenLeavingGroundUnits;
       private float highestHeightSinceLeavingUnits;

       private void Awake()
       {
           motor = GetComponent<PlayerMotor>();
       }

       private void FixedUpdate()
       {
           bool isGroundedNow = motor.IsGrounded;
           float currentHeightUnits = transform.position.y;

           if (wasGroundedLastStep && !isGroundedNow)
           {
               // Just left the ground: start a new measurement.
               heightWhenLeavingGroundUnits = currentHeightUnits;
               highestHeightSinceLeavingUnits = currentHeightUnits;
           }
           else if (!isGroundedNow)
           {
               // Airborne: Mathf.Max keeps the highest value seen so far.
               highestHeightSinceLeavingUnits = Mathf.Max(highestHeightSinceLeavingUnits, currentHeightUnits);
           }
           else if (!wasGroundedLastStep)
           {
               // Just landed: report the arc that finished.
               float apexUnits = highestHeightSinceLeavingUnits - heightWhenLeavingGroundUnits;
               Debug.Log($"apex = {apexUnits:F2} units");
           }

           wasGroundedLastStep = isGroundedNow;
       }
   }
   ```

2. Save, let Unity compile, and drag `JumpApexProbe.cs` onto the **`Player`** object.

3. Press **Play**, wait for the square to settle on the strip, then press **Space** once. When it lands, one
   line appears in the **Console**: `apex = 2.3x units` or similar.

4. Jump five more times without moving. Every line prints the **same value**, to the second decimal. That
   repeatability is the real result of this step: a jump that varies from press to press is a jump you cannot
   tune.

5. Now make the number move, so you can trust that it is measuring and not reciting. Stop Play Mode, set
   **Jump Velocity Units Per Second** to `7` — half — and play again. The apex is roughly a *quarter* of
   before, not half, because height goes with the **square** of the launch speed. Stop, set it back to `14`,
   and save the scene.

6. Write your measured apex down — in a comment, a note, anywhere. M5 refers to it as *your* baseline.

## Done when (this step)
- [ ] Landing after a jump prints exactly one `apex = …` line to the Console.
- [ ] Six consecutive jumps from a standstill print the **same** value to two decimals.
- [ ] That value sits between `2.20` and `2.60` units — a little under the ideal `2.50`, for the
      fixed-timestep reason above.
- [ ] Halving **Jump Velocity Units Per Second** to `7` prints an apex close to a quarter of your baseline;
      restoring `14` restores the baseline.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(player): add JumpApexProbe to measure jump height
```

## If it breaks
- **Nothing prints** → the probe is not on `Player`, or the player never actually leaves the ground (check
  the jump works at all, from [step 04](04_jump.md)).
- **Two lines print per jump** → there are two `JumpApexProbe` components on the object. Remove one.
- **The apex reads `0.00`** → `motor.IsGrounded` never becomes false, which means the ground check box is far
  too tall. Check `Ground Check Size Units` reads `0.9, 0.12`.
- **The value drifts by a few hundredths between jumps** → normal and harmless: the probe samples on the
  physics step, and which step catches the peak varies slightly. Drift of more than `0.1` is not normal and
  usually means you are pressing a direction while jumping — measure from a standstill.
- **The apex is a little different from the value printed in this page** → nothing is wrong, and this is
  exactly why the step tells you to write down your own. Use yours from here on.

---
> Nav: [← Jump once per press](04_jump.md) · [Overview](00_overview.md) · [Verify →](06_verify.md)
