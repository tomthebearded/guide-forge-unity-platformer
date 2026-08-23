# M12 · Step 01 of 06 — The options panel
> Nav: — · [Overview](00_overview.md) · [Volume sliders →](02_volume-sliders.md)

**Before you start:** M11's gate passed. Open the **`Menu`** scene — everything in this milestone is built
there, and the game reads the saved values wherever it runs.

## Why / design
The options screen lives in the menu rather than in the pause panel, for one practical reason: rebinding needs
the `Player` action map **disabled** while it listens for a key, and the menu is the one place where it
already is. Putting it in the pause menu as well is a copy of the panel and a second wiring job; it is a good
exercise once the first one works.

This step builds the furniture and nothing else — no behaviour, no scripts. That is deliberate: UI layout is
fiddly, and interleaving it with the interesting code turns a twenty-minute job into an hour of
context-switching. By the end of it you have a panel that opens, closes, and does nothing at all.

## Do this

1. Open `Menu`. Under its `Canvas`, right-click > **UI > Panel** and rename it **`OptionsPanel`**. Set its
   `Image` **Color** alpha to about `230` so it reads as a solid screen rather than an overlay.

2. Inside `OptionsPanel`, add these children. Positions are **illustrative** — lay them out however you like,
   as long as every one of them exists with **exactly these names**, because later steps wire them by name:

   | Object | Type | Text / notes |
   |---|---|---|
   | `OptionsTitle` | UI > Text - TextMeshPro | `OPTIONS`, font size `36`, centred |
   | `MasterRow` | UI > Slider | with a `Text - TextMeshPro` beside it reading `Master` |
   | `MusicRow` | UI > Slider | label `Music` |
   | `SfxRow` | UI > Slider | label `Effects` |
   | `FullscreenToggle` | UI > Toggle | label `Fullscreen` |
   | `RebindMoveLeftButton` | UI > Button - TextMeshPro | its text is set by code in [step 04](04_rebinding.md) |
   | `RebindMoveRightButton` | UI > Button - TextMeshPro | same |
   | `RebindJumpButton` | UI > Button - TextMeshPro | same |
   | `RebindDashButton` | UI > Button - TextMeshPro | same |
   | `ResetBindingsButton` | UI > Button - TextMeshPro | `Reset to defaults` |
   | `BackButton` | UI > Button - TextMeshPro | `Back` |

3. On each of the three sliders, set **Min Value** `0`, **Max Value** `1`, and **Value** `1`. Untick **Whole
   Numbers**. A 0–1 slider is the natural range for a volume control, and
   [step 02](02_volume-sliders.md) is where it meets the decibels the mixer actually wants.

4. Select `OptionsPanel` and **untick the checkbox beside its name** so it starts hidden.

5. Add an **`OptionsButton`** to the menu itself — outside the panel, beside `PlayButton` — reading `Options`.

6. Wire the two buttons that only show and hide. On `OptionsButton`'s **On Click ()**, press **+**, drag
   `OptionsPanel` into the object field, and choose **GameObject > SetActive (bool)** from the dropdown, then
   **tick** the checkbox that appears. On `BackButton`'s **On Click ()**, do the same with the checkbox left
   **unticked**.

   `SetActive(bool)` is in the *second* group of the function dropdown — the one whose entries take a value.
   That is the group you skipped in
   [M11 step 02](../MILESTONE_11_scenes-menus-hud-persistence/02_more-scenes.md), and this is what it is for.

7. Save the scene and press **Play** from `Menu`. **Options** opens the panel; **Back** closes it; **Play**
   still starts the game.

## Done when (this step)
- [ ] The menu shows `Play`, `Options` and `Quit`.
- [ ] **Options** opens a panel containing a title, three sliders with labels, a fullscreen toggle, four
      rebind buttons, a reset button and a back button.
- [ ] **Back** closes the panel and returns to the menu.
- [ ] The panel is hidden when the scene starts.
- [ ] Dragging the sliders moves them and does nothing else — no sound changes yet.
- [ ] The Console shows no red entries.

## Suggested commit
```
feat(ui): add an options panel to the menu
```

## If it breaks
- **The panel covers the menu and Back does not appear** → `BackButton` is a child of the panel (it should
  be), but it is behind another element. UI draws in Hierarchy order: move it to the bottom of the panel's
  children.
- **The button does nothing** → the **On Click ()** function is `No Function`, or you picked `SetActive` from
  the first group, which does not take the checkbox.
- **Clicking anywhere does nothing at all** → the scene lost its `EventSystem`. Add one with
  **GameObject > UI > Event System**.
- **The panel is enormous or clipped** → the `Canvas Scaler` settings differ from the ones in
  [M11 step 01](../MILESTONE_11_scenes-menus-hud-persistence/01_hud.md). All canvases in this project use
  `640 × 360`.

---
> Nav: — · [Overview](00_overview.md) · [Volume sliders →](02_volume-sliders.md)
