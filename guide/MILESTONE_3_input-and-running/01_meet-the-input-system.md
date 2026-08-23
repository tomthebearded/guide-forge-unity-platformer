# M3 · Step 01 of 06 — Meet the Input System
> Nav: — · [Overview](00_overview.md) · [Read the Move action →](02_input-reader.md)

**Before you start:** the `cavern-dash` project is open on `Level01`, with M2's gate passed. This step changes
no files — it is a tour of an asset the Universal 2D template already created for you.

## Glossary for this step
> New here: **[input action](../foundation/glossary.md#input-action)** (defined in *Why / design*) ·
> **[binding](../foundation/glossary.md#binding)** (defined in *Do this*, action 3) ·
> **[action map](../foundation/glossary.md#action-map)** (defined in *Do this*, action 2).

## Why / design
The naive way to read input is to ask "is the D key down?". It works until the day you add a gamepad, and then
again until someone plays on an AZERTY keyboard, and again when a player wants to rebind a key. Each of those
is a rewrite of every place that asked about a key.

The Input System inverts it. You declare **actions** — named intents like *Move* and *Jump* — and separately
declare **bindings**, the concrete controls that feed them. Your code asks the *action* what it is worth right
now, and never learns which device answered. That is what will let M12 rebind a key at runtime without
touching a line of gameplay code, and it is why the gamepad in this milestone costs you nothing.

> New concept — **input action**: a named intent (`Move`, `Jump`) with a value type. Your code reads the
> action; the bindings decide what can produce it. Documentation:
> <https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/manual/Actions.html>

Unity 6 ships this pre-built. There is a **project-wide actions asset** called `InputSystem_Actions` already
in your project, already containing a `Player` map with a `Move` action bound to WASD, the arrow keys and a
gamepad stick. You are going to read it rather than build it.

## Do this

1. Open **Edit > Project Settings > Input System Package**. The **Input Actions** section shows a
   **Project-wide Actions** field holding an asset named `InputSystem_Actions`. That asset is the single input
   configuration every script in this project will read.

2. Click the asset in that field to select it, then double-click it in the **Project** panel to open the
   **Input Actions editor** window. The left column lists **action maps**.

   > New concept — **action map**: a named group of actions that are enabled or disabled together — typically
   > one per context. The default asset has `Player` (gameplay) and `UI` (menus), which is exactly the split
   > this guide needs: in M11 the pause menu turns one off and the other on.

3. Select the **Player** map. The middle column lists its actions — `Move`, `Look`, `Attack`, `Jump`, and
   several more. Click **Move**, then expand it with the arrow at its left to reveal its **bindings**.

   > New concept — **binding**: a link from a physical control to an action, written as a path like
   > `<Keyboard>/a` or `<Gamepad>/leftStick`. One action can carry many; every one of them can produce it.

   Under `Move` you will find a **2D Vector composite** built from `W`/`A`/`S`/`D`, another from the arrow
   keys, and a binding to the gamepad's left stick. A *composite* is how four separate buttons combine into
   one two-dimensional value: press `D` and the action reads `(1, 0)`; press `A` and it reads `(-1, 0)`.

4. Click **Jump** and look at its bindings: `<Keyboard>/space` and the gamepad's south button (**A** on an
   Xbox pad, **✕** on a PlayStation pad). You do not need it this milestone —
   [M4](../MILESTONE_4_gravity-and-jumping/00_overview.md) uses it — but it is worth seeing that it is already
   there.

5. Close the Input Actions editor **without changing anything**. If Unity offers to save, choose **Don't
   Save**: nothing in this step is meant to modify the asset.

## Done when (this step)
- [ ] **Edit > Project Settings > Input System Package** shows **Project-wide Actions** = `InputSystem_Actions`
      (not `None`).
- [ ] In the Input Actions editor, the `Player` map contains an action named exactly **`Move`** whose bindings
      include a 2D Vector composite over `W`/`A`/`S`/`D` and a binding referencing `<Gamepad>/leftStick`.
- [ ] `git status --porcelain` → prints **nothing**: you changed no files.

## If it breaks
- **Project-wide Actions is `None`** → the field was cleared, or the project came from a template that does
  not set it. Click the field's object picker and choose `InputSystem_Actions`; if the asset does not exist,
  press **Create and assign** in that settings page, which generates the default asset with the `Player` and
  `UI` maps.
- **The Input System Package section is missing from Project Settings** → the package is not installed. Open
  **Window > Package Manager**, switch to **Unity Registry**, find **Input System**, and install it. The
  Universal 2D template normally installs it for you.
- **Unity asks to enable the new input backend and restart** → say yes. That switch is what makes the Input
  System active; the old `Input.GetAxis` API is not used anywhere in this guide.

---
> Nav: — · [Overview](00_overview.md) · [Read the Move action →](02_input-reader.md)
