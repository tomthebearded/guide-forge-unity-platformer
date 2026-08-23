<!-- Foundation doc. Section headings and table column keys stay English by design. Prose language: English. -->

# Audience model — Cavern Dash

> The guide's north star: **match explanation depth to the reader's level on *that specific topic*.**
> Over-explaining an Expert topic is as harmful as under-explaining a New one. Every step is written against
> this matrix plus the granularity setting below.

## Per-topic expertise → explanation-depth policy

The reader **can already program** (loops, functions, classes, types are not new) but has **never opened
Unity** and has never built a game.

| Topic | Level | Depth policy applied in the guide |
|-------|-------|-----------------------------------|
| C# the language (types, classes, methods, properties, `enum`, generics) | **Beginner** | Assumed known; only *Unity-flavoured* C# is taught — `MonoBehaviour`, `[SerializeField]`, the serialization rules, coroutines, `struct` vs `class` where physics cares. Doc link, brief why, no fundamentals. |
| The Unity Editor (Hub, Project/Hierarchy/Scene/Game/Inspector, prefabs, scenes, play mode) | **New** | Fullest tier: define + doc link + deep-dive callout + failure notes. Every panel named the first time the reader must find it; every menu path spelled out. |
| The MonoBehaviour lifecycle (`Awake`/`Start`/`Update`/`FixedUpdate`/`LateUpdate`, execution order) | **New — deepest tier** | The load-bearing mental model of the whole guide. Taught fully in M2 and re-stated at every point where the *choice* of callback matters. |
| 2D physics (`Rigidbody2D`, `Collider2D`, body types, `gravityScale`, layers, triggers, `OverlapBox`) | **New — deepest tier** | Every concept derived, not asserted. The "who moves the transform — you or the engine?" model is repeated wherever it recurs. |
| The Input System package (actions, action maps, bindings, `InputAction.CallbackContext`, devices, **interactive rebinding + binding overrides**) | **New** | Define the action/binding model from zero + doc link + the "project-wide actions asset" gotcha; gamepad is a first-class case, not a footnote. Rebinding is taught as what it really is — an *override layer* on top of the asset, serialized to JSON. |
| Tilemap & Tile Palette (grid, tile assets, `TilemapCollider2D`, `CompositeCollider2D`, Rule Tiles) | **New** | Full definitions + doc links; the pixels-per-unit / 1 tile = 1 unit relationship is taught before a single tile is painted. |
| Sprite import & animation (PPU, Filter Mode, Sprite Editor, Animator, states, parameters, transitions) | **New** | Full tier; the pixel-art import settings are exact values, not advice. |
| Cinemachine 3 (`CinemachineCamera`, Follow, Position Composer, Confiner) | **New** | Define the brain/camera split + doc link + the "this is Cinemachine 3, not 2 — the class was renamed" trap. |
| UI & TextMeshPro (Canvas, scaling, anchors, `TextMeshProUGUI`, TMP Essential Resources) | **New** | Full tier for the Canvas mental model; TMP usage taught at the level the HUD needs, no typography deep-dive. |
| Scene management & game flow (`SceneManager`, additive vs single, `Time.timeScale`, persistence across scenes) | **New** | Full tier; the "what dies on scene load" trap is taught before the reader's score dies on scene load. |
| Persistence (`PlayerPrefs`, `JsonUtility`) | **Beginner** | Define on first use + doc link + brief why; the reader knows what serialization is, so only Unity's take on it is taught. |
| Audio (`AudioSource`, `AudioClip`, `AudioMixer`, groups, exposed parameters) | **New** | Full tier for the mixer graph; SFX usage taught at HUD depth. |
| Git for a Unity project (`.gitignore`, `.meta` files, Git LFS, what must never be committed) | **Beginner** | The reader knows Git; only the *Unity-specific* rules are taught — meta files are mandatory, `Library/` never, binaries via LFS. |
| Building & shipping (Build Profiles, Player Settings, testing the build) | **New** | Full tier: Unity 6 renamed Build Settings to **Build Profiles**, and the reader has never seen either. |

**Depth-policy legend** — **Expert** → name it, nothing more · **Intermediate** → one-line reminder + doc link
· **Beginner** → define on first use + doc link + brief why · **New** → define + doc link + a short deep-dive
callout + extra failure-mode notes.

## Granularity

**Standard.** Atomic steps, but sub-actions are grouped once a workflow has been taught: the first time the
reader creates a GameObject the menu path is spelled out in full; the tenth time it is one line. Editor work
is always exact about *where* — panel, menu path, and the field's label as it appears in the Inspector —
because the reader cannot search for a thing they cannot name.
