# M1 · Step 06 of 07 — Lay out the project folders and name the scene
> Nav: [← Track binary assets with Git LFS](05_git-lfs.md) · [Overview](00_overview.md) · [Verify →](07_verify.md)

**Before you start:** [step 05](05_git-lfs.md) finished — the project's first commit exists and `git status` is
clean. Work in the **Unity Editor** for this step, not the terminal: Unity must be the one that moves and
renames assets, so that it can update the `.meta` files at the same time. The one terminal command at the end
runs from the `cavern-dash` folder, like every other one in this guide.

## Why / design
Everything you author goes in **`Assets/_Project/`** and everything you import from elsewhere goes in
**`Assets/ThirdParty/`**. The reason is one you will feel in M6: once a CC0 art pack drops two hundred files
into your project, "which of these did I write?" becomes a real question, and a folder boundary answers it for
free. The leading underscore in `_Project` is a common Unity convention that sorts your own work to the top of
an alphabetical list.

Renaming the scene now, rather than later, matters for a different reason: from M11 the game loads scenes **by
name**, and those names end up written into a build profile and into your code. `SampleScene` is a template
artefact, and renaming it after five milestones of screenshots and instructions would invalidate all of them.
It becomes **`Level01`** — a load-bearing name, matched exactly in [`../foundation/conventions.md`](../foundation/conventions.md).

## Do this

1. In the **Project** panel, right-click the `Assets` folder and choose **Create > Folder**. Name it
   `_Project` — exactly that, with the underscore. Unity creates `Assets/_Project/` on disk plus its `.meta`.

2. Inside `_Project`, create these seven folders the same way. The names are **cosmetic** in the sense that
   nothing breaks if you rename them, but every later step names paths in this layout, so following it keeps
   the guide readable:
   - `Scripts`
   - `Scenes`
   - `Prefabs`
   - `Art`
   - `Audio`
   - `Animation`
   - `Settings`

3. Back at the `Assets` level, create one more folder beside `_Project`, named `ThirdParty`. It stays empty
   until M6 brings in the art pack.

4. Find the template's scene at `Assets/Scenes/SampleScene.unity` in the Project panel. **Drag it** onto your
   `Assets/_Project/Scenes` folder. Unity moves the file and its `.meta` together and repairs every reference
   to it. Never move a Unity asset with your file manager or with `git mv` while the Editor is open — that is
   how references break.

5. With the scene selected in its new home, press **F2** (or right-click > **Rename**) and rename it to
   `Level01`. Exact spelling, capital L, no space — this string is matched by code from M11 onward.

6. The now-empty `Assets/Scenes` folder that the template created is left over. Right-click it and choose
   **Delete**, then confirm. Unity removes the folder and its `.meta`.

7. Save the scene with **Ctrl+S** / **Cmd+S**, then check what changed on disk — from the `cavern-dash`
   folder, where `--short` writes its paths relative to where you are standing:
   ```
   git status --short
   ```
   You will see renames and additions under `Assets/_Project/`, and deletions under `Assets/Scenes/`. Stage
   and commit them with the message below.

## Done when (this step)
- [ ] The **Project** panel shows `Assets/_Project` containing exactly the seven folders listed in action 2,
      and `Assets/ThirdParty` beside it.
- [ ] The scene file is at `Assets/_Project/Scenes/Level01.unity`, and the Editor's title bar reads
      `cavern-dash - Level01 - …`.
- [ ] `Assets/Scenes` no longer exists.
- [ ] `git status --short` after staging shows a `.meta` file accompanying **every** added folder — for
      example `A  Assets/_Project/Scripts.meta`. A folder added without its `.meta` means Git captured the
      move but not Unity's bookkeeping.
- [ ] The **Console** shows no red error entries.

## Suggested commit
```
chore(project): add the _Project layout and rename the scene to Level01
```

## If it breaks
- **The Console reports "The referenced script … is missing" after the move** → you moved the file outside
  Unity, so the `.meta` and the asset parted company. Undo the move, restore both files from your last commit
  (`git checkout -- Assets`, run from `cavern-dash`), and redo it by dragging inside the Project panel.
- **The renamed scene does not open on double-click** → you renamed the `.meta` instead of the asset. In the
  Project panel, Unity only ever shows you the asset; if you are seeing `.meta` files, you are looking at your
  file manager, not at Unity.
- **`git status` shows the scene as deleted-and-added rather than renamed** → harmless. Git detects renames by
  content similarity at diff time, not at commit time; the history is identical either way.

---
> Nav: [← Track binary assets with Git LFS](05_git-lfs.md) · [Overview](00_overview.md) · [Verify →](07_verify.md)
