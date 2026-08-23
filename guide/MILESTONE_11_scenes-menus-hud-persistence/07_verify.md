# M11 · Verify — Scenes, menus, HUD & persistence
> Nav: [← Game over](06_game-over.md) · [Overview](00_overview.md) · [Options: volume, display & key rebinding →](../MILESTONE_12_options-and-rebinding/00_overview.md)

## Done-when gate (the real test — check every box by hand)

Observed in **Play Mode in the Editor**, starting from the **`Menu`** scene.

- [ ] **The whole loop runs.** `Menu` → **Play** → `Level01` → its exit → `Level02` → its exit → `Win`, with
      no errors at any transition.
- [ ] **The HUD is live in both levels.** Coins and lives update the instant they change, and the time counts
      up in `mm:ss.hh`.
- [ ] **The run accumulates across the load.** The timer keeps counting into `Level02` rather than restarting,
      and the win screen's coin total is the sum of both levels.
- [ ] **Exactly one `GameSession`** sits under **DontDestroyOnLoad** in the Hierarchy after two scene loads.
- [ ] **Pause stops everything.** **Escape** (or gamepad **Start**) freezes enemies, platform and animation;
      movement, jump and dash do nothing; **nothing fires on resume**; the timer does not advance while paused.
- [ ] **Back to menu from pause** loads a menu that is **not frozen** — its buttons respond.
- [ ] **The best time persists.** Finish a run → the win screen shows the time and an equal best time. Finish
      a faster run → the best time improves. Finish a slower one → it does not. Leave Play Mode, come back,
      finish another → the old best time is still there.
- [ ] **Clearing it resets it.** **Edit > Clear All PlayerPrefs** followed by a run makes that run the best.
- [ ] **The run can be lost.** Losing lives one and two respawns at the last checkpoint; losing the third
      freezes the game, shows `GAME OVER`, does **not** respawn, and ignores Escape. **Back to menu** works,
      and the next run's pause key works again.
- [ ] **Nothing earlier regressed.** The moveset, coins, enemies, checkpoints, camera, parallax and audio all
      behave in both levels.
- [ ] **The project is clean.** No red Console entries; after committing, `git status --porcelain` prints
      nothing.

## Files after this milestone (the checkpoint)

_This checkpoint renders the complete contents of every guide-authored file created or modified in this
milestone (listed below). Pre-existing files this milestone only added to are shown as their added region
under "Pre-existing files modified", not reproduced whole. Files not listed were not touched this milestone._

### `Assets/_Project/Scripts/HudView.cs`
```csharp
using TMPro;
using UnityEngine;

// Draws the run's numbers. Subscribes to what already happens; owns nothing.
public class HudView : MonoBehaviour
{
    [SerializeField] private TMP_Text coinsLabel;
    [SerializeField] private TMP_Text livesLabel;

    [SerializeField] private PlayerStats stats;
    [SerializeField] private PlayerHealth health;

    private void OnEnable()
    {
        stats.CoinsChanged += ShowCoins;
        health.LivesChanged += ShowLives;

        // Draw the starting values too: the events only fire on a change, and
        // the player may not touch anything for a while.
        ShowCoins(stats.CoinsCollected);
        ShowLives(health.LivesRemaining);
    }

    private void OnDisable()
    {
        stats.CoinsChanged -= ShowCoins;
        health.LivesChanged -= ShowLives;
    }

    private void ShowCoins(int coinsCollected)
    {
        coinsLabel.text = $"Coins: {coinsCollected}";
    }

    private void ShowLives(int livesRemaining)
    {
        livesLabel.text = $"Lives: {livesRemaining}";
    }
}
```

### `Assets/_Project/Scripts/SceneRouter.cs`
```csharp
using UnityEngine;
using UnityEngine.SceneManagement;

// Every scene change in the game goes through here. Buttons call these methods
// directly from the Inspector, which is why they are public and take no arguments.
public class SceneRouter : MonoBehaviour
{
    // Load-bearing: these strings must match the scene file names and the
    // entries in the build scene list.
    public const string MenuScene = "Menu";
    public const string FirstLevelScene = "Level01";
    public const string SecondLevelScene = "Level02";

    public void LoadMenu() => SceneManager.LoadScene(MenuScene);

    public void LoadFirstLevel()
    {
        // Pressing Play on the menu always starts a fresh run.
        GameSession.Instance.StartNewRun();
        SceneManager.LoadScene(FirstLevelScene);
    }

    public void QuitGame()
    {
        // Application.Quit does nothing in the Editor, so say so out loud —
        // otherwise the button looks broken while you are testing.
        Debug.Log("quit requested");
        Application.Quit();
    }
}
```

### `Assets/_Project/Scripts/GameSession.cs`
```csharp
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

### `Assets/_Project/Scripts/LevelExit.cs`
```csharp
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

