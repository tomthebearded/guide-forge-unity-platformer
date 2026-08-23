# M9 · Step 02 of 06 — Collect coins
> Nav: [← Make a coin](01_coin-prefab.md) · [Overview](00_overview.md) · [An enemy that patrols →](03_enemy.md)

**Before you start:** [step 01](01_coin-prefab.md) finished — the `Coin` prefab exists and instances are
placed. This step touches **two new files, committed together**: `PlayerStats.cs` and `Collectible.cs`.

## Glossary for this step
> New here: **[C# event](../foundation/glossary.md#c-event)** (defined in *Why / design*).

## Why / design
A coin needs to tell somebody it was collected. The lazy version has the coin find the player and increment a
field on it, which means every pickup type in the game knows about the player, and the player's field is
public to the entire project.

The version that scales is an **event**. `PlayerStats` owns the count and announces when it changes; anything
that cares — the coin, and the HUD you build in M11 — subscribes. The coin knows only "I was touched by
something with a `PlayerStats`", and `PlayerStats` knows nothing about coins at all.

> New concept — **C# event**: a member other objects can subscribe to with `+=` and that the owning class
> can raise. Subscribers are called in turn and never know about each other. Unity's own callbacks
> (`OnTriggerEnter2D` and friends) work the same way — this is you doing it for your own facts.

There is no score on screen yet: the HUD belongs to M11. For now the count is announced to the Console, which
is exactly enough to prove the mechanism.

## Do this

1. Create a new MonoBehaviour script in `Assets/_Project/Scripts` named **`PlayerStats`** and replace its
   contents with this:

   ```csharp
   // Assets/_Project/Scripts/PlayerStats.cs — the whole file
   using System;
   using UnityEngine;

   // Owns the run's numbers. Anything that changes them goes through a method here,
   // and anything that displays them subscribes to the event.
   public class PlayerStats : MonoBehaviour
   {
       // How many coins have been collected this run.
       public int CoinsCollected { get; private set; }

       // Raised whenever CoinsCollected changes, carrying the new total.
       // [M10] PlayerAudio subscribes first, then [M11] the HUD; for now the
       // Console is the only reader.
       public event Action<int> CoinsChanged;

       public void AddCoin()
       {
           CoinsCollected++;

           // ?.Invoke calls the event only if something is subscribed —
           // raising an event with no subscribers would otherwise throw.
           CoinsChanged?.Invoke(CoinsCollected);

           Debug.Log($"coins = {CoinsCollected}");
       }
   }
   ```

2. Create a second MonoBehaviour script in the same folder named **`Collectible`** and replace its contents
   with this:

   ```csharp
   // Assets/_Project/Scripts/Collectible.cs — the whole file
   using UnityEngine;

   // Sits on a trigger. When the player overlaps it, it credits one coin and removes
   // itself. It knows nothing about scoring beyond "tell PlayerStats".
   [RequireComponent(typeof(Collider2D))]
   public class Collectible : MonoBehaviour
   {
       private void OnTriggerEnter2D(Collider2D other)
       {
           // TryGetComponent asks "does this object have one?" and hands it over in
           // the same call — no null check, no exception if it does not.
           if (!other.TryGetComponent(out PlayerStats stats))
           {
               return;
           }

           stats.AddCoin();

           // Destroy removes the GameObject at the end of the frame.
           Destroy(gameObject);
       }
   }
   ```

3. Save, let Unity compile. Then attach `PlayerStats` to the **`Player`** object by dragging the script onto
   it in the Hierarchy.

4. Attach `Collectible` to the **prefab**, not to the instances — that is what the prefab is for.
   Double-click `Assets/_Project/Prefabs/Coin.prefab` in the Project panel to open it in Prefab Mode, drag
   `Collectible.cs` onto the `Coin` root object in the small Hierarchy that appears, then click the **`<`**
   arrow at the top left of the Hierarchy to leave Prefab Mode. Every placed coin now carries the component.

5. Save the scene and press **Play**. Run into a coin: it disappears, and the Console prints `coins = 1`.
   Collect another: `coins = 2`. Coins you have not touched are still there.

6. Check that it is really the *player* doing it. Walk the moving platform into a coin: the coin stays. The
   platform has no `PlayerStats`, so `TryGetComponent` fails and the coin ignores it — which is exactly what
   you want when M9's enemy starts wandering around.

## Done when (this step)
- [ ] Touching a coin removes it from the scene and prints `coins = N` with N increasing by one each time.
- [ ] Collecting every placed coin leaves the level with no coins and the last count equal to the number you
      placed.
- [ ] The moving platform passing through a coin does **not** collect it.
- [ ] `Coin.prefab` carries the `Collectible (Script)` component, and every placed instance shows it too.
- [ ] `Player` carries `Player Stats (Script)`.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(gameplay): collect coins through a PlayerStats event
```

## If it breaks
- **Nothing happens when you touch a coin** → the coin's collider is not a trigger, or the coin is on a layer
  that the physics settings stop from interacting with `Default`. Check **Is Trigger** first.
- **`OnTriggerEnter2D` never fires** → neither object has a `Rigidbody2D`. The player's is what makes this
  work; if you removed it, nothing overlaps.
- **The Console prints but the coin stays** → `Destroy(gameObject)` was written as `Destroy(this)`, which
  removes only the component.
- **Every coin is collected the instant the level starts** → the coins are overlapping the player's start
  position. Move them, or move the player.
- **The component landed on one coin instead of all of them** → you dropped the script onto an instance in the
  scene rather than into Prefab Mode. Add it to the prefab and the instances inherit it.

---
> Nav: [← Make a coin](01_coin-prefab.md) · [Overview](00_overview.md) · [An enemy that patrols →](03_enemy.md)
