# M9 · Verify — Coins, enemies, damage, lives & checkpoints
> Nav: [← Checkpoints and respawn](05_checkpoints-and-respawn.md) · [Overview](00_overview.md) · [Animation, camera & audio →](../MILESTONE_10_animation-camera-audio/00_overview.md)

## Done-when gate (the real test — check every box by hand)

Observed in **Play Mode in the Editor**, `Level01` open, Console visible.

- [ ] **Coins count.** Touching a coin removes it and prints `coins = N`, N rising by one each time. Every
      placed coin can be collected; the moving platform passing through one does not collect it.
- [ ] **Enemies patrol.** Each enemy walks its ledge and turns at the edge and at walls, indefinitely,
      without falling off.
- [ ] **A stomp kills.** Landing on an enemy from above destroys it and bounces the player upward.
- [ ] **A touch hurts.** Walking into an enemy prints `lives = 2`; the same contact does nothing for the next
      **one second**; standing inside it costs one further life per second, not per frame.
- [ ] **Rising into an enemy hurts** rather than stomping it.
- [ ] **The run ends at zero.** A third hit prints `run over — lives reset` and the count returns to `3`.
- [ ] **Checkpoints arm once.** Touching one turns it green and prints `checkpoint set at (…)`; touching it
      again prints nothing.
- [ ] **Death returns you to the last checkpoint**, at rest, visible there on the first frame — and to the
      level's start position if no checkpoint has been touched.
- [ ] **Falling off the world costs a life**, even within a second of a previous hit.
- [ ] **Progress survives a respawn**: collected coins stay collected, stomped enemies stay dead.
- [ ] **Nothing earlier regressed.** The full moveset, the composite floor, the one-way ledge and the platform
      ride all behave.
- [ ] **The project is clean.** No red Console entries; after committing, `git status --porcelain` prints
      nothing.

## Files after this milestone (the checkpoint)

_This checkpoint renders the complete contents of every guide-authored file created or modified in this
milestone (listed below). Pre-existing files this milestone only added to are shown as their added region
under "Pre-existing files modified", not reproduced whole. Files not listed were not touched this milestone._

### `Assets/_Project/Scripts/PlayerStats.cs`
```csharp
using System;
using UnityEngine;

// Owns the run's numbers. Anything that changes them goes through a method here,
// and anything that displays them subscribes to the event.
public class PlayerStats : MonoBehaviour
{
    // How many coins have been collected this run.
    public int CoinsCollected { get; private set; }

    // Raised whenever CoinsCollected changes, carrying the new total.
    // [M10] PlayerAudio subscribes first, then [M11] the HUD; for now the
    // Console is the only reader.
    public event Action<int> CoinsChanged;

    public void AddCoin()
    {
        CoinsCollected++;

        // ?.Invoke calls the event only if something is subscribed —
        // raising an event with no subscribers would otherwise throw.
        CoinsChanged?.Invoke(CoinsCollected);

        Debug.Log($"coins = {CoinsCollected}");
    }
}
```

### `Assets/_Project/Scripts/Collectible.cs`
```csharp
using UnityEngine;

// Sits on a trigger. When the player overlaps it, it credits one coin and removes
// itself. It knows nothing about scoring beyond "tell PlayerStats".
[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // TryGetComponent asks "does this object have one?" and hands it over in
        // the same call — no null check, no exception if it does not.
        if (!other.TryGetComponent(out PlayerStats stats))
        {
            return;
        }

        stats.AddCoin();

        // Destroy removes the GameObject at the end of the frame.
        Destroy(gameObject);
    }
}
```

### `Assets/_Project/Scripts/EnemyPatrol.cs`
```csharp
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

### `Assets/_Project/Scripts/PlayerHealth.cs`
```csharp
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

    // Raised on every life lost, the last one included — PlayerRespawn puts the
    // player back at the checkpoint either way. [M11] splits the last life off
    // into an event of its own.
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

    // For hazards no amount of invulnerability should survive — falling out of the level.
    public void KillIgnoringInvulnerability()
    {
        invulnerableUntilTimeSeconds = float.NegativeInfinity;
        TakeDamage();
    }
}
```

### `Assets/_Project/Scripts/EnemyContact.cs`
```csharp
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

### `Assets/_Project/Scripts/PlayerRespawn.cs`
```csharp
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

### `Assets/_Project/Scripts/Checkpoint.cs`
```csharp
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