### `Assets/_Project/Scripts/PauseController.cs`
```csharp
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Freezes the game and stops gameplay input while the pause panel is up.
public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private InputAction pauseAction;
    private InputActionMap playerMap;

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        // The pause action lives in the UI map, which stays enabled while paused.
        pauseAction = InputSystem.actions.FindAction("UI/Pause");
        playerMap = InputSystem.actions.FindActionMap("Player");
    }

    private void Update()
    {
        if (pauseAction.WasPressedThisFrame())
        {
            SetPaused(!IsPaused);
        }
    }

    // Public so the Resume button can call it from the Inspector.
    public void Resume()
    {
        SetPaused(false);
    }

    public void ReturnToMenu()
    {
        // Always restore time before leaving: timeScale is global and survives
        // a scene load, so a menu loaded at zero would be frozen solid.
        Time.timeScale = 1f;
        playerMap.Enable();
        SceneManager.LoadScene(SceneRouter.MenuScene);
    }

    private void SetPaused(bool paused)
    {
        IsPaused = paused;
        pausePanel.SetActive(paused);

        // 0 freezes everything on the scaled clock, including FixedUpdate.
        Time.timeScale = paused ? 0f : 1f;

        if (paused)
        {
            playerMap.Disable();
        }
        else
        {
            playerMap.Enable();
        }
    }

    private void OnDisable()
    {
        // Leaving this scene while paused must not leave the game frozen.
        Time.timeScale = 1f;
    }
}
```

### `Assets/_Project/Scripts/LevelTimer.cs`
```csharp
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

### `Assets/_Project/Scripts/WinScreen.cs`
```csharp
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

### `Assets/_Project/Scripts/GameOverController.cs`
```csharp
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// Ends the run: freezes the game, stops all gameplay input, and offers the menu.
public class GameOverController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private PlayerHealth health;

    private InputAction pauseAction;
    private InputActionMap playerMap;

    private void Awake()
    {
        pauseAction = InputSystem.actions.FindAction("UI/Pause");
        playerMap = InputSystem.actions.FindActionMap("Player");
    }

    private void OnEnable()
    {
        health.RunEnded += ShowGameOver;
    }

    private void OnDisable()
    {
        health.RunEnded -= ShowGameOver;

        // Never leave the game frozen or the pause key dead for the next scene.
        Time.timeScale = 1f;
        pauseAction.Enable();
    }

    // Public so the panel's button can call it from the Inspector.
    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        playerMap.Enable();
        pauseAction.Enable();
        SceneManager.LoadScene(SceneRouter.MenuScene);
    }

    private void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        playerMap.Disable();

        // Stop Escape opening the pause panel on top of this one.
        pauseAction.Disable();
    }
}
```

### `Assets/_Project/Scripts/PlayerHealth.cs`
```csharp
using System;
using UnityEngine;

// Owns the run's lives and the invulnerability window. Anything that hurts the
// player calls TakeDamage; anything that cares about the result subscribes.
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int startingLives = 3;
    [SerializeField] private float invulnerabilitySeconds = 1f;

    public int LivesRemaining { get; private set; }

    // Raised whenever the count changes. The HUD subscribes to this.
    public event Action<int> LivesChanged;

    // Raised when a life is lost but the run continues. PlayerRespawn listens.
    public event Action Died;

    // Raised when the last life is gone. The run is finished; nothing respawns.
    public event Action RunEnded;

    private float invulnerableUntilTimeSeconds = float.NegativeInfinity;

    public bool IsInvulnerable => Time.time < invulnerableUntilTimeSeconds;

    private void Awake()
    {
        LivesRemaining = startingLives;
    }

    public void TakeDamage()
    {
        if (IsInvulnerable)
        {
            return;
        }

        LivesRemaining--;
        invulnerableUntilTimeSeconds = Time.time + invulnerabilitySeconds;
        LivesChanged?.Invoke(LivesRemaining);

        if (LivesRemaining <= 0)
        {
            RunEnded?.Invoke();
            return;
        }

        Debug.Log($"lives = {LivesRemaining}");
        Died?.Invoke();
    }

    // For hazards no amount of invulnerability should survive — falling out of the level.
    public void KillIgnoringInvulnerability()
    {
        invulnerableUntilTimeSeconds = float.NegativeInfinity;
        TakeDamage();
    }
}
```

### Editor checkpoint

