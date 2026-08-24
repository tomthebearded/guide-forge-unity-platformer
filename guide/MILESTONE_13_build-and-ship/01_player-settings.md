# M13 · Step 01 of 05 — Player settings
> Nav: — · [Overview](00_overview.md) · [Build it →](02_build-it.md)

**Before you start:** M12's gate passed. Play Mode is stopped.

## Why / design
Player Settings are what the *built game* knows about itself: its name, its icon, the window it opens in, the
folder your saved data lands in. Almost all of it is cosmetic, and two entries are not.

**Company Name and Product Name are load-bearing for saved data.** `PlayerPrefs` is stored under a path built
from those two strings — on Windows in the registry under `HKCU\Software\<Company>\<Product>`, on macOS in
`~/Library/Preferences/unity.<Company>.<Product>.plist`. Change either after release and every player's best
time and every rebind is orphaned: not deleted, just no longer looked at. Set them now, before anyone has any
data, and never touch them again.

Everything else here is presentation, and it is worth ten minutes because an executable called
`cavern-dash.exe` with the default Unity icon is the difference between something you made and something you
built.

## Do this

1. Open **Edit > Project Settings > Player**. At the top, set:

   | Field | Value | Why |
   |---|---|---|
   | **Company Name** | your name or handle | Half of the `PlayerPrefs` path. **Do not change it later.** |
   | **Product Name** | `Cavern Dash` | The other half, and the window title. |
   | **Version** | `1.0.0` | Shown in the executable's properties; bump it when you ship again. |

2. Set the icon. Still in Player Settings, find the **Icon** section, tick **Override for
   Windows/Mac/Linux**, and drag one of your Kenney sprites into the largest slot — Unity generates the
   smaller sizes.

   If the sprite refuses to drop, its **Texture Type** is `Sprite (2D and UI)` and the icon field wants a
   plain texture. Select the file, duplicate it (**Ctrl+D**), and set the duplicate's Texture Type to
   `Default`.

3. Open the **Resolution and Presentation** section and set:

   | Field | Value | Why |
   |---|---|---|
   | **Fullscreen Mode** | `Windowed` | So the built game opens in a window you can close while testing. M12's toggle switches it at runtime. |
   | **Default Screen Width** | `1280` | 16:9, and twice the `640` reference resolution the UI was designed against. |
   | **Default Screen Height** | `720` | |
   | **Resizable Window** | ticked | Lets you check the UI scaler does its job. |

4. In the same section, find **Run In Background** and **untick** it if it is ticked. A paused game that keeps
   simulating while you alt-tab is a surprise nobody wants from a single-player platformer.

5. Save the project (**File > Save Project**) and check what changed on disk, from the `cavern-dash` folder:
   ```
   git status --short
   ```
   You should see `ProjectSettings/ProjectSettings.asset` modified, plus the icon texture if you duplicated
   one.

## Done when (this step)
- [ ] **Edit > Project Settings > Player** shows a Company Name, **Product Name** `Cavern Dash`, and
      **Version** `1.0.0`.
- [ ] An icon is set for the Windows/Mac/Linux platform, visible as a preview in the Icon section.
- [ ] **Resolution and Presentation** reads `Windowed`, `1280` × `720`, resizable, with **Run In Background**
      unticked.
- [ ] `git status --short`, run from `cavern-dash`, lists `ProjectSettings/ProjectSettings.asset` as
      modified.
- [ ] Pressing Play still runs the game exactly as before — none of this affects the Editor.

## Suggested commit
```
chore(project): set product name, icon and default resolution
```

## If it breaks
- **The icon field will not take your sprite** → see action 2: it needs a texture, not a sprite.
- **Your best time disappears after changing the names** → exactly the trap described above. Change them
  back and the data is visible again; it was never deleted.
- **The Player settings page looks different from this description** → the sections are collapsible and
  ordered per platform. Make sure the **Windows, Mac, Linux** tab is selected at the top of the page.

---
> Nav: — · [Overview](00_overview.md) · [Build it →](02_build-it.md)
