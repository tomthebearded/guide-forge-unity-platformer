# M2 · Step 03 of 05 — Tune it while it runs
> Nav: [← Write your first MonoBehaviour](02_first-script.md) · [Overview](00_overview.md) · [Prove the motion is frame-rate independent →](04_frame-rate-independence.md)

**Before you start:** [step 02](02_first-script.md) finished — `ConstantMover` is attached to `Player` and the
square moves in Play Mode. This step writes no code and changes no files; it teaches the workflow you will use
to tune every value in M5, M8 and M9.

## Why / design
The reason tuning values live in the Inspector rather than in the source is that **you can change them while
the game runs**. Finding a jump that feels right means trying twenty numbers in ninety seconds, and
recompiling between each one makes that impossible.

The catch is the rule from [M1 step 03](../MILESTONE_1_project-and-version-control/03_find-your-way-around.md):
everything you change in Play Mode is discarded when you stop. So the loop is always the same — **play, tune,
read the number off the screen, stop, type it in again.** Learn it here with one harmless field, rather than
in M5 after twenty minutes of tuning you are about to lose.

## Do this

1. Select `Player` in the **Hierarchy** and press **Play**.

2. While the game is running, click into the `Constant Mover (Script)` component's **Move Speed Units Per
   Second** field and type `8`, then press Enter. The square immediately speeds up — no recompile, no restart.

3. Try `0.5`. The square crawls. Try `-3`: it travels left, because the speed multiplies a fixed
   direction vector, and a negative scalar flips it. Nothing here is special-cased for direction, which is
   why one field is enough.

4. Press **Play** to stop. Look at the field: it is back to `3`. Every number you just typed is gone. This is
   not a bug to work around — it is the guarantee that the scene on disk is the scene you saved.

5. Set the field to `3` while **stopped** (it already is — confirm it rather than change it), and save the
   scene with **Ctrl+S** / **Cmd+S**. `3` is the value the rest of this milestone's gate assumes.

## Done when (this step)
- [ ] Editing **Move Speed Units Per Second** during Play Mode visibly changes the square's speed within one
      frame, with no recompile.
- [ ] After leaving Play Mode, the field reads `3` again.
- [ ] `git status --porcelain` → prints **nothing**: this step changed no files.

## If it breaks
- **The field is greyed out during Play Mode** → you are looking at the script asset in the Project panel
  (which shows a read-only preview), not at the component on the `Player` object. Select the object in the
  Hierarchy.
- **Your tuning survived leaving Play Mode** → you were editing while stopped, not while playing. That is
  fine, and it means the value is really saved; press Ctrl+S to be sure.

---
> Nav: [← Write your first MonoBehaviour](02_first-script.md) · [Overview](00_overview.md) · [Prove the motion is frame-rate independent →](04_frame-rate-independence.md)
