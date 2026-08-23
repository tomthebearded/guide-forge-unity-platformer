# M1 · Step 03 of 07 — Find your way around the Editor
> Nav: [← Create the Universal 2D project](02_create-project.md) · [Overview](00_overview.md) · [Put the project under Git →](04_git-init.md)

**Before you start:** the `cavern-dash` project from [step 02](02_create-project.md) is open in the Editor.
This step changes nothing on disk — it is the one step in this guide you read with your hands off the
keyboard, because every later step names these panels and assumes you can find them.

## Glossary for this step
> New here: **[scene](../foundation/glossary.md#scene)** (defined in *Do this*, action 1) ·
> **[GameObject](../foundation/glossary.md#gameobject)** (defined in *Do this*, action 1) ·
> **[component](../foundation/glossary.md#component)** (defined in *Do this*, action 3) ·
> **[Play Mode](../foundation/glossary.md#play-mode)** (defined in *Do this*, action 5).

## Why / design
Unity's window is six panels, and almost every instruction in this guide is of the form "in *panel X*, do *Y*".
Learning which is which now is what makes the next eighty steps short. There is also one trap here that costs
every Unity beginner an afternoon exactly once — changes made while the game is running are thrown away when
it stops — and it is much cheaper to learn it deliberately than by losing work.

## Do this

1. Look at the **Hierarchy** panel (left). It lists everything in the currently open **scene** — the file that
   holds one screen's worth of world. The template's scene is called `SampleScene`, and it contains two
   entries: `Main Camera` and `Global Volume`.

   > New concept — **scene**: a file (`.unity`) holding a set of objects and their arrangement. A game is a
   > handful of scenes — a menu, each level — that you load one at a time.
   >
   > New concept — **GameObject**: the container every object in a scene is. On its own it does nothing at
   > all: it has a position and a name. What it *is* — a camera, a player, a coin — comes entirely from the
   > components attached to it.

2. Click **Main Camera** in the Hierarchy. The **Scene** view (centre) shows the world as an editable
   workspace, and the **Game** view (the tab beside it) shows what that camera renders — what a player would
   see. Switch between the two tabs a few times. Throughout this guide, "the Scene view" means the editable
   one and "the Game view" means the played one; they are different tabs and the difference matters.

3. With `Main Camera` still selected, look at the **Inspector** (right). It lists that GameObject's
   **components** — here a `Transform` and a `Camera`. Every value you will ever tune in this guide is a field
   in this panel.

   > New concept — **component**: a unit of behaviour or data attached to a GameObject. `Transform` holds
   > position, rotation and scale — every GameObject has one and it cannot be removed. `Camera` makes the
   > object render the world. Later you attach components you wrote yourself.

4. Look at the **Project** panel (bottom). This is your `Assets/` folder on disk, shown as a tree. Files here
   are *files*: creating a folder in this panel creates one on disk, and deleting from disk while the Editor
   is open confuses it. Beside it is the **Console** tab, where errors, warnings, and anything your code
   prints appear. Keep the Console visible from now on — a red line here is the first symptom of nearly every
   problem in this guide.

5. Press the **Play** button (the ▶ triangle at the top centre). The toolbar tint changes and the Editor
   switches to the Game view: you are in **Play Mode**, running the game. Press **Play** again to stop.

   > New concept — **Play Mode**: the Editor running your game. Everything you change while it runs — a field
   > in the Inspector, an object you add — is **discarded the moment you press Play again**. This is not a bug;
   > it is how Unity guarantees the stopped state is the saved state. Tune freely while playing to find a good
   > value, then write it down, leave Play Mode, and set it again.

6. Press **Ctrl+S** (**Cmd+S** on macOS) to save the scene. Get into the habit now: the Editor does not
   auto-save scenes, and entering Play Mode with unsaved changes is how they get lost.

## Done when (this step)
- [ ] You can name, without looking them up, which panel each of these lives in: the list of objects in the
      scene (Hierarchy), the fields of the selected object (Inspector), the files on disk (Project), the
      errors (Console).
- [ ] Pressing **Play** switches the Editor to the Game view and tints the toolbar; pressing it again returns
      to the stopped state.
- [ ] The Console shows **no red error entries** while Play Mode runs.

## If it breaks
- **A panel is missing entirely** → someone (or a stray drag) closed it. Re-open it from **Window > General >**
  the panel's name, or reset the whole layout with **Window > Layouts > Default**.
- **The Game view is black while the Scene view looks fine** → the camera is looking elsewhere, or the Game
  view's aspect dropdown is set to a size with nothing in it. Select `Main Camera` and check its `Transform`
  position is `(0, 0, -10)`.

---
> Nav: [← Create the Universal 2D project](02_create-project.md) · [Overview](00_overview.md) · [Put the project under Git →](04_git-init.md)
