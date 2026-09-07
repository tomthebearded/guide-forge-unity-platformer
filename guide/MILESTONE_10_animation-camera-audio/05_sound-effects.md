# M10 · Step 05 of 07 — Sound effects
> Nav: [← A background with depth](04_parallax.md) · [Overview](00_overview.md) · [The audio mixer →](06_audio-mixer.md)
> ⚠️ **Superseded 2026-09-07** — `PlayerAudio` read `health.LivesRemaining` from its own `Awake`, which
> is not guaranteed to run after `PlayerHealth.Awake` has filled that number in. Don't follow this step as
> written: the correction that brings it up to date is under *Before you continue — corrections* in
> [../MILESTONE_11_scenes-menus-hud-persistence/04_pause.md](../MILESTONE_11_scenes-menus-hud-persistence/04_pause.md).

**Before you start:** [step 04](04_parallax.md) finished — the background has depth. You need Kenney's CC0
sound packs: download **Impact Sounds** (<https://kenney.nl/assets/impact-sounds>) and **Interface Sounds**
(<https://kenney.nl/assets/interface-sounds>), and copy a handful of `.ogg` or `.wav` files into
`Assets/ThirdParty/Kenney/Audio`. Git LFS has covered both formats since
[M1 step 05](../MILESTONE_1_project-and-version-control/05_git-lfs.md).

## Why / design
Sound has to be triggered by *events*, not by polling. A component that checks every frame whether the player
"seems to have jumped" will fire twice on some frames and never on others; the movement code already knows the
exact moment, and the honest thing is for it to say so.

So `PlayerMotor` gains three announcements — `Jumped`, `Landed`, `Dashed` — and a listener subscribes. This is
the same event pattern as `PlayerStats.CoinsChanged` in M9, and it pays the same dividend: `PlayerMotor` gains
three lines and no knowledge of audio whatsoever, and if you later want a landing puff of dust, it subscribes
to `Landed` too and the motor never hears about it.

**Landing needs detecting**, because unlike jumping it is not something the player asks for. It is a
transition: not grounded last physics step, grounded this one. One remembered boolean.

## Do this

1. In `Assets/_Project/Scripts/PlayerMotor.cs`, **ADD** this line at the very top of the file, above
   `using UnityEngine;`:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — first line of the file
   using System;
   ```

2. **ADD** these events and the remembered flag directly below the existing
   `public bool IsDashing => state == PlayerMovementState.Dashing;` line:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — below the IsDashing property
   // Announcements. The motor does not know or care who listens.
   public event Action Jumped;
   public event Action Landed;
   public event Action Dashed;

   private bool wasGroundedLastStep;
   ```

3. **ADD** the landing detection inside `FixedUpdate`, directly below the `UpdateCoyoteTimer();` line:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — in FixedUpdate(), below UpdateCoyoteTimer()
   if (IsGrounded && !wasGroundedLastStep)
   {
       Landed?.Invoke();
   }

   wasGroundedLastStep = IsGrounded;
   ```

4. **ADD** the jump announcement inside `TickNormalState`, directly below the
   `input.ConsumeJumpRequest();` line that sits inside the jump block:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — inside TickNormalState()'s jump block
   Jumped?.Invoke();
   ```

5. **ADD** the same announcement inside `TickWallSlidingState`, directly below **its**
   `input.ConsumeJumpRequest();` line — a wall-jump is a jump:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — inside TickWallSlidingState()'s jump block
   Jumped?.Invoke();
   ```

6. **ADD** the dash announcement as the **last** line of `StartDash`:

   ```csharp
   // Assets/_Project/Scripts/PlayerMotor.cs — last line of StartDash()
   Dashed?.Invoke();
   ```

7. Create a new MonoBehaviour script in `Assets/_Project/Scripts` named **`PlayerAudio`**:

   ```csharp
   // Assets/_Project/Scripts/PlayerAudio.cs — the whole file
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

8. Save both files and let Unity compile. Select `Player`, **Add Component > Audio Source**, and on it
   **untick Play On Awake** — otherwise it tries to play a clip it does not have, at level start. Leave the
   rest at their defaults.

9. Drag `PlayerAudio.cs` onto the `Player`, then fill its five clip fields by dragging sounds from
   `Assets/ThirdParty/Kenney/Audio`. Which file goes where is **entirely cosmetic** — pick by ear. A short
   blip for jump, a duller thud for land, a whoosh for dash, a bright ping for coin, a harsh one for hurt.

   Leaving a field empty is safe: the `Play` method checks for null, so a missing clip is silence rather than
   an error.

10. Save the scene and press **Play**, then jump, land, dash, collect a coin and walk into an enemy. Each one
    makes its own sound, and none of them cuts another one off.

## Done when (this step)
- [ ] Jumping, landing, dashing, collecting a coin and being hurt each play a distinct sound.
- [ ] A wall-jump plays the jump sound too.
- [ ] Landing plays **once** per landing, not repeatedly while standing still.
- [ ] Collecting two coins in quick succession plays both sounds, overlapping rather than truncating.
- [ ] Losing the last life and resetting the run does **not** play the hurt sound twice.
- [ ] The **first** hit of a fresh run plays the hurt sound — if only the second one does, `livesLastSeen`
      started at `0` instead of `3`.
- [ ] Emptying one clip field makes that action silent, with no Console error.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(audio): announce jump, land and dash, and play sound effects
```

## If it breaks
- **No sound at all** → the Editor's **Mute Audio** button (the speaker in the Game view toolbar) is on, or
  the `Audio Source` is missing from the `Player`.
- **The landing sound fires every physics step while standing** → `wasGroundedLastStep` is not being updated,
  or the update sits above the comparison instead of below it.
- **The jump sound plays twice per jump** → `Jumped?.Invoke()` was added to both the jump block *and* the
  outer method body. It belongs inside the `if`.
- **`NullReferenceException` in `OnEnable`** → `PlayerAudio` is on an object without `PlayerMotor`,
  `PlayerStats` or `PlayerHealth`. All four live on the `Player`.
- **A sound plays at level start** → **Play On Awake** is still ticked on the `Audio Source`.
- **The hurt sound plays when the run resets** → the `livesLastSeen` comparison is missing; a reset raises
  `LivesChanged` with a *higher* number.
- **The hurt sound is silent on the first hit of a run, then works** → `livesLastSeen` was still read in
  `Awake`, which ran before `PlayerHealth.Awake`, so it started at `0` and the first `LivesChanged` did
  not look like a loss. It belongs in `Start`.

---
> Nav: [← A background with depth](04_parallax.md) · [Overview](00_overview.md) · [The audio mixer →](06_audio-mixer.md)
