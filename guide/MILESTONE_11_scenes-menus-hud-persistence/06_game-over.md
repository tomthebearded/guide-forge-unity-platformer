# M11 · Step 06 of 07 — Game over
> Nav: [← The timer and the best time](05_timer-and-best-time.md) · [Overview](00_overview.md) · [Verify →](07_verify.md)

**Before you start:** [step 05](05_timer-and-best-time.md) finished — the timer runs, the win screen reports
the run, and the best time persists.

## Why / design
Since M9, running out of lives has printed `run over — lives reset` and quietly given you three more. That was
a deliberate placeholder: a game-over screen needs a menu to return to, and the menu did not exist until
[step 02](02_more-scenes.md). Now it does, so the placeholder goes.

`PlayerHealth` gains one more announcement, `RunEnded`, and — this is the part worth noticing — it stops
raising `Died` when the run is over. Right now both fire, so the last death both ends the run *and* respawns
you at a checkpoint, which is two outcomes for one event. Splitting them is a one-line change that removes a
whole class of confusion.

Game over reuses the pause machinery: freeze time, disable gameplay input, show a panel. It also disables the
**`Pause` action** itself, so the pause panel cannot appear on top of the game-over panel — two overlapping
modal screens is exactly the sort of thing that ships.

## Do this

1. In `Assets/_Project/Scripts/PlayerHealth.cs`, **REPLACE** the `public event Action Died;` line together
   with the three comment lines above it (they begin `// Raised on every life lost`), with this pair — the
   corrected comment, and the new event that takes the last life off `Died`'s hands:

   ```csharp
   // Assets/_Project/Scripts/PlayerHealth.cs — replacing the Died comment and its event
   // Raised when a life is lost and the run continues. PlayerRespawn listens.
   public event Action Died;

   // Raised when the last life is gone. The run is finished; nothing respawns.
   public event Action RunEnded;
   ```

2. **REPLACE** the whole `if (LivesRemaining <= 0) { … } else { … }` block inside `TakeDamage`, together with
   the `Died?.Invoke();` line that follows it, with this. The placeholder reset disappears with it:

   ```csharp
   // Assets/_Project/Scripts/PlayerHealth.cs — replacing the lives-check block and the Died call
   if (LivesRemaining <= 0)
   {
       RunEnded?.Invoke();
       return;
   }

   Debug.Log($"lives = {LivesRemaining}");
   Died?.Invoke();
   ```

3. Build the panel in **`Level01`**. Under the `Canvas`, duplicate the `PausePanel` from
   [step 04](04_pause.md) (**Ctrl+D** / **Cmd+D**), rename the copy **`GameOverPanel`**, change its title text
   to `GAME OVER`, delete its `ResumeButton` (there is nothing to resume), and leave its remaining button
   reading `Back to menu`. Untick the panel's checkbox so it starts hidden.

4. Create a new MonoBehaviour script in `Assets/_Project/Scripts` named **`GameOverController`**:

   ```csharp
   // Assets/_Project/Scripts/GameOverController.cs — the whole file
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

5. Save, let Unity compile. Select the `Canvas` in `Level01`, drag `GameOverController.cs` onto it, and set
   **Game Over Panel** to `GameOverPanel` and **Health** to the `Player`.

6. Wire the panel's button: its **On Click ()** → **GameOverController > ReturnToMenu ()**, with the `Canvas`
   in the object field.

7. Repeat actions 3, 5 and 6 in **`Level02`**.

8. Save both scenes, open `Menu`, press **Play**, and lose three lives — walking into an enemy repeatedly is
   quickest, waiting a second between each. On the third, the game freezes, the panel appears, and neither
   movement nor **Escape** does anything. Click **Back to menu**: the menu loads and works.

9. Check the split worked. Lose **two** lives and confirm you still respawn at the last checkpoint each time —
   `Died` still fires for those. Only the third ends the run.

## Done when (this step)
- [ ] Losing the first and second lives respawns the player at the last checkpoint, as before.
- [ ] Losing the third freezes the game and shows `GAME OVER` — and does **not** respawn the player.
- [ ] While the panel is up, movement, jump, dash and **Escape** all do nothing.
- [ ] **Back to menu** loads the menu, unfrozen and clickable, with the pause key working again in the next
      run.
- [ ] The Console no longer prints `run over — lives reset` anywhere.
- [ ] Both `Level01` and `Level02` end the run this way.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(flow): end the run with a game over screen
```

## If it breaks
- **The player respawns *and* the panel appears** → the `return` is missing from the `RunEnded` branch, so
  `Died` fires as well.
- **The panel appears but the game keeps running** → `Time.timeScale = 0f` was left out of `ShowGameOver`.
- **Escape opens the pause panel over the game-over panel** → `pauseAction.Disable()` is missing.
- **The next run starts frozen, or Escape does nothing in it** → `OnDisable` is not restoring `timeScale` and
  re-enabling the action. Both are global and survive a scene load.
- **`NullReferenceException` on `health`** → the **Health** field is empty, or points at a prefab rather than
  at the `Player` in this scene.
- **Nothing happens on the third life** → `LivesRemaining` is not reaching zero because the M9 placeholder
  reset is still in the file. Action 2 replaces it.

---
> Nav: [← The timer and the best time](05_timer-and-best-time.md) · [Overview](00_overview.md) · [Verify →](07_verify.md)
