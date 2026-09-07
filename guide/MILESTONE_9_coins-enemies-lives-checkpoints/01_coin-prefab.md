# M9 · Step 01 of 06 — Make a coin
> Nav: — · [Overview](00_overview.md) · [Collect coins →](02_collect-coins.md)

**Before you start:** M8's gate passed. Play Mode is stopped, and `Level01` is open.

## Glossary for this step
> New here: **[prefab](../foundation/glossary.md#prefab)** (defined in *Why / design*) ·
> **[trigger](../foundation/glossary.md#trigger)** (defined in *Do this*, action 3).

## Why / design
A level needs twenty coins, not one, and the twenty must be identical — same size, same collider, same
behaviour — and stay identical when you change your mind about any of that.

> New concept — **prefab**: a GameObject saved as an asset, so it can be placed many times and edited once.
> Change the prefab and every instance in every scene changes with it. Prefabs are how "the level is content,
> not code" from [`../foundation/conventions.md`](../foundation/conventions.md) actually works in Unity.

This step builds the coin and saves it as a prefab; [step 02](02_collect-coins.md) makes it do something. The
split is deliberate — building the object and giving it behaviour are different kinds of work, and mixing them
is how a "just add a coin" task turns into an hour.

## Do this

1. In the **Hierarchy**, right-click and choose **2D Object > Sprites > Circle**. Rename it **`Coin`**.

2. Set its `Transform` **Scale** to `0.5, 0.5, 1` — half a tile, which reads as a pickup rather than an
   obstacle — and set its `Sprite Renderer` **Color** to a gold, hex `F2C14E`. The colour is cosmetic; the
   scale is worth keeping, because [step 02](02_collect-coins.md)'s collider is sized against it.

3. **Add Component > Circle Collider 2D**. Set **Radius** to `0.5` and tick **Is Trigger**.

   > New concept — **trigger**: a collider that detects overlap but stops nothing. Physics reports the overlap
   > to your code and lets both objects pass through each other. Coins, checkpoints and the level exit are all
   > triggers; floors and walls are not.
   >
   > One rule that catches everyone once: **at least one of the two overlapping objects must have a
   > `Rigidbody2D`** for the overlap to be reported at all. The coin does not need one — the `Player` has had
   > one since [M3](../MILESTONE_3_input-and-running/04_rigidbody-and-collider.md), and that is enough.

   The radius is in the coin's **local** units, which its scale of `0.5` then halves: the trigger is a quarter
   of a tile across in the world, slightly smaller than the sprite, so the coin is collected when you are
   clearly on it rather than when you brush past.

4. Create the folder `Assets/_Project/Prefabs` if it does not exist, then **drag the `Coin` object from the
   Hierarchy into that folder** in the Project panel. Unity writes `Coin.prefab` and the object in the scene
   turns blue — it is now an *instance* of the prefab rather than a loose object.

5. Delete the `Coin` from the Hierarchy (select it, press **Delete**). The prefab asset stays; the scene is
   clean.

6. Now place them. Drag `Coin.prefab` from the Project panel into the **Scene** view six or eight times, and
   position each one somewhere that takes a little effort to reach: over a gap, above a ledge, at the far end
   of the moving platform's path, in the arc of a wall-jump.

   Placement is **illustrative** — it is your level. What matters is that at least one coin needs a jump, one
   needs the moving platform, and one needs the dash, so that every later gate has to exercise all three
   abilities to collect them.

7. Save the scene and commit.

## Done when (this step)
- [ ] `Assets/_Project/Prefabs/Coin.prefab` exists, and the Hierarchy holds six or more `Coin` instances
      shown with the blue prefab icon.
- [ ] Selecting any coin shows `Circle Collider 2D` with **Radius** `0.5` and **Is Trigger** ticked.
- [ ] Pressing **Play** → the player runs straight through every coin without slowing, and nothing happens.
      *(Nothing is listening yet; [step 02](02_collect-coins.md) is the listener.)*
- [ ] Editing the prefab (double-click it in the Project panel, change the colour, save) changes **all** the
      placed coins at once — undo it afterwards if you like the gold.
- [ ] The Console shows no red entries.

## Suggested commit
```
feat(level): add a coin prefab and place coins around the cavern
```

## If it breaks
- **The player bumps into the coins and stops** → **Is Trigger** is unticked. A solid coin is a wall.
- **The coins are enormous** → the `Transform` scale is `1, 1, 1` and the collider radius was raised instead.
  Scale the object, keep the radius at `0.5`.
- **Dragging into the Project panel moved the object instead of making a prefab** → you dropped it onto a
  folder in the *Hierarchy*. The Project panel is the bottom one, showing files.
- **The placed coins do not update when you edit the prefab** → they were created by copy-paste before the
  prefab existed, so they are loose objects. Delete them and drag fresh ones from the Project panel.

---
> Nav: — · [Overview](00_overview.md) · [Collect coins →](02_collect-coins.md)
