# M10 · Verify — Animation, camera & audio
> Nav: [← The audio mixer](06_audio-mixer.md) · [Overview](00_overview.md) · [Scenes, menus, HUD & persistence →](../MILESTONE_11_scenes-menus-hud-persistence/00_overview.md)
> ⚠️ **Superseded 2026-09-07** — the `PlayerAudio` checkpoint below moved `livesLastSeen` out of `Awake`
> and into `Start`. Don't diff your file against the old listing: the correction that brings it up to date
> is under *Before you continue — corrections* in
> [../MILESTONE_11_scenes-menus-hud-persistence/04_pause.md](../MILESTONE_11_scenes-menus-hud-persistence/04_pause.md).

## Done-when gate (the real test — check every box by hand)

Observed in **Play Mode in the Editor**, `Level01` open, with sound unmuted in the Game view toolbar.

- [ ] **The player is a character.** It renders a Kenney sprite, untinted, drawn in front of the tiles, on the
      `Player` sorting layer.
- [ ] **Every animation transition fires** — the whole set, not a sample: idle → run (start moving), run →
      idle (stop), any → jump (rise), any → fall (descend), jump → idle and fall → idle (land). Watch the
      Animator window during play and see the active state move for each one.
- [ ] **The character faces where it moves**, flipping as you change direction.
- [ ] **Landing returns to idle or run immediately**, with no wait for a clip to finish.
- [ ] **The camera follows** smoothly, sits slightly ahead of the player, ignores a straight-up jump, and
      **stops at both ends of the level** without showing empty space.
- [ ] **The background has depth.** `BackgroundFar` moves visibly less than `BackgroundNear`, both less than
      the cavern, and neither edge ever comes into view. Setting both factors to `1` removes the effect;
      restoring `0.2` and `0.5` brings it back.
- [ ] **Five distinct sounds**: jump (including wall-jump), land (once per landing), dash, coin, hurt — and
      overlapping coins do not cut each other off. The hurt sound plays on the **first** hit of a fresh run,
      not only from the second: silence on the first means `livesLastSeen` started at `0`.
- [ ] **The mixer routes them.** Lowering the `Music` group during play silences music alone; lowering
      `Master` silences everything. The exposed parameters `MasterVolumeDb`, `MusicVolumeDb` and `SfxVolumeDb`
      all exist.
- [ ] **Nothing earlier regressed.** The whole moveset, coins, enemies, stomping, lives, checkpoints, the
      kill zone, the one-way ledge and the platform ride all behave.
- [ ] **The project is clean.** No red Console entries; after committing, `git status --porcelain` prints
      nothing, and `git lfs ls-files` lists the new audio files.

## Files after this milestone (the checkpoint)

_This checkpoint renders the complete contents of every guide-authored file created or modified in this
milestone (listed below). Pre-existing files this milestone only added to are shown as their added region
under "Pre-existing files modified", not reproduced whole. Files not listed were not touched this milestone._

### `Assets/_Project/Scripts/PlayerAnimationDriver.cs`
```csharp
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

### `Assets/_Project/Scripts/ParallaxLayer.cs`
```csharp
using UnityEngine;

// Moves this layer by a fraction of the camera's movement, so it reads as distant.
// 0 = pinned to the camera (infinitely far), 1 = moves with the world (not parallax).
public class ParallaxLayer : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float parallaxFactor = 0.3f;

    private Transform cameraTransform;
    private Vector3 previousCameraPosition;

    private void Start()
    {
        // Camera.main finds the camera tagged MainCamera. Once, in Start —
        // it is a scene search, not something to do every frame.
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;
    }

    // LateUpdate runs after every Update — and, crucially, after Cinemachine has
    // moved the camera this frame.
    private void LateUpdate()
    {
        Vector3 cameraMovement = cameraTransform.position - previousCameraPosition;

        // Move with the camera by the missing fraction: a factor of 0.2 means the
        // layer keeps 80% of the camera's movement, so it appears to lag behind.
        transform.position += new Vector3(cameraMovement.x, cameraMovement.y, 0f) * (1f - parallaxFactor);

        previousCameraPosition = cameraTransform.position;
    }
}
```

### `Assets/_Project/Scripts/PlayerAudio.cs`
```csharp
using UnityEngine;

