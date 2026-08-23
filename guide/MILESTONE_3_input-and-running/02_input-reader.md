# M3 · Step 02 of 06 — Read the Move action
> Nav: [← Meet the Input System](01_meet-the-input-system.md) · [Overview](00_overview.md) · [Give the world a floor →](03_ground-platform.md)

**Before you start:** [step 01](01_meet-the-input-system.md) confirmed that **Project-wide Actions** is set to
`InputSystem_Actions` and that the `Player` map contains `Move`.

## Why / design
This script does one thing: turn the `Move` action into a number other components can read. It does **not**
move anything. That split — one component that answers *what is being asked for*, another that decides *what
happens* — is the architecture rule from [`../foundation/conventions.md`](../foundation/conventions.md), and it
pays off three times in this guide: M8 adds dash and wall-jump without touching input, M12 rebinds keys
without touching movement, and every one of these components stays small enough to read in one sitting.

Reading input belongs in `Update` — once per rendered frame, which is when the Input System refreshes its
state by default. Acting on it belongs in `FixedUpdate`, which you meet in [step 05](05_move-with-velocity.md).

Input is a **scalar, not a branch**. The `Move` action gives you a number between `-1` and `+1`; the keyboard
produces exactly `-1`, `0` or `+1`, while a gamepad stick produces everything in between. Code that multiplies
by that number handles both. Code shaped `if (leftPressed) … else if (rightPressed) …` throws away the stick's
precision and doubles in size for no benefit.

## Do this

1. In the **Project** panel, right-click `Assets/_Project/Scripts` and choose **Create > Scripting >
   MonoBehaviour Script**. Name it **`PlayerInputReader`**.

2. Open the file and replace its contents with this. The `using UnityEngine.InputSystem;` line is what brings
   `InputSystem` and `InputAction` into scope — without it nothing here compiles.

   ```csharp
   // Assets/_Project/Scripts/PlayerInputReader.cs — the whole file
   using UnityEngine;
   using UnityEngine.InputSystem;

   // Answers "what is the player asking for?" — and nothing else.
   // Acting on it is PlayerMotor's job (step 05).
   public class PlayerInputReader : MonoBehaviour
   {
       // -1 = full left, 0 = nothing, +1 = full right. Read by other components.
       public float HorizontalInput { get; private set; }

       private InputAction moveAction;

       // Temporary, so you can watch the value in step 02. Removed in step 05.
       private float lastLoggedHorizontalInput = float.NaN;

       private void Awake()
       {
           // InputSystem.actions is the project-wide asset from step 01.
           // FindAction takes "<action map>/<action>" — "Move" alone works too,
           // but the full path is unambiguous if two maps ever share a name.
           moveAction = InputSystem.actions.FindAction("Player/Move");
       }

       private void Update()
       {
           // ReadValue<Vector2>() returns the composite as (x, y). A platformer
           // only cares about x; y is what a top-down game would use.
           HorizontalInput = moveAction.ReadValue<Vector2>().x;

           // Mathf.Approximately compares floats with a tolerance — never use == on them.
           if (!Mathf.Approximately(HorizontalInput, lastLoggedHorizontalInput))
           {
               Debug.Log($"HorizontalInput = {HorizontalInput:F2}");
               lastLoggedHorizontalInput = HorizontalInput;
           }
       }
   }
   ```

3. Save, return to Unity, and let it compile. Then drag `PlayerInputReader.cs` from the Project panel onto the
   **`Player`** object in the Hierarchy.

4. Press **Play** and watch the **Console**, then:
   - Hold **D** (or **→**) → a line appears reading `HorizontalInput = 1.00`.
   - Release it → `HorizontalInput = 0.00`.
   - Hold **A** (or **←**) → `HorizontalInput = -1.00`.

   The square keeps drifting right on its own — `ConstantMover` is still attached and still ignoring you.
   [Step 04](04_rigidbody-and-collider.md) removes it.

5. If you have a gamepad, plug it in (Unity picks it up without a restart) and push the left stick part way.
   The Console now prints intermediate values — `0.43`, `0.71`, `1.00` — because a stick is analogue. Nothing
   in your code mentions a gamepad: the binding you looked at in step 01 is doing the work.

## Done when (this step)
- [ ] Holding **D** during Play Mode → the Console prints `HorizontalInput = 1.00` exactly once per press
      (the value only logs when it changes).
- [ ] Releasing → `HorizontalInput = 0.00`; holding **A** → `HorizontalInput = -1.00`.
- [ ] With a gamepad connected, a half-pushed left stick prints a value strictly between `0.00` and `1.00`.
      (No gamepad? Skip this box and check it at the M13 gate instead.)
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(input): add PlayerInputReader reading the Move action
```

## If it breaks
- **`NullReferenceException` on the `ReadValue` line** → `FindAction("Player/Move")` returned `null`: the
  project-wide actions asset is unset, or the map/action is spelled differently. Re-check
  [step 01](01_meet-the-input-system.md)'s first box.
- **The Console prints `0.00` for ever, whatever you press** → the action exists but is disabled. Unity 6
  enables project-wide actions for you; if yours are not, add `moveAction.Enable();` on the line after
  `FindAction` in `Awake`.
- **`The type or namespace name 'InputSystem' could not be found`** → the `using UnityEngine.InputSystem;`
  line is missing, or the Input System package is not installed (see step 01's *If it breaks*).
- **Keys do nothing but the Game view has focus** → click once inside the **Game** view. Unity sends input to
  the focused panel, and after editing a field the Inspector may still hold focus.

---
> Nav: [← Meet the Input System](01_meet-the-input-system.md) · [Overview](00_overview.md) · [Give the world a floor →](03_ground-platform.md)
