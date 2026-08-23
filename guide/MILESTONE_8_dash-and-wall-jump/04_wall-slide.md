# M8 · Step 04 of 06 — Cling to a wall
> Nav: [← Dash](03_dash.md) · [Overview](00_overview.md) · [Jump off the wall →](05_wall-jump.md)

**Before you start:** [step 03](03_dash.md) finished — the dash covers about five units and respects its
cooldown.

## Why / design
A wall slide is two things: **knowing there is a wall**, and **falling slowly while you hold yourself against
it**.

Knowing is the ground check again, turned sideways. Two overlap boxes, one on each flank, tall and thin
instead of wide and flat — and reported as a **direction** (`-1`, `0`, `+1`) rather than a pair of booleans,
because every rule downstream wants to know *which* wall, and a direction makes the wall-jump in
[step 05](05_wall-jump.md) a single multiplication instead of a branch.

The layer mask is **`Ground` only**, deliberately not `OneWay`. A one-way ledge is something you pass through;
clinging to the side of it would be absurd, and keeping the two masks separate is exactly why M4 reserved
three layer names instead of one.

Sliding is a **clamp, not a replacement**: the fall speed is limited to 2.5 units per second, so gravity still
does the work and the wall just slows it. Entering the state needs three conditions together — airborne, a
wall on the side you are pressing towards, and already falling. That last one keeps a rising jump from
snagging on a wall halfway up.

## Do this

1. In `Assets/_Project/Scripts/PlayerMotor.cs`, **ADD** these fields below the `facingDirection` field:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — below the facingDirection field
   [Header("Wall")]
   [SerializeField] private Vector2 wallCheckSizeUnits = new Vector2(0.12f, 0.8f);
   [SerializeField] private float wallCheckDistanceFromCentreUnits = 0.5f;
   [SerializeField] private LayerMask wallLayers;
   [SerializeField] private float wallSlideSpeedUnitsPerSecond = 2.5f;

   // -1 = wall on the left, +1 = wall on the right, 0 = neither.
   public int WallDirection { get; private set; }
   ```

2. **ADD** this method below the existing `UpdateGroundedState` method:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — below UpdateGroundedState()
   private void UpdateWallContact()
   {
       Vector2 centre = body.position;
       Vector2 offset = Vector2.right * wallCheckDistanceFromCentreUnits;

       bool wallOnRight = Physics2D.OverlapBox(centre + offset, wallCheckSizeUnits, 0f, wallLayers) != null;
       bool wallOnLeft = Physics2D.OverlapBox(centre - offset, wallCheckSizeUnits, 0f, wallLayers) != null;

       // If somehow both, prefer the one being pressed towards; ties go to the right.
       WallDirection = wallOnRight ? 1 : wallOnLeft ? -1 : 0;
   }
   ```

3. **ADD** the call to `FixedUpdate`, directly below `UpdateGroundedState();`:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — in FixedUpdate(), below UpdateGroundedState()
   UpdateWallContact();
   ```

4. **ADD** this helper below `UpdateWallContact`. Both this step and
   [step 05](05_wall-jump.md) ask the same question, so it is worth a name:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — below UpdateWallContact()
   private bool IsPressingIntoWall()
   {
       if (WallDirection == 0 || Mathf.Approximately(input.HorizontalInput, 0f))
       {
           return false;
       }

       // Same sign means the input points at the wall the player is touching.
       return Mathf.Sign(input.HorizontalInput) == WallDirection;
   }
   ```

