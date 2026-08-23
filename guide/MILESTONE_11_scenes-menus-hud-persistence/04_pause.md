# M11 · Step 04 of 07 — Pause
> Nav: [← Carry the run across scenes](03_game-session.md) · [Overview](00_overview.md) · [The timer and the best time →](05_timer-and-best-time.md)

**Before you start:** [step 03](03_game-session.md) finished — the level exits chain `Level01` → `Level02` →
`Win`, and the run survives the loads.

## Glossary for this step
> New here: **[time scale](../foundation/glossary.md#time-scale)** (defined in *Why / design*).

## Why / design
Pausing is two independent things, and doing only one of them is the usual bug.

**Stop the world.** `Time.timeScale = 0` freezes everything driven by scaled time: `FixedUpdate` stops being
called, `Time.deltaTime` becomes zero, animations stop. It is one assignment and it is remarkably complete.

> New concept — **time scale**: the multiplier Unity applies to the passage of game time. `1` is normal,
> `0` is frozen, `0.5` is slow motion. `Time.deltaTime` is already scaled by it; `Time.unscaledDeltaTime` is
> not, which is how a pause menu can still animate while the game is stopped.

**Stop the input.** A frozen game still receives input, so the player can queue a dash and a jump while
staring at the pause menu, and both fire the instant they resume. The fix is the reason M3 made you look at
**action maps**: disable the **`Player`** map while paused, leave **`UI`** enabled, and gameplay simply stops
listening.

That is also why the pause button itself belongs in the **`UI`** map — a pause action in the `Player` map
would disable itself and there would be no way back.

## Do this

1. Open **`InputSystem_Actions`** (double-click it in `Assets`). Select the **`UI`** action map, press **+** on
   the Actions list, and name the new action **`Pause`**. Set its **Action Type** to `Button`.

2. Give it two bindings, the same way as
   [M8 step 01](../MILESTONE_8_dash-and-wall-jump/01_add-the-dash-action.md): **Escape** on the keyboard, and
   **Start** on the gamepad (the path is `Gamepad > Start`). Press **Save Asset**.

3. Build the pause panel in **`Level01`**. Under the existing `Canvas`:
   - Right-click the `Canvas` > **UI > Panel**, rename it **`PausePanel`**. In its `Image` component set the
     **Color**'s alpha to about `180` so the game shows through dimmed.
   - Inside the panel add **UI > Text - TextMeshPro** reading `PAUSED`, centred, **Font Size** `40`.
   - Inside the panel add two **UI > Button - TextMeshPro**s: **`ResumeButton`** (`Resume`) and
     **`MenuButton`** (`Back to menu`), stacked in the middle.
   - Select `PausePanel` and **untick the checkbox beside its name** at the top of the Inspector, so it starts
     hidden.

4. Create a new MonoBehaviour script in `Assets/_Project/Scripts` named **`PauseController`**:

   ```csharp
   // Assets/_Project/Scripts/PauseController.cs — the whole file
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

5. Save, let Unity compile. Select the **`Canvas`** in `Level01`, drag `PauseController.cs` onto it, and set
   its **Pause Panel** field to the `PausePanel` object.

6. Wire the two buttons through their **On Click ()** lists, exactly as in
   [step 02](02_more-scenes.md): `ResumeButton` → **PauseController > Resume ()**, `MenuButton` →
   **PauseController > ReturnToMenu ()**. Drag the `Canvas` into each object field.

7. Repeat actions 3, 5 and 6 in **`Level02`**. (Copying the `PausePanel` object from `Level01` and pasting it
   into `Level02`'s `Canvas` saves the layout work; you still add the component and re-wire the buttons,
   because references do not survive a copy between scenes.)

8. Save both scenes and press **Play** in `Level01`. Press **Escape**: the panel appears, the enemies stop
   mid-stride, and the animation freezes. Press movement keys and dash — nothing happens. Press **Escape**
   again, or click **Resume**: everything continues from exactly where it stopped, and no queued input fires.

9. Click **Back to menu** while paused: the menu loads and **is not frozen** — its buttons respond.

## Done when (this step)
- [ ] **Escape** (or gamepad **Start**) shows the pause panel; pressing it again hides it.
- [ ] While paused: enemies, the moving platform and the player's animation are all frozen.
- [ ] While paused: movement, jump and dash do nothing — and **nothing fires on resume** either.
- [ ] The **Resume** button unpauses; the game continues from exactly where it stopped.
- [ ] **Back to menu** loads the menu and the menu's buttons work — proof that `timeScale` was restored.
- [ ] Both `Level01` and `Level02` pause.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(ui): pause the game with timeScale and a disabled Player map
```

## If it breaks
- **Escape does nothing** → the `Pause` action was added to the `Player` map instead of `UI`, or **Save
  Asset** was not pressed.
- **The menu loads frozen and unclickable** → `Time.timeScale = 1f` is missing from `ReturnToMenu`.
  `timeScale` is global and survives scene loads, which is the whole trap.
- **A dash fires the instant you unpause** → the `Player` map is not being disabled; the buffered press is
  waiting.
- **The panel is visible at level start** → it was not unticked in the Inspector in action 3.
- **The pause panel appears but the game keeps running** → `pausePanel` is set but the `Time.timeScale` line
  was left out, or the field points at the wrong object.
- **`NullReferenceException` on `playerMap`** → `FindActionMap("Player")` is misspelled; the map name is
  case-sensitive.

---
> Nav: [← Carry the run across scenes](03_game-session.md) · [Overview](00_overview.md) · [The timer and the best time →](05_timer-and-best-time.md)
