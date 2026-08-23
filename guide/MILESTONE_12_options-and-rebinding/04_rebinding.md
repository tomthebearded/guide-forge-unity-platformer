# M12 · Step 04 of 06 — Rebind a key
> Nav: [← Display settings](03_display-settings.md) · [Overview](00_overview.md) · [Make rebinds stick →](05_persist-rebinds.md)

**Before you start:** [step 03](03_display-settings.md) finished — the fullscreen toggle persists. This is the
feature the Input System exists for, and it is the last big piece of C# in the guide.

## Glossary for this step
> New here: **[binding override](../foundation/glossary.md#override-layer-binding-override)** (defined in *Why / design*) ·
> **[rebinding operation](../foundation/glossary.md#rebinding-operation)** (defined in *Do this*, action 2).

## Why / design
Rebinding never edits `InputSystem_Actions`. It adds an **override** on top of it: the asset keeps saying
`<Keyboard>/space`, and a separate layer says "for this player, that binding is `<Keyboard>/j` instead".

> New concept — **binding override**: a per-player change stored *on top of* an action asset rather than in
> it. That is what makes rebinding safe (your shipped defaults are never modified), resettable (drop the
> layer), and savable ([step 05](05_persist-rebinds.md) serializes exactly this layer).

Listening for "the next thing the player presses" sounds easy and is not: you must ignore mouse movement,
which reports constantly and would bind itself instantly; you need a cancel key; and the action being rebound
must be **disabled** while you listen, or pressing the key fires the action you are trying to change. The
package's `PerformInteractiveRebinding` handles all three, and this is a **borrow** for exactly that reason.

One wrinkle specific to `Move`. It is not a simple button — it is a **composite**, four part-bindings (`up`,
`down`, `left`, `right`) feeding one two-dimensional value. So a rebind button for "move left" has to target
the *part*, not the action, and the robust way to find it is by name rather than by counting rows in the
editor.

## Do this

1. In the `Menu` scene, add one more button inside `OptionsPanel`, named **`RebindJumpGamepadButton`** — the
   five rebind buttons now cover move-left, move-right, jump and dash on the keyboard, and jump on the
   gamepad.

2. Create a new MonoBehaviour script in `Assets/_Project/Scripts` named **`RebindButton`**:

   ```csharp
   // Assets/_Project/Scripts/RebindButton.cs — the whole file
   using TMPro;
   using UnityEngine;
   using UnityEngine.InputSystem;
   using UnityEngine.UI;

   // One button rebinds one binding of one action. Press it, then press the control
   // you want; Escape cancels.
   [RequireComponent(typeof(Button))]
   public class RebindButton : MonoBehaviour
   {
       [SerializeField] private string actionPath = "Player/Jump";

       // Which binding of that action. For a plain button: 0 is usually the keyboard
       // binding and 1 the gamepad one, in the order they appear in the asset.
       [SerializeField] private int bindingIndex;

       // For a composite like Move, name the part instead: "left", "right", "up", "down".
       // Leave empty for a plain binding.
       [SerializeField] private string compositePartName = "";

       [SerializeField] private TMP_Text label;
       [SerializeField] private string listeningText = "press a key...";

       private InputAction action;
       private InputActionRebindingExtensions.RebindingOperation operation;

       private void Awake()
       {
           action = InputSystem.actions.FindAction(actionPath);
           GetComponent<Button>().onClick.AddListener(StartRebind);
       }

       private void OnEnable()
       {
           ShowCurrentBinding();
       }

       private void ShowCurrentBinding()
       {
           // GetBindingDisplayString turns "<Keyboard>/space" into "Space".
           label.text = action.GetBindingDisplayString(ResolveBindingIndex());
       }

       private int ResolveBindingIndex()
       {
           if (string.IsNullOrEmpty(compositePartName))
           {
               return bindingIndex;
           }

           // A composite occupies several consecutive entries in the action's binding
           // list: the composite itself, then one per part. Find the part by name so
           // the code survives someone reordering the asset.
           for (int i = 0; i < action.bindings.Count; i++)
           {
               if (action.bindings[i].isPartOfComposite && action.bindings[i].name == compositePartName)
               {
                   return i;
               }
           }

           Debug.LogWarning($"No composite part '{compositePartName}' on {actionPath}");
           return bindingIndex;
       }

       private void StartRebind()
       {
           label.text = listeningText;

           // The action must be off while the operation listens, or the very press
           // being bound would also fire the action.
           action.Disable();

           operation = action.PerformInteractiveRebinding(ResolveBindingIndex())
               .WithControlsExcluding("<Mouse>/position")
               .WithControlsExcluding("<Mouse>/delta")
               .WithCancelingThrough("<Keyboard>/escape")
               .OnComplete(_ => FinishRebind())
               .OnCancel(_ => FinishRebind())
               .Start();
       }

       private void FinishRebind()
       {
           // A RebindingOperation holds unmanaged memory. Not disposing it leaks,
           // every single time.
           operation?.Dispose();
           operation = null;

           action.Enable();
           ShowCurrentBinding();
       }

       private void OnDisable()
       {
           // Closing the panel mid-rebind must not leave an operation listening.
           if (operation != null)
           {
               FinishRebind();
           }
       }
   }
   ```

   > New concept — **rebinding operation**: the object returned by `PerformInteractiveRebinding`. It listens
   > for input, applies an override when something arrives, and reports through `OnComplete` or `OnCancel`.
   > It must be **disposed**, and the action must be **disabled** while it runs.
   > API reference: <https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/api/UnityEngine.InputSystem.InputActionRebindingExtensions.html>

3. Save, let Unity compile, and drag `RebindButton.cs` onto **each of the five rebind buttons**. Then
   configure each one — every button also needs its **Label** field set to its own child `Text (TMP)` object:

   | Button | Action Path | Binding Index | Composite Part Name |
   |---|---|---|---|
   | `RebindMoveLeftButton` | `Player/Move` | `0` | `left` |
   | `RebindMoveRightButton` | `Player/Move` | `0` | `right` |
   | `RebindJumpButton` | `Player/Jump` | `0` | *(empty)* |
   | `RebindJumpGamepadButton` | `Player/Jump` | `1` | *(empty)* |
   | `RebindDashButton` | `Player/Dash` | `0` | *(empty)* |

4. Save the scene and press **Play** from `Menu`, then open **Options**. Each button shows its current
   control — `A`, `D`, `Space`, the gamepad's south button, `Left Shift`.

5. Click **Jump**'s button: its label changes to `press a key...`. Press **J**. The label becomes `J`.

6. Press **Play** to start the level and try it: **Space** no longer jumps, **J** does. Move and dash are
   untouched.

7. Go back to the menu (pause → **Back to menu**) and open Options again: the label still reads `J`. The
   override is live for as long as the game runs — but quit and relaunch, and it is gone. That is
   [step 05](05_persist-rebinds.md).

8. Check the two traps. Click a rebind button and press **Escape**: it cancels and the old binding comes back.
   Click one and wiggle the mouse: nothing binds, because pointer movement is excluded.

## Done when (this step)
- [ ] All five buttons show their current control on opening the panel — not blank, not `press a key...`.
- [ ] Clicking one shows `press a key...`, and the next control you press becomes its new binding, shown on
      the label.
- [ ] A rebound key **works in the level**, and the original one no longer does.
- [ ] **Escape** cancels a rebind and restores the previous binding.
- [ ] Moving the mouse during a rebind binds nothing.
- [ ] Rebinding `Jump` on the gamepad button changes the pad without touching the keyboard binding.
- [ ] Rebinding move-left changes only the left part of the composite — right, up and down are unaffected.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(options): rebind actions interactively from the options panel
```

## If it breaks
- **The label reads `press a key...` for ever** → the operation never completed, usually because every
  control it saw was excluded. Press a keyboard key rather than moving a stick slightly.
- **Pressing the key both binds it *and* jumps** → `action.Disable()` is missing before `.Start()`.
- **The binding changes but the game still uses the old key** → `action.Enable()` is missing from
  `FinishRebind`, so the action is left disabled and nothing responds at all.
- **`No composite part 'left'`** in the Console → the part name is capitalised differently. The Input Actions
  editor shows the part names on the indented rows under `Move`; they are lowercase.
- **The move-left button rebinds the whole composite** → **Composite Part Name** is empty for it, so it fell
  back to `bindingIndex`.
- **A second rebind does nothing** → the previous operation was not disposed. Every path out of a rebind
  must reach `FinishRebind`.

---
> Nav: [← Display settings](03_display-settings.md) · [Overview](00_overview.md) · [Make rebinds stick →](05_persist-rebinds.md)
