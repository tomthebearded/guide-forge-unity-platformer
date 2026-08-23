<!-- Foundation doc. Section headings stay English by design. Prose language: English.
     WORDS ONLY — never a function. A function that needs explaining gets an inline code comment where it is
     used. Each term is a `### <term>` HEADING (never a bullet), so that `../glossary.md#<slug>` deep-links
     from steps resolve natively on GitHub. Prose language: English. -->

# Glossary — Cavern Dash

> Terms the guide introduces, defined in plain language. **Words and concepts only — never a function**
> (functions are explained by an inline code comment where they're used). Each `### heading` is a stable
> deep-link target — steps link here with `../glossary.md#<slug>`.

### .meta file
The sidecar file Unity writes beside every asset (`Player.cs` → `Player.cs.meta`) holding its import settings
and its permanent GUID. Every reference in every scene points at that GUID, so a lost `.meta` silently breaks
references. It belongs in version control, always.

### action map
A named group of input actions enabled or disabled together, typically one per context — `Player` for gameplay, `UI` for menus.

### animation clip
A recorded change to some properties over time — for a 2D character, which sprite the `SpriteRenderer` shows on each frame.

### Animator controller
An asset holding animation states, their transitions and the parameters those transitions read. It is the only thing that chooses which clip plays; code only sets parameters.

### Animator parameter
A named value (`bool`, `float`, `int`, `trigger`) that a script sets and that an Animator state machine reads
to decide which animation to play. It is the only sanctioned way for code to drive animation.

### asset
Anything in your project's `Assets/` folder that Unity imports and can use — a script, a sprite, a scene, a sound, a prefab, a settings file.

### AudioMixer
An asset describing an audio routing graph: sources output to groups, groups feed Master, and volume is set per group.

### binding
A link from a physical control to an input action, written as a path like `<Keyboard>/a` or `<Gamepad>/leftStick`. One action can carry many.

### body type
Which of three rules Unity applies to a `Rigidbody2D`: **Dynamic** (the engine moves it — gravity and
collisions apply), **Kinematic** (only your code moves it, but it still collides), **Static** (it never moves).
The player in this guide is Dynamic.

### build profile
Unity 6's replacement for the old Build Settings window: a saved configuration naming the target platform, the
scenes to include and the player settings a build uses.

### build scene list
The ordered list of scenes included in a build, in **File > Build Profiles**. A scene missing from it cannot be loaded at runtime, and index 0 is what a built game opens with.

### C# event
A member other objects subscribe to with `+=` and the owning class raises. Subscribers are called in turn and never know about each other — the way components in this project talk without depending on one another.

### Canvas
The object every UI element lives under. Its coordinates are screen pixels rather than world units, and it draws on top of the world.

### CC0
A public-domain dedication: the work may be used, modified and redistributed by anyone, commercially included, with no attribution required. Kenney's asset packs are CC0, which is what lets this project commit them to a public repository.

### Cinemachine brain
The component on the actual `Camera` that listens to every `CinemachineCamera` in the scene and drives the
real camera towards whichever one is active. Without it, Cinemachine cameras do nothing.

### collider
The shape the physics engine uses for an object, independent of what is drawn. `BoxCollider2D` is a rectangle described in world units by its `Size` and `Offset`.

### component
A unit of behaviour or data attached to a GameObject. `Transform` (position, rotation, scale) is the one every GameObject has and cannot remove.

### composite collider
A collider that merges many neighbouring colliders into one continuous outline. On a Tilemap it removes the
internal seams between tiles — which is what stops a moving player from catching on a joint between two
identically-flat tiles.

### coyote time
A short window (**0.10 s** here) after you walk off a ledge during which a jump still counts as a ground jump.
Named after the cartoon coyote who hangs in the air before falling. It exists because players press jump a few
frames late and are right to expect it to work.

### decibel
A logarithmic measure of loudness: `0 dB` is unchanged, `-80 dB` is silence, `-6 dB` is roughly half as loud. A 0–1 slider is linear, so it must be converted with `Mathf.Log10(value01) * 20` before it is written to a mixer.

### delta time
The number of seconds the last frame took. Multiplying motion by it makes movement cover the same distance per
second regardless of frame rate. In `FixedUpdate` the equivalent is the fixed timestep, which does not vary.

### effector
A 2D physics component that changes how a collider behaves without changing its shape — a
`PlatformEffector2D` makes a platform solid from above and passable from below.

### exposed parameter
An AudioMixer value published under a name so a script can set it at runtime (`SetFloat`). Mixer volumes cannot
be set from code any other way.

### FixedUpdate
The callback Unity runs on the fixed physics timestep (0.02 s by default), independently of the frame rate. Read input in `Update`; act on physics in `FixedUpdate`.

### GameObject
The container every object in a Unity scene is. It does nothing by itself: what it *is* comes from the
**components** attached to it.

### Git LFS
An extension that replaces large files in Git history with lightweight pointers and keeps the real content in a side store, so clones stay fast. Configure it *before* the first commit.

### gizmo
A shape Unity draws in the Scene view for your benefit only, never in the game. `OnDrawGizmosSelected` draws while the object is selected.

### Grid
The parent component that defines the cell size and layout its child Tilemaps share. With Cell Size `1, 1, 0`, one cell is one world unit.

### ground check
The test a character controller runs each physics step to decide whether it is standing on something —
here an overlap query in a small box under the player's feet, filtered to the `Ground` layer.

### i-frames (invulnerability frames)
A period after taking damage (**1.0 s** here) during which further hits are ignored, so one contact with an
enemy costs one life rather than draining them all in a few frames.

### IL2CPP
Unity's scripting backend that converts C# to C++ and then to native code instead of shipping .NET assemblies. Slower to build, faster to run, and the default for standalone platforms.

### input action
A named intent (`Move`, `Jump`) with a value type. Your code reads the action; the bindings decide which physical controls can produce it — which is what lets one action serve keyboard and gamepad, and lets M12 rebind it at runtime.

### jump buffering
Remembering a jump press for a short window (**0.12 s** here) so that a press made just *before* landing fires
the moment the player touches the ground, instead of being thrown away.

### layer
A named bucket a GameObject belongs to, used to answer "what should collide with what" and to filter physics
queries. This guide uses `Ground`, `Player`, `OneWay` and `Hazard`.

### layer mask
A value that names a set of layers, passed to a physics query so it only reports hits on those layers.

### LTS (Long Term Support)
A Unity release line that receives fixes for two years and no new features. This guide pins **Unity 6.3 LTS**,
supported until December 2027.

### MonoBehaviour
The base class a script must inherit from to be attachable to a GameObject as a component. It is what gives a
script Unity's lifecycle callbacks.

### one-way platform
A platform you can jump up *through* from below but land on from above — implemented with a
`PlatformEffector2D` rather than with collision code.

### override layer (binding override)
A set of changes stored *on top of* an input action asset rather than in it, so a rebound key can be saved,
restored and reset without ever modifying the asset the guide shipped.

### pixels per unit (PPU)
How many pixels of a sprite make up one Unity world unit. With Kenney's 18×18 tiles and **PPU 18**, one tile is
exactly one unit — which is what lets every distance in this guide be quoted in units.

### Play Mode
The Editor running your game. Everything you change while it runs is **discarded** when you stop — which is how Unity guarantees the stopped state is the saved state.

### PlayerPrefs
Unity's small key-value store, saved per user per game, handling `int`, `float` and `string` only. Right for settings and scores; wrong for a save game — no schema, no versioning, no structure.

### prefab
A GameObject saved as an asset so it can be placed many times and edited in one place. Coins, enemies and
checkpoints are prefabs here.

### rebinding operation
The object returned by `PerformInteractiveRebinding`: it listens for input, applies an override when something arrives, and reports through `OnComplete` or `OnCancel`. It must be disposed, and its action must be disabled while it runs.

### Rigidbody2D
The component that hands a GameObject to the 2D physics engine. Once present, the engine — not your
`transform.position` — is what moves the object.

### Rule Tile
A tile asset that picks its sprite from its neighbours, using a 3 × 3 pattern per rule where each neighbour is *must match*, *must not match*, or *don't care*. It ships in the 2D Tilemap Extras package.

### scene
A file (`.unity`) holding a set of GameObjects and their arrangement. A game is a handful of scenes — a menu, each level — loaded one at a time.

### serialization
Unity's process of saving a component's field values into the scene or prefab file, which is also what makes
them appear in the Inspector. A `private` field needs `[SerializeField]` to take part.

### sorting layer
A named draw order for 2D renderers: layers are painted in list order, and **Order in Layer** breaks ties inside one. Unrelated to the physics layers used by collision masks, despite the name.

### SpriteRenderer
The component that draws a 2D image for a GameObject: `Sprite` is what it draws, `Color` tints it, `Order in Layer` decides what sits in front of what.

### state machine
A structure that keeps an object in exactly one named state at a time (grounded, airborne, dashing,
wall-sliding) with explicit transitions between them — the alternative to a growing pile of booleans.

### TextMeshPro
Unity's text renderer, shipped inside the Editor as part of the uGUI package. Its fonts and shaders must be imported once per project via **Window > TextMeshPro > Import TMP Essential Resources**.

### tile asset
A small asset pairing a sprite with tilemap-specific settings. It is what a Tilemap stores in each cell.

### tile palette
The Editor window holding the set of tiles you paint a Tilemap with, built from an imported sprite sheet.

### Tilemap
A grid-based component that stores which tile sits in which cell, so a level is data you paint rather than
hundreds of individual GameObjects.

### time scale
The multiplier Unity applies to the passage of game time: `1` normal, `0` frozen, `0.5` slow motion. `Time.deltaTime` is scaled by it; `Time.unscaledDeltaTime` is not.

### trigger
A collider that detects overlap but does not stop anything — how coins, checkpoints and the level exit notice
the player without blocking them.

### Unity Hub
The small launcher that manages which Editor versions you have installed and which project uses which. You
install it once by hand; every Editor arrives through it.

### Universal Render Pipeline (URP)
Unity's modern, scriptable rendering path, with a 2D Renderer configuration this project uses. Here it is scenery rather than a subject: the guide never writes a shader.

### variable jump height
Making a tapped jump peak lower than a held one, by increasing gravity while the player is rising and the jump
button is already released.

### velocity
How fast and in which direction a body travels, in units per second, as a `Vector2`. On a `Rigidbody2D` the property is `linearVelocity`; the older `velocity` name is obsolete in Unity 6.

### VSync
Locking the game's frame rate to the monitor's refresh rate to avoid tearing. While it is on, `Application.targetFrameRate` is ignored.
