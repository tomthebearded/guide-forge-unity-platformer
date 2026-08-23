# M12 · Step 05 of 06 — Make rebinds stick
> Nav: [← Rebind a key](04_rebinding.md) · [Overview](00_overview.md) · [Verify →](06_verify.md)

**Before you start:** [step 04](04_rebinding.md) finished — the five buttons rebind their controls and the
change works in the level, but only until the game closes.

## Why / design
The override layer from [step 04](04_rebinding.md) is exactly the thing to save, and the Input System will
hand it to you as a JSON string: `SaveBindingOverridesAsJson()` on the actions asset serializes every override
currently applied, and `LoadBindingOverridesFromJson()` puts them back. Two calls, and a `PlayerPrefs` string
between them.

Doing it any other way — writing your own file of action names and control paths — means reimplementing a
format that already exists and will silently disagree with the asset the first time someone adds an action.

Two questions the design has to answer:

**When to load.** Before anything reads an action, in every scene, however the game was started. A tiny
component with `[RuntimeInitializeOnLoadMethod]` does it once at startup, before the first scene's `Awake`,
which is earlier and more reliable than putting it on a menu object the player might never visit.

**When to save.** Immediately after each rebind. A rebind is rare and a `PlayerPrefs` write is cheap; batching
them would only add a way to lose one.

**Reset to defaults** is the third call — `RemoveAllBindingOverrides()` — plus clearing the stored string.
Without clearing the string, the next launch loads the overrides straight back and the button looks broken.

## Do this

1. Create a new script in `Assets/_Project/Scripts` named **`BindingStorage`**. It is a plain static class,
   not a `MonoBehaviour` — nothing needs to attach it to anything:

   ```csharp
   // Assets/_Project/Scripts/BindingStorage.cs — the whole file
   using UnityEngine;
   using UnityEngine.InputSystem;

   // Saves and restores the player's rebinds. The Input System serializes the
   // override layer for us; this only decides where the string lives.
   public static class BindingStorage
   {
       // Load-bearing: the PlayerPrefs key holding the overrides JSON.
       private const string BindingOverridesKey = "cavernDash.bindingOverrides";

       // Runs once at startup, before the first scene's Awake, however the game
       // was launched — including pressing Play in the middle of a level.
       [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
       public static void Load()
       {
           string json = PlayerPrefs.GetString(BindingOverridesKey, string.Empty);

           if (string.IsNullOrEmpty(json))
           {
               return;
           }

           InputSystem.actions.LoadBindingOverridesFromJson(json);
       }

       public static void Save()
       {
           PlayerPrefs.SetString(BindingOverridesKey, InputSystem.actions.SaveBindingOverridesAsJson());
           PlayerPrefs.Save();
       }

       public static void ResetToDefaults()
       {
           InputSystem.actions.RemoveAllBindingOverrides();

           // Clear the stored string too, or the next launch reloads what you
           // just removed and the reset appears not to have worked.
           PlayerPrefs.DeleteKey(BindingOverridesKey);
           PlayerPrefs.Save();
       }
   }
   ```

2. In `Assets/_Project/Scripts/RebindButton.cs`, **ADD** this line to `FinishRebind`, directly below the
   `ShowCurrentBinding();` call:

   ```csharp
   // Assets/_Project/Scripts/RebindButton.cs — in FinishRebind(), below ShowCurrentBinding()
   BindingStorage.Save();
   ```

3. Give the reset button something to call, and make the labels refresh afterwards. Create one more
   MonoBehaviour script named **`ResetBindingsButton`**:

   ```csharp
   // Assets/_Project/Scripts/ResetBindingsButton.cs — the whole file
   using UnityEngine;

   // Puts every binding back to what the asset ships with, and refreshes the labels.
   public class ResetBindingsButton : MonoBehaviour
   {
       [SerializeField] private RebindButton[] rebindButtons;

       // Public so the button can call it from the Inspector.
       public void ResetAll()
       {
           BindingStorage.ResetToDefaults();

           // The buttons show their binding when they are enabled, so tell them
           // to look again — nothing about them changed by itself.
           foreach (RebindButton button in rebindButtons)
           {
               button.RefreshLabel();
           }
       }
   }
   ```

4. That calls a method `RebindButton` does not have. In `Assets/_Project/Scripts/RebindButton.cs`,
   **REPLACE** the line `private void ShowCurrentBinding()` with this, making it callable from outside — the
   body stays exactly as it is:

   ```csharp
   // Assets/_Project/Scripts/RebindButton.cs — replacing the ShowCurrentBinding() signature
   public void RefreshLabel()
   ```

   Now fix the three call sites inside the same file so the project compiles: in `OnEnable`, in
   `StartRebind`'s completion path (`FinishRebind`), replace each `ShowCurrentBinding();` with
   `RefreshLabel();`. There are exactly two calls to change.

5. Save, let Unity compile. In the `Menu` scene, select **`OptionsPanel`**, drag `ResetBindingsButton.cs` onto
   it, and set its **Rebind Buttons** array **Size** to `5`, filling the five slots with the five rebind
   button objects from the Hierarchy.

6. Wire the button: `ResetBindingsButton`'s **On Click ()** → **ResetBindingsButton > ResetAll ()**, with
   `OptionsPanel` in the object field.

7. Save the scene and test the whole cycle:
   - Play from `Menu`, open **Options**, rebind **Jump** to **J**, and confirm it works in the level.
   - **Stop Play Mode entirely.** Press Play again, open Options: the button still reads `J`, and `J` jumps in
     the level. That is the part that was missing before this step.
   - Click **Reset to defaults**: every label goes back to its original control, and **Space** jumps again.
   - Stop and restart once more: still the defaults. The stored string was cleared, not just the live layer.

## Done when (this step)
- [ ] A rebind survives leaving and re-entering Play Mode — the label and the actual control both.
- [ ] Rebinding several actions and restarting restores **all** of them.
- [ ] **Reset to defaults** restores every label and every control immediately.
- [ ] After a reset, restarting still shows the defaults — the stored overrides were deleted.
- [ ] Rebinding after a reset works and persists again.
- [ ] **Edit > Clear All PlayerPrefs** also returns everything to the asset's defaults.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(options): persist binding overrides and add reset to defaults
```

## If it breaks
- **The rebind is lost on restart** → `BindingStorage.Save()` is not being called from `FinishRebind`, or the
  load method is not marked `[RuntimeInitializeOnLoadMethod]`.
- **The reset works, then the old rebinds come back on the next launch** → `PlayerPrefs.DeleteKey` is missing
  from `ResetToDefaults`. This is the trap the design note warns about.
- **The labels do not change after a reset** → the `Rebind Buttons` array is empty or the wrong size. It is a
  fixed-size array in the Inspector; set **Size** first.
- **`The name 'ShowCurrentBinding' does not exist`** → action 4 renamed the method but not both call sites
  inside `RebindButton`.
- **Everything is default at every launch and nothing saves** → `InputSystem.actions` is returning a different
  asset than the one being rebound. There is only one project-wide asset; check **Project Settings > Input
  System Package** still points at `InputSystem_Actions`.

---
> Nav: [← Rebind a key](04_rebinding.md) · [Overview](00_overview.md) · [Verify →](06_verify.md)
