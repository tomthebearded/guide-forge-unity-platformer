# M11 · Step 03 of 07 — Carry the run across scenes
> Nav: [← More scenes](02_more-scenes.md) · [Overview](00_overview.md) · [Pause →](04_pause.md)

**Before you start:** [step 02](02_more-scenes.md) finished — four scenes exist, all four are in the build
scene list, and the menu's Play button loads `Level01`.

## Why / design
Loading a scene destroys everything in the old one. Every GameObject, every component, every field —
including the coins you collected. That is usually what you want, and exactly once per project it is not.

The exception is the handful of facts that belong to the **run** rather than to the level: coins so far,
elapsed time. Unity's answer is `DontDestroyOnLoad`, which marks an object as surviving scene loads. It is a
mechanism, not a design — the design is yours, and the discipline is to keep exactly one such object and keep
it small.

> This is the **one deliberate singleton** in *Cavern Dash*, and
> [`../foundation/conventions.md`](../foundation/conventions.md) names it as the exception to "objects talk
> through events". The trade-off is real: anything can reach `GameSession.Instance` from anywhere, and if you
> let it hold gameplay logic it becomes the place where everything hides. It holds two numbers and three
> methods. Keep it that way.

It also creates itself on first use. That sounds like a trick, and it buys something concrete: you can press
Play in `Level02` while working on it, and the game runs, instead of throwing a null reference because you did
not come in through the menu.

## Do this

1. Create a new MonoBehaviour script in `Assets/_Project/Scripts` named **`GameSession`**:

   ```csharp
   // Assets/_Project/Scripts/GameSession.cs — the whole file
   using UnityEngine;

   // The one object that survives a scene load. Holds what a run accumulates across
   // levels — and nothing else, deliberately.
   public class GameSession : MonoBehaviour
   {
       private static GameSession instance;

       // Creates itself the first time anything asks, so pressing Play in any scene
       // works without coming in through the menu.
       public static GameSession Instance
       {
           get
           {
               if (instance == null)
               {
                   GameObject holder = new GameObject(nameof(GameSession));
                   instance = holder.AddComponent<GameSession>();
                   DontDestroyOnLoad(holder);
               }

               return instance;
           }
       }

       public int TotalCoins { get; private set; }
       public float ElapsedSeconds { get; private set; }

       private void Awake()
       {
           // If a scene ever contains its own copy, the survivor wins and the
           // newcomer removes itself. Two sessions would each hold half a run.
           if (instance != null && instance != this)
           {
               Destroy(gameObject);
               return;
           }

           instance = this;
           DontDestroyOnLoad(gameObject);
       }

       public void StartNewRun()
       {
           TotalCoins = 0;
           ElapsedSeconds = 0f;
       }

       public void AddCoins(int count)
       {
           TotalCoins += count;
       }

       public void AddTime(float seconds)
       {
           ElapsedSeconds += seconds;
       }
   }
   ```

2. Create a second script named **`LevelExit`** — the trigger that ends a level:

   ```csharp
   // Assets/_Project/Scripts/LevelExit.cs — the whole file
   using UnityEngine;
   using UnityEngine.SceneManagement;

   // The end of a level. Banks what this level produced into the run, then loads
   // whatever comes next.
   [RequireComponent(typeof(Collider2D))]
   public class LevelExit : MonoBehaviour
   {
       [SerializeField] private string nextSceneName = SceneRouter.SecondLevelScene;

       private void OnTriggerEnter2D(Collider2D other)
       {
           if (!other.TryGetComponent(out PlayerStats stats))
           {
               return;
           }

           GameSession.Instance.AddCoins(stats.CoinsCollected);
           SceneManager.LoadScene(nextSceneName);
       }
   }
   ```

3. Start each run clean. In `Assets/_Project/Scripts/SceneRouter.cs`, **REPLACE** the line reading
   `public void LoadFirstLevel() => SceneManager.LoadScene(FirstLevelScene);` with this:

   ```csharp
   // Assets/_Project/Scripts/SceneRouter.cs — replacing LoadFirstLevel()
   public void LoadFirstLevel()
   {
       // Pressing Play on the menu always starts a fresh run.
       GameSession.Instance.StartNewRun();
       SceneManager.LoadScene(FirstLevelScene);
   }
   ```

4. Save, let Unity compile. Open **`Level01`** and build the exit: create a **2D Object > Sprites > Square**,
   rename it **`LevelExit`**, set `Transform` **Scale** to `1, 2, 1` and `Sprite Renderer` **Color** to a
   bright green `5FD068` (cosmetic), set its **Sorting Layer** to `Level`, add a **Box Collider 2D** with
   **Is Trigger** ticked, and drag `LevelExit.cs` onto it.

5. Place it at the far end of the level — the end the player reaches last. Leave its **Next Scene Name** at
   `Level02`.

6. Open **`Level02`** and do the same, with one change: type **`Win`** into that exit's **Next Scene
   Name** field. It is a plain string rather than a constant because nothing in code loads the win scene —
   only this Inspector field does, and a constant nobody calls is dead weight.

7. Save both scenes. Open `Menu`, press **Play**, press the **Play** button, and run to the end of `Level01`:
   `Level02` loads with the player at its start. Run to the end of that: the `Win` scene appears.

8. Watch what survives. Collect two coins in `Level01` before reaching the exit, then look at the HUD in
   `Level02`: it reads `Coins: 0`. That is correct — the HUD shows *this level's* `PlayerStats`, while the
   run's total lives in the session. [Step 05](05_timer-and-best-time.md) is where the Win screen reports the
   total.

## Done when (this step)
- [ ] Running into `Level01`'s exit loads `Level02`; running into `Level02`'s exit loads `Win`.
- [ ] During Play Mode, the **DontDestroyOnLoad** section at the bottom of the Hierarchy contains a
      `GameSession` object, and it is **still there** — the same one — after a scene change.
- [ ] Exactly **one** `GameSession` exists after two scene loads, not three.
- [ ] Coins collected in `Level01` are banked: pause the Editor after the load and check `GameSession`'s
      **Total Coins** in the Inspector.
- [ ] Pressing Play directly in `Level02` (without going through the menu) works and does not throw.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(flow): carry the run between levels with level exits
```

## If it breaks
- **`NullReferenceException` on `GameSession.Instance`** → the lazy creation was left out of the property, so
  a scene without one has none. The `get` must create it.
- **Two `GameSession` objects appear** → the `Awake` guard is missing, or one scene contains a hand-placed
  copy while the property also creates one. Nothing should place it by hand.
- **The exit fires the moment the level loads** → the trigger overlaps the player's start position.
- **The exit does nothing** → its collider is not a trigger, or `Next Scene Name` is misspelled. The name must
  match the scene file exactly, and the scene must be in the build list.
- **Coins reset to zero between levels on the HUD** → correct, and explained in action 8.
- **`Level02` loads but the player falls forever** → you moved the player's start position in `Level02` above
  a gap. Levels are independent scenes; each needs its own sane start.

---
> Nav: [← More scenes](02_more-scenes.md) · [Overview](00_overview.md) · [Pause →](04_pause.md)
