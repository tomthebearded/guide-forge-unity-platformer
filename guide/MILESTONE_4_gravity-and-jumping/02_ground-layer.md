# M4 · Step 02 of 06 — Put the ground on its own layer
> Nav: [← Give gravity some weight](01_tune-gravity.md) · [Overview](00_overview.md) · [Ask whether the player is grounded →](03_ground-check.md)

**Before you start:** [step 01](01_tune-gravity.md) finished — `Gravity Scale` is `4` and the scene is saved.

## Glossary for this step
> New here: **[layer](../foundation/glossary.md#layer)** (defined in *Why / design*) ·
> **[layer mask](../foundation/glossary.md#layer-mask)** (defined in *Why / design*).

## Why / design
[Step 03](03_ground-check.md) needs to ask a very specific question: *is there solid floor directly under the
player's feet?* Not "is anything under the feet" — a coin, a passing enemy or the player's own collider would
all answer yes, and the player would jump off thin air.

The way you scope that question in Unity is a **layer**. Every GameObject belongs to exactly one, layers have
names you choose, and physics queries take a **layer mask** that says which ones to look at.

> New concept — **layer**: a named bucket a GameObject belongs to. Unity gives you eight built-in layers and
> twenty-four slots of your own. The layer is set by the dropdown at the top right of the Inspector.
>
> New concept — **layer mask**: a value naming a *set* of layers, passed to a physics query so it reports hits
> on those layers only. In the Inspector it appears as a dropdown with tick boxes; in code it is a
> `LayerMask` field.

You create three layers now, though only `Ground` is used this milestone, because a layer's number is baked
into every object that uses it and inserting one later renumbers nothing but confuses everything. `OneWay` is
consumed in M7 and `Hazard` in M9; both are single dropdown entries and cost nothing to reserve.

The names are **load-bearing** — `Ground`, `OneWay`, `Hazard`, exactly as spelled, matching
[`../foundation/conventions.md`](../foundation/conventions.md) — because scripts and Inspector masks reference
them by name.

## Do this

1. Select any object in the Hierarchy (`Ground` is convenient). At the **top right of the Inspector**, open
   the **Layer** dropdown and choose **Add Layer…** at the bottom of the list. Unity opens the **Tags and
   Layers** settings.

2. In the **Layers** list, find the first empty **User Layer** slot — it will be `User Layer 6`, since Unity
   reserves 0–5 and the template may use a couple more. Click its text field and type `Ground`. Press Enter.

3. In the next two empty slots, add `OneWay` and then `Hazard`, the same way. Leave every built-in layer name
   (`Default`, `TransparentFX`, `Ignore Raycast`, `Water`, `UI`) untouched.

   Which numeric slot each name lands in does not matter and will differ between projects — that is the whole
   point of referring to layers by name.

4. Close the settings window. Select the **`Ground`** object in the Hierarchy, open the **Layer** dropdown at
   the top right of its Inspector, and choose **Ground**. The object's layer field now reads `Ground`.

5. Confirm the `Player` is still on **`Default`**. It stays there for the whole guide: the ground check in
   [step 03](03_ground-check.md) looks only at the `Ground` layer, so a player that shared it would detect
   itself and believe it was permanently standing on something.

6. Save the scene.

## Done when (this step)
- [ ] **Edit > Project Settings > Tags and Layers** lists `Ground`, `OneWay` and `Hazard` in three
      consecutive User Layer slots, spelled exactly like that.
- [ ] The `Ground` object's Inspector shows **Layer: Ground** at its top right.
- [ ] The `Player` object's Inspector shows **Layer: Default**.
- [ ] `git status --short` → lists `ProjectSettings/TagManager.asset` and
      `Assets/_Project/Scenes/Level01.unity` as modified.

## Suggested commit
```
chore(project): add the Ground, OneWay and Hazard layers
```

## If it breaks
- **The Layer dropdown has no "Add Layer…" entry** → you are looking at the **Tag** dropdown, which sits
  directly above it. They are two separate dropdowns with similar lists.
- **Unity asks "change layer of all child objects?"** → answer either way; neither `Ground` nor `Player` has
  children. It matters from M6, where the tilemap does.
- **You typed the name into a Sorting Layer instead** → sorting layers control *draw order*, not physics.
  They are a different list further down the same settings page; clear it and use the **Layers** list.

---
> Nav: [← Give gravity some weight](01_tune-gravity.md) · [Overview](00_overview.md) · [Ask whether the player is grounded →](03_ground-check.md)
