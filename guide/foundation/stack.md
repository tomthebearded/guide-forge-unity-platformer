<!-- Foundation doc. Section headings and table column keys stay English by design (they are the schema
     the GuideForge skills look things up by). Prose language: English. -->

# Verified stack — Cavern Dash

> Pinned versions and official docs for this guide, verified online on **2026-08-22**.
> Every milestone uses these exact versions. If you're following this later, re-check the "Latest stable"
> column — if it moved, reconcile before following (the review-before-follow gate).

> **Target OS / shell(s):** **Windows (PowerShell)** and **macOS (zsh/bash)** — the guide targets both, and the
> handful of commands it runs are `git`/`git lfs` invocations that are identical on either. Everything else
> happens inside the Unity Editor, which is the same on both platforms. **Load-bearing:** any command added
> later must run on both shells, or carry a variant per shell.

| Tool / package | Pinned version | Latest stable (as of 2026-08-22) | Official docs | Notes |
|---|---|---|---|---|
| **Unity Editor** | **6.3 LTS — `6000.3.x`** (use the newest `6000.3` patch Unity Hub offers) | Unity 6.3 LTS is the current LTS; 6.4/6.5 are non-LTS tech-stream releases | https://unity.com/releases/unity-6 | Supported until **December 2027**. **Unity 6.0 LTS support ends October 2026** — do not pin it. Docs for this line: https://docs.unity3d.com/6000.3/Documentation/Manual/ |
| **Unity Hub** | latest | latest | https://unity.com/unity-hub | The installer/version manager; the guide installs the Editor through it. |
| **C#** | **C# 9.0** | C# 9.0 is the language version Unity 6.3 supports | https://docs.unity3d.com/6000.3/Documentation/Manual/csharp-compiler.html | No C# 10+ syntax anywhere in the guide (no file-scoped namespaces, no `global using`). |
| **Universal 2D template (URP 17)** | bundled with 6.3 | URP 17 is the Unity 6 line | https://docs.unity3d.com/6000.3/Documentation/Manual/urp/Setup.html | The Hub's **Universal 2D** template. URP is used as-is: no custom shaders, no 2D lights. |
| **Input System** | **`com.unity.inputsystem` 1.19.x** | 1.19.0 released for Unity 6000.4/6000.6; 6.3 resolves the matching 1.19 build | https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/ | Installed by default in Unity 6 projects. Unity 6 ships a **project-wide actions asset** named `InputSystem_Actions` with `Player` (Move, Jump, …) and `UI` maps already present — the guide *edits* it rather than creating one. **Rebinding APIs used:** `PerformInteractiveRebinding`, `SaveBindingOverridesAsJson`, `LoadBindingOverridesFromJson` — https://docs.unity3d.com/Packages/com.unity.inputsystem@latest/api/UnityEngine.InputSystem.InputActionRebindingExtensions.html |
| **Cinemachine** | **`com.unity.cinemachine` 3.1.x** | 3.1.6 / 3.1.7 for the 6000.3 line | https://docs.unity3d.com/Packages/com.unity.cinemachine@3.1/ | **Breaking rename:** `CinemachineVirtualCamera` → **`CinemachineCamera`**, namespace → `Unity.Cinemachine`, `m_` field prefixes dropped. Upgrade notes: https://docs.unity3d.com/Packages/com.unity.cinemachine@3.1/manual/CinemachineUpgradeFrom2.html |
| **2D Tilemap** | bundled with the 2D template | — | https://docs.unity3d.com/6000.3/Documentation/Manual/class-Tilemap.html | Grid, Tilemap, Tile Palette, `TilemapCollider2D`. |
| **2D Tilemap Extras** | **`com.unity.2d.tilemap.extras`** (install via Package Manager) | 6.0.x doc line published | https://docs.unity3d.com/Packages/com.unity.2d.tilemap.extras@latest/ | Only for the **Rule Tile** (auto-tiling). Docs: https://docs.unity3d.com/Manual/com.unity.2d.tilemap.extras.html |
| **TextMeshPro** | via **`com.unity.ugui` 2.x** | — | https://docs.unity3d.com/Packages/com.unity.ugui@2.0/manual/TextMeshPro/index.html | TMP was **merged into the uGUI package** and ships with the Editor — nothing to install, but **TMP Essential Resources must be imported once** (`Window > TextMeshPro > Import TMP Essential Resources`). The old `com.unity.textmeshpro` package is deprecated. |
| **Build Profiles** | built in | — | https://docs.unity3d.com/6000.3/Documentation/Manual/create-build-profile.html | Unity 6 **replaced the Build Settings window with Build Profiles** (`File > Build Profiles`). Every build instruction in the guide uses the new name. |
| **Git** | latest | — | https://git-scm.com/doc | The reader already knows Git; only Unity-specific rules are taught. |
| **Git LFS** | latest | — | https://git-lfs.com | Tracks `*.png *.wav *.ogg *.psd` from M1. GitHub's free tier: 1 GB storage + 1 GB/month bandwidth. |
| **Art — Kenney "Pixel Platformer"** | pack v1.2 | CC0 | https://kenney.nl/assets/pixel-platformer | **18 × 18 px tiles**, ~200 files, tiles + characters + backgrounds. **Pixels Per Unit = 18** is a load-bearing value in this guide. |
| **Audio — Kenney "Impact Sounds", "Interface Sounds", "Music Jingles"** | current | CC0 | https://kenney.nl/assets/impact-sounds · https://kenney.nl/assets/interface-sounds · https://kenney.nl/assets/music-jingles | Jump/coin/hit SFX, menu clicks, a win jingle. |
| **Licence of all Kenney assets** | **CC0 (public domain)** | — | https://kenney.nl/support | *"All game assets on the asset pages are public domain licensed (CC0). You're free to use them, even in commercial projects."* Attribution **not required**; the Kenney **logo** may not be reused. This is what makes committing them to a public repo legal. |