5. **ADD** this method below `TickDashingState`:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — below TickDashingState()
   private void TickWallSlidingState()
   {
       // Leave the moment any of the three conditions stops holding.
       if (IsGrounded || !IsPressingIntoWall())
       {
           state = PlayerMovementState.Normal;
           body.gravityScale = baseGravityScale;
           return;
       }

       // Clamp the fall rather than replacing it: Mathf.Max keeps the larger
       // (less negative) of the two, so gravity may pull slower but never faster.
       float clampedFallSpeed = Mathf.Max(body.linearVelocity.y, -wallSlideSpeedUnitsPerSecond);
       body.linearVelocity = new Vector2(0f, clampedFallSpeed);
   }
   ```

6. **ADD** the case to the `switch` in `FixedUpdate`, below the `Dashing` case:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — in FixedUpdate()'s switch, below the Dashing case
   case PlayerMovementState.WallSliding:
       TickWallSlidingState();
       break;
   ```

7. **ADD** the transition into the state at the **end** of `TickNormalState`, directly below the
   `ApplyJumpGravityMultipliers();` line:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — last lines inside TickNormalState()
   if (!IsGrounded && IsPressingIntoWall() && body.linearVelocity.y < 0f)
   {
       state = PlayerMovementState.WallSliding;
   }
   ```

8. **ADD** the wall boxes to the gizmo method, directly below the existing `Gizmos.DrawWireCube` line in
   `OnDrawGizmosSelected`:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — in OnDrawGizmosSelected(), below the existing DrawWireCube
   Gizmos.color = Color.yellow;
   Vector3 wallOffset = Vector3.right * wallCheckDistanceFromCentreUnits;
   Vector3 wallBoxSize = new Vector3(wallCheckSizeUnits.x, wallCheckSizeUnits.y, 0f);
   Gizmos.DrawWireCube(transform.position + wallOffset, wallBoxSize);
   Gizmos.DrawWireCube(transform.position - wallOffset, wallBoxSize);
   ```

9. Save, let Unity compile, then select `Player` and set **Wall Layers** to **`Ground`** — and only `Ground`.
   Leave `OneWay` unticked.

10. Look at the **Scene** view with `Player` selected: two thin yellow boxes now stand at its sides, alongside
    the green one under its feet.

11. Save the scene and press **Play**. Jump at one of the cavern's end walls and **hold the direction into it**
    while falling: the player slows to a controlled slide down the wall. Let go of the direction: it drops
    normally again.

    If your painted cavern has no wall tall enough to slide down, paint one now — three or four cells is
    plenty. Painting is [M6 step 04](../MILESTONE_6_tilemap-level/04_paint-the-level.md), and the palette is
    still where you left it.

## Done when (this step)
- [ ] Selecting `Player` shows two thin yellow boxes at its left and right sides in the Scene view.
- [ ] `Player Motor (Script)` → **Wall Layers** reads `Ground`.
- [ ] Jumping into a wall while holding the direction into it → the fall slows visibly to a steady slide.
- [ ] Releasing the direction, or touching the ground, ends the slide immediately and the player falls
      normally.
- [ ] Rising *up* past a wall while holding into it does **not** trigger the slide — it only starts once the
      player is falling.
- [ ] Sliding down the side of the `OneWayLedge` does **not** happen (its layer is not in the mask).
- [ ] The dash and everything before it still behave.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(player): add wall detection and a controlled wall slide
```

## If it breaks
- **The slide never starts** → **Wall Layers** is `Nothing`, or the wall check boxes are too far from the
  player's centre. `0.5` matches a one-unit-wide player exactly.
- **The player sticks to walls without any input** → the `IsPressingIntoWall()` condition is missing from the
  transition in action 7.
- **The player slides up walls** → the `body.linearVelocity.y < 0f` condition is missing, so a rising jump
  enters the state and gets clamped.
- **The player slides in mid-air, nowhere near a wall** → the wall boxes are enormous. Check
  **Wall Check Size Units** reads `0.12, 0.8`.
- **The slide is jerky** → `Mathf.Max` has become `Mathf.Min`, which clamps the wrong way and fights gravity
  every step.

---
> Nav: [← Dash](03_dash.md) · [Overview](00_overview.md) · [Jump off the wall →](05_wall-jump.md)
