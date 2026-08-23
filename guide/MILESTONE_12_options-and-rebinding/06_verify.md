# M12 · Verify — Options: volume, display & key rebinding
> Nav: [← Make rebinds stick](05_persist-rebinds.md) · [Overview](00_overview.md) · [Build & ship →](../MILESTONE_13_build-and-ship/00_overview.md)

## Done-when gate (the real test — check every box by hand)

Observed in **Play Mode in the Editor**, starting from the **`Menu`** scene, with sound unmuted.

- [ ] **The panel opens and closes.** **Options** shows it, **Back** hides it, and it is hidden at scene start.
- [ ] **Each of the three volumes works, and works separately.** Master changes everything; Music changes only
      the music; Effects changes only the game's sounds. Each one changes **smoothly across the whole slider's
      travel**, not just at its end, and zero is silence with no Console error.
- [ ] **Volumes persist.** Leave Play Mode and return: the sliders are where you left them and the volume is
      already applied before you open the panel.
- [ ] **The volumes reach the game**, not just the menu: starting `Level01` keeps them.
- [ ] **The fullscreen toggle stores its state.** Clicking it prints `fullscreen = True` / `False`; leaving
      Play Mode and returning shows the same state. *(The window itself changing is a build-only effect,
      checked at the [M13 gate](../MILESTONE_13_build-and-ship/00_overview.md).)*
- [ ] **All five rebind buttons show their current control** when the panel opens.
- [ ] **A rebind takes effect on the next input.** Rebind `Jump` to **J** → **J** jumps in the level and
      **Space** does not.
- [ ] **The gamepad rebinds too**, from the same screen, without disturbing the keyboard binding.
- [ ] **A composite part rebinds alone**: changing move-left leaves move-right, up and down untouched.
- [ ] **Escape cancels** a rebind and restores the previous control; **mouse movement binds nothing**.
- [ ] **Rebinds survive a restart.** Leave Play Mode entirely, return, and both the label and the control are
      still the rebound one.
- [ ] **Reset to defaults works, and stays.** Every label and control returns to the asset's default, and a
      restart still shows the defaults.
- [ ] **Break it to prove the store.** Delete the `cavernDash.bindingOverrides` key — **Edit > Clear All
      PlayerPrefs** is the blunt way — and restart: the bindings are the defaults again. Rebind, restart, and
      they come back.
- [ ] **Nothing earlier regressed.** The whole M11 loop still runs: menu → two levels → win screen, pause,
      game over, best time.
- [ ] **The project is clean.** No red Console entries; after committing, `git status --porcelain` prints
      nothing.

## Files after this milestone (the checkpoint)

_This checkpoint renders the complete contents of every guide-authored file created or modified in this
milestone (listed below). Pre-existing files this milestone only added to are shown as their added region
under "Pre-existing files modified", not reproduced whole. Files not listed were not touched this milestone._

### `Assets/_Project/Scripts/AudioSettings.cs`
```csharp
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Connects three sliders to three exposed mixer parameters, and remembers them.
public class AudioSettings : MonoBehaviour
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
        PlayerPrefs.SetFloat(preferenceKey, value01);
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

### `Assets/_Project/Scripts/DisplaySettings.cs`
```csharp
using UnityEngine;
using UnityEngine.UI;

// One toggle: fullscreen or windowed, remembered between launches.
public class DisplaySettings : MonoBehaviour
{
    // Load-bearing: the PlayerPrefs key. PlayerPrefs has no bool, so 1/0 it is.
    private const string FullscreenKey = "cavernDash.display.fullscreen";

    [SerializeField] private Toggle fullscreenToggle;

    private void Start()
    {
        // Default to whatever the player is currently running in, so a first
        // launch does not change anything behind their back.
        int savedValue = PlayerPrefs.GetInt(FullscreenKey, Screen.fullScreen ? 1 : 0);
        bool wantsFullscreen = savedValue == 1;

        Screen.fullScreen = wantsFullscreen;
        // WithoutNotify sets the control without firing its listener — the same
        // reason the sliders use SetValueWithoutNotify.
        fullscreenToggle.SetIsOnWithoutNotify(wantsFullscreen);

        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    private void SetFullscreen(bool wantsFullscreen)
    {
        Screen.fullScreen = wantsFullscreen;
        PlayerPrefs.SetInt(FullscreenKey, wantsFullscreen ? 1 : 0);
        PlayerPrefs.Save();

        // In the Editor this changes the stored value and the property, and
        // nothing visible: the Game view is a panel, not a window.
        Debug.Log($"fullscreen = {Screen.fullScreen}");
    }
}
```

### `Assets/_Project/Scripts/RebindButton.cs`
```csharp
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// One button rebinds one binding of one action. Press it, then press the control
// you want; Escape cancels.
[RequireComponent(typeof(Button))]
public class RebindButton : MonoBehaviour
{
    [SerializeField] private string actionPath = "Player/Jump";

