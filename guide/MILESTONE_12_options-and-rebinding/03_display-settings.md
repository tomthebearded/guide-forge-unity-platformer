# M12 · Step 03 of 06 — Display settings
> Nav: [← Volume sliders](02_volume-sliders.md) · [Overview](00_overview.md) · [Rebind a key →](04_rebinding.md)

**Before you start:** [step 02](02_volume-sliders.md) finished — the three sliders drive the mixer and
persist.

## Why / design
One toggle: fullscreen or windowed. It is the display setting players actually use, and unlike a resolution
list it needs no enumeration of what the monitor supports.

The interesting part is that **it does almost nothing in the Editor**. `Screen.fullScreen` addresses the game
window, and in the Editor the "game window" is a docked panel that will not go fullscreen on your behalf. The
toggle will save its value, the value will be read back, and the visible effect arrives only in a built
game — which this guide does not make, because it stops at development.

That is worth stating plainly rather than discovering: a gate that says "the screen goes fullscreen" would
fail here for a correct implementation. So this step's gate reads the **stored value** and the
**`Screen.fullScreen` property**, both of which are observable now. The window itself is something you can
only watch move in a standalone build, which is outside this guide's scope.

## Do this

1. Create a new MonoBehaviour script in `Assets/_Project/Scripts` named **`DisplaySettings`**:

   ```csharp
   // Assets/_Project/Scripts/DisplaySettings.cs — the whole file
   using UnityEngine;
   using UnityEngine.UI;

   // One toggle: fullscreen or windowed, remembered between launches.
   public class DisplaySettings : MonoBehaviour
   {
       // Load-bearing: the PlayerPrefs key. PlayerPrefs has no bool, so 1/0 it is.
       private const string FullscreenKey = "cavernDash.display.fullscreen";

       [SerializeField] private Toggle fullscreenToggle;

       private void Start()
       {
           // Default to whatever the player is currently running in, so a first
           // launch does not change anything behind their back.
           int savedValue = PlayerPrefs.GetInt(FullscreenKey, Screen.fullScreen ? 1 : 0);
           bool wantsFullscreen = savedValue == 1;

           Screen.fullScreen = wantsFullscreen;

           // WithoutNotify sets the control without firing its listener — the same
           // reason the sliders use SetValueWithoutNotify.
           fullscreenToggle.SetIsOnWithoutNotify(wantsFullscreen);

           fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
       }

       private void SetFullscreen(bool wantsFullscreen)
       {
           Screen.fullScreen = wantsFullscreen;
           PlayerPrefs.SetInt(FullscreenKey, wantsFullscreen ? 1 : 0);
           PlayerPrefs.Save();

           // In the Editor this changes the stored value and the property, and
           // nothing visible: the Game view is a panel, not a window.
           Debug.Log($"fullscreen = {Screen.fullScreen}");
       }
   }
   ```

2. Save, let Unity compile. In `Menu`, select **`OptionsController`** — the always-active object, for the
   same reason as [step 02](02_volume-sliders.md): the stored choice has to be applied at launch, and `Start`
   never runs on the switched-off panel. Drag `DisplaySettings.cs` onto it, then set its **Fullscreen Toggle**
   field by dragging the `FullscreenToggle` object in from inside `OptionsPanel` — an active component may
   hold a reference into an inactive object.

3. Save the scene and press **Play** from `Menu`. Open **Options** and click the toggle: the Console prints
   `fullscreen = True`, then `fullscreen = False` when you click it again.

4. Stop Play Mode with the toggle **on**, press Play again, and open Options: the toggle is still on. The
   value was stored and read back.

## Done when (this step)
- [ ] Clicking the toggle prints `fullscreen = True` / `fullscreen = False` to the Console, matching its
      state.
- [ ] Leaving Play Mode with the toggle on and returning shows it still on.
- [ ] **Edit > Clear All PlayerPrefs** followed by a restart shows the toggle in whatever state the Editor is
      running in, with nothing having changed behind your back.
- [ ] The Console shows no red entries; the project compiles.
- [ ] *(The window itself changing is not checked here, or anywhere in this guide — it is a build-only
      effect, and the guide stops at development.)*

## Suggested commit
```
feat(options): add a persisted fullscreen toggle
```

## If it breaks
- **The toggle prints nothing** → the listener was not attached, or the **Fullscreen Toggle** field is empty.
- **The toggle flips itself back** → `SetIsOnWithoutNotify` was written as `isOn`, so setting it in `Start`
  fires the listener, which writes and re-reads.
- **The setting does not persist** → `PlayerPrefs.SetInt` and `GetInt` disagree on the key string. It is a
  constant so that cannot happen; check you did not add a second copy of the component.
- **You expected the Game view to go fullscreen** → it will not, by design. See *Why / design*.

---
> Nav: [← Volume sliders](02_volume-sliders.md) · [Overview](00_overview.md) · [Rebind a key →](04_rebinding.md)
