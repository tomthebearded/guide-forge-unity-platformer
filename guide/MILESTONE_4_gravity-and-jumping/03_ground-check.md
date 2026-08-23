# M4 · Step 03 of 06 — Ask whether the player is grounded
> Nav: [← Put the ground on its own layer](02_ground-layer.md) · [Overview](00_overview.md) · [Jump once per press →](04_jump.md)

**Before you start:** [step 02](02_ground-layer.md) finished — the `Ground` object is on the `Ground` layer
and the three layers exist.

## Glossary for this step
> New here: **[ground check](../foundation/glossary.md#ground-check)** (defined in *Why / design*) ·
> **[gizmo](../foundation/glossary.md#gizmo)** (defined in *Do this*, action 4).

## Why / design
"Am I standing on something?" has no built-in answer in Unity, and the two obvious candidates are both traps.
`OnCollisionStay2D` tells you that you are touching something, but not *where* — a character pressed against a
wall is touching it and cannot jump off it. Counting collisions gets you a character that can jump while
scraping a ceiling.

The reliable answer is a **ground check**: every physics step, ask whether a small box just below the feet
overlaps anything on the `Ground` layer. It is one line, it is explicit, and — this matters from M7 — it keeps
working when the thing under your feet is a moving platform or a one-way ledge.

> New concept — **ground check**: an overlap query run each physics step in a small region under the
> character's feet, filtered to the layers that count as ground. The result is the `IsGrounded` flag every
> other movement rule is built on.

The box is **wider than it is tall and narrower than the player**: 0.9 by 0.12 units against a player one unit
wide. Too tall and the player is "grounded" while still a hand's width above the floor; too wide and it clips
the wall beside a ledge and grants a jump the player has not earned.

## Do this

1. Open `Assets/_Project/Scripts/PlayerMotor.cs`. **ADD** these fields directly below the existing
   `moveSpeedUnitsPerSecond` line:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — below the moveSpeedUnitsPerSecond field
   [Header("Ground check")]
   [SerializeField] private Vector2 groundCheckSizeUnits = new Vector2(0.9f, 0.12f);
   [SerializeField] private float groundCheckDistanceBelowCentreUnits = 0.5f;
   [SerializeField] private LayerMask groundLayers;

   // True while a solid surface is directly under the feet. Read by the jump (step 04).
   public bool IsGrounded { get; private set; }
   ```

   `groundCheckDistanceBelowCentreUnits` is `0.5` because the player is one unit tall and its origin is at its
   centre, so its feet are half a unit down.

2. **ADD** this method to the same class, directly below the existing `Awake` method:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — below Awake()
   private void UpdateGroundedState()
   {
       Vector2 boxCentre = (Vector2)transform.position + Vector2.down * groundCheckDistanceBelowCentreUnits;

       // OverlapBox(point, size, angle, layerMask) returns the first collider it finds
       // in that box on those layers, or null if there is none.
       IsGrounded = Physics2D.OverlapBox(boxCentre, groundCheckSizeUnits, 0f, groundLayers) != null;
   }
   ```

3. **ADD** a call to it as the **first** line inside the existing `FixedUpdate`, above the
   `desiredHorizontalSpeed` line — the flag must be current before anything reads it:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — first line inside FixedUpdate()
   UpdateGroundedState();
   ```

4. **ADD** this method at the end of the class, after `FixedUpdate`. It draws the check box in the Scene view
   so you can see what you are testing rather than imagining it:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — after FixedUpdate()
   // Unity calls this in the Editor while the object is selected. Editor-only: it
   // is stripped from a build and costs nothing at runtime.
   private void OnDrawGizmosSelected()
   {
       Gizmos.color = Color.green;
       Vector3 boxCentre = transform.position + Vector3.down * groundCheckDistanceBelowCentreUnits;
       Gizmos.DrawWireCube(boxCentre, new Vector3(groundCheckSizeUnits.x, groundCheckSizeUnits.y, 0f));
   }
   ```

   > New concept — **gizmo**: a shape Unity draws in the Scene view for your benefit only — never in the game.
   > `OnDrawGizmosSelected` runs while the object is selected. Making an invisible test visible is the
   > cheapest debugging you will ever do.

5. Save and let Unity compile. Select `Player`: the Inspector now shows a **Ground check** header with three
   fields, and the Scene view shows a thin green rectangle under the square's feet.

6. Set **Ground Layers** — the `LayerMask` field — by opening its dropdown and ticking **`Ground`**, and only
   `Ground`. It reads `Ground` when set correctly, and `Nothing` when it is not. **This is load-bearing and it
   is the single most common thing to forget in this milestone**: an unset mask means the query looks at no
   layers at all and `IsGrounded` is false for ever.

7. Save the scene and press **Play**. Select `Player` while it runs: in the Inspector, `Player Motor (Script)`
   does not show `IsGrounded` (it is a property, not a serialized field), so check it the direct way — walk
   the player off the end of the strip with **D** held. It falls. That fall is the ground check's answer
   becoming false in a way you can see; [step 04](04_jump.md) makes it audible in the mechanics.

## Done when (this step)
- [ ] Selecting `Player` shows a green wireframe rectangle in the **Scene** view, sitting just under the
      square, wider than it is tall and slightly narrower than the square itself.
- [ ] `Player Motor (Script)` shows **Ground Layers** reading `Ground` — not `Nothing`, not `Mixed...`.
- [ ] The green box overlaps the top of the `Ground` strip when the player is resting on it, and clears it
      entirely when the player is in the air.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(player): add a layer-filtered ground check to PlayerMotor
```

## If it breaks
- **The green box is drawn in the middle of the player, not below it** →
  `groundCheckDistanceBelowCentreUnits` is `0`, or you used `Vector3.up`.
- **`Ground Layers` shows `Mixed...`** → more than one layer is ticked. Open the dropdown, choose
  **Nothing**, then tick `Ground` alone.
- **The gizmo does not appear at all** → the **Gizmos** toggle in the Scene view's toolbar is off, or the
  object is not selected (`OnDrawGizmosSelected` only draws for the selection).
- **The compiler complains about `Vector2.down`** → it exists; the likely typo is `Vector2.Down`. C# is
  case-sensitive and Unity's constants are lowercase.

---
> Nav: [← Put the ground on its own layer](02_ground-layer.md) · [Overview](00_overview.md) · [Jump once per press →](04_jump.md)
