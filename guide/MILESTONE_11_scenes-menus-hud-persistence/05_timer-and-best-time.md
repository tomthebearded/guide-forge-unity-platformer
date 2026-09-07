# M11 · Step 05 of 07 — The timer and the best time
> Nav: [← Pause](04_pause.md) · [Overview](00_overview.md) · [Game over →](06_game-over.md)

**Before you start:** [step 04](04_pause.md) finished — both levels pause and resume cleanly.

## Glossary for this step
> New here: **[PlayerPrefs](../foundation/glossary.md#playerprefs)** (defined in *Why / design*).

## Why / design
A time to beat is what turns "I finished it" into "I can finish it faster", and it costs one number.

The timer accumulates **scaled** time — `Time.deltaTime`, not `Time.unscaledDeltaTime` — so pausing pauses the
clock. That is a design decision rather than an accident: a timer that runs while you read the pause menu
would make pausing a punishment.

The best time survives the game closing, and that needs storage outside the process.

> New concept — **`PlayerPrefs`**: Unity's small key-value store, saved per user per game, in a
> platform-specific place (the registry on Windows, a plist on macOS). It handles `int`, `float` and `string`,
> nothing else. It is right for a handful of settings and scores and wrong for a save game: no schema, no
> versioning, no structure.
> Reference: <https://docs.unity3d.com/6000.3/Documentation/ScriptReference/PlayerPrefs.html>

The time is stored as an **`int` of milliseconds** rather than a `float` of seconds — integers compare exactly,
which is what you want for "is this a record?", and formatting is trivial arithmetic.

The key is **`cavernDash.run.bestTimeMs`**, and it is load-bearing: change the string and every existing best
time becomes unreachable.

## Do this

1. Create a new MonoBehaviour script in `Assets/_Project/Scripts` named **`LevelTimer`**:

   ```csharp
   // Assets/_Project/Scripts/LevelTimer.cs — the whole file
   using TMPro;
   using UnityEngine;

   // Adds this level's elapsed time to the run, and shows the running total.
   public class LevelTimer : MonoBehaviour
   {
       [SerializeField] private TMP_Text timeLabel;

       private void Update()
       {
           // Time.deltaTime is scaled, so a paused game does not accumulate time.
           GameSession.Instance.AddTime(Time.deltaTime);

           timeLabel.text = FormatTime(Mathf.RoundToInt(GameSession.Instance.ElapsedSeconds * 1000f));
       }

       // Shared with the win screen: one place decides what a time looks like.
       public static string FormatTime(int milliseconds)
       {
           int totalSeconds = milliseconds / 1000;
           int minutes = totalSeconds / 60;
           int seconds = totalSeconds % 60;
           int hundredths = milliseconds % 1000 / 10;

           // "00" pads to two digits: 7 becomes "07".
           return $"{minutes:00}:{seconds:00}.{hundredths:00}";
       }
   }
   ```

2. In **`Level01`**, select the `Canvas`, drag `LevelTimer.cs` onto it, and set its **Time Label** field to
   the `TimeLabel` object you created in [step 01](01_hud.md). Do the same in **`Level02`**.

3. Open the **`Win`** scene. Add one more label under its `Canvas`: **UI > Text - TextMeshPro**, renamed
   **`CoinsLabel`**, centred below the others. You should now have `TitleLabel`, `TimeLabel`, `BestTimeLabel`,
   `CoinsLabel` and `MenuButton`.

4. Create a new MonoBehaviour script named **`WinScreen`**:

   ```csharp
   // Assets/_Project/Scripts/WinScreen.cs — the whole file
   using TMPro;
   using UnityEngine;

   // Reports the finished run and keeps the best time. Runs once, on Start.
   public class WinScreen : MonoBehaviour
   {
       // Load-bearing: change this string and every stored best time is orphaned.
       public const string BestTimeKey = "cavernDash.run.bestTimeMs";

       [SerializeField] private TMP_Text timeLabel;
       [SerializeField] private TMP_Text bestTimeLabel;
       [SerializeField] private TMP_Text coinsLabel;

       private void Start()
       {
           int runMilliseconds = Mathf.RoundToInt(GameSession.Instance.ElapsedSeconds * 1000f);

           // The second argument is the default when the key has never been written:
           // the largest possible int, so the first run always beats it.
           int bestMilliseconds = PlayerPrefs.GetInt(BestTimeKey, int.MaxValue);

           if (runMilliseconds < bestMilliseconds)
           {
               bestMilliseconds = runMilliseconds;
               PlayerPrefs.SetInt(BestTimeKey, bestMilliseconds);

               // Save() writes to disk now. Unity also writes on a clean quit, but a
               // crash between the two would lose the record.
               PlayerPrefs.Save();
           }

           timeLabel.text = $"Time  {LevelTimer.FormatTime(runMilliseconds)}";
           bestTimeLabel.text = $"Best  {LevelTimer.FormatTime(bestMilliseconds)}";
           coinsLabel.text = $"Coins  {GameSession.Instance.TotalCoins}";
       }
   }
   ```

5. Save, let Unity compile. In the `Win` scene, select the `Canvas`, drag `WinScreen.cs` onto it, and fill its
   three fields with `TimeLabel`, `BestTimeLabel` and `CoinsLabel`.

6. The `Win` scene's button needs somewhere to point. Create an **empty GameObject** named **`Router`**, drag
   `SceneRouter.cs` onto it, then wire `MenuButton`'s **On Click ()** to **SceneRouter > LoadMenu ()** with
   the `Router` in the object field.

7. Save every scene. Open `Menu`, press **Play**, and play the whole game through: `Level01` → `Level02` →
   `Win`. The win screen shows a time like `Time  01:23.40`, a best time equal to it on the first run, and the
   total coins from both levels.

8. Play it again and finish faster. The best time now shows the *better* of the two. Finish slower on a third
   run: the best time stays where it was.

9. **Prove it persists.** Stop Play Mode entirely, then press Play again and finish another run — the best
   time from before is still there. (`PlayerPrefs` writes to disk, so a built game would survive a real
   quit-and-relaunch the same way; leaving Play Mode is the Editor's equivalent.)

10. Prove the key is what holds it. Stop the game, open **Edit > Clear All PlayerPrefs**, then play a run: the
    best time is whatever you just scored, because the stored value is gone.

## Done when (this step)
- [ ] The HUD's time label counts up while playing, in `mm:ss.hh`.
- [ ] The timer **stops** while the game is paused and continues from the same value on resume.
- [ ] The time keeps counting across the load from `Level01` into `Level02` rather than restarting.
- [ ] The win screen shows the run's time, the best time, and the coins collected across **both** levels.
- [ ] A faster run replaces the best time; a slower run does not.
- [ ] Leaving Play Mode and returning keeps the best time.
- [ ] **Edit > Clear All PlayerPrefs** resets it.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(flow): add a run timer and a persisted best time
```

## If it breaks
- **The timer runs while paused** → `Time.unscaledDeltaTime` was used instead of `Time.deltaTime`.
- **The time restarts at each level** → `LevelTimer` is accumulating into a local field rather than into
  `GameSession`, which is the object that survives the load.
- **The best time is always the current run** → `PlayerPrefs.GetInt` is being called with a default of `0`
  rather than `int.MaxValue`, so nothing ever beats it.
- **The best time is `00:00.00`** → the same, from the other direction: a stored `0` from an early experiment.
  Clear all PlayerPrefs and play a run.
- **`NullReferenceException` in `WinScreen.Start`** → one of the three label fields is empty, or the scene has
  no `GameSession` — which cannot happen if the property creates one, so check the script matches
  [step 03](03_game-session.md).
- **The time is enormous** → `AddTime` is being called from more than one place; only `LevelTimer` should call
  it, and only one `LevelTimer` should exist per scene.

---
> Nav: [← Pause](04_pause.md) · [Overview](00_overview.md) · [Game over →](06_game-over.md)
