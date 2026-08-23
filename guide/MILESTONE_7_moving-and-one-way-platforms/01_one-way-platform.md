# M7 · Step 01 of 04 — A ledge you can jump up through
> Nav: — · [Overview](00_overview.md) · [A platform that travels →](02_moving-platform.md)

**Before you start:** M6's gate passed — the cavern is painted and solid, and the player can reach every
ledge.

## Glossary for this step
> New here: **[effector](../foundation/glossary.md#effector)** (defined in *Why / design*) ·
> **[one-way platform](../foundation/glossary.md#one-way-platform)** (defined in *Why / design*).

## Why / design
Every surface in the cavern is solid from all six directions, which means the only way up is a jump that
clears the whole ledge. Real platformers are full of thin platforms you rise **through** and then land **on**
— it is what makes vertical space usable without turning the level into a staircase.

> New concept — **one-way platform**: geometry that blocks from above and is transparent from below.
>
> New concept — **effector**: a 2D physics component that changes how a collider behaves without changing its
> shape. The `PlatformEffector2D` is the one that implements one-way collisions, and it also strips side
> friction and bounce, which stops a player from catching on a platform's edge as they pass it.

Writing this by hand means inspecting every contact's normal, deciding whether the player is above or below,
and enabling or disabling the collider accordingly — a well-known bug farm involving a player who is
half-through the platform when the decision flips. The engine ships the correct version behind two Inspector
fields, so this is a borrow, and the interesting work is elsewhere.

The one thing you must change on your side: `PlayerMotor`'s ground check looks only at the `Ground` layer, and
this ledge goes on **`OneWay`** — reserved back in
[M4 step 02](../MILESTONE_4_gravity-and-jumping/02_ground-layer.md) — so that later milestones can treat the
two differently. Without adding it to the mask, the player would stand on the ledge and be unable to jump off
it.

## Do this

1. In the **Hierarchy**, right-click and choose **2D Object > Sprites > Square**. Rename it
   **`OneWayLedge`**.

2. Set its `Transform`: **Scale** `4, 0.25, 1`, and **Position** somewhere the player can reach — above the
   floor and beside a wall works well, for example `X 2`, `Y -1`, `Z 0`. Position is **illustrative**; pick a
   spot in your own cavern that is at most two units above a surface you can jump from.

3. In its `Sprite Renderer`, set **Color** to something distinct from the tiles — this guide uses `7FB069`,
   a green. Cosmetic, but a one-way platform the player cannot recognise on sight is a trap rather than a
   mechanic.

4. **Add Component > Box Collider 2D**. Leave **Size** `1, 1` and **Offset** `0, 0` — the `Transform` scale
   already stretches it to 4 by 0.25 units. Then tick **Used By Effector**. That tick is what hands the
   collider over to the component you add next; without it the effector does nothing at all.

5. **Add Component > Platform Effector 2D**. Set:

   | Field | Value | Why |
   |---|---|---|
   | **Use One Way** | ticked (the default) | The whole point: solid from above, transparent from below. |
   | **Surface Arc** | leave at its default, `180` | The arc, centred on the platform's local *up*, that counts as the solid surface. 180° is the entire top half. |
   | **Use Side Friction** | unticked | Stops the player sticking to the platform's edge while passing it. |
   | **Use Side Bounce** | unticked (the default) | Same reason. |

   Leave every other field at its default.

6. Set the object's **Layer** (top right of the Inspector) to **`OneWay`**.

7. Teach the ground check about it. Select `Player`, and on `Player Motor (Script)` open the **Ground
   Layers** dropdown: tick **`OneWay`** as well as `Ground`. The field now reads `Mixed...`, which is
   correct here — it means exactly two layers are selected. (M4's gate wanted `Ground` alone because `OneWay`
   did not exist yet.)

8. Save the scene and press **Play**. Jump at the ledge from underneath: the player passes straight through
   it and lands on top. Walk off its side: the player falls past it without catching. Jump while standing on
   it: it works, because the ground check now sees the `OneWay` layer.

## Done when (this step)
- [ ] Jumping into the ledge from **below** → the player passes through and lands on top of it.
- [ ] Standing on the ledge → the player does **not** sink through, and pressing **Space** jumps.
- [ ] Walking off the ledge's side → the player falls cleanly, without sticking to the edge.
- [ ] `OneWayLedge`'s Inspector shows `Box Collider 2D` with **Used By Effector** ticked, a
      `Platform Effector 2D` with **Use One Way** ticked, and Layer `OneWay`.
- [ ] `Player Motor (Script)` → **Ground Layers** includes both `Ground` and `OneWay`.
- [ ] The Console shows no red entries.

## Suggested commit
```
feat(level): add a one-way ledge using PlatformEffector2D
```

## If it breaks
- **The player bumps into the ledge from below instead of passing through** → **Used By Effector** is
  unticked on the `Box Collider 2D`. The effector only governs colliders that opt in.
- **The player falls through the ledge from above too** → **Use One Way** is unticked, or the object was
  rotated so its local *up* is no longer up. Check `Rotation` reads `0, 0, 0`.
- **The player stands on the ledge but cannot jump** → **Ground Layers** does not include `OneWay`.
- **The player sticks to the ledge's side while falling past it** → **Use Side Friction** is still ticked.
- **The player lands on the ledge and slowly sinks** → the collider is far thinner than the distance the
  player moves in one physics step. Keep the ledge at least `0.25` units thick, as above.

---
> Nav: — · [Overview](00_overview.md) · [A platform that travels →](02_moving-platform.md)
