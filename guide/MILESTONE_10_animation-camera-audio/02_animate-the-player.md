# M10 · Step 02 of 07 — Animate the player
> Nav: [← Dress the player](01_dress-the-player.md) · [Overview](00_overview.md) · [The camera follows →](03_cinemachine-camera.md)

**Before you start:** [step 01](01_dress-the-player.md) finished — the player wears a character sprite on the
`Player` sorting layer.

## Glossary for this step
> New here: **[animation clip](../foundation/glossary.md#animation-clip)** (defined in *Do this*, action 2) ·
> **[Animator controller](../foundation/glossary.md#animator-controller)** (defined in *Why / design*) ·
> **[Animator parameter](../foundation/glossary.md#animator-parameter)** (defined in *Do this*, action 5).

## Why / design
Animation in Unity is its own state machine, sitting beside the one you wrote in M8 — and the two must not
know about each other in detail. The rule that keeps them apart is: **code sets parameters; the controller
decides clips.** Your movement code never says "play the jump clip". It says "vertical velocity is 6.2 and I
am not grounded", and the controller works out what that looks like.

> New concept — **Animator controller**: an asset holding animation states, the transitions between them, and
> the parameters those transitions read. It is a graph you edit visually, and it is the only thing that
> chooses which clip plays.

That indirection is worth the extra asset. It means an artist can add a landing squash, retime a run cycle or
insert a whole extra state without a line of C# changing — and it means your `PlayerMotor`, which is already
doing enough, never grows a rendering concern.

Four states, four parameters, and one small component that publishes the parameters every frame.

## Do this

1. Select the `Player`. Open **Window > Animation > Animation** and dock the panel somewhere you can see it.
   It reads *"To begin animating Player, create an Animation Clip"* with a **Create** button.

2. Press **Create**. Save the clip as `PlayerIdle.anim` in `Assets/_Project/Animation`. Unity does three
   things at once: it writes the clip, it writes an **Animator controller** next to it, and it adds an
   **`Animator`** component to the `Player`.

   > New concept — **animation clip**: a recorded change to some properties over time — here, which sprite
   > the `SpriteRenderer` shows on each frame.

3. With `PlayerIdle` selected in the Animation panel's clip dropdown, drag your character's **standing**
   sprite from the Project panel into the panel's timeline area. Set the frame rate — the **Samples** field —
   to `8`, which is a comfortable speed for pixel art. If your character has two idle poses, drag both; if it
   has one, one is fine and the idle simply holds.

4. Create the other three clips from the same dropdown — click it, choose **Create New Clip…** — saving each
   into `Assets/_Project/Animation`:
   - **`PlayerRun`** — the walking poses, `Samples` `10`. Two frames is a perfectly good run cycle in pixel
     art; use the same pose twice if your character has only one, and the state machine still works.
   - **`PlayerJump`** — the rising pose (or the standing one), a single frame.
   - **`PlayerFall`** — the falling pose (or the standing one), a single frame.

5. Open **Window > Animation > Animator** to see the controller graph. It shows your four clips as boxes, one
   of them orange (the default state). In the **Parameters** tab at the top left, press **+** and add exactly
   these four — the names are **load-bearing** and match
   [`../foundation/conventions.md`](../foundation/conventions.md):

   | Name | Type |
   |---|---|
   | `Speed` | `Float` |
   | `IsGrounded` | `Bool` |
   | `VerticalVelocity` | `Float` |
   | `IsDashing` | `Bool` |

   > New concept — **Animator parameter**: a named value the controller reads to decide transitions. Your code
   > writes them with `SetFloat` and `SetBool`; nothing else may.

6. Right-click `PlayerIdle` in the graph and choose **Set as Layer Default State** if it is not already
   orange. Then wire the transitions by right-clicking a state, choosing **Make Transition**, and clicking the
   target. For each one, select the arrow and set its **Conditions** in the Inspector — and untick **Has Exit
   Time** on every single one, or the transition waits for the clip to finish before it will fire, which
   reads as a character that responds late:

   | From | To | Conditions |
   |---|---|---|
   | `PlayerIdle` | `PlayerRun` | `Speed` **Greater** `0.1` |
   | `PlayerRun` | `PlayerIdle` | `Speed` **Less** `0.1` |
   | `Any State` | `PlayerJump` | `IsGrounded` **false**, `VerticalVelocity` **Greater** `0.1` |
   | `Any State` | `PlayerFall` | `IsGrounded` **false**, `VerticalVelocity` **Less** `-0.1` |
   | `PlayerJump` | `PlayerIdle` | `IsGrounded` **true** |
   | `PlayerFall` | `PlayerIdle` | `IsGrounded` **true** |

   `Any State` is the box already in the graph: a transition from it can fire whatever is currently playing,
   which is exactly right for "you are in the air now".

7. Create a new MonoBehaviour script in `Assets/_Project/Scripts` named **`PlayerAnimationDriver`**:

   ```csharp
   // Assets/_Project/Scripts/PlayerAnimationDriver.cs — the whole file
   using UnityEngine;

   // The one bridge from movement to animation: it publishes parameters and nothing
   // else. It never names a clip, so the controller stays free to change.
   [RequireComponent(typeof(Animator))]
   [RequireComponent(typeof(PlayerMotor))]
   [RequireComponent(typeof(Rigidbody2D))]
   public class PlayerAnimationDriver : MonoBehaviour
   {
       private Animator animator;
       private PlayerMotor motor;
       private Rigidbody2D body;
       private SpriteRenderer spriteRenderer;

       // Hashing the names once is the idiomatic way to set parameters: comparing
       // ints every frame beats comparing strings every frame.
       private static readonly int SpeedId = Animator.StringToHash("Speed");
       private static readonly int IsGroundedId = Animator.StringToHash("IsGrounded");
       private static readonly int VerticalVelocityId = Animator.StringToHash("VerticalVelocity");
       private static readonly int IsDashingId = Animator.StringToHash("IsDashing");

       private void Awake()
       {
           animator = GetComponent<Animator>();
           motor = GetComponent<PlayerMotor>();
           body = GetComponent<Rigidbody2D>();
           spriteRenderer = GetComponent<SpriteRenderer>();
       }

       // Animation is a rendering concern, so it belongs on the frame clock.
       private void Update()
       {
           animator.SetFloat(SpeedId, Mathf.Abs(body.linearVelocity.x));
           animator.SetBool(IsGroundedId, motor.IsGrounded);
           animator.SetFloat(VerticalVelocityId, body.linearVelocity.y);
           animator.SetBool(IsDashingId, motor.IsDashing);

           // Face the way you are going. Flipping the renderer costs nothing and
           // avoids a second set of mirrored sprites.
           if (Mathf.Abs(body.linearVelocity.x) > 0.1f)
           {
               spriteRenderer.flipX = body.linearVelocity.x < 0f;
           }
       }
   }
   ```

8. That script reads `motor.IsDashing`, which does not exist yet. In
   `Assets/_Project/Scripts/PlayerMotor.cs`, **ADD** this property directly below the existing
   `public int WallDirection { get; private set; }` line, so the project compiles:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — below the WallDirection property
   // True while the dash state is running. Read by the animation driver.
   public bool IsDashing => state == PlayerMovementState.Dashing;
   ```

9. Save both files, let Unity compile, and drag `PlayerAnimationDriver.cs` onto the `Player`.

10. Save the scene and press **Play**. Stand still — the idle plays. Run — the run plays, and the sprite faces
    the way you move. Jump — the jump pose while rising, the fall pose while falling, back to idle on landing.

## Done when (this step)
- [ ] Standing still plays `PlayerIdle`; running plays `PlayerRun`; the sprite flips to face the direction of
      travel.
- [ ] Rising plays `PlayerJump` and falling plays `PlayerFall`, with the switch happening at the top of the
      arc rather than on landing.
- [ ] Landing returns to `PlayerIdle` (or `PlayerRun` if you are still holding a direction) **immediately** —
      no wait for a clip to finish.
- [ ] Selecting `Player` during Play Mode and watching the **Animator** window shows the active state moving
      around the graph as you play, and the four parameters changing.
- [ ] `PlayerMotor` exposes `IsDashing`, and the Animator's `IsDashing` parameter goes true for the length of
      a dash.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(player): animate the character from movement parameters
```

## If it breaks
- **The character responds a beat late** → **Has Exit Time** is still ticked on a transition. Untick it on all
  six.
- **The animation flickers between two states** → your thresholds are too close to zero. `0.1` on `Speed`
  gives a dead zone; `0` makes floating-point noise flip the state.
- **`Parameter 'Speed' does not exist`** in the Console → the name in the Animator differs from the string in
  `StringToHash`. They are case-sensitive.
- **Nothing animates at all** → the `Animator` component's **Controller** field is empty, which happens if the
  controller asset was moved after creation. Drag it back into the field.
- **The sprite faces backwards** → the artwork points left by default; invert the `flipX` comparison.
- **The jump state never plays** → the `Any State` transitions have their conditions the wrong way round, or
  `IsGrounded` is being set from a stale value; it comes from `PlayerMotor.IsGrounded`, updated each physics
  step.

---
> Nav: [← Dress the player](01_dress-the-player.md) · [Overview](00_overview.md) · [The camera follows →](03_cinemachine-camera.md)
