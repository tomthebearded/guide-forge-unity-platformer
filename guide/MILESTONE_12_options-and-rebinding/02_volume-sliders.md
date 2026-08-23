# M12 · Step 02 of 06 — Volume sliders
> Nav: [← The options panel](01_options-panel.md) · [Overview](00_overview.md) · [Display settings →](03_display-settings.md)

**Before you start:** [step 01](01_options-panel.md) finished — the options panel opens and closes, with three
0–1 sliders in it.

## Why / design
The mixer parameters have been sitting exposed since
[M10 step 06](../MILESTONE_10_animation-camera-audio/06_audio-mixer.md), waiting for exactly this.

The whole difficulty is one unit conversion, and skipping it produces a control that *seems* to work. A
slider is **linear** from 0 to 1. A mixer volume is in **decibels**, a logarithmic scale where `0 dB` is
unchanged and `-80 dB` is silence. Wire the slider value straight into `SetFloat` and the top 99% of the
slider's travel does almost nothing audible, then the last hair of it cuts to silence — because you have
mapped 0–1 onto the decibel range's last whisker.

The conversion is `Mathf.Log10(value01) * 20`, and it has one hole: `Log10(0)` is negative infinity, which the
mixer will not accept. So zero is special-cased to `-80 dB`, the mixer's own floor for silence.

Each slider also writes a `PlayerPrefs` float, so the setting survives the game closing — the same store as
the best time. **When** it reaches the disk matters: `onValueChanged` fires on every frame of a drag, so the
component keeps `PlayerPrefs.SetFloat` in the handler (cheap, in memory) and calls `PlayerPrefs.Save()` once
in `OnDisable`, when the menu is left behind. One flush per visit instead of sixty a second. The keys are
load-bearing:
**`cavernDash.volume.master01`**, **`cavernDash.volume.music01`**, **`cavernDash.volume.sfx01`**. The `01`
suffix records what is stored: the **linear 0–1 value**, not the decibels. Store the decibels and you have to
convert backwards to place the slider handle.

## Do this

1. Create a new MonoBehaviour script in `Assets/_Project/Scripts` named **`AudioOptions`**:

   ```csharp
   // Assets/_Project/Scripts/AudioOptions.cs — the whole file
   using UnityEngine;
   using UnityEngine.Audio;
   using UnityEngine.UI;

   // Connects three sliders to three exposed mixer parameters, and remembers them.
   public class AudioOptions : MonoBehaviour
   {
       // Load-bearing: these must match the exposed parameter names in GameMixer.
       private const string MasterParameter = "MasterVolumeDb";
       private const string MusicParameter = "MusicVolumeDb";
       private const string SfxParameter = "SfxVolumeDb";

       // Load-bearing: the PlayerPrefs keys. "01" means a linear 0-1 value.
       private const string MasterKey = "cavernDash.volume.master01";
       private const string MusicKey = "cavernDash.volume.music01";
       private const string SfxKey = "cavernDash.volume.sfx01";

       // The mixer's floor. Anything quieter is silence as far as it is concerned.
       private const float SilenceDecibels = -80f;

       [SerializeField] private AudioMixer mixer;
       [SerializeField] private Slider masterSlider;
       [SerializeField] private Slider musicSlider;
       [SerializeField] private Slider sfxSlider;

       private void Start()
       {
           // Put the saved values into the sliders and into the mixer. The default
           // of 1 is full volume for a player who has never touched them.
           ApplyAndShow(masterSlider, MasterParameter, MasterKey);
           ApplyAndShow(musicSlider, MusicParameter, MusicKey);
           ApplyAndShow(sfxSlider, SfxParameter, SfxKey);

           // onValueChanged fires whenever the handle moves, by mouse or by code.
           masterSlider.onValueChanged.AddListener(value => Set(MasterParameter, MasterKey, value));
           musicSlider.onValueChanged.AddListener(value => Set(MusicParameter, MusicKey, value));
           sfxSlider.onValueChanged.AddListener(value => Set(SfxParameter, SfxKey, value));
       }

       private void ApplyAndShow(Slider slider, string parameterName, string preferenceKey)
       {
           float saved = PlayerPrefs.GetFloat(preferenceKey, 1f);

           // SetValueWithoutNotify avoids firing onValueChanged before it is wired,
           // and avoids writing back a value that was just read.
           slider.SetValueWithoutNotify(saved);
           mixer.SetFloat(parameterName, LinearToDecibels(saved));
       }

       private void Set(string parameterName, string preferenceKey, float value01)
       {
           mixer.SetFloat(parameterName, LinearToDecibels(value01));

           // SetFloat only updates PlayerPrefs in memory, which is what you want
           // here: this runs on every frame of a drag.
           PlayerPrefs.SetFloat(preferenceKey, value01);
       }

       // Leaving the menu — to a level, or by quitting — is when the values reach
       // the disk. One write per visit instead of one per frame of a drag.
       private void OnDisable()
       {
           PlayerPrefs.Save();
       }

       // 1 -> 0 dB (unchanged), 0.5 -> about -6 dB (half as loud), 0 -> silence.
       private static float LinearToDecibels(float value01)
       {
           if (value01 <= 0.0001f)
           {
               // Log10(0) is negative infinity, which the mixer refuses.
               return SilenceDecibels;
           }

           return Mathf.Log10(value01) * 20f;
       }
   }
   ```

   The name avoids a trap. `UnityEngine` already contains a type called **`AudioSettings`**, and a class of
   your own with that name would quietly win over it everywhere in your code — leaving Unity's version
   reachable only as `UnityEngine.AudioSettings`. Naming yours `AudioOptions` costs nothing and keeps both
   names meaning what they say.