| Scene | GameObject | Component | Field | Exact value |
|---|---|---|---|---|
| all | `Canvas` | `Canvas Scaler` | UI Scale Mode / Reference Resolution / Match | `Scale With Screen Size` / `640 × 360` / `0.5` |
| `Level01`, `Level02` | `Canvas` | `Hud View (Script)` | Coins Label / Lives Label / Stats / Health | `CoinsLabel` / `LivesLabel` / `Player` / `Player` |
| `Level01`, `Level02` | `Canvas` | `Level Timer (Script)` | Time Label | `TimeLabel` |
| `Level01`, `Level02` | `Canvas` | `Pause Controller (Script)` | Pause Panel | `PausePanel` (starts disabled) |
| `Level01`, `Level02` | `Canvas` | `Game Over Controller (Script)` | Game Over Panel / Health | `GameOverPanel` (starts disabled) / `Player` |
| `Level01` | `LevelExit` | `Level Exit (Script)` | Next Scene Name | `Level02` |
| `Level02` | `LevelExit` | `Level Exit (Script)` | Next Scene Name | `Win` |
| `Level01`, `Level02` | `LevelExit` | `Box Collider 2D` | Is Trigger | ticked |
| `Menu` | `Router` | `Scene Router (Script)` | — | wired to `PlayButton` → `LoadFirstLevel ()`, `QuitButton` → `QuitGame ()` |
| `Win` | `Canvas` | `Win Screen (Script)` | Time Label / Best Time Label / Coins Label | the three labels |
| `Win` | `Router` | `Scene Router (Script)` | — | wired to `MenuButton` → `LoadMenu ()` |
| `InputSystem_Actions` | `UI` map | new action | `Pause`, type `Button`, bound to `<Keyboard>/escape` and `<Gamepad>/start` |
| File > Build Profiles | Scene List | order | `Menu` (0), `Level01` (1), `Level02` (2), `Win` (3) — all ticked |

### Pre-existing files modified
- `Assets/InputSystem_Actions.inputactions` — the `Pause` action added to the `UI` map in
  [step 04](04_pause.md).
- `Assets/_Project/Scenes/Level01.unity` — the `Canvas`, HUD labels, both panels, the `LevelExit`, and four
  new components. Edited through the Editor.
- `ProjectSettings/EditorBuildSettings.asset` — the four scenes registered in [step 02](02_more-scenes.md).
- `Assets/TextMesh Pro/` — the TMP essential resources imported in [step 01](01_hud.md).

### Created this milestone (scenes)
- `Assets/_Project/Scenes/Level02.unity` — duplicated from `Level01` and rearranged.
- `Assets/_Project/Scenes/Menu.unity`, `Assets/_Project/Scenes/Win.unity`.

### Unchanged this milestone
- `PlayerMotor.cs`, `PlayerInputReader.cs`, `PlayerMovementState.cs`, `PlayerAnimationDriver.cs`,
  `PlayerAudio.cs`, `ParallaxLayer.cs`, `MovingPlatform.cs`, `PlatformRiderCarrier.cs`, `PlayerStats.cs`,
  `Collectible.cs`, `EnemyPatrol.cs`, `EnemyContact.cs`, `PlayerRespawn.cs`, `Checkpoint.cs`, `KillZone.cs` —
  unchanged since M10 and earlier.
- `Packages/manifest.json` — unchanged since M10.

## Troubleshooting

| Symptom | Likely cause → fix |
|---|---|
| `Scene 'X' couldn't be loaded` | The scene is missing from **File > Build Profiles > Scene List**, or unticked. |
| The menu loads frozen | `Time.timeScale` was not restored before the scene load. It is global and survives loads. |
| A queued dash fires on resume | The `Player` action map is not being disabled while paused. |
| Two `GameSession` objects | A hand-placed copy plus the lazily created one. Nothing should place it by hand. |
| The timer restarts each level | `LevelTimer` accumulates locally instead of into `GameSession`. |
| The best time is always the current run | `PlayerPrefs.GetInt` defaults to `0` instead of `int.MaxValue`. |
| The player respawns after the third death | The `return` is missing from the `RunEnded` branch in `TakeDamage`. |
| A button does nothing | The **On Click ()** entry has no object, or the scene lost its **EventSystem**. |
| `Level02`'s HUD shows nothing | Its `HudView` fields still point at `Level01`'s objects; references do not survive a scene duplicate cleanly if objects were replaced. |

## Handoff
- **You now have:** the complete game loop. A `Menu` scene leads into `Level01`, a `LevelExit` chains it to
  `Level02` and then to a `Win` screen that reports the run's time, its best time — persisted through
  `PlayerPrefs` under `cavernDash.run.bestTimeMs` — and the coins collected across both levels. A HUD shows
  coins, lives and a running timer; **Escape** pauses by freezing `timeScale` and disabling the `Player`
  action map; the third death ends the run with a game-over screen. One `GameSession` survives scene loads and
  holds the run's totals.
- **Open / deferred:** the player cannot change anything about how the game sounds, looks or controls — the
  mixer parameters exposed in M10 still have nothing attached to them, and the key bindings are whatever the
  asset says. And the game only exists inside the Editor.
- **Next:** **[M12 — Options: volume, display & key rebinding](../MILESTONE_12_options-and-rebinding/00_overview.md)** —
  three volume sliders, a fullscreen toggle, and interactive rebinding that survives a relaunch.

---
> Nav: [← Game over](06_game-over.md) · [Overview](00_overview.md) · [Options: volume, display & key rebinding →](../MILESTONE_12_options-and-rebinding/00_overview.md)
