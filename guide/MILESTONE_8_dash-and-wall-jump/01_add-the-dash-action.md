# M8 · Step 01 of 06 — Add the Dash action
> Nav: — · [Overview](00_overview.md) · [Turn the motor into a state machine →](02_movement-state-machine.md)

**Before you start:** M7's gate passed. This is the first time you edit `InputSystem_Actions` — until now you
have only read it, in [M3 step 01](../MILESTONE_3_input-and-running/01_meet-the-input-system.md).

## Why / design
`Move` and `Jump` came ready-made in Unity 6's project-wide actions asset. `Dash` does not, so you add it —
and adding it is the whole reason the Input System is worth its setup cost. You declare an intent, bind two
controls to it, and every consumer from here to M12's rebinding screen reads the intent.

The bindings: **Left Shift** on the keyboard, and the **west face button** on a gamepad (X on an Xbox pad,
□ on a PlayStation pad). Dash sits under the thumb on a pad and under the little finger on a keyboard, which
is where players expect a movement modifier.

The action's name — **`Dash`** — is **load-bearing**: `PlayerInputReader` looks it up by that exact string in
[step 03](03_dash.md), and [`../foundation/conventions.md`](../foundation/conventions.md) records it.

## Do this

1. In the **Project** panel, find **`InputSystem_Actions`** (it sits at the root of `Assets`) and
   double-click it to open the **Input Actions editor**.

2. Select the **`Player`** action map in the left column. In the middle column, press the **`+`** at the top
   right of the **Actions** list. A new entry appears in rename mode — type **`Dash`** and press Enter.

3. With `Dash` selected, look at the right-hand pane and set **Action Type** to **`Button`**. A button action
   reports pressed and released; the default, `Value`, is for continuous inputs like a stick.

4. Expand `Dash` with the arrow at its left. It has one empty binding, labelled `<No Binding>`. Select it,
   then in the right-hand pane click the **Path** dropdown and press **Listen**. Now press **Left Shift** on
   your keyboard: the list fills with matching controls. Choose **Left Shift [Keyboard]**.

5. Add the gamepad binding: with `Dash` selected, press the **`+`** on its row and choose **Add Binding**.
   Select the new empty binding, press **Listen** in the Path dropdown, and press the **west face button** on
   your gamepad (X on Xbox, □ on PlayStation). Choose the entry it offers — it reads as
   `Button West [Gamepad]`.

   No gamepad to hand? Set the path by typing instead: open the Path dropdown, navigate **Gamepad > Button
   West**, and select it. The result is identical.

6. Press **Save Asset** at the top of the editor window. The Input Actions editor does **not** auto-save, and
   an unsaved action is the reason for half of all "my binding does nothing" reports.

7. Close the editor and check the file changed: `git status --short` lists
   `Assets/InputSystem_Actions.inputactions` as modified.

## Done when (this step)
- [ ] The `Player` map contains an action named exactly **`Dash`**, of **Action Type** `Button`.
- [ ] `Dash` has **two** bindings: `<Keyboard>/leftShift` and `<Gamepad>/buttonWest`.
- [ ] The editor's title no longer shows an unsaved-changes marker after **Save Asset**.
- [ ] `git status --short` → lists `Assets/InputSystem_Actions.inputactions` as modified.
- [ ] The Console shows no red entries. *(Nothing reads the action yet — that is [step 03](03_dash.md).)*

## Suggested commit
```
feat(input): add a Dash action bound to left shift and gamepad west
```

## If it breaks
- **The `+` adds an action to the wrong map** → the `UI` map was selected. Actions belong to the map that is
  highlighted when you press `+`; drag it across or delete and re-add under `Player`.
- **"Listen" never picks anything up** → the device is not being seen by the Editor. Click inside the Input
  Actions window first, and check the gamepad appears under **Window > Analysis > Input Debugger**.
- **The binding shows as `<None>` after saving** → the path was set on the action rather than on its binding
  child. Expand the action and select the indented binding row underneath it.
- **The asset shows no change in `git status`** → **Save Asset** was not pressed.

---
> Nav: — · [Overview](00_overview.md) · [Turn the motor into a state machine →](02_movement-state-machine.md)
