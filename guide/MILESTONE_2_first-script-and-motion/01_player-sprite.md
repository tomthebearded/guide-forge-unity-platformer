# M2 · Step 01 of 05 — Add the player sprite
> Nav: — · [Overview](00_overview.md) · [Write your first MonoBehaviour →](02_first-script.md)

**Before you start:** the `Level01` scene from [M1 step 06](../MILESTONE_1_project-and-version-control/06_project-folders.md)
is open in the Editor. If the Hierarchy shows anything other than `Main Camera` and `Global Volume`, open
`Assets/_Project/Scenes/Level01.unity` by double-clicking it in the Project panel.

## Glossary for this step
> New here: **[SpriteRenderer](../foundation/glossary.md#spriterenderer)** (defined in *Do this*, action 2).

## Why / design
Before anything can move, something has to be visible. Unity ships a handful of built-in sprite shapes — a
square, a circle, a capsule — precisely so you can build and test mechanics before you own any art. You use
the square as a stand-in player until M10, where the animation milestone replaces it with a real character
sprite. It stays a square through the tilemap and the moveset — that is deliberate: shape is the last thing
that should distract you while tuning how something moves.

Its **size on screen is cosmetic for now** — do not spend time matching it to anything. From M3 the shape that
matters for gameplay is the collider, whose dimensions you set in world units directly, and from M6 the art
sets the scale.

## Do this

1. In the **Hierarchy** panel, right-click the empty space below the existing objects and choose
   **2D Object > Sprites > Square**. Unity adds a new GameObject to the scene showing a white square in the
   Scene and Game views.

2. With the new object selected, look at the **Inspector**. It has a `Transform` and a **`Sprite Renderer`**.

   > New concept — **SpriteRenderer**: the component that draws a 2D image for a GameObject. Its `Sprite`
   > field holds *what* to draw, its `Color` field tints it, and its `Order in Layer` decides what is drawn in
   > front of what when two sprites overlap. Official reference:
   > <https://docs.unity3d.com/6000.3/Documentation/ScriptReference/SpriteRenderer.html>

3. At the very top of the Inspector, rename the object to **`Player`** — click the name field, type it, press
   Enter. This name is **load-bearing from M9 onward**, where enemies and hazards identify what they hit;
   spell it exactly, capital P.

4. In the `Transform` component, set **Position** to `X 0`, `Y 0`, `Z 0`. Leave `Rotation` at
   `0, 0, 0` and `Scale` at `1, 1, 1` — you will not touch either again until M6.

   Z is not a typo: a 2D game still has a Z axis, and Unity's 2D camera looks down it. Sprites live at
   `Z = 0`; the camera sits at `Z = -10`, which is why it can see them.

5. In the `Sprite Renderer` component, set **Color** to a colour you can pick out against the background —
   this guide uses a warm orange, hex `E8B04B`. Click the colour swatch, paste `E8B04B` into the **Hexadecimal**
   field, and close the picker. **This value is cosmetic**: any colour that contrasts with the grey background
   works.

6. Save the scene with **Ctrl+S** / **Cmd+S**. Scene changes only reach disk when you save, and an unsaved
   scene is the most common cause of "my change disappeared".

## Done when (this step)
- [ ] The **Hierarchy** lists a GameObject named exactly `Player`.
- [ ] The **Game** view shows a solid orange square roughly at the centre of the frame.
- [ ] The `Player`'s Inspector shows `Transform` Position `0, 0, 0` and a `Sprite Renderer` whose `Sprite`
      field is not `None`.
- [ ] `git status --short`, run from `cavern-dash`, → lists `M  Assets/_Project/Scenes/Level01.unity` (the
      scene changed, nothing else). `--short` writes its paths relative to the folder you run it in.

## Suggested commit
```
feat(player): add the player sprite object to Level01
```

## If it breaks
- **The square is invisible in the Game view but visible in the Scene view** → its `Z` is behind the camera,
  or the camera is not looking at the origin. Set the `Player`'s Z to `0` and check `Main Camera` sits at
  `0, 0, -10`.
- **The menu has no "Sprites" entry under 2D Object** → the project was created from a 3D template. Check the
  Hub row for `cavern-dash`: this guide needs the **Universal 2D** template from
  [M1 step 02](../MILESTONE_1_project-and-version-control/02_create-project.md).
- **`git status` shows no change after saving** → you renamed the object but did not save the scene. Press
  Ctrl+S with the Scene view focused.

---
> Nav: — · [Overview](00_overview.md) · [Write your first MonoBehaviour →](02_first-script.md)
