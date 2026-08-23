# M13 · Verify — Build & ship
> Nav: [← The repository](04_the-repository.md) · [Overview](00_overview.md) · [Back to the guide](../README.md)

## Done-when gate (the real test — check every box by hand)

**This gate is observed in the built player, not in the Editor** — that is the whole point of the milestone.
Close Unity, or at least stop Play Mode, before you start: the Editor and the build keep separate saved data,
and running both confuses which is which.

- [ ] **It launches by double-click.** `Cavern Dash.exe` (or `.app`) opens a 1280 × 720 window titled
      `Cavern Dash`, with your icon, and **no** `(Development Build)` in the title.
- [ ] **The whole game plays through in the build**: menu → `Level01` → `Level02` → win screen, with the HUD,
      camera, animation, parallax, music and effects all working.
- [ ] **Every mechanic survives the build.** Run, accelerate, jump with coyote time and buffering, tap versus
      hold height, dash, wall slide, wall-jump, ride the moving platform, pass up through the one-way ledge,
      collect coins, stomp an enemy, take a hit, respawn at a checkpoint, fall into the kill zone, lose the
      run to the game-over screen.
- [ ] **Every coin in both levels is reachable** — checked one by one, not sampled.
- [ ] **Pause works in the build**: Escape freezes everything, the timer stops, nothing fires on resume, and
      **Back to menu** loads an unfrozen menu.
- [ ] **The best time persists across a real quit.** First launch: best equals the run. Quit, relaunch, run
      slower: the best time is the earlier, faster one.
- [ ] **Fullscreen actually works.** The Options toggle takes the window fullscreen and back — the effect the
      Editor could not show — and the choice survives a relaunch.
- [ ] **All three volume sliders work in the build**, separately, and persist.
- [ ] **Rebinding works in the build**: rebind `Jump`, play with it, quit, relaunch, and it is still bound.
      **Reset to defaults** restores the originals and they stay restored after another relaunch.
- [ ] **Both devices work.** Keyboard and gamepad each drive move, jump, dash and pause — with the gamepad
      connected before launch. *(This is where the gamepad box left open at the M3 and M12 gates is closed.)*
- [ ] **Quit closes the window.**
- [ ] **The repository is publishable.** `README.md`, `LICENSE` and `CREDITS.md` exist; the README's screenshot
      renders and its control table matches the game; `git check-ignore Builds Library` prints both;
      `git lfs ls-files` lists the binaries; `git status --porcelain` prints nothing; the `v1.0.0` tag exists
      on the remote with a build attached to its release.
- [ ] **Someone else can use it.** Download your own release zip on a machine without Unity — or ask a
      friend — unzip it, and play it.

## Files after this milestone (the checkpoint)

_This milestone authored three repository documents and changed one settings file; it wrote no C#. Files not
listed were not touched this milestone._

### Created this milestone

| Path | What it is |
|---|---|
| `cavern-dash/README.md` | the repository's front door — what it is, how to play, how to build, credits |
| `cavern-dash/LICENSE` | MIT, with your name and year |
| `cavern-dash/CREDITS.md` | the CC0 asset packs, with links |
| `cavern-dash/docs/screenshot.png` | the README's screenshot |
| `cavern-dash/Builds/<platform>/` | the build output — **ignored by Git**, distributed through a release |

### Editor checkpoint

| Where | Setting | Exact value |
|---|---|---|
| Project Settings → Player | Company Name | yours — **never changed after release** |
| Project Settings → Player | Product Name | `Cavern Dash` |
| Project Settings → Player | Version | `1.0.0` |
| Project Settings → Player | Icon (Windows/Mac/Linux) | overridden, a Kenney texture |
| Project Settings → Player | Fullscreen Mode | `Windowed` |
| Project Settings → Player | Default Screen Width / Height | `1280` / `720` |
| Project Settings → Player | Resizable Window / Run In Background | ticked / unticked |
| File → Build Profiles | Platform | Windows, Mac, Linux — your platform and architecture |
| File → Build Profiles | Development Build | **unticked** |
| File → Build Profiles | Scene List | `Menu` (0), `Level01` (1), `Level02` (2), `Win` (3), all ticked |

### Pre-existing files modified
- `ProjectSettings/ProjectSettings.asset` — product name, icon, resolution and presentation, from
  [step 01](01_player-settings.md).

### Unchanged this milestone
- Every script under `Assets/_Project/Scripts` — the game's code was finished at M12 and the build required no
  change to it.
- Every scene, prefab and asset.

## Troubleshooting

| Symptom | Likely cause → fix |
|---|---|
| The build opens to a black screen | A scene is missing from the build list, or `Menu` is not at index 0. |
| The title says `(Development Build)` | That box was ticked in the build profile. Rebuild without it. |
| macOS refuses to open the app | Gatekeeper. Right-click the app → **Open** → confirm. |
| `Build target not supported` | The platform's build-support module is not installed; add it in the Hub. |
| IL2CPP fails on a missing C++ toolchain | Windows: the *Desktop development with C++* workload. macOS: `xcode-select --install`. |
| The built game has no saved data | Correct on a first launch: builds and the Editor keep separate `PlayerPrefs`. |
| The gamepad works in the Editor but not the build | Connect it before launching, and check no overlay has claimed it. |
| The UI is clipped in fullscreen | A `Canvas Scaler` left at `Constant Pixel Size` in one scene. |
| `git status` lists the build folder | The output is not in `Builds/`, or is inside `Assets/`. |
| The push is rejected for a large file | A build was committed earlier; that needs a history rewrite. |

## Handoff
- **You now have:** *Cavern Dash* — a finished 2D platformer, built as a standalone executable, tested outside
  the Editor, and published from a repository with a README, a licence, credits and a tagged release. It is
  Unity 6.3 LTS, C# 9, the Input System, Cinemachine, a tilemap level and CC0 art, and every line of it came
  from a decision you can point at in
  [`../foundation/decision-log.md`](../foundation/decision-log.md).
- **Open / deferred:** the guide's explicit non-goals stay out — no CI pipeline, no automated tests, no
  multiplayer, no mobile build, no procedural generation, no localization, no store publishing. They were
  excluded on purpose, and they are all reachable from where you are standing.
- **Next:** there is no next milestone — this is the last one. Where to go from here is yours to choose; the
  obvious three are more levels (you have the tilemap and the prefabs, and a level is a scene duplicate), a
  boss or a second enemy type (`EnemyPatrol` and `EnemyContact` are the pattern), and a real save file
  (`PlayerPrefs` is where the guide stopped, and `JsonUtility` with `Application.persistentDataPath` is where
  it would continue). Start from the guide's [README](../README.md) if you want to re-read how any of it was
  put together.

---
> Nav: [← The repository](04_the-repository.md) · [Overview](00_overview.md) · [Back to the guide](../README.md)
