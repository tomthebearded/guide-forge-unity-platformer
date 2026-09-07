# M9 · Step 04 of 06 — Stomp it, or lose a life
> Nav: [← An enemy that patrols](03_enemy.md) · [Overview](00_overview.md) · [Checkpoints and respawn →](05_checkpoints-and-respawn.md)
> ⚠️ **Superseded 2026-09-07** — the stomp test compared the player's feet with the enemy's *head* inside a
> 0.1-unit tolerance, which a falling player crosses in a single physics step, so landing on an enemy hurt
> you instead of killing it. Don't follow this step as written: the correction that brings it up to date is
> under *Before you continue — corrections* in [../MILESTONE_11_scenes-menus-hud-persistence/07_verify.md](../MILESTONE_11_scenes-menus-hud-persistence/07_verify.md).

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

**By position and motion** — the player is stomping when it comes down on the enemy's **upper half** *and* it
is moving downward. Two facts, both unambiguous, both readable in the Inspector when you are debugging. That
is the one this guide uses, and it is the same shape as the rider test, deliberately.

The reference point is where this gets decided, and "the enemy's head" is the wrong one. A trigger callback
runs **after** the physics step that produced the overlap: by the time Unity tells you the two boxes touch,
the player has already moved into the enemy by as much as `|velocity.y| × 0.02`. Falling is fast here — a drop
of four units arrives at `22` units per second, which is `0.44` units of sink in one step, and a measured
contact at `5.41` u/s already sat `0.128` below the head. Against a `0.1` tolerance the test reads false and
the player takes the hit, and no larger tolerance is available: one big enough to survive a fast landing would
also call a walk into the enemy's side a stomp.

Measuring against the **centre** has room instead of a margin. The upper half of an enemy `0.8` units tall is
`0.4` deep, so "came down on its top half" is still true when your code finally looks — for any landing up to
`20` units per second, which covers every ordinary jump — and it is still false for a player walking into its
side. Above that speed it becomes very likely rather than certain, and this game accepts that: measured drops
from one, four and eight units (impacts `10.7`, `22.0` and `31.9` u/s) all stomped.

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

       // Raised on every life lost, the last one included — PlayerRespawn (step 05)
       // puts the player back at the checkpoint either way. [M11] splits the last
       // life off into an event of its own.
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

           // Against the centre, not the head: this callback runs after the physics
           // step, and a falling player is already well inside the enemy by now.
           bool comingDownOnTop =
               playerBody != null &&
               playerBody.linearVelocity.y < 0f &&
               other.bounds.min.y >= ownCollider.bounds.center.y;

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
   the **`<`** arrow. Leave **Stomp Bounce Velocity Units Per Second** at `10` — it is the only field.

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
- [ ] Landing on an enemy from above → the enemy is destroyed and the player bounces upward. Test it from a
      **full jump**, from as high as you can reach, not by stepping off a ledge beside it: a gentle approach
      passes even when a fast one does not, and a fast one is what the game is played at.
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
- **Landing on an enemy from a jump hurts you, but stepping onto one from a ledge kills it** → the test is
  measuring against the enemy's *head* with a small tolerance. The callback runs after the physics step, so a
  fast fall is already deeper than any such tolerance. Compare against `ownCollider.bounds.center.y` instead.
- **The enemy dies when the player walks into its side** → the comparison is against something lower than the
  enemy's centre, so the feet count as "on top" from anywhere. `bounds.center.y` is the line.
- **Nothing happens at all** → `EnemyContact` landed on an instance rather than on the prefab, or the enemy's
  collider is not a trigger.
- **`Died` never seems to do anything** → correct for now: [step 05](05_checkpoints-and-respawn.md) is the
  subscriber.

---
> Nav: [← An enemy that patrols](03_enemy.md) · [Overview](00_overview.md) · [Checkpoints and respawn →](05_checkpoints-and-respawn.md)
