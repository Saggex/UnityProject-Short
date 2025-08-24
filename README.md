# UnityProject-Short

This repository contains a minimal Unity project with foundational scripts for a 1-bit style adventure prototype. These scripts provide basic gameplay behaviour for movement, inventory management, and more. Utility components also supply menu navigation, save/load functionality, and configurable audio settings.

## Scripts
All scripts are located in `Assets/Scripts/`. Attach them to appropriate GameObjects in your scenes and configure their fields in the Unity Inspector.

### PlayerController.cs
Handles player movement, tiptoeing, and interaction input.
- Attach to the player GameObject with a `Rigidbody2D`.
- Uses global `InventorySystem` and `UIManager` singletons to pick up `ItemPickup` objects and satisfy `GhostAI` requirements.

### InventorySystem.cs
Stores collected items and provides methods for checking and using them.
- Attach to a persistent object such as a game manager.
- Raises `ItemAdded` and `ItemRemoved` events when inventory changes.

### Item.cs
ScriptableObject defining collectible items with id, name, and description.
- Create new items via `Create > Game > Item` in the project window.

### ItemPickup.cs
Component that exposes an `Item` which the player can collect when interacted with, optionally requiring a specific key item.


### GhostAI.cs
Manages ghost behavior that blocks progress until a specific item is used.
- Set the required item id for each ghost.
- UnityEvents `onDefeated` and `onFailed` fire on success or failure.

### RoomManager.cs
Tracks and loads room scenes.
- `LoadRoom` transitions to the target scene and records it as the current room.

### SoundManager.cs
Plays background music and sound effects.
- Requires two `AudioSource` components: one for music and one for SFX.
- Provides `StopMusic` to halt the current track and exposes `SetMusicVolume` and `SetSFXVolume` for runtime volume control.

### SaveLoadManager.cs
Persists player progress using Unity's `PlayerPrefs` system.
- Call `Save()` and `Load()` to write or read data.
- Stores the current room and player position. Inventory is saved separately by `InventorySystem`.

### MainMenu.cs
Button callbacks for the title screen.
- `StartNewGame()` clears an existing save and loads the first scene.
- `ContinueGame()` loads the save file if present.
- `QuitGame()` exits the application.

### PauseMenu.cs
Toggles gameplay pausing and exposes additional menu actions.
- Pressing *Escape* calls `Pause()`/`Resume()`.
- `SaveGame()` and `LoadGame()` bridge to `SaveLoadManager`.
- `QuitToMenu()` returns to the main menu scene.

### SettingsManager.cs & SettingsMenu.cs
Maintain and display user configurable options.
- Currently supports music and SFX volume with values stored in `PlayerPrefs`.
- `SettingsMenu` links UI sliders to the manager.

### UIManager.cs
Displays inventory contents, flavour text, and interaction prompts.
- Fill in the `inventoryContainer`, `inventoryButtonPrefab`, `flavourText`, and `prompt` fields with UI elements.

### Door.cs
Transitions the player to another scene when interacted with. Doors can require a key item and invoke UnityEvents on success or failure.


### PrefabBuilder.cs (Editor)
Located in `Assets/Editor/`, this editor utility generates placeholder prefabs for the core scripts.
- Open the Unity editor and select **Tools > Build Prefabs**.
- Prefabs will be created in `Assets/Prefabs/` with each script component attached.

## Interaction Systems
The gameplay loop revolves around several small systems that communicate through trigger collisions and shared references:

1. **Item collection** – `PlayerController` gathers nearby `ItemPickup` objects and stores their `Item` in the `InventorySystem`, prompting the `UIManager` to refresh its display.
2. **Ghost encounters** – pressing the interact key near a `GhostAI` checks the inventory for any required items. If all are present, `GhostAI` fires `onDefeated` events and hides the ghost.
3. **Room transitions** – `RoomManager` loads new scenes and applies ambience by calling the `SoundManager` and adjusting lighting.

## Prefab Setup
Prefab templates for these systems are provided in `Assets/Prefabs/`. Drop them into a scene and assign required references as outlined in [`Assets/Prefabs/README.md`](Assets/Prefabs/README.md).

## Usage
1. Create scenes for **Bedroom**, **Dark Hallway**, **Living Room**, **Bathroom**, and **Garden**.
2. Populate scenes with interactable objects and ghosts, assigning the appropriate scripts.
3. Create `Item` assets for Lamp, Spatula, Shoes, Insecticide, Stool, Toilet Brush, Garden Key, Jacket, Cough Medicine, Bucket, and Shovel.
4. Run **Tools > Build Prefabs** to generate prefabs for the core systems.
5. Set up UI canvases using `MainMenu`, `PauseMenu`, and `SettingsMenu` to enable saving/loading and audio options.

These scripts form a basic framework for the prototype—extend them further to complete the game.