    // Which binding of that action. For a plain button: 0 is usually the keyboard
    // binding and 1 the gamepad one, in the order they appear in the asset.
    [SerializeField] private int bindingIndex;

    // For a composite like Move, name the part instead: "left", "right", "up", "down".
    // Leave empty for a plain binding.
    [SerializeField] private string compositePartName = "";

    [SerializeField] private TMP_Text label;
    [SerializeField] private string listeningText = "press a key...";

    private InputAction action;
    private InputActionRebindingExtensions.RebindingOperation operation;

    private void Awake()
    {
        action = InputSystem.actions.FindAction(actionPath);
        GetComponent<Button>().onClick.AddListener(StartRebind);
    }

    private void OnEnable()
    {
        RefreshLabel();
    }

    public void RefreshLabel()
    {
        // GetBindingDisplayString turns "<Keyboard>/space" into "Space".
        label.text = action.GetBindingDisplayString(ResolveBindingIndex());
    }

    private int ResolveBindingIndex()
    {
        if (string.IsNullOrEmpty(compositePartName))
        {
            return bindingIndex;
        }

        // A composite occupies several consecutive entries in the action's binding
        // list: the composite itself, then one per part. Find the part by name so
        // the code survives someone reordering the asset.
        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (action.bindings[i].isPartOfComposite && action.bindings[i].name == compositePartName)
            {
                return i;
            }
        }

        Debug.LogWarning($"No composite part '{compositePartName}' on {actionPath}");
        return bindingIndex;
    }

    private void StartRebind()
    {
        label.text = listeningText;

        // The action must be off while the operation listens, or the very press
        // being bound would also fire the action.
        action.Disable();

        operation = action.PerformInteractiveRebinding(ResolveBindingIndex())
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(_ => FinishRebind())
            .OnCancel(_ => FinishRebind())
            .Start();
    }

    private void FinishRebind()
    {
        // A RebindingOperation holds unmanaged memory. Not disposing it leaks,
        // every single time.
        operation?.Dispose();
        operation = null;

        action.Enable();
        RefreshLabel();
        BindingStorage.Save();
    }

    private void OnDisable()
    {
        // Closing the panel mid-rebind must not leave an operation listening.
        if (operation != null)
        {
            FinishRebind();
        }
    }
}
```

### `Assets/_Project/Scripts/BindingStorage.cs`
```csharp
using UnityEngine;
using UnityEngine.InputSystem;

// Saves and restores the player's rebinds. The Input System serializes the
// override layer for us; this only decides where the string lives.
public static class BindingStorage
{
    // Load-bearing: the PlayerPrefs key holding the overrides JSON.
    private const string BindingOverridesKey = "cavernDash.bindingOverrides";

    // Runs once at startup, before the first scene's Awake, however the game
    // was launched — including pressing Play in the middle of a level.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Load()
    {
        string json = PlayerPrefs.GetString(BindingOverridesKey, string.Empty);

        if (string.IsNullOrEmpty(json))
        {
            return;
        }

        InputSystem.actions.LoadBindingOverridesFromJson(json);
    }

    public static void Save()
    {
        PlayerPrefs.SetString(BindingOverridesKey, InputSystem.actions.SaveBindingOverridesAsJson());
        PlayerPrefs.Save();
    }

    public static void ResetToDefaults()
    {
        InputSystem.actions.RemoveAllBindingOverrides();

        // Clear the stored string too, or the next launch reloads what you
        // just removed and the reset appears not to have worked.
        PlayerPrefs.DeleteKey(BindingOverridesKey);
        PlayerPrefs.Save();
    }
}
```

### `Assets/_Project/Scripts/ResetBindingsButton.cs`
```csharp
using UnityEngine;

// Puts every binding back to what the asset ships with, and refreshes the labels.
public class ResetBindingsButton : MonoBehaviour
{
    [SerializeField] private RebindButton[] rebindButtons;

