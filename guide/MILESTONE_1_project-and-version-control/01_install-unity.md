# M1 · Step 01 of 07 — Install Unity Hub and Unity 6.3 LTS
> Nav: — · [Overview](00_overview.md) · [Create the Universal 2D project →](02_create-project.md)

**Before you start:** you need a machine running Windows 10/11 or macOS, about 15 GB of free disk space, and
an internet connection. You do **not** need Unity installed — that is what this step does. Git is needed from
[step 04](04_git-init.md); if `git --version` prints nothing, install it from
[git-scm.com](https://git-scm.com/downloads) while the Editor downloads.

You also need this guide as a **Git repository on your machine**, not as loose files: the Unity project you
create in [step 02](02_create-project.md) becomes a folder inside it, beside `guide/`, and
[step 04](04_git-init.md) commits into it. If you are reading these files on the web or from a downloaded zip,
`git clone` the repository they live in first, and read from your clone.

## Glossary for this step
> New here: **[Unity Hub](../foundation/glossary.md#unity-hub)** (defined under *Why / design*) · **[LTS](../foundation/glossary.md#lts-long-term-support)** (defined under *Why / design*).

## Why / design
Unity is not one program. **Unity Hub** is a small launcher that manages *which versions of the Unity Editor
you have installed* and *which projects use which version*; the **Unity Editor** is the large application you
actually build the game in. You install the Hub once and use it to install Editors — plural, because a Unity
project is bound to the exact Editor version that opened it, and real machines carry several.

**LTS** means *Long Term Support*: a Unity release line that receives fixes for two years and no new features.
This guide pins **Unity 6.3 LTS**, supported until **December 2027**. That choice matters more than it looks:
Unity **6.0 LTS reaches end of support in October 2026**, so a guide written against it would go stale almost
immediately. The non-LTS releases (6.4, 6.5) move too fast to write against at all.

Every version number in this guide comes from [`../foundation/stack.md`](../foundation/stack.md), which was
verified online on 2026-08-22. If you are reading this much later, re-check that file first.

## Do this

1. Download **Unity Hub** from the official page at <https://unity.com/unity-hub> and install it the way your
   platform installs anything — the `.exe` installer on Windows, dragging the app into Applications on macOS.
   The Hub is the only thing you install by hand; everything else comes through it.

2. Open the Hub and sign in with a **Unity account** (create one if you have none — it is free). Unity requires
   a signed-in account and a licence before an Editor will launch. When prompted for a licence, choose the
   **Personal** licence: it is free and it covers everything in this guide.

3. In the Hub's left sidebar click **Installs**, then the **Install Editor** button at the top right. In the
   list that appears, find the entry labelled **Unity 6.3 LTS** — its version number starts with `6000.3.`
   (for example `6000.3.16f1`). Take whichever `6000.3.x` patch the Hub offers; the patch digit does not
   matter, the `6000.3` line does.

   > New concept — **the Unity version number**: `6000.3.16f1` reads as *major* `6000` (marketing name
   > "Unity 6"), *minor* `3` (the 6.3 release line), *patch* `16`, and `f1` meaning a final, non-beta build.
   > Two projects on `6000.3.14f1` and `6000.3.16f1` are compatible; `6000.0.x` and `6000.3.x` are not.

4. Before you press Continue, tick the **modules** — the optional extras Unity installs alongside the Editor.
   These are mandatory for this guide:
   - **Windows Build Support (IL2CPP)** if you are on Windows, or **Mac Build Support (IL2CPP)** if you are on
     macOS. This guide never packages a standalone executable — it stops at development, and every gate is
     observed in the Editor — but this is the module you would need the day you want one, and adding it
     later means a second download.
   - **Microsoft Visual Studio Community** (Windows) or the Rider/Visual Studio integration your platform
     offers, unless you already have a C# editor you intend to use.

   Everything else — Android, iOS, WebGL, the other language packs — **leave unticked**. They are large, and
   nothing in this guide needs them.

5. Press **Continue** and accept the licence terms. The download is several gigabytes; leave it running.

## Done when (this step)
- [ ] The Hub's **Installs** tab lists an Editor whose version begins with `6000.3.` and shows no "install
      pending" spinner.
- [ ] Clicking the gear icon beside that install and choosing **Add modules** shows **Windows Build Support
      (IL2CPP)** — or **Mac Build Support (IL2CPP)** on macOS — with its checkbox already ticked.

## If it breaks
- **The Hub lists 6.0 LTS but no 6.3 LTS** → you are looking at the "Official releases" short list. Use the
  **Archive** tab in the Install Editor dialog, or download 6.3 LTS directly from
  <https://unity.com/releases/unity-6> and let it open in the Hub.
- **The Editor will not launch: "no licence found"** → you are signed in but have not activated a licence. In
  the Hub, open the account menu at the top right, choose **Manage licences**, then **Add licence** →
  **Get a free personal licence**.
- **The install fails partway on Windows** → almost always a permissions or antivirus block on the temp
  folder. Re-run the Hub as administrator and start the install again; a half-finished install is discarded
  and re-downloaded rather than resumed.

---
> Nav: — · [Overview](00_overview.md) · [Create the Universal 2D project →](02_create-project.md)
