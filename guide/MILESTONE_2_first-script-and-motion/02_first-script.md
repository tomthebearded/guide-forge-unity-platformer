# M2 · Step 02 of 05 — Write your first MonoBehaviour
> Nav: [← Add the player sprite](01_player-sprite.md) · [Overview](00_overview.md) · [Tune it while it runs →](03_tune-in-inspector.md)

**Before you start:** the `Player` object from [step 01](01_player-sprite.md) exists in `Level01` and the
scene is saved.

## Glossary for this step
> New here: **[MonoBehaviour](../foundation/glossary.md#monobehaviour)** (defined in *Why / design*) ·
> **[serialization](../foundation/glossary.md#serialization)** (defined in *Do this*, action 3) ·
> **[delta time](../foundation/glossary.md#delta-time)** (defined in *Do this*, action 4).

## Why / design
You know how to write a class. What is new is how Unity *runs* one.

A C# class becomes part of the game by inheriting from **`MonoBehaviour`** and being attached to a GameObject
as a component. You never call it: Unity does, through a fixed set of named methods it looks for. Two of them
matter here — `Awake`, called once when the object comes to life, and `Update`, called once per rendered
frame. There is no `main` loop you write; `Update` *is* the loop, and every object in the scene gets its own.

That immediately raises the question this whole guide is built on. `Update` runs once per frame, and frames do
not arrive at a fixed rate — a fast machine may render 240 of them a second, a busy one 40. Any code shaped
`position += speed` therefore moves faster on faster hardware, which is the oldest bug in game programming.
The fix is **delta time**, and you will multiply by it in almost every step from here on.

This step's script moves the player at a constant speed with no input at all. That is deliberate: it isolates
the lifecycle and delta time so you meet one new thing at a time. In M3 the same object gets real input and
real physics, and this script is deleted.

## Do this

1. In the **Project** panel, right-click `Assets/_Project/Scripts` and choose **Create > Scripting >
   MonoBehaviour Script**. Name it **`ConstantMover`** — exactly, no space. Unity creates
   `Assets/_Project/Scripts/ConstantMover.cs` plus its `.meta`.

   The file name is **load-bearing**: for a `MonoBehaviour`, the file name must match the class name inside it,
   or Unity refuses to attach the script and prints *"The script doesn't inherit a native class that can
   manage a script"*.

2. Double-click the file to open it in your C# editor. Replace everything in it with the code below, then save
   the file (**Ctrl+S** / **Cmd+S**) and return to the Unity window — Unity compiles on focus, and the
   spinner at the bottom right tells you it is doing so.

   ```csharp
   // Assets/_Project/Scripts/ConstantMover.cs — the whole file
   using UnityEngine;

   // Moves whatever it is attached to steadily along +X, at a speed set in the Inspector.
   // A stepping stone: M3 replaces it with input-driven, physics-based movement.
   public class ConstantMover : MonoBehaviour
   {
       [SerializeField] private float moveSpeedUnitsPerSecond = 3f;

       // Update() is called by Unity once per rendered frame.
       private void Update()
       {
           // Vector3.right is (1, 0, 0). Multiplying by Time.deltaTime converts
           // "units per second" into "units this frame".
           transform.position += Vector3.right * (moveSpeedUnitsPerSecond * Time.deltaTime);
       }
   }
   ```

3. Read the field declaration before you move on, because this pattern appears in every script in the guide:

   ```csharp
   [SerializeField] private float moveSpeedUnitsPerSecond = 3f;
   ```

   `private` keeps the value out of reach of other scripts, which is what you want. `[SerializeField]` tells
   Unity to **serialize** it anyway — to save its value with the object and show it in the Inspector.

   > New concept — **serialization**: Unity saving a component's field values into the scene or prefab file,
   > which is also what makes them editable in the Inspector. A `public` field is serialized automatically; a
   > `private` one needs `[SerializeField]`. This guide always uses `[SerializeField] private`, because a
   > tunable value is not the same thing as a value other code may reach into and change.
   >
   > The value in the file (`3f`) is only the **default for new components**. Once a component exists on an
   > object, the Inspector's value wins — editing the file will not change it. That surprises everyone once.

4. Now the multiplication. `Time.deltaTime` is the number of seconds the previous frame took.

   > New concept — **delta time**: `Time.deltaTime` is the interval in seconds from the last frame to this
   > one. At 60 fps it is about `0.0167`; at 15 fps, about `0.0667`. Multiplying a *per-second* speed by it
   > gives the distance to travel *this frame*, so total distance depends on elapsed time and not on how many
   > frames happened. Official reference:
   > <https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Time-deltaTime.html>
   >
   > Read the expression as English: `3 units/second × 0.0167 seconds = 0.05 units this frame`. The seconds
   > cancel. Any time you see a raw number added to a position without a `Time.deltaTime` beside it, that is
   > the bug.

5. Back in Unity, drag `ConstantMover.cs` from the Project panel **onto the `Player` object** in the
   Hierarchy. (Selecting `Player` and using **Add Component > Constant Mover** in the Inspector does the same
   thing.) The Inspector now shows a `Constant Mover (Script)` component with one field, **Move Speed Units
   Per Second**, showing `3`.

   Unity displays `moveSpeedUnitsPerSecond` as "Move Speed Units Per Second" — it splits camelCase into words
   for the label. The field is still spelled `moveSpeedUnitsPerSecond` in code.

6. Press **Play**. The orange square glides to the right and off the edge of the Game view. Press **Play**
   again to stop; the square returns to where you left it, because Play Mode changes are discarded.

## Done when (this step)
- [ ] The **Console** shows no red entries, and the Editor's bottom-right corner shows no compile spinner —
      the script compiled.
- [ ] The `Player`'s Inspector shows a `Constant Mover (Script)` component with **Move Speed Units Per
      Second** = `3`.
- [ ] Pressing **Play** makes the square travel right and leave the frame within a few seconds; stopping
      returns it to the centre.

## Suggested commit
```
feat(player): add ConstantMover with frame-rate-independent motion
```

## If it breaks
- **"The script doesn't inherit a native class that can manage a script"** → the class name and the file name
  differ. The file must be `ConstantMover.cs` and the class `ConstantMover`.
- **The component cannot be added: "Can't add script behaviour"** → the script has a compile error. Look at
  the Console first: nothing can be attached while the project fails to compile.
- **The square moves, but far too fast or slowly, and the Inspector says 3** → you are editing a *different*
  copy of the object. Check you attached the script to the `Player` in the Hierarchy, not to a prefab or a
  second object.
- **Nothing moves at all** → `Update` is spelled with a lowercase `u`, or is `public void Update` inside
  another method. Unity finds these callbacks by exact name; `update()` is never called.

---
> Nav: [← Add the player sprite](01_player-sprite.md) · [Overview](00_overview.md) · [Tune it while it runs →](03_tune-in-inspector.md)
