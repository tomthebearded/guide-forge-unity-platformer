# M9 · Step 05 of 06 — Checkpoints and respawn
> Nav: [← Stomp it, or lose a life](04_stomp-and-damage.md) · [Overview](00_overview.md) · [Verify →](06_verify.md)

**Before you start:** [step 04](04_stomp-and-damage.md) finished — lives are lost, i-frames work, and stomping
kills an enemy. This step touches **three new files, committed together**: `PlayerRespawn.cs`,
`Checkpoint.cs` and `KillZone.cs`.

## Why / design
Losing a life currently costs nothing: the player stands exactly where they were and carries on. A platformer
needs the loss to *move* you — back to somewhere you earned — and it needs falling off the world to count as
losing a life rather than as falling for ever.

Three small pieces, each with one job:

- **`PlayerRespawn`** remembers where to come back to and does the coming back. It subscribes to
  `PlayerHealth.Died`, which is why step 04 raised an event instead of calling a method: health knows nothing
  about respawning, and respawning knows nothing about enemies.
- **`Checkpoint`** is a trigger that hands `PlayerRespawn` a new position and then switches itself off.
- **`KillZone`** is a wide trigger under the level that costs a life the moment the player falls into it.

The kill zone needs to bypass invulnerability. A player who takes a hit and then falls has one second of
i-frames left, and without the bypass they would fall through the kill zone unharmed and keep falling for
ever. Falling out of the world is never survivable, so it says so explicitly.

## Do this

1. Create a new MonoBehaviour script in `Assets/_Project/Scripts` named **`PlayerRespawn`**:

   ```csharp
   // Assets/_Project/Scripts/PlayerRespawn.cs — the whole file
   using UnityEngine;

   // Remembers the last checkpoint and puts the player back there when a life is lost.
   [RequireComponent(typeof(PlayerHealth))]
   [RequireComponent(typeof(Rigidbody2D))]
   public class PlayerRespawn : MonoBehaviour
   {
       private PlayerHealth health;
       private Rigidbody2D body;
       private Vector2 respawnPosition;

       private void Awake()
       {
           health = GetComponent<PlayerHealth>();
           body = GetComponent<Rigidbody2D>();

           // Until a checkpoint is touched, the start of the level is the checkpoint.
           respawnPosition = body.position;
       }

       // OnEnable/OnDisable are the safe place to subscribe and unsubscribe: an
       // event still holding a destroyed object is a leak and, eventually, an error.
       private void OnEnable()
       {
           health.Died += RespawnAtCheckpoint;
       }

       private void OnDisable()
       {
           health.Died -= RespawnAtCheckpoint;
       }

       public void SetCheckpoint(Vector2 position)
       {
           respawnPosition = position;
           Debug.Log($"checkpoint set at {position}");
       }

       private void RespawnAtCheckpoint()
       {
           // Stop dead first: arriving with the fall's velocity would drop the player
           // straight back through the floor they respawned above.
           body.linearVelocity = Vector2.zero;

           // MovePosition would sweep through the level; a respawn is a teleport.
           body.position = respawnPosition;

           // The transform is normally the engine's to write, but a body that was
           // just teleported must have its transform agree, or one frame renders
           // the player at the old place.
           transform.position = respawnPosition;
       }
   }
   ```

2. Create a second script named **`Checkpoint`**:

   ```csharp
   // Assets/_Project/Scripts/Checkpoint.cs — the whole file
   using UnityEngine;

   // A trigger that tells the player where to come back to, once.
   [RequireComponent(typeof(Collider2D))]
   public class Checkpoint : MonoBehaviour
   {
       [SerializeField] private Color inactiveColor = new Color(0.4f, 0.4f, 0.45f);
       [SerializeField] private Color activeColor = new Color(0.45f, 0.85f, 0.5f);

       private SpriteRenderer spriteRenderer;
       private bool alreadyReached;

       private void Awake()
       {
           spriteRenderer = GetComponent<SpriteRenderer>();
           spriteRenderer.color = inactiveColor;
       }

       private void OnTriggerEnter2D(Collider2D other)
       {
           if (alreadyReached || !other.TryGetComponent(out PlayerRespawn respawn))
           {
               return;
           }

           alreadyReached = true;
           spriteRenderer.color = activeColor;

           // Respawn a little above the flag's base, so the player lands rather than
           // materialising inside the floor.
           respawn.SetCheckpoint((Vector2)transform.position + Vector2.up * 0.5f);
       }
   }
   ```

