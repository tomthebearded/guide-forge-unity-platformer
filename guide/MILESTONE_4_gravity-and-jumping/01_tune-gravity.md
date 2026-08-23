# M4 · Step 01 of 06 — Give gravity some weight
> Nav: — · [Overview](00_overview.md) · [Put the ground on its own layer →](02_ground-layer.md)

**Before you start:** M3's gate passed — the `Player` runs on the `Ground` strip. Play Mode is stopped.

## Why / design
A falling body in Unity accelerates at `Physics2D.gravity` × the body's `Gravity Scale`. The project default
for `Physics2D.gravity` is `(0, −9.81)` — real-world metres per second squared — and the default scale is `1`.

Realistic gravity makes a bad platformer. At 9.81 units per second squared, a character one unit tall drifts
through the air like a balloon: the jump takes most of a second, and the player spends it waiting. Nearly every
2D platformer you have enjoyed runs gravity several times higher than reality and compensates with a stronger
jump — the arc is the same shape, but it happens fast enough to feel like a decision rather than a commute.

This guide uses **`Gravity Scale = 4`**, giving an effective **39.24 units per second squared**, and pairs it
in [step 04](04_jump.md) with a jump velocity of **14 units per second**. Those two numbers together put the apex
at **2.5 units** on paper — two and a half times the player's height — and a little under that in practice, which
is why [step 05](05_measure-the-jump.md) has you measure it rather than take it on trust.

Leave `Physics2D.gravity` itself alone. Changing the project-wide constant would change every body in the
game; the per-body `Gravity Scale` is the knob meant for tuning one character, and M5 uses it again to make
falling faster than rising.

## Do this

1. Confirm the project-wide value first: open **Edit > Project Settings > Physics 2D** and read the
   **Gravity** field. It must be `X 0`, `Y -9.81`. **Leave it exactly as it is** — you are only reading it, so
   this action changes no file.

2. Select `Player` in the **Hierarchy**. In its `Rigidbody 2D` component, set **Gravity Scale** to `4`.
   Leave every other field as M3 left it (`Dynamic`, `Continuous`, `Interpolate`, Freeze Rotation Z ticked).

3. Save the scene, then press **Play** and watch the square land. Compare it with what you remember from M3:
   the fall is noticeably brisker, and it arrives with a sense of weight rather than settling.

4. Stop Play Mode. It is worth seeing the other end of the dial once, so you can recognise the symptom later:
   set **Gravity Scale** to `0.3` and press Play. The square drifts down like a feather — this is what
   "floaty" means when someone says a platformer feels floaty, and it is almost always this number.

5. Stop, set **Gravity Scale** back to `4`, and save the scene. Every value from here to M13 assumes `4`.

## Done when (this step)
- [ ] **Edit > Project Settings > Physics 2D** → **Gravity** reads `0, -9.81`, unchanged.
- [ ] `Player` → `Rigidbody 2D` → **Gravity Scale** reads `4`.
- [ ] Pressing Play → the square falls to the strip visibly faster than it did in M3 and rests at Position Y
      between `-3.00` and `-2.98`, as before.
- [ ] `git status --short` → lists `Assets/_Project/Scenes/Level01.unity` as modified and nothing else.

## Suggested commit
```
feat(player): raise gravity scale to 4 for platformer weight
```

## If it breaks
- **The square now falls through the floor** → at higher speeds a `Discrete` collision check can step straight
  past a thin collider. `Collision Detection` must be `Continuous`, as set in
  [M3 step 04](../MILESTONE_3_input-and-running/04_rigidbody-and-collider.md).
- **Nothing about the fall changed** → you edited the `Gravity Scale` on a different object, or while Play
  Mode was running (in which case it was discarded on stop). Set it with the game stopped and save.
- **The whole scene's physics changed, not just the player** → you edited **Project Settings > Physics 2D >
  Gravity** instead of the body's `Gravity Scale`. Put it back to `-9.81`.

---
> Nav: — · [Overview](00_overview.md) · [Put the ground on its own layer →](02_ground-layer.md)
