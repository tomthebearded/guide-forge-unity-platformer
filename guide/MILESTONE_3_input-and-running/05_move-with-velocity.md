# M3 · Step 05 of 06 — Move the body with velocity
> Nav: [← Give the player a body](04_rigidbody-and-collider.md) · [Overview](00_overview.md) · [Verify →](06_verify.md)

**Before you start:** [step 04](04_rigidbody-and-collider.md) finished — `Player` has a `Rigidbody2D`, a
`Box Collider 2D`, and `PlayerInputReader`; `ConstantMover` is gone; the square falls and rests on the strip.

## Glossary for this step
> New here: **[FixedUpdate](../foundation/glossary.md#fixedupdate)** (defined in *Why / design*) ·
> **[velocity](../foundation/glossary.md#velocity)** (defined in *Do this*, action 2).

## Why / design
Two things make this step's ten lines worth reading slowly.

**The first is *where* physics code runs.** `Update` runs once per rendered frame, at whatever rate the
machine manages. `FixedUpdate` runs on the physics clock — a **fixed** 0.02 seconds per step, 50 times a
second, no matter what the frame rate does. Anything that touches a `Rigidbody2D` belongs there, because
writing a velocity twice between two physics steps just means the second write wins, and writing it zero times
means a step passes with stale data.

> New concept — **`FixedUpdate`**: the callback Unity runs on the fixed physics timestep (0.02 s by default),
> which may be more or fewer times per frame than `Update`. Read input in `Update`; act on physics in
> `FixedUpdate`. Inside it, `Time.deltaTime` returns the *fixed* step rather than the frame time — Unity
> substitutes it deliberately, so a delta-time multiplication written there is still correct.

**The second is *what* you write.** You do not add to the position; you set the **velocity** and let the
engine integrate it. And you set only its X — reading the current Y back and putting it straight in again —
because the Y component is where gravity lives. Overwrite the whole vector and you cancel gravity every
physics step, producing a character that hovers.

That single line is also why this component will still be recognisable in M8, when dash and wall-jump join in:
they change *what the desired velocity is*, not *how it gets applied*.

## Do this

1. In `Assets/_Project/Scripts/PlayerInputReader.cs`, the temporary logging from
   [step 02](02_input-reader.md) has done its job. **DELETE** the field declaration line that reads:

   ```csharp
   private float lastLoggedHorizontalInput = float.NaN;
   ```

   and **DELETE** the whole `if` block inside `Update` that begins with the `Mathf.Approximately` comparison,
   including its `Debug.Log` line and its closing brace. What remains inside `Update` is the single assignment
   to `HorizontalInput`, and the file still compiles — nothing else referenced either of them.

2. In the **Project** panel, create a new MonoBehaviour script in `Assets/_Project/Scripts` named
   **`PlayerMotor`**, and replace its contents with this:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — the whole file
   using UnityEngine;

   // Turns the intent from PlayerInputReader into motion on the Rigidbody2D.
   // RequireComponent makes Unity add the dependencies automatically and refuse to
   // let you remove them while this script is attached.
   [RequireComponent(typeof(Rigidbody2D))]
   [RequireComponent(typeof(PlayerInputReader))]
   public class PlayerMotor : MonoBehaviour
   {
       [SerializeField] private float moveSpeedUnitsPerSecond = 7f;

       private Rigidbody2D body;
       private PlayerInputReader input;

       private void Awake()
       {
           // GetComponent finds another component on this same GameObject.
           // Doing it once in Awake and caching it avoids the lookup every step.
           body = GetComponent<Rigidbody2D>();
           input = GetComponent<PlayerInputReader>();
       }

       // FixedUpdate runs on the physics clock — 50 times a second by default.
       private void FixedUpdate()
       {
           float desiredHorizontalSpeed = input.HorizontalInput * moveSpeedUnitsPerSecond;

           // Set X, keep Y: the Y component is gravity's, and overwriting it would
           // cancel the fall every step.
           body.linearVelocity = new Vector2(desiredHorizontalSpeed, body.linearVelocity.y);
       }
   }
   ```

   > New concept — **velocity**: how fast and in which direction a body is travelling, in units per second, as
   > a `Vector2`. `Rigidbody2D.linearVelocity` is the property to set — in Unity 6 the older name `velocity`
   > is obsolete and will warn.
   > Reference: <https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Rigidbody2D-linearVelocity.html>

3. Save, return to Unity, and let it compile. Drag `PlayerMotor.cs` onto the **`Player`** object.

4. In the Inspector, confirm `Player Motor (Script)` shows **Move Speed Units Per Second** = `7`. That figure
   is **load-bearing**: it is the number M5's acceleration and M8's dash are tuned against, and it is quoted in
   [`../foundation/conventions.md`](../foundation/conventions.md).

5. Save the scene and press **Play**. Hold **D**: the square runs right along the strip. Hold **A**: it runs
   back. Release: it stops dead — no slide, because you are setting velocity outright rather than pushing.
   (That instant stop is exactly what M5 replaces with acceleration.)

6. Measure the speed rather than trusting it. While holding **D**, expand the **Info** foldout at the bottom
   of the `Rigidbody 2D` component: **Speed** reads `7`. If your Inspector has no Info foldout, do it the
   other way — stop, set `Player`'s Position X to `0`, press Play, hold **D** for two full seconds, and read
   Position X: it will be close to `14`.

## Done when (this step)
- [ ] Holding **D** in Play Mode → the square runs right and keeps up with the key; releasing stops it in the
      same frame.
- [ ] Holding **A** → it runs left; the sprite does not rotate at any point.
- [ ] `Rigidbody 2D` → **Info** → **Speed** reads `7` while a direction is held (or Position X advances by
      about `14` over two seconds).
- [ ] With a gamepad, a half-pushed left stick moves the square at visibly less than full speed.
- [ ] The square never leaves the top surface of the strip while running.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(player): drive horizontal movement through Rigidbody2D velocity
```

## If it breaks
- **The square sinks slowly into the floor while you hold a direction** → you wrote
  `body.linearVelocity = new Vector2(x, 0f)`. Read the existing Y back instead, as the code above does.
- **The square hovers and never falls** → same cause, opposite symptom: something is writing a Y of `0` every
  step. Only `desiredHorizontalSpeed` belongs in the X slot.
- **Movement is jerky at high frame rates** → the velocity is being set in `Update` instead of `FixedUpdate`,
  or `Interpolate` is `None` on the `Rigidbody2D`.
- **`NullReferenceException` in `FixedUpdate`** → `PlayerInputReader` is not on the same GameObject.
  `[RequireComponent]` adds it automatically for *new* attachments; on an object that already existed, add it
  by hand.
- **The square runs but the Inspector's speed field reads 0** → you are reading the `Info` foldout while Play
  Mode is stopped. It only reports during play.

---
> Nav: [← Give the player a body](04_rigidbody-and-collider.md) · [Overview](00_overview.md) · [Verify →](06_verify.md)