### `Assets/_Project/Scripts/KillZone.cs`
```csharp
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

### Editor checkpoint

| GameObject / asset | Component | Field | Exact value |
|---|---|---|---|
| `Player` | `Player Stats (Script)` | — | no fields |
| `Player` | `Player Health (Script)` | Starting Lives / Invulnerability Seconds | `3` / `1` |
| `Player` | `Player Respawn (Script)` | — | no fields |
| `Coin.prefab` | `Transform` | Scale | `0.5, 0.5, 1` |
| `Coin.prefab` | `Circle Collider 2D` | Radius / Is Trigger | `0.5` / ticked |
| `Coin.prefab` | `Collectible (Script)` | — | no fields |
| `Enemy.prefab` | `Transform` | Scale | `0.8, 0.8, 1` |
| `Enemy.prefab` | GameObject | Layer | `Hazard` |
| `Enemy.prefab` | `Box Collider 2D` | Size / Is Trigger | `1, 1` / ticked |
| `Enemy.prefab` | `Rigidbody 2D` | Body Type / Interpolate | `Kinematic` / `Interpolate` |
| `Enemy.prefab` | `Enemy Patrol (Script)` | Speed / Ground Layers | `2` / `Ground` |
| `Enemy.prefab` | `Enemy Patrol (Script)` | Probe Distance Ahead / Floor Probe Depth / Probe Radius | `0.45` / `0.55` / `0.08` |
| `Enemy.prefab` | `Enemy Contact (Script)` | Stomp Bounce Velocity / Stomp Tolerance | `10` / `0.1` |
| `Checkpoint.prefab` | `Transform` | Scale | `0.4, 1.4, 1` |
| `Checkpoint.prefab` | `Box Collider 2D` | Is Trigger | ticked |
| `KillZone` | `Transform` | Position | `0, -20, 0` |
| `KillZone` | `Box Collider 2D` | Size / Is Trigger | `200, 4` / ticked |

### Pre-existing files modified
- `Assets/_Project/Scenes/Level01.unity` — coin, enemy and checkpoint instances placed; the `KillZone` added;
  `PlayerStats`, `PlayerHealth` and `PlayerRespawn` attached to `Player`. Edited through the Editor.

### Unchanged this milestone
- `Assets/_Project/Scripts/PlayerMotor.cs`, `PlayerInputReader.cs`, `PlayerMovementState.cs`,
  `MovingPlatform.cs`, `PlatformRiderCarrier.cs` — unchanged since M8 and M7. The whole game layer was added
  without touching movement.
- `Assets/InputSystem_Actions.inputactions`, `Packages/manifest.json`, `ProjectSettings/*` — unchanged since
  M8 and earlier.

## Troubleshooting

| Symptom | Likely cause → fix |
|---|---|
| Coins do nothing | The collider is not a trigger, or `Collectible` is on the instances rather than the prefab. |
| A single touch drains every life | The i-frame early return is missing from `TakeDamage`. |
| The player is hurt when landing on an enemy | The player was not falling at contact, or the bounds test is inverted. |
| Walking into an enemy's side kills it | `Stomp Tolerance Units` is far too large. |
| The enemy walks off its ledge | `Ground Layers` is `Nothing` on `EnemyPatrol`. |
| The enemy vibrates in place | The wall probe is finding the floor: `Floor Probe Depth Units` too small. |
| Respawn drops the player through the floor | The velocity is not zeroed, or the checkpoint's offset sits inside the ground. |
| Falling off the level does nothing | The kill zone is out of reach, or its collider is not a trigger. |
| The player flickers at the old position for one frame after respawn | `transform.position` was not set alongside `body.position`. |

## Handoff
- **You now have:** the M1 project and clean repository; the full moveset from M8 on a tilemap cavern with a
  one-way ledge and a rider-carrying platform; and a game loop — coins that count through a `PlayerStats`
  event, patrolling enemies you can stomp or be hurt by, three lives with one second of invulnerability per
  hit, checkpoints that arm on contact, respawn on death, and a kill zone under the world. Eight new scripts,
  none of which required a change to the movement code.
- **Open / deferred:** everything is still coloured rectangles, the camera never moves, and the only feedback
  is text in the Console — no score on screen, no sound, and losing your last life silently resets the count
  instead of ending the run. M10 takes the presentation; M11 takes the game structure.
- **Next:** **[M10 — Animation, camera & audio](../MILESTONE_10_animation-camera-audio/00_overview.md)** —
  the milestone where it starts to look and sound like a game rather than a diagram.

---
> Nav: [← Checkpoints and respawn](05_checkpoints-and-respawn.md) · [Overview](00_overview.md) · [Animation, camera & audio →](../MILESTONE_10_animation-camera-audio/00_overview.md)