    // Public so the button can call it from the Inspector.
    public void ResetAll()
    {
        BindingStorage.ResetToDefaults();

        // The buttons show their binding when they are enabled, so tell them
        // to look again — nothing about them changed by itself.
        foreach (RebindButton button in rebindButtons)
        {
            button.RefreshLabel();
        }
    }
}
```

### Editor checkpoint (all in the `Menu` scene)

| GameObject | Component | Field | Exact value |
|---|---|---|---|
| `OptionsPanel` | — | active at start | **unticked** |
| `OptionsPanel` | `Audio Settings (Script)` | Mixer / three sliders | `GameMixer` / `MasterRow`, `MusicRow`, `SfxRow` |
| `OptionsPanel` | `Display Settings (Script)` | Fullscreen Toggle | `FullscreenToggle` |
| `OptionsPanel` | `Reset Bindings Button (Script)` | Rebind Buttons | size `5`, the five rebind buttons |
| `MasterRow`, `MusicRow`, `SfxRow` | `Slider` | Min / Max / Whole Numbers | `0` / `1` / unticked |
| `RebindMoveLeftButton` | `Rebind Button (Script)` | Action Path / Binding Index / Composite Part Name | `Player/Move` / `0` / `left` |
| `RebindMoveRightButton` | `Rebind Button (Script)` | same | `Player/Move` / `0` / `right` |
| `RebindJumpButton` | `Rebind Button (Script)` | same | `Player/Jump` / `0` / *(empty)* |
| `RebindJumpGamepadButton` | `Rebind Button (Script)` | same | `Player/Jump` / `1` / *(empty)* |
| `RebindDashButton` | `Rebind Button (Script)` | same | `Player/Dash` / `0` / *(empty)* |
| every rebind button | `Rebind Button (Script)` | Label | its own child `Text (TMP)` |
| `OptionsButton` / `BackButton` | `Button` | On Click | `OptionsPanel` → `GameObject.SetActive` ticked / unticked |
| `ResetBindingsButton` | `Button` | On Click | `OptionsPanel` → `ResetBindingsButton.ResetAll ()` |
| `Main Camera` | `Audio Source` | Clip / Output / Loop / Play On Awake | menu music / `Music` group / ticked / ticked |

### Pre-existing files modified
- `Assets/_Project/Scenes/Menu.unity` — the options panel and everything in it, the options button, and the
  menu's music source. Edited through the Editor.

### Unchanged this milestone
- Every gameplay script from M3 to M11 — `PlayerMotor.cs`, `PlayerInputReader.cs`, `PlayerHealth.cs`,
  `HudView.cs`, `GameSession.cs` and the rest. The options screen was added without touching one line of the
  game, which is the payoff for actions, events and a mixer.
- `Assets/InputSystem_Actions.inputactions` — read and *overridden* at runtime, never edited. That is the
  whole point of the override layer.
- `Packages/manifest.json`, `ProjectSettings/*` — unchanged since M11.

## Troubleshooting

| Symptom | Likely cause → fix |
|---|---|
| A slider does nothing until its last few pixels | The decibel conversion is missing. |
| `SetFloat` has no effect | The exposed parameter name does not match the mixer's. |
| Sliders reset each launch | `PlayerPrefs.Save()` missing, or the read and write keys differ. |
| The key both binds and fires the action | `action.Disable()` missing before `.Start()`. |
| Nothing responds after a rebind | `action.Enable()` missing from `FinishRebind` — the action is left off. |
| A second rebind does nothing | The first operation was never disposed. |
| The move-left button rebinds everything | **Composite Part Name** is empty on it. |
| Rebinds vanish on restart | `BindingStorage.Save()` is not called, or `Load` is not marked `[RuntimeInitializeOnLoadMethod]`. |
| Reset works, then the rebinds return next launch | `PlayerPrefs.DeleteKey` missing from `ResetToDefaults`. |
| The fullscreen toggle changes nothing visible | Expected in the Editor; it is a build-only effect. |

## Handoff
- **You now have:** the complete game from M11, plus an options screen in the menu: three volume sliders that
  drive the mixer's exposed parameters through a proper linear-to-decibel conversion and persist, a fullscreen
  toggle that stores its state, and five rebind buttons — four keyboard, one gamepad, including a single part
  of the `Move` composite — that apply binding overrides interactively, save them as JSON in `PlayerPrefs`,
  reload them before the first scene, and can all be reset to the asset's defaults. Every gameplay script from
  M3 to M11 was left untouched.
- **Open / deferred:** the game exists only inside the Unity Editor. Nobody else can play it, the fullscreen
  toggle has never actually moved a window, and the repository has no README explaining what any of this is.
- **Next:** **[M13 — Build & ship](../MILESTONE_13_build-and-ship/00_overview.md)** — the executable, tested
  outside the Editor, and the repository someone else can clone.

---
> Nav: [← Make rebinds stick](05_persist-rebinds.md) · [Overview](00_overview.md) · [Build & ship →](../MILESTONE_13_build-and-ship/00_overview.md)