// Listens to what the player does and plays a sound for it. Owns no gameplay:
// remove this component and the game plays on, silently.
[RequireComponent(typeof(AudioSource))]
public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip landClip;
    [SerializeField] private AudioClip dashClip;
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip hurtClip;

    private AudioSource source;
    private PlayerMotor motor;
    private PlayerStats stats;
    private PlayerHealth health;

    // LivesChanged also fires when the run resets, so compare rather than assume.
    private int livesLastSeen;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        motor = GetComponent<PlayerMotor>();
        stats = GetComponent<PlayerStats>();
        health = GetComponent<PlayerHealth>();
    }

    // Not Awake: PlayerHealth fills LivesRemaining in its own Awake, and Unity
    // does not promise which component's Awake runs first. Start does — it runs
    // only once every Awake in the scene has.
    private void Start()
    {
        livesLastSeen = health.LivesRemaining;
    }

    private void OnEnable()
    {
        motor.Jumped += PlayJump;
        motor.Landed += PlayLand;
        motor.Dashed += PlayDash;
        stats.CoinsChanged += PlayCoin;
        health.LivesChanged += PlayHurtIfLifeLost;
    }

    private void OnDisable()
    {
        motor.Jumped -= PlayJump;
        motor.Landed -= PlayLand;
        motor.Dashed -= PlayDash;
        stats.CoinsChanged -= PlayCoin;
        health.LivesChanged -= PlayHurtIfLifeLost;
    }

    private void PlayJump() => Play(jumpClip);
    private void PlayLand() => Play(landClip);
    private void PlayDash() => Play(dashClip);

    // The event carries the new total; this listener does not need it.
    private void PlayCoin(int coinsCollected) => Play(coinClip);

    private void PlayHurtIfLifeLost(int livesRemaining)
    {
        if (livesRemaining < livesLastSeen)
        {
            Play(hurtClip);
        }

        livesLastSeen = livesRemaining;
    }

    private void Play(AudioClip clip)
    {
        // PlayOneShot layers sounds instead of cutting off the previous one,
        // which is what you want for short effects.
        if (clip != null)
        {
            source.PlayOneShot(clip);
        }
    }
}
```

### `Assets/_Project/Scripts/PlayerMotor.cs`
```csharp
using System;
using UnityEngine;

