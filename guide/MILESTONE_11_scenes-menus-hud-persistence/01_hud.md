# M11 · Step 01 of 07 — The HUD
> Nav: — · [Overview](00_overview.md) · [More scenes →](02_more-scenes.md)

**Before you start:** M10's gate passed. `Level01` is open and Play Mode is stopped.

## Glossary for this step
> New here: **[Canvas](../foundation/glossary.md#canvas)** (defined in *Why / design*) ·
> **[TextMeshPro](../foundation/glossary.md#textmeshpro)** (defined in *Do this*, action 1).

## Why / design
Coins and lives have been going to the Console since M9. That was the right call — a HUD before the mechanics
would have been decoration — but it is now the only way to see the state of the run, and the Console is not
in the built game.

> New concept — **Canvas**: the object every UI element lives under. It is a separate rendering space from the
> world: its coordinates are screen pixels, not world units, and it draws on top of everything. A scene can
> have several; this game needs one per scene that shows anything.

The HUD listens to the events M9 already raises — `PlayerStats.CoinsChanged` and `PlayerHealth.LivesChanged`
— and that is the payoff for having written them as events. The display knows nothing about coins, enemies or
damage; it knows about two numbers and where to draw them.

## Do this

1. Import TextMeshPro's resources, once per project: **Window > TextMeshPro > Import TMP Essential
   Resources**, then press **Import** in the dialog. This writes a `TextMesh Pro` folder into `Assets/` — leave
   it exactly where it lands, and commit it.

   > New concept — **TextMeshPro**: Unity's text renderer, and the one to use. It ships inside the Editor as
   > part of the uGUI package, but its fonts and shaders arrive only when you import them, which is why a
   > fresh project shows a warning the first time you add text.

2. In the **Hierarchy**, right-click and choose **UI > Text - TextMeshPro**. Unity creates three things at
   once: a **`Canvas`**, an **`EventSystem`**, and a `Text (TMP)` under the canvas.

3. Select the **`Canvas`** and set, on its `Canvas Scaler` component:

   | Field | Value | Why |
   |---|---|---|
   | **UI Scale Mode** | `Scale With Screen Size` | Otherwise the HUD is tiny on a big monitor. |
   | **Reference Resolution** | `640` × `360` | A 16:9 pixel-art resolution; the HUD scales from it. |
   | **Match** | `0.5` | Splits the scaling between width and height. |

4. Rename the `Text (TMP)` object to **`CoinsLabel`**. In the Inspector:
   - Press the **anchor preset** box at the top left of its `Rect Transform`, hold **Alt** (**Option** on
     macOS), and click the **top-left** preset. That anchors it to the corner *and* moves it there.
   - Set **Pos X** `70`, **Pos Y** `-30`, **Width** `200`, **Height** `40`.
   - In `TextMeshPro - Text (UI)`, set **Text** to `Coins: 0`, **Font Size** to `24`, and **Alignment** to
     left and middle.

5. Duplicate it twice (**Ctrl+D** / **Cmd+D**) and adjust:
   - **`LivesLabel`** — **Pos Y** `-70`, text `Lives: 3`.
   - **`TimeLabel`** — **Pos Y** `-110`, text `0.0` — it stays a placeholder until
     [step 05](05_timer-and-best-time.md) drives it.

6. Create a new MonoBehaviour script in `Assets/_Project/Scripts` named **`HudView`**:

   ```csharp
   // Assets/_Project/Scripts/HudView.cs — the whole file
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

7. Save, let Unity compile, then select the **`Canvas`** and drag `HudView.cs` onto it. Fill its four fields by
   dragging from the Hierarchy: **Coins Label** ← `CoinsLabel`, **Lives Label** ← `LivesLabel`,
   **Stats** ← `Player`, **Health** ← `Player`.

   Dragging the `Player` object into a `PlayerStats` field works because Unity takes the matching component
   off it. That is worth knowing: object fields accept objects *and* their components.

8. Save the scene and press **Play**. The HUD reads `Coins: 0` and `Lives: 3`. Collect a coin — the count
   rises on screen. Take a hit — the lives count falls.

## Done when (this step)
- [ ] `Assets/TextMesh Pro` exists in the project (the imported essential resources).
- [ ] The Game view shows `Coins: 0` and `Lives: 3` in the top-left corner from the first frame, without
      touching anything.
- [ ] Collecting a coin updates the coin label; losing a life updates the lives label — both immediately.
- [ ] Resizing the Game view keeps the HUD in the corner at a sensible size rather than fixed pixels.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(ui): add a coins and lives HUD driven by gameplay events
```

## If it breaks
- **The text is invisible or the Console warns about missing TMP resources** → the essential resources were
  not imported. Redo action 1.
- **The labels sit in the middle of the screen** → the anchor preset was clicked without **Alt**/**Option**,
  which sets the anchor but leaves the position. Alt-click moves it too.
- **`NullReferenceException` in `OnEnable`** → one of the four fields on `HudView` is empty.
- **The HUD shows `Coins: 0` for ever** → `HudView` is subscribed to a *different* `PlayerStats` — check the
  field points at the `Player` in this scene, not at a prefab in the Project panel.
- **The HUD is enormous or minuscule in the built game** → the `Canvas Scaler` is still `Constant Pixel Size`.

---
> Nav: — · [Overview](00_overview.md) · [More scenes →](02_more-scenes.md)
