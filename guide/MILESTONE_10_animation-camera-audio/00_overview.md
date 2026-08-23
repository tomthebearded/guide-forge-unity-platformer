# Milestone 10 — Animation, camera & audio
> Part 4 — The game · milestone 10 of 13 · prev: [Coins, enemies, damage, lives & checkpoints](../MILESTONE_9_coins-enemies-lives-checkpoints/00_overview.md) · next: [Scenes, menus, HUD & persistence](../MILESTONE_11_scenes-menus-hud-persistence/00_overview.md) · start: [Dress the player](01_dress-the-player.md)

## Goal
By the end of this milestone *Cavern Dash* looks and sounds like a game. The orange square becomes an animated
character whose idle, run, jump and fall states follow what the movement code is actually doing; the camera
follows with a little look-ahead and never shows the void beyond the level; a layered background drifts behind
the action; and jumping, landing, collecting and being hurt each make a distinct sound through a mixer you can
turn down.

## Prerequisite
M9's gate passed: coins count, enemies can be stomped or hurt you, lives and i-frames work, and checkpoints
respawn you.

## Steps at a glance

**Sitting 1 — A character, not a square (01–02)**
1. [Dress the player](01_dress-the-player.md)
2. [Animate the player](02_animate-the-player.md)

**Sitting 2 — A camera and a world behind it (03–04)**
3. [The camera follows](03_cinemachine-camera.md)
4. [A background with depth](04_parallax.md)

**Sitting 3 — Sound (05–06)**
5. [Sound effects](05_sound-effects.md)
6. [The audio mixer](06_audio-mixer.md)

7. [Verify](07_verify.md)

## Design / decisions folded in
- Sorting layers and draw order — taught in [step 01](01_dress-the-player.md).
- Animator parameters as the one channel from code to animation — taught in [step 02](02_animate-the-player.md); recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d17--build-vs-borrow-sprite-animation-and-transitions).
- Cinemachine 3 (and why every tutorial you find names a class that no longer exists) — taught in [step 03](03_cinemachine-camera.md); recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d16--build-vs-borrow-camera-follow-look-ahead-and-bounds).
- Events from the motor rather than polling — taught in [step 05](05_sound-effects.md).
- An `AudioMixer` with exposed parameters, and the decibel-versus-linear trap — taught in [step 06](06_audio-mixer.md); recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d18--build-vs-borrow-volume-control-and-audio-routing).

---
> Part 4 — The game · milestone 10 of 13 · prev: [Coins, enemies, damage, lives & checkpoints](../MILESTONE_9_coins-enemies-lives-checkpoints/00_overview.md) · next: [Scenes, menus, HUD & persistence](../MILESTONE_11_scenes-menus-hud-persistence/00_overview.md) · start: [Dress the player](01_dress-the-player.md)