// Turns the intent from PlayerInputReader into motion on the Rigidbody2D.
// RequireComponent makes Unity add the dependencies automatically and refuse to
// let you remove them while this script is attached.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputReader))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private float moveSpeedUnitsPerSecond = 7f;
    [SerializeField] private float groundAccelerationUnitsPerSecondSquared = 60f;
    [SerializeField] private float airAccelerationUnitsPerSecondSquared = 35f;

    [Header("Ground check")]
    [SerializeField] private Vector2 groundCheckSizeUnits = new Vector2(0.9f, 0.12f);
    [SerializeField] private float groundCheckDistanceBelowCentreUnits = 0.5f;
    [SerializeField] private LayerMask groundLayers;

    // True while a solid surface is directly under the feet.
    public bool IsGrounded { get; private set; }

    [Header("Jump")]
    [SerializeField] private float jumpVelocityUnitsPerSecond = 14f;
    [SerializeField] private float coyoteTimeSeconds = 0.10f;
    [SerializeField] private float jumpBufferSeconds = 0.12f;
    [SerializeField] private float fallGravityMultiplier = 1.8f;
    [SerializeField] private float lowJumpGravityMultiplier = 2.2f;

    [Header("Dash")]
    [SerializeField] private float dashDistanceUnits = 5f;
    [SerializeField] private float dashDurationSeconds = 0.15f;
    [SerializeField] private float dashCooldownSeconds = 0.60f;

    private float dashEndTimeSeconds;
    private float nextDashAllowedTimeSeconds;

    // +1 while facing right, -1 while facing left. A dash with no input uses this.
    private int facingDirection = 1;

    [Header("Wall")]
    [SerializeField] private Vector2 wallCheckSizeUnits = new Vector2(0.12f, 0.8f);
    [SerializeField] private float wallCheckDistanceFromCentreUnits = 0.5f;
    [SerializeField] private LayerMask wallLayers;
    [SerializeField] private float wallSlideSpeedUnitsPerSecond = 2.5f;
    [SerializeField] private Vector2 wallJumpVelocity = new Vector2(9f, 13f);
    [SerializeField] private float wallJumpControlLockSeconds = 0.15f;

    // Until this moment passes, horizontal input does not steer.
    private float horizontalControlLockedUntilTimeSeconds;

    // -1 = wall on the left, +1 = wall on the right, 0 = neither.
    public int WallDirection { get; private set; }

    // True while the dash state is running. Read by the animation driver.
    public bool IsDashing => state == PlayerMovementState.Dashing;

    // Announcements. The motor does not know or care who listens.
    public event Action Jumped;
    public event Action Landed;
    public event Action Dashed;

    private bool wasGroundedLastStep;

    // The Inspector's Gravity Scale, captured once so the multipliers below
    // always scale the original value rather than compounding on themselves.
    private float baseGravityScale;

    // Counts down while airborne; refilled while grounded; spent by a jump.
    private float coyoteTimeRemainingSeconds;

    // The one state the player is in. Every transition in this class is an
    // assignment to this field, so they are easy to find.
    private PlayerMovementState state = PlayerMovementState.Normal;

    private Rigidbody2D body;
    private PlayerInputReader input;

    private void Awake()
    {
        // GetComponent finds another component on this same GameObject.
        // Doing it once in Awake and caching it avoids the lookup every step.
        body = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInputReader>();
        baseGravityScale = body.gravityScale;
    }

    private void UpdateGroundedState()
    {
        Vector2 boxCentre = (Vector2)transform.position + Vector2.down * groundCheckDistanceBelowCentreUnits;

        // OverlapBox(point, size, angle, layerMask) returns the first collider it finds
        // in that box on those layers, or null if there is none.
        IsGrounded = Physics2D.OverlapBox(boxCentre, groundCheckSizeUnits, 0f, groundLayers) != null;
    }

    private void UpdateWallContact()
    {
        Vector2 centre = body.position;
        Vector2 offset = Vector2.right * wallCheckDistanceFromCentreUnits;

        bool wallOnRight = Physics2D.OverlapBox(centre + offset, wallCheckSizeUnits, 0f, wallLayers) != null;
        bool wallOnLeft = Physics2D.OverlapBox(centre - offset, wallCheckSizeUnits, 0f, wallLayers) != null;

        // If somehow both, prefer the one being pressed towards; ties go to the right.
        WallDirection = wallOnRight ? 1 : wallOnLeft ? -1 : 0;
    }

    private bool IsPressingIntoWall()
    {
        if (WallDirection == 0 || Mathf.Approximately(input.HorizontalInput, 0f))
        {
            return false;
        }

        // Same sign means the input points at the wall the player is touching.
        return Mathf.Sign(input.HorizontalInput) == WallDirection;
    }

    // FixedUpdate runs on the physics clock — 50 times a second by default.
    private void FixedUpdate()
    {
        // Facts first: every state is entitled to know these before it decides anything.
        UpdateGroundedState();
        UpdateWallContact();
        UpdateCoyoteTimer();

        if (IsGrounded && !wasGroundedLastStep)
        {
            Landed?.Invoke();
        }

        wasGroundedLastStep = IsGrounded;

        // Then exactly one behaviour runs.
        switch (state)
        {
            case PlayerMovementState.Normal:
                TickNormalState();
                break;
            case PlayerMovementState.Dashing:
                TickDashingState();
                break;
            case PlayerMovementState.WallSliding:
                TickWallSlidingState();
                break;
        }
    }

    private void UpdateCoyoteTimer()
    {
        // Refill only while grounded AND not rising, so the ground check can't re-arm the
        // window for a step after takeoff and let a mashed press double-jump in mid-air.
        if (IsGrounded && body.linearVelocity.y <= 0f)
        {
            coyoteTimeRemainingSeconds = coyoteTimeSeconds;
        }
        else if (!IsGrounded)
        {
            coyoteTimeRemainingSeconds -= Time.fixedDeltaTime;
        }
    }

    private void TickNormalState()
    {
        if (!Mathf.Approximately(input.HorizontalInput, 0f))
        {
            facingDirection = input.HorizontalInput > 0f ? 1 : -1;
        }

        if (input.DashRequested && Time.time >= nextDashAllowedTimeSeconds)
        {
            StartDash();
            return;   // the rest of this state does not run on the step the dash begins
        }

        // Drop a request that arrived during the cooldown, so it cannot fire late.
        input.ConsumeDashRequest();

        bool horizontalControlLocked = Time.time < horizontalControlLockedUntilTimeSeconds;

        if (!horizontalControlLocked)
        {
            float desiredHorizontalSpeed = input.HorizontalInput * moveSpeedUnitsPerSecond;
            float accelerationThisStep = IsGrounded
                ? groundAccelerationUnitsPerSecondSquared
                : airAccelerationUnitsPerSecondSquared;

            // MoveTowards walks the current value towards the target by at most the third
            // argument — never overshooting it.
            float newHorizontalSpeed = Mathf.MoveTowards(
                body.linearVelocity.x,
                desiredHorizontalSpeed,
                accelerationThisStep * Time.fixedDeltaTime);

            body.linearVelocity = new Vector2(newHorizontalSpeed, body.linearVelocity.y);
        }

        bool jumpIsBuffered = input.TimeSinceJumpPressedSeconds < jumpBufferSeconds;

        if (jumpIsBuffered && coyoteTimeRemainingSeconds > 0f)
        {
            // Replace the vertical velocity outright rather than adding to it, so a jump
            // always reaches the same height however the player was already moving.
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpVelocityUnitsPerSecond);

            // Spend both windows, so one press produces exactly one jump.
            coyoteTimeRemainingSeconds = 0f;
            input.ConsumeJumpRequest();
            Jumped?.Invoke();
        }

        ApplyJumpGravityMultipliers();

        if (!IsGrounded && !horizontalControlLocked && IsPressingIntoWall() && body.linearVelocity.y < 0f)
        {
            state = PlayerMovementState.WallSliding;
        }
    }

    private void StartDash()
    {
        state = PlayerMovementState.Dashing;
        dashEndTimeSeconds = Time.time + dashDurationSeconds;
        nextDashAllowedTimeSeconds = Time.time + dashCooldownSeconds;
        input.ConsumeDashRequest();

        // No gravity for the duration: a dash is a straight line.
        body.gravityScale = 0f;

        // Distance over duration is the speed the dash must hold.
        float dashSpeedUnitsPerSecond = dashDistanceUnits / dashDurationSeconds;
        body.linearVelocity = new Vector2(facingDirection * dashSpeedUnitsPerSecond, 0f);

        Dashed?.Invoke();
    }

    private void TickDashingState()
    {
        // Nothing to do while it runs: the velocity set at the start is the dash.
        if (Time.time < dashEndTimeSeconds)
        {
            return;
        }

        state = PlayerMovementState.Normal;
        body.gravityScale = baseGravityScale;

        // Come out at running speed rather than at dash speed.
        float exitSpeed = Mathf.Clamp(body.linearVelocity.x, -moveSpeedUnitsPerSecond, moveSpeedUnitsPerSecond);
        body.linearVelocity = new Vector2(exitSpeed, body.linearVelocity.y);
    }

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

        bool jumpIsBuffered = input.TimeSinceJumpPressedSeconds < jumpBufferSeconds;

        if (jumpIsBuffered)
        {
            // Away from the wall and up. -WallDirection is "the other way".
            body.linearVelocity = new Vector2(-WallDirection * wallJumpVelocity.x, wallJumpVelocity.y);

            horizontalControlLockedUntilTimeSeconds = Time.time + wallJumpControlLockSeconds;
            input.ConsumeJumpRequest();
            Jumped?.Invoke();

            state = PlayerMovementState.Normal;
            body.gravityScale = baseGravityScale;
        }
    }

    private void ApplyJumpGravityMultipliers()
    {
        if (body.linearVelocity.y < 0f)
        {
            // Falling: come down faster than you went up.
            body.gravityScale = baseGravityScale * fallGravityMultiplier;
        }
        else if (body.linearVelocity.y > 0f && !input.IsJumpHeld)
        {
            // Rising, but the button is already released: cut the climb short.
            body.gravityScale = baseGravityScale * lowJumpGravityMultiplier;
        }
        else
        {
            // Rising with the button held, or standing still.
            body.gravityScale = baseGravityScale;
        }
    }

    // Unity calls this in the Editor while the object is selected. Editor-only: it
    // is stripped from a build and costs nothing at runtime.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 boxCentre = transform.position + Vector3.down * groundCheckDistanceBelowCentreUnits;
        Gizmos.DrawWireCube(boxCentre, new Vector3(groundCheckSizeUnits.x, groundCheckSizeUnits.y, 0f));

        Gizmos.color = Color.yellow;
        Vector3 wallOffset = Vector3.right * wallCheckDistanceFromCentreUnits;
        Vector3 wallBoxSize = new Vector3(wallCheckSizeUnits.x, wallCheckSizeUnits.y, 0f);
        Gizmos.DrawWireCube(transform.position + wallOffset, wallBoxSize);
        Gizmos.DrawWireCube(transform.position - wallOffset, wallBoxSize);
    }
}
```

### Assets created this milestone

| Path | What it is |
|---|---|
| `Assets/_Project/Animation/PlayerIdle.anim`, `PlayerRun.anim`, `PlayerJump.anim`, `PlayerFall.anim` | the four clips |
| `Assets/_Project/Animation/Player.controller` | the Animator controller Unity created with the first clip |
| `Assets/_Project/Audio/GameMixer.mixer` | the mixer, with `Music` and `SFX` under `Master` |
| `Assets/ThirdParty/Kenney/Audio/` | the CC0 sound effects and music track |

### Editor checkpoint

| GameObject / where | Component | Field | Exact value |
|---|---|---|---|
| Project Settings → Tags and Layers | Sorting Layers | order | `Default`, `Background`, `Level`, `Player` |
| `Player` | `Sprite Renderer` | Sprite / Color / Sorting Layer | a Kenney character / `FFFFFF` / `Player` |
| `Player` | `Animator` | Controller | `Player.controller` |
| `Player` | `Audio Source` | Output / Play On Awake | `SFX` group of `GameMixer` / unticked |
| `Player` | `Player Audio (Script)` | five clip fields | one Kenney sound each (cosmetic which) |
| Animator | Parameters | names and types | `Speed` (Float), `IsGrounded` (Bool), `VerticalVelocity` (Float), `IsDashing` (Bool) |
| Animator | every transition | Has Exit Time | **unticked** |
| `GroundTilemap` | `Tilemap Renderer` | Sorting Layer | `Level` |
| `Grid` | `Polygon Collider 2D` | Is Trigger | ticked, shaped around the whole cavern |
| `FollowCamera` | `Cinemachine Camera` | Follow | `Player` |
| `FollowCamera` | `Cinemachine Position Composer` | Damping / Lookahead Time / Lookahead Smoothing | `1, 1` / `0.3` / `5` |
| `FollowCamera` | `Cinemachine Position Composer` | Dead Zone Width / Height | `0.15` / `0.2` |
| `FollowCamera` | `Cinemachine Confiner 2D` | Bounding Shape 2D | `Grid` |
| `Main Camera` | `Cinemachine Brain` | — | added automatically |
| `BackgroundFar` | `Transform` / `Parallax Layer` | Position, Scale / Parallax Factor | `0, 0, 10`, `60, 30, 1` / `0.2` |
| `BackgroundNear` | `Transform` / `Parallax Layer` | Position, Scale / Parallax Factor | `0, -2, 9`, `50, 16, 1` / `0.5` |
| `Music` | `Audio Source` | Output / Loop / Play On Awake / Volume | `Music` group / ticked / ticked / `0.5` |
| `GameMixer` | Exposed Parameters | names | `MasterVolumeDb`, `MusicVolumeDb`, `SfxVolumeDb` |

### Pre-existing files modified
- `Assets/_Project/Scenes/Level01.unity` — the sprite swap, sorting layers, `FollowCamera`, the two background
  layers, the `Music` object, and the new components on `Player`. Edited through the Editor.
- `Packages/manifest.json` — **Cinemachine** added in [step 03](03_cinemachine-camera.md).
- `ProjectSettings/TagManager.asset` — three sorting layers added in [step 01](01_dress-the-player.md).
- The `Coin`, `Enemy` and `Checkpoint` prefabs — Sorting Layer set to `Level`.

### Unchanged this milestone
- `PlayerInputReader.cs`, `PlayerMovementState.cs`, `MovingPlatform.cs`, `PlatformRiderCarrier.cs`,
  `PlayerStats.cs`, `Collectible.cs`, `EnemyPatrol.cs`, `PlayerHealth.cs`, `EnemyContact.cs`,
  `PlayerRespawn.cs`, `Checkpoint.cs`, `KillZone.cs` — unchanged since M9 and earlier.
- `Assets/InputSystem_Actions.inputactions` — unchanged since M8.

## Troubleshooting

| Symptom | Likely cause → fix |
|---|---|
| The character responds a beat late | **Has Exit Time** still ticked on a transition. |
| Animation flickers between two states | Thresholds too close to `0`; `0.1` gives a dead zone. |
| `Parameter 'Speed' does not exist` | The Animator's parameter name differs from the hashed string; they are case-sensitive. |
| No `CinemachineCamera` in Add Component | A Cinemachine 2 tutorial, or the package is still importing. The menu is **Cinemachine > 2D Camera**. |
| The camera judders | **Damping** `0`, or the player's `Interpolate` was turned off. |
| Black beyond the level edges | The confiner's bounding polygon is smaller than the view. |
| The background drifts or shimmers | Movement in `Update` instead of `LateUpdate`, or `previousCameraPosition` not updated. |
| No sound | The Game view's **Mute Audio** toggle, or the `Audio Source` **Output** is `None`. |
| The land sound repeats while standing | `wasGroundedLastStep` is not updated after the comparison. |
| The hurt sound fires on a run reset | The `livesLastSeen` comparison is missing. |

## Handoff
- **You now have:** the M1 project and clean repository; the full M8 moveset and the full M9 game loop, now
  wearing a character sprite animated from four Animator parameters, followed by a Cinemachine camera with
  look-ahead and level bounds, in front of two parallax background layers, with five sound effects and looping
  music routed through a mixer whose three volumes are exposed to script.
- **Open / deferred:** there is still nothing on screen telling you how many coins or lives you have, there is
  one level and no way to leave it, losing your last life silently resets the count, and the exposed mixer
  parameters have no controls attached to them.
- **Next:** **[M11 — Scenes, menus, HUD & persistence](../MILESTONE_11_scenes-menus-hud-persistence/00_overview.md)** —
  a menu, two levels, a pause, a HUD, a timer, and a best time that survives quitting the game.

---
> Nav: [← The audio mixer](06_audio-mixer.md) · [Overview](00_overview.md) · [Scenes, menus, HUD & persistence →](../MILESTONE_11_scenes-menus-hud-persistence/00_overview.md)
