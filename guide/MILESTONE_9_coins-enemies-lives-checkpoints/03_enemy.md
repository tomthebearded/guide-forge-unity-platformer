# M9 · Step 03 of 06 — An enemy that patrols
> Nav: [← Collect coins](02_collect-coins.md) · [Overview](00_overview.md) · [Stomp it, or lose a life →](04_stomp-and-damage.md)

**Before you start:** [step 02](02_collect-coins.md) finished — coins are collected and counted in the
Console.

## Why / design
The enemy walks a ledge and turns round at the end of it. That is the entire behaviour, and it is the right
amount: an enemy that patrols predictably is something the player can *read* and plan around, which is what
makes a platformer level a puzzle rather than a lottery.

Two decisions worth stating.

**It is kinematic and moved by script**, like the platform in M7 — for the same reason. A dynamic enemy would
be shoved around by the player and would fall off its own ledge.

**Its collider is a trigger, on the `Hazard` layer.** The player passes through it rather than standing on it,
and [step 04](04_stomp-and-damage.md) decides what the overlap *means* — a stomp or a hit — by looking at
where the player is. Making the enemy solid would mean the player could stand on its head indefinitely, which
is a different game.

Turning round needs two questions: *is there still floor ahead of me?* and *is there a wall in front of me?*
Both are the overlap trick from the ground check, pointed somewhere new. The floor probe is the interesting
one — it is why the enemy stops at the edge of a ledge instead of walking into the air.

## Do this

1. In the **Hierarchy**, right-click and choose **2D Object > Sprites > Square**. Rename it **`Enemy`**, set
   its `Transform` **Scale** to `0.8, 0.8, 1`, and set its `Sprite Renderer` **Color** to a red, hex `C1443C`
   (cosmetic).

2. Set the object's **Layer** (top right of the Inspector) to **`Hazard`** — reserved back in
   [M4 step 02](../MILESTONE_4_gravity-and-jumping/02_ground-layer.md) and used for real from here.

3. **Add Component > Box Collider 2D**. Leave **Size** `1, 1` and **Offset** `0, 0`, and tick **Is Trigger**.

4. **Add Component > Rigidbody 2D**. Set **Body Type** to **`Kinematic`** and **Interpolate** to
   **`Interpolate`**. Leave the rest at their defaults.

5. Create a new MonoBehaviour script in `Assets/_Project/Scripts` named **`EnemyPatrol`** and replace its
   contents with this:

   ```csharp
   // Assets/_Project/Scripts/EnemyPatrol.cs — the whole file
   using UnityEngine;

   // Walks back and forth along a surface, turning round at a ledge or a wall.
   // Kinematic and script-driven, so the player cannot push it off its own platform.
   [RequireComponent(typeof(Rigidbody2D))]
   public class EnemyPatrol : MonoBehaviour
   {
       [SerializeField] private float speedUnitsPerSecond = 2f;
       [SerializeField] private LayerMask groundLayers;

       [Header("Probes")]
       [SerializeField] private float probeDistanceAheadUnits = 0.45f;
       [SerializeField] private float floorProbeDepthUnits = 0.55f;
       [SerializeField] private float probeRadiusUnits = 0.08f;

       private Rigidbody2D body;

       // +1 walking right, -1 walking left.
       private int direction = 1;

       private void Awake()
       {
           body = GetComponent<Rigidbody2D>();
       }

       private void FixedUpdate()
       {
           if (!IsFloorAhead() || IsWallAhead())
           {
               direction = -direction;
           }

           Vector2 step = Vector2.right * (direction * speedUnitsPerSecond * Time.fixedDeltaTime);
           body.MovePosition(body.position + step);
       }

       private Vector2 AheadPosition()
       {
           return body.position + Vector2.right * (direction * probeDistanceAheadUnits);
       }

       private bool IsFloorAhead()
       {
           // A small circle just beyond the leading foot, below the enemy's feet.
           Vector2 probe = AheadPosition() + Vector2.down * floorProbeDepthUnits;
           return Physics2D.OverlapCircle(probe, probeRadiusUnits, groundLayers) != null;
       }

       private bool IsWallAhead()
       {
           // The same circle, at body height rather than below it.
           return Physics2D.OverlapCircle(AheadPosition(), probeRadiusUnits, groundLayers) != null;
       }

       private void OnDrawGizmosSelected()
       {
           Vector3 ahead = transform.position + Vector3.right * (direction * probeDistanceAheadUnits);

           Gizmos.color = Color.magenta;
           Gizmos.DrawWireSphere(ahead + Vector3.down * floorProbeDepthUnits, probeRadiusUnits);
           Gizmos.DrawWireSphere(ahead, probeRadiusUnits);
       }
   }
   ```

6. Save, let Unity compile, and drag `EnemyPatrol.cs` onto the `Enemy` object. In the Inspector set
   **Ground Layers** to **`Ground`** — and only `Ground`, so the enemy treats a one-way ledge as thin air and
   stays on solid floor.

7. Position the enemy on one of your ledges — somewhere with a clear edge on at least one side. With it
   selected, the Scene view shows two magenta circles ahead of it: one at body height (the wall probe) and one
   below the feet (the floor probe).

8. Drag the `Enemy` from the Hierarchy into `Assets/_Project/Prefabs` to make it a prefab, then place a second
   one somewhere else in the cavern.

9. Save the scene and press **Play**. Each enemy walks to the end of its ledge, turns, and walks back. Run
   into one: you pass straight through it and nothing happens — the trigger is being ignored because nothing
   is listening yet, which is [step 04](04_stomp-and-damage.md).

## Done when (this step)
- [ ] Each `Enemy` walks along its ledge and **turns round at the edge** rather than walking off it.
- [ ] An enemy that meets a wall turns round there too.
- [ ] Selecting an enemy shows two magenta probe circles ahead of it in the Scene view, which flip to the
      other side when it turns.
- [ ] The enemy's Layer reads `Hazard`, its collider has **Is Trigger** ticked, and its `Rigidbody 2D` is
      `Kinematic`.
- [ ] `Assets/_Project/Prefabs/Enemy.prefab` exists and at least two instances are placed.
- [ ] Running into an enemy → the player passes through, nothing happens yet.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(enemy): add a patrolling enemy that turns at ledges and walls
```

## If it breaks
- **The enemy walks off the edge and keeps going** → **Ground Layers** is `Nothing`, so the floor probe never
  finds anything and `IsFloorAhead()` is always false — which flips the direction every step and looks like
  vibration, or never finds floor at all. Set it to `Ground`.
- **The enemy vibrates on the spot** → the two probes are both hitting: the wall probe is finding the floor
  because `floorProbeDepthUnits` is too small for the enemy's size. `0.55` suits a scale of `0.8`.
- **The enemy sinks or floats** → nothing sets its height; it keeps whatever Y you gave it. Place it so its
  feet sit on the surface, since a kinematic body ignores gravity by design.
- **The enemy passes through walls instead of turning** → the wall probe's layer mask excludes the tilemap, or
  the probe distance is shorter than half the enemy's width. `0.45` suits a scale of `0.8`.

---
> Nav: [← Collect coins](02_collect-coins.md) · [Overview](00_overview.md) · [Stomp it, or lose a life →](04_stomp-and-damage.md)
