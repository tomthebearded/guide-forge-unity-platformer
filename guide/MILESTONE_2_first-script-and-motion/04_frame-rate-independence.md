# M2 · Step 04 of 05 — Prove the motion is frame-rate independent
> Nav: [← Tune it while it runs](03_tune-in-inspector.md) · [Overview](00_overview.md) · [Verify →](05_verify.md)

**Before you start:** [step 03](03_tune-in-inspector.md) finished — `Move Speed Units Per Second` is `3`, the
scene is saved, and `git status --porcelain` is silent.

## Glossary for this step
> New here: **[VSync](../foundation/glossary.md#vsync)** (defined in *Do this*, action 1).

## Why / design
[Step 02](02_first-script.md) *claimed* that multiplying by `Time.deltaTime` makes motion independent of the
frame rate. A claim you have not watched fail is a claim you do not really hold, and this one is load-bearing
for every physics value in the rest of the guide — so you are going to force the game to run at a quarter of
its normal speed and check that the square still covers the same ground per second.

Making the number visible is the other half. Nothing you can see on screen tells you whether the square moved
3.0 or 3.4 units, so the script prints the one figure that settles it: **distance divided by elapsed time**,
which is the speed actually achieved. If delta time is doing its job, that figure reads `3.0` at any frame
rate; if it were missing, it would read wildly different numbers at 60 and 15 frames per second.

## Do this

1. Open **Edit > Project Settings > Quality**. In the **Rendering** section, set **VSync Count** to
   **Don't Sync**.

   > New concept — **VSync**: locking the game's frame rate to the monitor's refresh rate to avoid tearing.
   > Unity's `Application.targetFrameRate` — which you are about to use — is **ignored while VSync is on**:
   > the documented rule is that if `QualitySettings.vSyncCount != 0`, `targetFrameRate` has no effect.
   > Turning VSync off is what gives the next action any power at all.
   > Reference: <https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Application-targetFrameRate.html>

   This edits `ProjectSettings/QualitySettings.asset`, which is a tracked file — it is part of this step's
   commit.

2. In `Assets/_Project/Scripts/ConstantMover.cs`, **ADD** two fields directly below the existing
   `moveSpeedUnitsPerSecond` line:

   ```csharp
   // Assets/_Project/Scripts/ConstantMover.cs — below the moveSpeedUnitsPerSecond field
   [Header("Frame-rate test")]
   [SerializeField] private int targetFrameRateForTesting = 60;

   private float nextReportTimeSeconds = 1f;   // first report one second in, so the division below is safe
   ```

   `[Header("…")]` draws a bold label above the following fields in the Inspector. It is cosmetic, and it is
   how every tunable block in this guide from M5 on is kept readable.

3. **ADD** an `Awake` method directly above the existing `Update` method. `Awake` runs once, when the object
   comes to life — before the first frame:

   ```csharp
   // Assets/_Project/Scripts/ConstantMover.cs — above Update()
   private void Awake()
   {
       // Ask the platform for a specific frame rate. Only has an effect with VSync off (action 1).
       Application.targetFrameRate = targetFrameRateForTesting;
   }
   ```

4. **ADD** the reporting block inside `Update`, immediately after the line that changes `transform.position`:

   ```csharp
   // Assets/_Project/Scripts/ConstantMover.cs — inside Update(), after the transform.position line
   if (Time.time >= nextReportTimeSeconds)
   {
       // Time.time is seconds since Play started; x / t is the speed actually achieved.
       float achievedSpeedUnitsPerSecond = transform.position.x / Time.time;
       // Debug.Log writes a line to the Console panel.
       Debug.Log($"t={Time.time:F2}s  x={transform.position.x:F2}  x/t={achievedSpeedUnitsPerSecond:F1}  frame={Time.deltaTime * 1000f:F0} ms");
       nextReportTimeSeconds += 1f;
   }
   ```

5. Save the file, return to Unity, and wait for the compile spinner to clear. Select `Player` and confirm the
   Inspector now shows a **Frame-rate test** header with **Target Frame Rate For Testing** = `60`.

6. Press **Play** and watch the **Console**. Three lines appear, one per second. Read the **third** one: `x/t`
   reads `3.0` and `frame` reads roughly `17` ms.

   Read the **third** line rather than the first because of the `frame` column, not the `x/t` one:
   `Application.targetFrameRate` takes a moment to take hold, and the startup frames are long. `x/t` needs no
   settling at all — the position accumulates `speed × Time.deltaTime` and `Time.time` accumulates those same
   `Time.deltaTime` values, so their ratio *is* the speed you set, on every line. A line that reads anything
   else means the multiplication is missing. (The report starts at one second so the very first, long
   interval does not dominate the `frame` reading.)

7. Stop Play Mode. Set **Target Frame Rate For Testing** to `15` **while stopped**, so the value is saved.
   Press **Play** again and read the third Console line: `frame` now reads roughly `67` ms — four times
   longer, a quarter of the frames — and **`x/t` still reads `3.0`**.

8. Stop, set **Target Frame Rate For Testing** back to `60`, save the scene, and commit.

## Done when (this step)
- [ ] **Edit > Project Settings > Quality** shows **VSync Count** = **Don't Sync**.
- [ ] With **Target Frame Rate For Testing** = `60`, the third Console line reads `x/t=3.0` and `frame≈17 ms`.
- [ ] With **Target Frame Rate For Testing** = `15`, the third Console line reads `x/t=3.0` and `frame≈67 ms`.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(player): report achieved speed and pin the test frame rate
```

## If it breaks
- **`frame` stays around 17 ms no matter what you set** → VSync is still on, so `targetFrameRate` is ignored.
  Re-check action 1; the setting is per **quality level**, so make sure you changed the level currently
  selected (the one with the tick in Project Settings > Quality).
- **`x/t` reads something like `1.9` at 15 fps and `2.9` at 60** → your `Update` is missing the
  `Time.deltaTime` factor; compare the position line against
  [step 02](02_first-script.md).
- **The Console prints nothing** → the Console's filter buttons at its top right have **Log** messages
  switched off, or **Collapse** is on and hiding repeats. Both are toggles in the Console's toolbar.
- **`git status` also lists `ProjectSettings/QualitySettings.asset`** → correct and expected; that is action
  1, and it belongs in this commit.

---
> Nav: [← Tune it while it runs](03_tune-in-inspector.md) · [Overview](00_overview.md) · [Verify →](05_verify.md)
