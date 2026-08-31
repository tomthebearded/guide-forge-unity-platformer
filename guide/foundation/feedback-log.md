# Feedback log — Cavern Dash

> Append-only field log of friction readers hit while following this guide. Its purpose is to improve the
> guide **and** the GuideForge method over time — so entries are captured even when the guide is not (yet)
> changed. This is a **record, not a to-do**: logging an entry here does not by itself change the guide. To
> actually fix the guide from a report, run `/report-issue` (it fixes the root cause and sweeps for siblings).
>
> Written by `/log-feedback` (capture) and, when a fix ships, by `/report-issue`. Newest entries on top; one
> entry per distinct piece of friction. Use absolute dates (`2026-07-09`), never "today".

## 2026-08-31 — No "Used By Composite" checkbox on the Tilemap Collider 2D
- **Where:** `MILESTONE_6_tilemap-level/05_composite-collider.md` (step 4), swept into `06_verify.md` and `PLAN.md`
- **Reader:** the guide's target reader, following M6 in Unity 6.3 LTS
- **What happened:** the step said to tick **Used By Composite** on the `Tilemap Collider 2D`; the reader's Inspector had no such checkbox — instead a **Composite Operation** dropdown (`None` / `Merge` / `Intersect` / `Difference` / `Flip`). They could not tell how to feed the composite.
- **Suspected class:** stale value/command/API
- **Severity:** blocker
- **Tags:** `versions`, `M6`, `unity-6.3`, `physics2d`, `composite-collider`, `terminology`
- **Status:** fixed via /report-issue (2026-08-31)
- **Quote:** "NON HO USED BY COMPOSITE"
