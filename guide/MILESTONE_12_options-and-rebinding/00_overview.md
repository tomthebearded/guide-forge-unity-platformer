# Milestone 12 — Options: volume, display & key rebinding
> Part 5 — Shipping · milestone 12 of 13 · prev: [Scenes, menus, HUD & persistence](../MILESTONE_11_scenes-menus-hud-persistence/00_overview.md) · next: [Build & ship](../MILESTONE_13_build-and-ship/00_overview.md) · start: [The options panel](01_options-panel.md)

## Goal
By the end of this milestone the player controls the game rather than the other way round. Three sliders set
master, music and effects volume; a toggle switches fullscreen; and every action in the `Player` map can be
rebound by pressing the key or button you want — on keyboard **and** on gamepad. Everything survives quitting
and relaunching, and a **Reset to defaults** button puts it all back.

## Prerequisite
M11's gate passed: the whole loop runs from menu to win screen, pause works, and the best time persists.

## Steps at a glance

**Sitting 1 — Somewhere to put the controls (01)**
1. [The options panel](01_options-panel.md)

**Sitting 2 — Sound and screen (02–03)**
2. [Volume sliders](02_volume-sliders.md)
3. [Display settings](03_display-settings.md)

**Sitting 3 — Rebinding (04–05)**
4. [Rebind a key](04_rebinding.md)
5. [Make rebinds stick](05_persist-rebinds.md)

6. [Verify](06_verify.md)

## Design / decisions folded in
- A hidden panel cannot run `Start`, so the option scripts live on an always-active `OptionsController` while the panel keeps only the furniture — taught in [step 01](01_options-panel.md).
- Decibels versus a linear slider — taught in [step 02](02_volume-sliders.md); recorded in [../foundation/glossary.md](../foundation/glossary.md#decibel).
- When a setting reaches the disk: per drag frame is wrong, per visit is right — taught in [step 02](02_volume-sliders.md); recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d28--volume-settings-flush-to-disk-on-panel-close-not-on-every-slider-frame).
- Interactive rebinding as a borrowed operation with a lifecycle — taught in [step 04](04_rebinding.md); recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d23--build-vs-borrow-interactive-key-and-button-rebinding).
- Binding overrides as a layer on top of the asset, serialized to JSON — taught in [step 05](05_persist-rebinds.md); recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d24--build-vs-borrow-persisting-rebinds-across-launches).
- Why the options UI itself is hand-written — recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d25--build-vs-borrow-the-options-ui-itself-sliders-the-rebind-buttons-states).

---
> Part 5 — Shipping · milestone 12 of 13 · prev: [Scenes, menus, HUD & persistence](../MILESTONE_11_scenes-menus-hud-persistence/00_overview.md) · next: [Build & ship](../MILESTONE_13_build-and-ship/00_overview.md) · start: [The options panel](01_options-panel.md)
