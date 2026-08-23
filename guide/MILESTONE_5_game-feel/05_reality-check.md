# M5 · Step 05 of 06 — Stop and play it
> Nav: [← Variable jump height](04_variable-jump-height.md) · [Overview](00_overview.md) · [Verify →](06_verify.md)

**Before you start:** [step 04](04_variable-jump-height.md) finished — acceleration, coyote time, jump
buffering and variable jump height are all in, and the project compiles clean.

## Why / design
This is the **reality-check gate** — the one point in this guide that asks you to stop building and use the
thing.

The reason it sits here and not earlier is that this is the first moment *Cavern Dash* is fun. M3 gave you
something that moves, M4 something that jumps; neither is a game you would play voluntarily. What you have now
is the layer that separates a physics demo from a platformer, and everything in the eight milestones ahead —
tilemap levels, dash and wall-jump, enemies, menus, a build — is built on top of exactly this feel. If the
movement is wrong, it is wrong in every one of them, and it is a hundred times cheaper to notice that now.

The second reason is honesty about the guide's numbers. Every value you have typed came from this page.
They are good defaults, not laws, and the ones you will most want to argue with are the air acceleration and
the fall multiplier. Arguing with them is the exercise.

## Do this

1. Press **Play** and actually play for **five minutes**. Not thirty seconds of checking that it works — five
   minutes of running the strip end to end, jumping, turning, trying to fall off, trying to catch the edge.
   Set a timer if that feels silly; it is the least silly thing in this milestone.

2. As you play, answer these five questions out loud. They are the ones the next eight milestones depend on:
   - Does the character start and stop with a weight you like, or does it feel slippery?
   - When you jump off the end of the strip a fraction late, does it feel forgiving or does it feel like the
     game cheated for you?
   - When you press jump slightly early on landing, does it feel responsive or twitchy?
   - Is the difference between a tap and a hold big enough to be a decision, or so big it feels like two
     different moves?
   - Does the fall feel decisive, or heavy?

3. Tune whatever bothered you. Play, change one value in the Inspector, feel the difference, stop, and type
   the value in again while stopped — the workflow from
   [M2 step 03](../MILESTONE_2_first-script-and-motion/03_tune-in-inspector.md). One value at a time; two at
   once and you learn nothing from either. This table is the map of what each one does:

   | If it feels… | Change | Guide's value | Try |
   |---|---|---|---|
   | Slippery on the ground | `Ground Acceleration Units Per Second Squared` | `60` | `80`–`100` |
   | Sluggish to get going | same | `60` | `40` gives more of a run-up, `100` is nearly instant |
   | Uncontrollable in the air | `Air Acceleration Units Per Second Squared` | `35` | `50` for more authority |
   | Too easy to steer mid-air | same | `35` | `20` for a more committed jump |
   | Like the game jumps for you | `Coyote Time Seconds` | `0.1` | `0.06` |
   | Like late jumps still fail | same | `0.1` | `0.15` |
   | Twitchy on landing | `Jump Buffer Seconds` | `0.12` | `0.08` |
   | Floaty at the top of a jump | `Fall Gravity Multiplier` | `1.8` | `2.2`–`2.6` |
   | Too heavy on the way down | same | `1.8` | `1.4` |
   | Like taps barely register | `Low Jump Gravity Multiplier` | `2.2` | `1.6` |

4. **Write down whatever you changed** and put the same numbers into
   [`../foundation/conventions.md`](../foundation/conventions.md), in the canonical tuning table. Later
   milestones quote these figures; if yours differ, yours are right and the table should say so. This is the
   one place in the guide where you are expected to overwrite its values with your own.

5. Decide, deliberately, whether to continue. If the movement feels good, the rest of this guide is worth
   building. If it does not, no amount of tilemap or enemy work will fix it — go back to step 01 and tune
   until it does.

6. When you are done tuning, remove the measuring tool — **last**, and only once
   [step 04](04_variable-jump-height.md)'s two apex readings (held, tapped) are written down. The milestone
   gate refers back to those numbers rather than asking you to measure again, so losing them means replaying
   step 04. Then select `Player`, and on the `Jump Apex Probe (Script)` component use
   **⋮ > Remove Component**; delete `Assets/_Project/Scripts/JumpApexProbe.cs` in the Project panel. It has
   done its job: from here you tune by feel, and instrumentation left in the project is instrumentation that
   rots.

7. Save the scene and commit.

## Done when (this step)
- [ ] You have played for five uninterrupted minutes and can answer all five questions in action 2.
- [ ] Any value you changed is reflected both in the Inspector **and** in
      [`../foundation/conventions.md`](../foundation/conventions.md)'s tuning table.
- [ ] Step 04's held-jump and tapped-jump apex readings are written down somewhere you can still find them.
- [ ] `Player` no longer has a `Jump Apex Probe (Script)` component, and
      `Assets/_Project/Scripts/JumpApexProbe.cs` no longer exists.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
chore(player): remove the jump apex probe after tuning
```

## If it breaks
- **"It feels wrong but I can't say which value it is"** → jump on the spot with nothing else pressed and
  watch only the arc; that isolates gravity, the two multipliers and the jump velocity from anything to do
  with running.
- **You tuned for twenty minutes and lost it all** → the changes were made during Play Mode. They are gone;
  this is the discard rule, and the table above is why you wrote the numbers down.
- **`The referenced script on this Behaviour is missing`** after deleting the probe → you deleted the file
  before removing the component. Select `Player` and remove the empty component entry with **⋮ > Remove
  Component**.

---
> Nav: [← Variable jump height](04_variable-jump-height.md) · [Overview](00_overview.md) · [Verify →](06_verify.md)
