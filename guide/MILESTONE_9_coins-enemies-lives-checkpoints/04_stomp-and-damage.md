# M9 · Step 04 of 06 — Stomp it, or lose a life
> Nav: [← An enemy that patrols](03_enemy.md) · [Overview](00_overview.md) · [Checkpoints and respawn →](05_checkpoints-and-respawn.md)

**Before you start:** [step 03](03_enemy.md) finished — enemies patrol their ledges and the player passes
through them. This step touches **two new files, committed together**: `PlayerHealth.cs` and
`EnemyContact.cs`.

## Glossary for this step
> New here: **[i-frames (invulnerability frames)](../foundation/glossary.md#i-frames-invulnerability-frames)** (defined in *Why / design*).

## Why / design
The same overlap has to mean two opposite things. Land on the enemy from above and it dies; touch it any other
way and you are hurt. Deciding which is the whole step, and there are two ways to do it.

**By contact normal** — ask physics which way the surfaces faced. It works, but the direction the normal
points depends on which collider Unity reports as "this" one, and getting that backwards produces an enemy you
can only kill by walking into its side. That is exactly the bug the platform carrier in
[M7 step 03](../MILESTONE_7_moving-and-one-way-platforms/03_carry-the-rider.md) avoided.

**By position and motion** — the player is stomping when its feet are at or above the enemy's head *and* it is
moving downward. Two facts, both unambiguous, both readable in the Inspector when you are debugging. That is
the one this guide uses, and it is the same shape as the rider test, deliberately.

The other half is **i-frames**. Without them, a single walk into an enemy costs every life you have: the
overlap is reported every physics step, and fifty steps a second is fifty hits.

> New concept — **i-frames (invulnerability frames)**: a period after taking damage during which further
> damage is ignored — **1 second** here — so one contact costs exactly one life. The name is inherited from
> games that counted it in frames rather than seconds.

## Do this

1. Create a new MonoBehaviour script in `Assets/_Project/Scripts` named **`PlayerHealth`** and replace its
   contents with this:

   ```csharp
   // Assets/_Project/Scripts/PlayerHealth.cs — the whole file
   using System;
   using UnityEngine;

   // Owns the run's lives and the invulnerability window. Anything that hurts the
   // player calls TakeDamage; anything that cares about the result subscribes.
   public class PlayerHealth : MonoBehaviour
   {
       [SerializeField] private int startingLives = 3;
       [SerializeField] private float invulnerabilitySeconds = 1f;

       public int LivesRemaining { get; private set; }

       // Raised whenever the count changes. [M10] PlayerAudio subscribes first,
       // then [M11] the HUD.
       public event Action<int> LivesChanged;

       // Raised when a life is lost but the run continues. Step 05 respawns on it.
       public event Action Died;

       private float invulnerableUntilTimeSeconds = float.NegativeInfinity;

       public bool IsInvulnerable => Time.time < invulnerableUntilTimeSeconds;

       private void Awake()
       {
           LivesRemaining = startingLives;
       }

       public void TakeDamage()
       {
           if (IsInvulnerable)
           {
               return;
           }

           LivesRemaining--;
           invulnerableUntilTimeSeconds = Time.time + invulnerabilitySeconds;
           LivesChanged?.Invoke(LivesRemaining);

           if (LivesRemaining <= 0)
           {
               // [M11] replaces this with a Game Over screen. For now the run resets
               // so the level stays playable while you build the rest of it.
               Debug.Log("run over — lives reset");
               LivesRemaining = startingLives;
               LivesChanged?.Invoke(LivesRemaining);
           }
           else
           {
               Debug.Log($"lives = {LivesRemaining}");
           }

           Died?.Invoke();
       }
   }
   ```

2. Create a second MonoBehaviour script named **`EnemyContact`** and replace its contents with this:

   ```csharp
   // Assets/_Project/Scripts/EnemyContact.cs — the whole file
   using UnityEngine;

   // Decides what an overlap with the player means: a stomp from above kills this
   // enemy and bounces the player; anything else costs the player a life.
   [RequireComponent(typeof(Collider2D))]
   public class EnemyContact : MonoBehaviour
   {
       [SerializeField] private float stompBounceVelocityUnitsPerSecond = 10f;

       // How far below this enemy's head the player's feet may be and still stomp.
       [SerializeField] private float stompToleranceUnits = 0.1f;

       private Collider2D ownCollider;

       private void Awake()
       {
           ownCollider = GetComponent<Collider2D>();
       }

       // Enter catches the moment of contact; Stay catches a player who is still
       // standing inside the enemy when their invulnerability runs out.
       private void OnTriggerEnter2D(Collider2D other) => HandleContact(other);
       private void OnTriggerStay2D(Collider2D other) => HandleContact(other);

       private void HandleContact(Collider2D other)
       {
           if (!other.TryGetComponent(out PlayerHealth health))
           {
               return;
           }

           Rigidbody2D playerBody = other.attachedRigidbody;

           bool comingDownOnTop =
               playerBody != null &&
               playerBody.linearVelocity.y < 0f &&
               other.bounds.min.y >= ownCollider.bounds.max.y - stompToleranceUnits;

           if (comingDownOnTop)
           {
               playerBody.linearVelocity = new Vector2(playerBody.linearVelocity.x, stompBounceVelocityUnitsPerSecond);
               Destroy(gameObject);
               return;
           }

           health.TakeDamage();
       }
   }
   ```

3. Save, let Unity compile. Attach **`PlayerHealth`** to the `Player` object, and confirm the Inspector shows
   **Starting Lives** `3` and **Invulnerability Seconds** `1`.

4. Attach **`EnemyContact`** to the **`Enemy` prefab**, not to the instances: double-click
   `Assets/_Project/Prefabs/Enemy.prefab`, drag the script onto the root object, and leave Prefab Mode with
   the **`<`** arrow. Leave **Stomp Bounce Velocity Units Per Second** at `10` and **Stomp Tolerance Units**
   at `0.1`.

5. Save the scene and press **Play**, then try all three:
   - **Walk into an enemy from the side** → the Console prints `lives = 2`, and walking into it again
     immediately does nothing for one second.
   - **Jump onto an enemy from above** → it disappears and the player bounces upward, higher than a step but
     lower than a full jump.
   - **Walk into the second enemy three times** → `lives = 2`, `lives = 1`, then `run over — lives reset`.

6. Check the tolerance is not doing something silly: run into an enemy while *rising* through it — the player
   should be hurt, not stomp it. Only a downward player kills.

## Done when (this step)
- [ ] Walking into an enemy → one life lost, printed to the Console, and no further loss for one second.
- [ ] Standing inside an enemy for more than a second → a second life is lost, once per second, not per frame.
- [ ] Landing on an enemy from above → the enemy is destroyed and the player bounces upward.
- [ ] Rising into an enemy from below → a life lost, **not** a stomp.
- [ ] Losing the third life prints `run over — lives reset` and the count returns to `3`.
- [ ] Coins, the platform, the one-way ledge and the whole moveset still behave.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(gameplay): add lives, i-frames and stomp-or-be-hurt enemy contact
```

## If it breaks
- **Every touch drains all three lives at once** → the i-frame check is missing or `invulnerabilitySeconds` is
  `0`. `TakeDamage` must return early while `IsInvulnerable`.
- **The player is hurt when landing on the enemy** → the stomp test is inverted, or the player is not actually
  falling at contact: a stomp only counts with `linearVelocity.y < 0`.
- **The enemy dies when the player walks into its side** → `stompToleranceUnits` is far too large, so the feet
  count as "above the head" from anywhere. `0.1` against an enemy 0.8 units tall is right.
- **Nothing happens at all** → `EnemyContact` landed on an instance rather than on the prefab, or the enemy's
  collider is not a trigger.
- **`Died` never seems to do anything** → correct for now: [step 05](05_checkpoints-and-respawn.md) is the
  subscriber.

---
> Nav: [← An enemy that patrols](03_enemy.md) · [Overview](00_overview.md) · [Checkpoints and respawn →](05_checkpoints-and-respawn.md)
