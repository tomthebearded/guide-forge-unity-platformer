# M11 · Step 02 of 07 — More scenes
> Nav: [← The HUD](01_hud.md) · [Overview](00_overview.md) · [Carry the run across scenes →](03_game-session.md)

**Before you start:** [step 01](01_hud.md) finished — the HUD shows coins and lives.

## Glossary for this step
> New here: **[build scene list](../foundation/glossary.md#build-scene-list)** (defined in *Why / design*).

## Why / design
Four scenes: **`Menu`**, **`Level01`**, **`Level02`**, **`Win`**. The names are **load-bearing** — code loads
them by string, and [`../foundation/conventions.md`](../foundation/conventions.md) records them.

One rule catches everybody exactly once: **a scene that is not in the build scene list cannot be loaded at
runtime**, and the failure looks nothing like the cause. In the Editor you get an error saying the scene is
not in the build settings; in a built game you get a black screen.

> New concept — **build scene list**: the ordered list of scenes included in a build, in **File > Build
> Profiles**. Loading by name only works for scenes in this list, and the first entry is what a built game
> opens with.

Level02 is not a new level design problem: duplicate Level01, rearrange it, and you inherit every setting —
the camera, the layers, the HUD, the kill zone — for free. That is what a scene *is*, and duplicating one is
how every real project makes its second level.

## Do this

1. **Make Level02.** In the **Project** panel, select `Assets/_Project/Scenes/Level01.unity` and press
   **Ctrl+D** / **Cmd+D**. Rename the copy to **`Level02`** — exactly that.

2. Open `Level02` (double-click it) and change the level so the two are not identical: move some tiles with
   the brush and eraser, reposition the coins and enemies, move the checkpoints. Ten minutes is plenty. Keep
   the `Player`, `Grid`, `Canvas`, `FollowCamera`, `KillZone`, backgrounds and `Music` exactly where they are
   — they are what makes it work.

   Do not forget the confiner: if you painted beyond the old bounds, reshape the `Grid`'s
   `Polygon Collider 2D` to cover the new level.

3. **Make the Menu scene.** **File > New Scene**, choose the **Basic 2D** template, and press **Create**. Save
   it immediately as `Assets/_Project/Scenes/Menu.unity` (**Ctrl+S** / **Cmd+S**).

4. In `Menu`, build a title and two buttons:
   - **UI > Text - TextMeshPro** → rename it `TitleLabel`, set its text to `CAVERN DASH`, **Font Size** `48`,
     alignment centred, and anchor it to the top-centre with **Pos Y** `-80`.
   - **UI > Button - TextMeshPro** → rename it `PlayButton`, centre it, and set its child `Text (TMP)` to
     `Play`.
   - Duplicate the button → rename it `QuitButton`, **Pos Y** `-60` below the first, text `Quit`.
   - Set the `Canvas Scaler` exactly as in [step 01](01_hud.md): `Scale With Screen Size`, `640` × `360`,
     Match `0.5`.

5. **Make the Win scene** the same way: **File > New Scene**, Basic 2D, save as
   `Assets/_Project/Scenes/Win.unity`, and add a centred `TitleLabel` reading `YOU MADE IT`, a `TimeLabel` and
   a `BestTimeLabel` under it (placeholders for [step 05](05_timer-and-best-time.md)), and a
   `MenuButton` reading `Back to menu`. Same `Canvas Scaler` settings.

6. **Register all four scenes.** Open **File > Build Profiles**. In the **Scene List** section press
   **Add Open Scenes** (or drag the scene assets in from the Project panel) until the list holds, in this
   exact order:

   | Index | Scene |
   |---|---|
   | 0 | `Menu` |
   | 1 | `Level01` |
   | 2 | `Level02` |
   | 3 | `Win` |

   Order matters for exactly one reason: **index 0 is what a built game starts with**. Everything else in this
   guide loads by name.

7. Create a new MonoBehaviour script in `Assets/_Project/Scripts` named **`SceneRouter`**. It is the one place
   that knows the scene names, so a rename is one edit rather than a search:

   ```csharp
   // Assets/_Project/Scripts/SceneRouter.cs — the whole file
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
       public void LoadFirstLevel() => SceneManager.LoadScene(FirstLevelScene);

       public void QuitGame()
       {
           // Application.Quit does nothing in the Editor, so say so out loud —
           // otherwise the button looks broken while you are testing.
           Debug.Log("quit requested");
           Application.Quit();
       }
   }
   ```

8. In the `Menu` scene, create an **empty GameObject** named **`Router`** and drag `SceneRouter.cs` onto it.

9. Wire the buttons. Select `PlayButton`, find the **On Click ()** list at the bottom of its `Button`
   component, press **+**, drag the `Router` object into the object field, and choose
   **SceneRouter > LoadFirstLevel ()** from the function dropdown. Do the same for `QuitButton` with
   **SceneRouter > QuitGame ()**.

   Pick the method under the **first** heading in that dropdown — the one *without* a value slot. The second
   group takes a fixed argument and is not what you want here.

10. Save every scene (**File > Save**), then open `Menu` and press **Play**. Clicking **Play** loads
    `Level01` and the game runs as before. Clicking **Quit** prints `quit requested` to the Console.

## Done when (this step)
- [ ] `Assets/_Project/Scenes` contains `Menu.unity`, `Level01.unity`, `Level02.unity`, `Win.unity`.
- [ ] **File > Build Profiles > Scene List** lists them in that exact order, `Menu` at index 0, all ticked.
- [ ] Playing from `Menu` and pressing **Play** loads `Level01`, with the HUD, camera and audio all working.
- [ ] `Level02` is recognisably a different arrangement and plays correctly — its camera confines to its own
      painted bounds.
- [ ] The **Quit** button prints `quit requested` in the Editor.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(scenes): add Menu, Level02 and Win scenes with a scene router
```

## If it breaks
- **`Scene 'Level01' couldn't be loaded because it has not been added to the build settings`** → exactly what
  it says. Add it to **File > Build Profiles > Scene List**, and check the tick beside it.
- **The button does nothing and the Console is silent** → the **On Click ()** entry has no object, or the
  function is still `No Function`.
- **The button click never registers** → the scene has no **EventSystem**. Unity adds one with the first UI
  element; if you deleted it, add it back with **GameObject > UI > Event System**.
- **Level02 plays but the camera shows the void** → its `Grid` polygon confiner still describes Level01's
  shape.
- **The Menu scene is pitch black** → normal: it has no light and needs none, but check its `Camera`'s
  **Background** colour if you want something other than blue-grey.
- **Duplicating the scene duplicated the HUD's broken references** → open `Level02` and check `HudView`'s four
  fields point at *its* `Player`, not at `Level01`'s.

---
> Nav: [← The HUD](01_hud.md) · [Overview](00_overview.md) · [Carry the run across scenes →](03_game-session.md)
