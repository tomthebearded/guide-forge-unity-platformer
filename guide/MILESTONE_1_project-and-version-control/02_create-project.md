# M1 · Step 02 of 07 — Create the Universal 2D project
> Nav: [← Install Unity Hub and Unity 6.3 LTS](01_install-unity.md) · [Overview](00_overview.md) · [Find your way around the Editor →](03_find-your-way-around.md)

**Before you start:** [step 01](01_install-unity.md) finished — the Hub's **Installs** tab lists a
`6000.3.x` Editor with its build-support module.

## Glossary for this step
> New here: **[asset](../foundation/glossary.md#asset)** (defined in *Do this*, action 4) ·
> **[Universal Render Pipeline (URP)](../foundation/glossary.md#universal-render-pipeline-urp)** (defined in *Why / design*).

## Why / design
A Unity project is a **folder**, not a file. You point the Hub at a location, pick a **template**, and it
writes that folder for you — pre-loaded with the packages and settings that template implies. Choosing the
right template now saves a long afternoon of adding packages by hand later.

You want **Universal 2D**. It configures the project for the **Universal Render Pipeline** — Unity's modern,
scriptable rendering path — with its **2D Renderer** already set up, and it pre-installs the packages this
guide leans on: the Input System, 2D Tilemap, and TextMeshPro. This guide never writes a shader or a light, so
URP is scenery rather than a subject; you pick it because it is the default 2D path in Unity 6 and because
choosing the old built-in pipeline would leave you on a route Unity is steadily retiring.

**The project folder name is load-bearing for nothing** — call it what you like. This guide calls it
`cavern-dash`, and every path it quotes is relative to that folder, so a different name simply means reading
`cavern-dash/Assets/…` as `<your-folder>/Assets/…`.

## Do this

1. In the Hub's left sidebar click **Projects**, then the **New project** button at the top right.

2. At the top of the dialog, open the **Editor Version** dropdown and select your **`6000.3.x`** install. Do
   this *first*: the template list is filtered by the version you pick, and Unity 6 templates do not appear
   under an older Editor.

3. In the template list on the left, select **Universal 2D**. If it shows a **Download template** button
   instead of a preview, press it and wait — templates are fetched on first use.

   Leave the **Connect to Unity Cloud** and **Use Unity Version Control** toggles **off**. This guide uses
   Git, which you set up in [step 04](04_git-init.md), and Unity Cloud plays no part in it.

4. On the right, set **Project name** to `cavern-dash` and **Location** to wherever you keep code. Press
   **Create project**. Unity writes the folder, imports every **asset** in the template, and opens the Editor —
   the first import takes a few minutes.

   > New concept — **asset**: anything in your project's `Assets/` folder that Unity imports and can use — a
   > script, a sprite, a scene, a sound, a prefab, a settings file. "Importing" is Unity reading the source
   > file and producing its own internal version of it, which is why the first open is slow and later ones are
   > not.

5. When the Editor has opened, look at the folder Unity created. You will not touch most of it, but you need
   to recognise the four top-level names, because [step 04](04_git-init.md) decides which of them Git tracks:

   | Folder | What it is | Goes into Git? |
   |---|---|---|
   | `Assets/` | Everything you author or import. This is the project. | **Yes** |
   | `Packages/` | `manifest.json` — the list of packages this project uses, by version. | **Yes** |
   | `ProjectSettings/` | Every setting from the Project Settings window, as text files. | **Yes** |
   | `Library/` | Unity's generated import cache. Gigabytes, machine-specific, rebuilt on demand. | **No — never** |

## Done when (this step)
- [ ] The Unity Editor is open and its title bar reads `cavern-dash - SampleScene - Windows, Mac, Linux -
      Unity 6000.3.<patch>` (the exact patch digits are whatever you installed).
- [ ] The **Project** panel at the bottom shows an `Assets` folder containing at least `Scenes` and
      `Settings`.
- [ ] The **Console** panel shows **no red error entries** after the import finishes. Yellow warnings are
      normal and can be ignored.
- [ ] Your `cavern-dash` folder on disk contains `Assets`, `Packages`, `ProjectSettings` and `Library`.

## If it breaks
- **"Universal 2D" is missing from the template list** → the Editor Version dropdown is still on an older
  install. Set it to `6000.3.x` first; the list re-filters.
- **The Editor opens on a magenta or blank Game view** → the template's render pipeline asset did not get
  assigned. Close the Editor, delete the `Library/` folder inside `cavern-dash`, and open the project again
  from the Hub: Unity re-imports and re-links it. Deleting `Library/` is always safe — that is the whole point
  of it being generated.
- **Import appears frozen at "Importing assets"** → it is usually genuinely working. Give it ten minutes on a
  first open before concluding anything; Unity is compiling shaders.

---
> Nav: [← Install Unity Hub and Unity 6.3 LTS](01_install-unity.md) · [Overview](00_overview.md) · [Find your way around the Editor →](03_find-your-way-around.md)
