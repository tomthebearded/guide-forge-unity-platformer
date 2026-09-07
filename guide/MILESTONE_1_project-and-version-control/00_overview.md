# Milestone 1 — Project, Editor & version control
> Part 1 — Foundations · milestone 1 of 12 · prev: — · next: [First script & frame-rate-independent motion](../MILESTONE_2_first-script-and-motion/00_overview.md) · start: [Install Unity Hub and Unity 6.3 LTS](01_install-unity.md)

## Goal
By the end of this milestone you have a Unity 6.3 LTS project that opens, enters Play Mode without errors,
and lives beside `guide/` in this guide's own repository — `Library/` ignored, `.meta` files tracked, Git LFS
already configured for the binaries you have not imported yet. Nothing moves on screen: this is the ground
everything else is built on.

## Prerequisite
None. This is where the guide starts. You need a machine that can run Unity, a browser, Git already installed,
and roughly half an hour of unattended time while the Editor downloads.

## Steps at a glance

**Sitting 1 — Install and create (01–03)**
1. [Install Unity Hub and Unity 6.3 LTS](01_install-unity.md)
2. [Create the Universal 2D project](02_create-project.md)
3. [Find your way around the Editor](03_find-your-way-around.md)

**Sitting 2 — Version control and project layout (04–06)**
4. [Put the project under Git](04_git-init.md)
5. [Track binary assets with Git LFS](05_git-lfs.md)
6. [Lay out the project folders and name the scene](06_project-folders.md)

7. [Verify](07_verify.md)

## Design / decisions folded in
- Unity **6.3 LTS** rather than 6.0 LTS — taught in [step 01](01_install-unity.md); recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d1--pin-unity-63-lts-60003x).
- Git and Git LFS **before any asset exists** — taught in [steps 04](04_git-init.md)–[05](05_git-lfs.md); recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d5--git--git-lfs-from-milestone-1).
- The project as a **folder in this repository** rather than a repository of its own — taught in [steps 02](02_create-project.md) and [04](04_git-init.md); recorded in [../foundation/decision-log.md](../foundation/decision-log.md#d30--the-unity-project-is-a-folder-in-the-guides-repository-not-a-repository-of-its-own).
- The `Assets/_Project/` layout and the `Level01` scene name — taught in [step 06](06_project-folders.md); recorded in [../foundation/conventions.md](../foundation/conventions.md).

---
> Part 1 — Foundations · milestone 1 of 12 · prev: — · next: [First script & frame-rate-independent motion](../MILESTONE_2_first-script-and-motion/00_overview.md) · start: [Install Unity Hub and Unity 6.3 LTS](01_install-unity.md)