2. Save, let Unity compile. In the `Menu` scene, select **`OptionsController`** — the always-active object
   from [step 01](01_options-panel.md), *not* `OptionsPanel` — and drag `AudioOptions.cs` onto it. On the
   panel it would never reach `Start`, because the panel is switched off when the scene loads, and a player
   who never opens Options would hear the default volume every launch.

3. Fill its four fields: **Mixer** ← `GameMixer` (drag it from `Assets/_Project/Audio`), and the three slider
   fields ← `MasterRow`, `MusicRow`, `SfxRow` from the Hierarchy.

4. The menu has no sound of its own to test with, so give it one: select the `Menu` scene's `Main Camera`,
   add an **Audio Source**, set its **AudioClip** to your Kenney music track, its **Output** to the `Music`
   group of `GameMixer`, and tick **Loop** and **Play On Awake**.

5. Save the scene and press **Play** from `Menu`. Open **Options** and drag the **Music** slider: the menu
   music fades smoothly across the whole travel of the slider — not only at the very end. Drag **Master**: the
   same. Drag it to zero: silence.

6. Prove it persists — and that it does not wait for the panel. With the music slider at about a quarter,
   stop Play Mode, then press Play again and **do not open Options**: the menu music is already quiet from the
   first frame. That is `OptionsController` running `Start` while `OptionsPanel` is still switched off. Now
   open Options: the slider is sitting where you left it.

7. Prove it reaches the game, not just the menu. Press **Play** to start `Level01`: the level's music and
   effects are at the volumes you set, because the mixer is a project asset and the values were applied to it.

## Done when (this step)
- [ ] Dragging **Music** changes the music volume **smoothly across the whole slider**, not only in the last
      few pixels.
- [ ] Dragging **Master** changes everything; dragging **Effects** changes the game's sound effects but not
      the music.
- [ ] A slider at `0` gives silence, with no Console error.
- [ ] Leaving Play Mode and returning applies the saved volume **without the panel ever being opened**, and
      the sliders are where you left them once you do open it.
- [ ] Starting a level carries the settings into it.
- [ ] The Console shows no red entries; the project compiles.

## Suggested commit
```
feat(options): wire volume sliders to the mixer and persist them
```

## If it breaks
- **The slider does nothing until the very end of its travel** → the value is being written to the mixer
  without the decibel conversion.
- **`SetFloat` returns false / nothing happens** → the parameter name does not match the exposed name in the
  mixer exactly. Check the **Exposed Parameters** dropdown in the Audio Mixer window.
- **The music does not change but the effects do** → the menu's `Audio Source` **Output** is `None` rather
  than the `Music` group, so it bypasses the mixer.
- **The sliders reset every time** → `PlayerPrefs.Save()` is missing, or the keys differ between the read and
  the write. They are constants for exactly that reason.
- **The volume is applied but the handles are at the far right** → `SetValueWithoutNotify` was skipped, or
  the saved value is being read after the listeners are attached.

---
> Nav: [← The options panel](01_options-panel.md) · [Overview](00_overview.md) · [Display settings →](03_display-settings.md)
