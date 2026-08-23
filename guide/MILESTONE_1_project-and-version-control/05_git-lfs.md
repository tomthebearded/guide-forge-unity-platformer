# M1 · Step 05 of 07 — Track binary assets with Git LFS
> Nav: [← Put the project under Git](04_git-init.md) · [Overview](00_overview.md) · [Lay out the project folders and name the scene →](06_project-folders.md)

**Before you start:** [step 04](04_git-init.md) finished — `cavern-dash` is a Git repository on branch `main`
with a working `.gitignore`, and nothing has been committed yet. Your terminal is still at the `cavern-dash`
folder.

## Glossary for this step
> New here: **[Git LFS](../foundation/glossary.md#git-lfs)** (defined in *Why / design*).

## Why / design
Git stores a full copy of a file's contents in history every time that file changes. For text that is cheap —
Git compresses it and only the changed lines really cost anything. For a binary — a PNG, a WAV, a font — it is
not: change one pixel and the whole file is stored again. A game project is mostly binaries, and a year of
small art tweaks can leave a repository ten times larger than the project it holds.

**Git LFS** (Large File Storage) fixes this by storing a small text pointer in Git and the real bytes in a
separate store. Clones stay fast because only the versions you check out are fetched.

> New concept — **Git LFS**: an extension that replaces large files in Git history with lightweight pointers,
> keeping the real content in a side store. Documentation: <https://git-lfs.com>. GitHub's free tier gives
> every account 1 GB of LFS storage and 1 GB of bandwidth per month — far more than this project needs, since
> the art you import in M6 is a few hundred kilobytes.

Two rules make or break this. **Configure LFS before the first commit** — a file committed normally stays in
normal history for ever unless you rewrite it. And **never put `.meta` files in LFS**: they are small text
files Unity must read directly.

## Do this

1. Install the LFS filters into your Git installation. This is a **once per machine** command, not once per
   project; running it again is harmless:
   ```
   git lfs install
   ```
   It prints `Updated Git hooks.` — LFS is now available to every repository on this machine. If the command
   is not found, install Git LFS from <https://git-lfs.com> and run it again.

2. Create a file named **`.gitattributes`** in the `cavern-dash` folder, beside `.gitignore`. Each `filter=lfs`
   line hands one file type to LFS; the `-text` at the end tells Git never to apply line-ending conversion to
   it, which would corrupt a binary.

   ```gitattributes
   # cavern-dash/.gitattributes
   # Normalise line endings for text; hand binaries to Git LFS.
   * text=auto

   # Images
   *.png filter=lfs diff=lfs merge=lfs -text
   *.jpg filter=lfs diff=lfs merge=lfs -text
   *.jpeg filter=lfs diff=lfs merge=lfs -text
   *.gif filter=lfs diff=lfs merge=lfs -text
   *.psd filter=lfs diff=lfs merge=lfs -text
   *.aseprite filter=lfs diff=lfs merge=lfs -text

   # Audio
   *.wav filter=lfs diff=lfs merge=lfs -text
   *.ogg filter=lfs diff=lfs merge=lfs -text
   *.mp3 filter=lfs diff=lfs merge=lfs -text

   # Fonts
   *.ttf filter=lfs diff=lfs merge=lfs -text
   *.otf filter=lfs diff=lfs merge=lfs -text
   ```

   Deliberately absent: `*.meta`, `*.unity`, `*.prefab`, `*.asset`, `*.cs`. Those are text files Unity and Git
   both need to read and merge line by line — sending them to LFS would break diffing for no gain.

3. Confirm the patterns are live. `git lfs track` with no arguments lists what is currently tracked:
   ```
   git lfs track
   ```
   It prints `Listing tracked patterns` followed by one line per pattern, each naming `.gitattributes` as its
   source.

4. Stage everything and make the first commit. `git add -A` stages every file that `.gitignore` does not
   exclude:
   ```
   git add -A
   git status --short
   ```
   Read the output before committing: you should see `A` lines for `Assets/…`, `Packages/manifest.json`,
   `ProjectSettings/…`, `.gitignore` and `.gitattributes` — and **not one line mentioning `Library`**.

5. Commit, using the message under *Suggested commit* below.

## Done when (this step)
- [ ] `git lfs track` → prints `Listing tracked patterns` and includes the line
      `*.png (.gitattributes)`.
- [ ] `git status --short` after staging → no line contains `Library`.
- [ ] After committing, `git log --oneline` → prints exactly one line: the short hash and your commit subject.
- [ ] `git status --porcelain` → prints **nothing at all** (an empty response is the pass).

## Suggested commit
```
chore(repo): add gitignore, LFS rules and the initial project
```

## If it breaks
- **`git lfs install` → "git: 'lfs' is not a git command"** → Git LFS is not installed. Get it from
  <https://git-lfs.com> (on macOS with Homebrew, `brew install git-lfs`), then re-run.
- **`git lfs track` prints only the header and no patterns** → `.gitattributes` is in the wrong folder or
  misnamed. It must sit beside `Assets`, spelled exactly `.gitattributes`.
- **The commit lists thousands of files** → that is expected and correct: a fresh Universal 2D project really
  does contain a few thousand small files, almost all of them package metadata and `.meta` sidecars. What
  matters is that none of them are under `Library/`.

---
> Nav: [← Put the project under Git](04_git-init.md) · [Overview](00_overview.md) · [Lay out the project folders and name the scene →](06_project-folders.md)
