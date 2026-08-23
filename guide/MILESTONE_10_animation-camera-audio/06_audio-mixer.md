# M10 · Step 06 of 07 — The audio mixer
> Nav: [← Sound effects](05_sound-effects.md) · [Overview](00_overview.md) · [Verify →](07_verify.md)

**Before you start:** [step 05](05_sound-effects.md) finished — the five effects play. For music, download
Kenney's **Music Jingles** (<https://kenney.nl/assets/music-jingles>) and copy one loopable track into
`Assets/ThirdParty/Kenney/Audio`.

## Glossary for this step
> New here: **[AudioMixer](../foundation/glossary.md#audiomixer)** (defined in *Why / design*) ·
> **[exposed parameter](../foundation/glossary.md#exposed-parameter)** (defined in *Do this*, action 4) ·
> **[decibel](../foundation/glossary.md#decibel)** (defined in *Do this*, action 5).

## Why / design
Right now every sound goes straight to the speakers at whatever volume its file was recorded at. To let a
player turn the music down without turning the effects down — which is what M12's options screen does — the
sounds have to travel through something that can be adjusted **per group**.

> New concept — **AudioMixer**: an asset describing a routing graph. `AudioSource`s send their output to a
> **group**, groups feed into other groups, and the whole thing arrives at **Master**. Volume, and any
> effects, are set per group.

Two groups is the right number here: **Music** and **SFX**, both feeding **Master**. That gives a player the
three sliders they expect, and gives you one place to discover that the music is drowning everything.

The wiring is done now, in one step, because it is a five-minute job while the audio is fresh, and because
M12's sliders need the **exposed parameters** to exist before they can move them.

## Do this

1. In the **Project** panel, right-click `Assets/_Project/Audio` and choose **Create > Audio Mixer**. Name it
   **`GameMixer`**. Double-click it to open the **Audio Mixer** window.

2. The window shows one group, **Master**. Press the **+** beside **Groups** twice to add two children, and
   name them exactly **`Music`** and **`SFX`**. Both should sit indented under `Master` — if one lands in the
   wrong place, drag it onto `Master` in the Groups list.

3. Route the effects. Select the `Player` in the Hierarchy, and on its **`Audio Source`** set **Output** to
   the **`SFX`** group by clicking the field's picker and choosing `SFX` from `GameMixer`.

4. Expose the three volumes. In the Audio Mixer window, select the **`Master`** group, then in the
   **Inspector** right-click the **Volume** label and choose **Expose 'Volume (of Master)' to script**. Repeat
   for `Music` and for `SFX`.

   > New concept — **exposed parameter**: a mixer value published under a name so a script can set it at
   > runtime. Mixer volumes cannot be set from code any other way — a mixer group is not a component with a
   > field on it.

   Now name them. Open the **Exposed Parameters** dropdown at the top right of the Audio Mixer window,
   double-click each entry, and rename them to exactly — these strings are **load-bearing** and recorded in
   [`../foundation/conventions.md`](../foundation/conventions.md):

   | Group | Exposed parameter name |
   |---|---|
   | Master | `MasterVolumeDb` |
   | Music | `MusicVolumeDb` |
   | SFX | `SfxVolumeDb` |

   Nothing reads them yet. [M12](../MILESTONE_12_options-and-rebinding/00_overview.md) is where the sliders
   arrive; the names are put in place here so that milestone is wiring rather than archaeology.

5. Understand what those values are, because it is the trap in this whole area. Mixer volumes are in
   **decibels**, not percentages.

   > New concept — **decibel (dB)**: a logarithmic measure of loudness. `0 dB` is unchanged, `-80 dB` is
   > silence, and `-6 dB` is roughly half as loud as `0`. A slider that runs 0 to 1 is **linear**, so setting
   > a mixer volume straight from a slider gives a control that does nothing for most of its travel and then
   > collapses at the end. The conversion is `Mathf.Log10(value01) * 20`, with `0` special-cased to `-80`.

   You do not write that conversion today — M12 does — but knowing it is why `Db` is in the parameter names.

6. Add the music. In the **Hierarchy**, create an **empty GameObject** named **`Music`**. Add an
   **Audio Source** to it and set:

   | Field | Value |
   |---|---|
   | **AudioClip** | your chosen Kenney track |
   | **Output** | the `Music` group of `GameMixer` |
   | **Play On Awake** | ticked |
   | **Loop** | ticked |
   | **Volume** | `0.5` — music sits under effects, and this is a taste setting |

7. Save the scene and press **Play**. The music loops, the effects play over it, and both are audible at once.

8. Check the routing really works. With the game running, open the **Audio Mixer** window and drag the
   **`Music`** group's volume slider down: the music fades and the effects stay. Drag **`Master`** down: both
   go. Put both back to `0`.

   Mixer changes made in Play Mode **do** persist, unlike component values — the mixer is an asset, not a
   scene object. That is convenient here and a genuine surprise later, so it is worth meeting once.

## Done when (this step)
- [ ] `Assets/_Project/Audio/GameMixer.mixer` exists with `Music` and `SFX` groups under `Master`.
- [ ] The `Player`'s `Audio Source` **Output** is the `SFX` group; the `Music` object's is the `Music` group.
- [ ] The Audio Mixer window's **Exposed Parameters** dropdown lists exactly `MasterVolumeDb`,
      `MusicVolumeDb` and `SfxVolumeDb`.
- [ ] Music loops from the moment Play starts and the effects play over it.
- [ ] Lowering the `Music` group's volume during play silences the music alone; lowering `Master` silences
      everything.
- [ ] The Console shows no red entries.

## Suggested commit
```
feat(audio): route sound through a mixer with music and SFX groups
```

## If it breaks
- **The exposed parameter list is empty** → the right-click landed on the slider rather than on the **Volume**
  *label*. It is the label that carries the context menu.
- **Renaming an exposed parameter does nothing** → double-click the entry in the **Exposed Parameters**
  dropdown to edit it; the Inspector shows the group, not the parameter name.
- **The music plays but the effects do not go through the mixer** → the `Player`'s `Audio Source` **Output**
  is still `None`, which routes it straight to the listener and out of the mixer's reach.
- **Everything is silent** → the Game view's **Mute Audio** toggle, or a group left at `-80 dB` after
  experimenting in action 8.
- **The music restarts on every respawn** → the `Music` object is being destroyed and recreated. It should be
  a plain scene object; nothing in M9 touches it.

---
> Nav: [← Sound effects](05_sound-effects.md) · [Overview](00_overview.md) · [Verify →](07_verify.md)