### Install (the exact commands, at the pinned versions)

```text
1. Install Unity Hub            → https://unity.com/unity-hub
2. In the Hub: Installs > Install Editor > Unity 6.3 LTS (6000.3.x)
   Modules to tick: Windows Build Support (IL2CPP) on Windows,
                    Mac Build Support (IL2CPP) on macOS,
                    Documentation (optional), Visual Studio / Rider integration.
3. git --version        (any recent Git)
4. git lfs install      (once per machine)
```

### Load-bearing API facts (steps must honor these exact spellings)

- Physics runs in **`FixedUpdate`**; input is read in **`Update`**. Never move a `Rigidbody2D` from `Update`.
- Move a Dynamic body by writing **`rb.linearVelocity`** — `Rigidbody2D.velocity` was renamed; the guide uses
  `linearVelocity` throughout.
- The Input System's project-wide asset is **`InputSystem_Actions`**, with the **`Player`** action map.
- Cinemachine's camera component is **`CinemachineCamera`** (`Unity.Cinemachine`), *not*
  `CinemachineVirtualCamera`.
- TMP text on a Canvas is **`TextMeshProUGUI`**; **TMP Essential Resources must be imported once**.
- Rebinding is an **override layer**, never an edit of the asset: start it with
  **`action.PerformInteractiveRebinding(bindingIndex)`**, persist it with
  **`SaveBindingOverridesAsJson()`**, restore it on launch with **`LoadBindingOverridesFromJson(json)`**, and
  clear it with **`RemoveAllBindingOverrides()`**.
- Mixer volume is set in **decibels** via an **exposed parameter** (`SetFloat`), while a slider is linear
  **0–1** — the conversion is `Mathf.Log10(value01) * 20`, and `0` must be special-cased to `-80 dB`.
- Builds are made from **`File > Build Profiles`**, not "Build Settings".
- Kenney Pixel Platformer tiles are **18 × 18 px** → **Pixels Per Unit = 18**, Filter Mode **Point (no
  filter)**, Compression **None**.

---
