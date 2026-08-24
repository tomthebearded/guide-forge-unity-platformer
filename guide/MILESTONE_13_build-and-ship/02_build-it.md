# M13 · Step 02 of 05 — Build it
> Nav: [← Player settings](01_player-settings.md) · [Overview](00_overview.md) · [Test the build →](03_test-the-build.md)

**Before you start:** [step 01](01_player-settings.md) finished — the product name, icon and window size are
set. You need the build-support module you installed in
[M1 step 01](../MILESTONE_1_project-and-version-control/01_install-unity.md); if you skipped it, add it now
through the Hub (**Installs > ⚙ > Add modules**) — it is a download, not a reinstall.

## Glossary for this step
> New here: **[build profile](../foundation/glossary.md#build-profile)** (defined in *Why / design*) ·
> **[IL2CPP](../foundation/glossary.md#il2cpp)** (defined in *Do this*, action 3).

## Why / design
A build takes your project and produces something that runs without Unity: the scenes, the assets they
reference, and your compiled code, packaged for one platform.

> New concept — **build profile**: a saved build configuration — the target platform, the scenes to include,
> and the settings to use. Unity 6 replaced the old **Build Settings** window with **Build Profiles**, so
> older tutorials will send you to a menu entry that no longer exists.
> Documentation: <https://docs.unity3d.com/6000.3/Documentation/Manual/create-build-profile.html>

The one thing that reliably goes wrong is the **scene list**. It is the same list you filled in
[M11 step 02](../MILESTONE_11_scenes-menus-hud-persistence/02_more-scenes.md), and a scene missing from it is
simply not in the build — producing a game that opens to a black screen with no error a player could report.

## Do this

1. Open **File > Build Profiles**.

2. Check the **Scene List** first, before anything else. It must hold exactly four entries, all ticked, in
   this order:

   | Index | Scene |
   |---|---|
   | 0 | `Menu` |
   | 1 | `Level01` |
   | 2 | `Level02` |
   | 3 | `Win` |

   Index 0 is what the built game opens with. If `Level01` is at the top, the game skips the menu.

3. In the **Platforms** list on the left, select **Windows, Mac, Linux** and check the settings on the right:

   | Field | Value |
   |---|---|
   | **Target Platform** | `Windows` on Windows, `macOS` on a Mac |
   | **Architecture** | `Intel 64-bit` on Windows; on Apple silicon, `Apple silicon` |
   | **Development Build** | **unticked** |

   **Development Build** is the one to get right. Ticked, it includes the profiler and the debug console and
   labels the window `(Development Build)`; it is for diagnosing, not for shipping.

   > New concept — **IL2CPP**: Unity's scripting backend that converts your C# to C++ and then to native code,
   > rather than shipping it as .NET assemblies. It builds more slowly and runs faster, and it is the default
   > for standalone platforms — which is why M1 had you install its module.

4. Press **Build** (not *Build And Run* — you want to launch it yourself in
   [step 03](03_test-the-build.md)). Choose a folder **outside `Assets/`**: create
   `cavern-dash/Builds/Windows` (or `Builds/macOS`) and select it.

   `Builds/` is already ignored by `cavern-dash/.gitignore`, the file you wrote in
   [M1 step 04](../MILESTONE_1_project-and-version-control/04_git-init.md) — the entry is `/[Bb]uilds/`, and
   it anchors to the project folder that holds it. A build is output, and output does not belong in a
   repository.

5. Wait. The first IL2CPP build of a project takes several minutes and pins a CPU core; later ones are much
   quicker. When it finishes, Unity opens the folder.

6. Look at what it produced. On Windows: `Cavern Dash.exe`, a `Cavern Dash_Data` folder, and some DLLs — the
   `.exe` alone is not the game, and moving it out of its folder breaks it. On macOS: a single
   `Cavern Dash.app` bundle.

7. Check the Console. A finished build ends with a line reporting the build size and the time it took, and
   **no errors**. Warnings about shader variants or unused assets are normal.

## Done when (this step)
- [ ] **File > Build Profiles > Scene List** holds the four scenes, all ticked, `Menu` at index 0.
- [ ] **Development Build** is unticked.
- [ ] The build completes with **no errors** in the Console.
- [ ] `cavern-dash/Builds/<platform>/` contains `Cavern Dash.exe` plus its `_Data` folder (Windows), or
      `Cavern Dash.app` (macOS).
- [ ] `git status --porcelain` shows **nothing** from the build — proof the `Builds/` ignore rule works.

## If it breaks
- **`Build target not supported` / the platform is greyed out** → the build-support module is not installed.
  Add it through the Hub.
- **The build fails on a script error** → the build compiles more strictly than the Editor. Read the first
  error only; the rest are usually consequences.
- **The build succeeds but `git status` lists hundreds of files** → the output folder is inside `Assets/`, or
  is not named `Builds`. Move it and check `git check-ignore Builds` prints `Builds`.
- **IL2CPP fails with a missing C++ toolchain** → on Windows, install the *Desktop development with C++*
  workload in the Visual Studio Installer; on macOS, run `xcode-select --install`.
- **The build takes twenty minutes** → normal for a first IL2CPP build. It is not stuck.

---
> Nav: [← Player settings](01_player-settings.md) · [Overview](00_overview.md) · [Test the build →](03_test-the-build.md)