3. Create a third script named **`KillZone`**:

   ```csharp
   // Assets/_Project/Scripts/KillZone.cs — the whole file
   using UnityEngine;

   // Anything that falls out of the level lands here. Costs a life regardless of
   // invulnerability: falling off the world is never survivable.
   [RequireComponent(typeof(Collider2D))]
   public class KillZone : MonoBehaviour
   {
       private void OnTriggerEnter2D(Collider2D other)
       {
           if (other.TryGetComponent(out PlayerHealth health))
           {
               health.KillIgnoringInvulnerability();
           }
       }
   }
   ```

4. `KillZone` calls a method `PlayerHealth` does not have yet, so add it now — the project must compile at the
   end of this step. In `Assets/_Project/Scripts/PlayerHealth.cs`, **ADD** this method directly below the
   existing `TakeDamage` method:

   ```csharp
   // Assets/_Project/Scripts/PlayerHealth.cs — below TakeDamage()
   // For hazards no amount of invulnerability should survive — falling out of the level.
   public void KillIgnoringInvulnerability()
   {
       invulnerableUntilTimeSeconds = float.NegativeInfinity;
       TakeDamage();
   }
   ```

5. Save and let Unity compile. Attach **`PlayerRespawn`** to the `Player` object.

6. Build a checkpoint. In the **Hierarchy**, create a **2D Object > Sprites > Square**, rename it
   **`Checkpoint`**, set its `Transform` **Scale** to `0.4, 1.4, 1`, and add a **Box Collider 2D** with
   **Is Trigger** ticked. Drag `Checkpoint.cs` onto it. Then drag the object into
   `Assets/_Project/Prefabs` to make it a prefab, delete it from the Hierarchy, and place **two** instances in
   the cavern — one about a third of the way along, one about two thirds.

7. Build the kill zone. Create an **empty GameObject**, rename it **`KillZone`**, and set its `Transform`
   **Position** to `X 0`, `Y -20`, `Z 0` — comfortably below anything you painted. Add a **Box Collider 2D**
   with **Size** `200, 4` and **Is Trigger** ticked, then drag `KillZone.cs` onto it. It has no sprite, so it
   is invisible in the Game view and drawn as a green outline in the Scene view.

8. Save the scene and press **Play**:
   - Walk into a checkpoint → it turns green and the Console prints `checkpoint set at (…)`.
   - Walk into an enemy → a life is lost and you reappear **at that checkpoint**, standing still.
   - Walk off the end of the level → you fall, and a moment later you are back at the checkpoint with one
     fewer life.
   - Before touching any checkpoint, take a hit → you reappear at the level's start position.

## Done when (this step)
- [ ] Touching a checkpoint turns it green and prints `checkpoint set at (…)` once — touching it again prints
      nothing.
- [ ] Losing a life to an enemy puts the player back at the **most recent** checkpoint, with zero velocity.
- [ ] Falling off the level costs a life and respawns the player, **even within a second of a previous hit**.
- [ ] Before any checkpoint is touched, a death returns the player to where the level started.
- [ ] The respawned player is visible at the checkpoint on the very first frame — no flicker at the old
      position.
- [ ] Collected coins stay collected across a respawn; a stomped enemy stays dead.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(gameplay): add checkpoints, respawn and a kill zone
```

## If it breaks
- **The player respawns but immediately falls through the floor** → the checkpoint's respawn offset is inside
  the ground. The `Vector2.up * 0.5f` in `Checkpoint` is what lifts it clear; check the flag is standing on a
  surface rather than sunk into one.
- **The player keeps the fall's speed after respawning and dies again instantly** → the
  `body.linearVelocity = Vector2.zero;` line is missing or below the position assignment.
- **Falling off the level does nothing** → the kill zone is too narrow or too high to be reached, or its
  collider is not a trigger. `200` wide catches a player who dashed a long way sideways.
- **The player falls through the kill zone unharmed** → `KillIgnoringInvulnerability` is not being called, or
  it does not clear the window before calling `TakeDamage`.
- **`NullReferenceException` in `Checkpoint.Awake`** → the checkpoint object has no `SpriteRenderer`, which
  happens if you built it from an empty GameObject rather than from a sprite.
- **The respawn fires twice per death** → `PlayerRespawn` is subscribed twice; `OnEnable`/`OnDisable` must
  pair exactly, and the component must appear only once on the object.

---
> Nav: [← Stomp it, or lose a life](04_stomp-and-damage.md) · [Overview](00_overview.md) · [Verify →](06_verify.md)
