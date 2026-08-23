# M13 · Step 04 of 05 — The repository
> Nav: [← Test the build](03_test-the-build.md) · [Overview](00_overview.md) · [Verify →](05_verify.md)

**Before you start:** [step 03](03_test-the-build.md) finished — the built game plays through, persists, and
quits. Anything you fixed while testing is committed.

## Why / design
The last thing between "I built a game" and "here is my game" is a repository someone else can make sense of.
Three files do most of that work, and one of them you owe to other people rather than to yourself.

**The README** answers, in this order: what is this, what does it look like, how do I play it, how do I build
it. A visitor decides in about fifteen seconds.

**The licence** says what others may do with your work. MIT is the usual choice for a project like this: do
what you like, keep the notice, no warranty.

**The credits** are the one that is not optional in spirit. Kenney's assets are **CC0**, so attribution is not
legally required — and shipping several hundred files somebody else drew without saying where they came from
is still the wrong thing to do. It costs three lines.

One practical check before you push: **the build output must not be in the repository**. `Builds/` has been
ignored since M1, and a repository carrying a hundred megabytes of compiled game is unpleasant to clone and
awkward to undo.

## Do this

1. In the `cavern-dash` folder — beside `Assets`, not inside it — create **`README.md`**:

   ```markdown
   # Cavern Dash

   A small 2D platformer built in Unity 6.3 LTS: run, jump, dash and wall-jump through a
   tilemap cavern, collect coins, stomp enemies, and reach the exit before the clock does.

   ![screenshot](docs/screenshot.png)

   ## Play it

   Download the build for your platform from the Releases page, unzip it, and run
   `Cavern Dash.exe` (Windows) or `Cavern Dash.app` (macOS).

   | Action | Keyboard | Gamepad |
   |---|---|---|
   | Move | A / D or arrow keys | Left stick |
   | Jump | Space | South button |
   | Dash | Left Shift | West button |
   | Pause | Escape | Start |

   All controls can be rebound from **Options** on the title screen.

   ## Build it yourself

   1. Install Unity **6.3 LTS** (`6000.3.x`) through Unity Hub, with the build-support
      module for your platform.
   2. Clone this repository and open the `cavern-dash` folder as a project.
   3. **File > Build Profiles**, select your platform, press **Build**.

   ## Credits

   Art and audio by [Kenney](https://kenney.nl) — [Pixel Platformer](https://kenney.nl/assets/pixel-platformer),
   [Impact Sounds](https://kenney.nl/assets/impact-sounds), [Interface Sounds](https://kenney.nl/assets/interface-sounds)
   and [Music Jingles](https://kenney.nl/assets/music-jingles) — all released under CC0.

   Built by following [a step-by-step guide](../guide/README.md).

   ## Licence

   [MIT](LICENSE) for the code. The Kenney assets are CC0 (public domain).
   ```

   Adjust the last line under *Credits* to point wherever your copy of the guide lives, or delete it.

2. Take a screenshot. Run the built game, capture a moment that shows the character mid-jump over a cavern
   with coins in view, and save it as `cavern-dash/docs/screenshot.png`. A README with a picture is read; one
   without is skimmed.

3. Add **`LICENSE`** beside the README. Copy the MIT licence text from <https://choosealicense.com/licenses/mit/>,
   and put your name and the year in its copyright line.

4. Add **`CREDITS.md`**, which is where the detail goes so the README can stay short:

   ```markdown
   # Credits

   ## Art
   - **Pixel Platformer** by Kenney — https://kenney.nl/assets/pixel-platformer — CC0

   ## Audio
   - **Impact Sounds** by Kenney — https://kenney.nl/assets/impact-sounds — CC0
   - **Interface Sounds** by Kenney — https://kenney.nl/assets/interface-sounds — CC0
   - **Music Jingles** by Kenney — https://kenney.nl/assets/music-jingles — CC0

   All Kenney assets are released under the Creative Commons Zero (CC0) public domain
   dedication: no attribution is required. This file exists anyway, because it should.

   The Kenney logo is not included and is not covered by CC0.
   ```

5. Check the repository is clean before it becomes public:

   ```
   git status --porcelain
   git check-ignore Builds Library
   git lfs ls-files
   ```

   The first should list only your new files; the second must print both `Builds` and `Library`; the third
   should list your PNGs and audio, proving the binaries went through LFS rather than into Git proper.

6. Commit, then push to your remote and tag the release:

   ```
   git tag v1.0.0
   git push origin main --tags
   ```

   On GitHub, open **Releases > Draft a new release**, choose the `v1.0.0` tag, and attach a zip of your
   `Builds/<platform>` folder. That is what makes the README's *"download the build"* line true — the
   executable reaches players through a release, never through the repository itself.

## Done when (this step)
- [ ] `cavern-dash/README.md`, `LICENSE` and `CREDITS.md` all exist, and the README shows a screenshot that
      renders.
- [ ] The README's control table matches what the game actually does.
- [ ] `git check-ignore Builds Library` prints **both** names.
- [ ] `git lfs ls-files` lists the imported PNGs and audio files.
- [ ] `git status --porcelain` prints nothing after committing.
- [ ] The tag `v1.0.0` exists locally (`git tag` lists it) and on the remote after the push.
- [ ] A release on the remote carries a zip of the build, and downloading it and unzipping it yields a game
      that runs on a machine without Unity.

## Suggested commit
```
docs(repo): add README, licence and credits for the 1.0.0 release
```

## If it breaks
- **`git check-ignore Builds` prints nothing** → the build output landed somewhere other than a folder named
  `Builds`, or inside `Assets/`. Move it; the ignore rule is `/[Bb]uilds/`.
- **The push is rejected for a file over 100 MB** → a build folder was committed before the ignore rule
  matched it. It is in history now, and removing it means rewriting that history — `git filter-repo` or a
  fresh clone-and-recommit are the two honest routes.
- **The screenshot does not render on the remote** → the path is case-sensitive there and often is not
  locally. `docs/screenshot.png` and `docs/Screenshot.PNG` are different files on GitHub.
- **Someone clones it and Unity opens an empty project** → they opened the repository root instead of the
  `cavern-dash` folder. Say so in the README's build instructions, as the text above does.
- **`git lfs ls-files` prints nothing** → the binaries were committed before LFS was configured. They work,
  but they are in ordinary history; the fix is a history rewrite, and for a project this size it is usually
  not worth it.

---
> Nav: [← Test the build](03_test-the-build.md) · [Overview](00_overview.md) · [Verify →](05_verify.md)
