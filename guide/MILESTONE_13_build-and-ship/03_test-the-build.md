# M13 · Step 03 of 05 — Test the build
> Nav: [← Build it](02_build-it.md) · [Overview](00_overview.md) · [The repository →](04_the-repository.md)

**Before you start:** [step 02](02_build-it.md) finished — the executable exists in
`cavern-dash/Builds/<platform>/`. **Close the Unity Editor**, or at least stop Play Mode: two copies of the
game writing the same `PlayerPrefs` will confuse you about which one saved what.

## Why / design
A build is not a faster Play Mode. It is a **different environment**, and a short list of things behave
differently in it — every one of which has shipped as a bug in somebody's game:

- **Scenes** come from the build list, not from whatever is open in the Editor.
- **The window** is a real window, so `Screen.fullScreen` finally does something. This is the first time
  M12's toggle can be seen working.
- **`PlayerPrefs`** are stored under the Company and Product names from
  [step 01](01_player-settings.md) — a *different* store from the Editor's, so the built game starts with no
  best time and no rebinds. That is correct, and it is the only chance you get to see a first launch.
- **Input** comes from the OS rather than through a focused Editor panel, which is where a gamepad that
  "worked in the Editor" sometimes stops working.
- **There is no Console.** Everything you have been reading in it since M2 is gone. What the game shows on
  screen is all a player has.

So this step is not a formality. It is the first honest test of everything.

## Do this

1. Double-click **`Cavern Dash.exe`** (Windows) or **`Cavern Dash.app`** (macOS). The game opens in a
   1280 × 720 window whose title bar reads `Cavern Dash`.

   On macOS the first launch is blocked as an unidentified developer: right-click the app, choose **Open**,
   and confirm. That is Gatekeeper, not a bug in your build.

2. Play the whole game: **Play** → `Level01` → collect coins, stomp an enemy, take a hit, use a checkpoint →
   the exit → `Level02` → its exit → the win screen. Note the time it reports.

3. On the win screen, check the best time: on this first launch it equals your run, because the built game's
   `PlayerPrefs` are empty.

4. **Quit the game entirely** and launch it again. Play another run — deliberately slower. The win screen
   shows the new run's time and the **old, faster** best time. That is the persistence gate, tested where it
   actually matters.

5. Test the options, in the built window this time:
   - Drag each volume slider. All three work.
   - Tick **Fullscreen**. The window goes fullscreen — the thing you could not see in the Editor. Untick it:
     it returns.
   - Rebind **Jump** to **J**, play a moment, and confirm **J** jumps.
   - Quit, relaunch, and open Options: **J** is still bound and fullscreen is still where you left it.
   - Press **Reset to defaults**, and confirm **Space** jumps again.

6. Test the gamepad if you have one — plug it in **before** launching. Move with the left stick, jump with the
   south button, dash with the west button, pause with **Start**. If you left the gamepad box open at the M3
   or M12 gate, this is where it closes.

7. Play the whole game once more and try to reach **every coin** you placed in both levels. The point is not
   the coins: it is that a level with an unreachable collectible is a level you cannot finish, and the Editor
   never told you.

8. Press **Escape** during play and use **Back to menu**; then use **Quit** on the menu. The window closes —
   the one thing `Application.Quit()` could not do in the Editor.

## Done when (this step)
- [ ] The executable launches by double-click, in a 1280 × 720 window titled `Cavern Dash`, with your icon.
- [ ] The whole loop plays through: menu → `Level01` → `Level02` → win screen.
- [ ] The **first** launch shows a best time equal to the run; a **second** launch keeps the better of the
      two.
- [ ] The **Fullscreen** toggle actually makes the window fullscreen, and back.
- [ ] A rebind survives quitting and relaunching; **Reset to defaults** restores the originals, and they stay
      restored after another relaunch.
- [ ] All three volume sliders work in the build.
- [ ] With a gamepad: move, jump, dash and pause all work — **and** the keyboard still works.
- [ ] Every coin in both levels is reachable.
- [ ] The **Quit** button closes the window.
- [ ] Nothing on screen mentions a missing scene, and no scene loads to a black screen.

## If it breaks
- **The game opens to a black screen** → a scene is missing from the build list, or `Menu` is not at index 0.
  Rebuild after fixing the list.
- **The window is titled `Cavern Dash (Development Build)`** → **Development Build** was ticked. Rebuild
  without it.
- **The best time is already set on the first launch** → you ran the build twice, or you are looking at the
  Editor's store. They are separate, and both are real.
- **The gamepad does nothing in the build but worked in the Editor** → plug it in before launching, and check
  it is not being claimed by another running application (a launcher overlay is the usual culprit).
- **Fullscreen makes the UI enormous or clipped** → the `Canvas Scaler` is not `Scale With Screen Size` in
  that scene. All four scenes use `640 × 360`, Match `0.5`.
- **A coin is unreachable** → open the level in the Editor and move it. This is exactly what the step is for;
  rebuild and check again.
- **The game runs but has no sound** → the volume sliders are stored at zero from a previous experiment.
  Options, drag them up.

---
> Nav: [← Build it](02_build-it.md) · [Overview](00_overview.md) · [The repository →](04_the-repository.md)
