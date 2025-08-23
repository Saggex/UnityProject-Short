# UnityProject-Short

This repository contains a minimal Unity project with foundational scripts for a 1-bit style adventure prototype. These scripts provide basic gameplay behaviour for movement, inventory management, and more.

## Scripts
All scripts are located in `Assets/Scripts/`. Attach them to appropriate GameObjects in your scenes and configure their fields in the Unity Inspector.

### PlayerController.cs
Handles player movement, tiptoeing, and interaction input.
- Attach to the player GameObject with a `Rigidbody2D`.
- Assign references for `InventorySystem` and `UIManager`.
- Uses a raycast in the facing direction to pick up `ItemPickup` objects and satisfy `GhostAI` using items from the inventory.

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
Loads room scenes and applies atmosphere cues.
- Define `RoomSettings` to pair room ids with ambient clips and lighting.
- Use `LoadRoom` to transition to another scene and call `ApplyAtmosphere` after loading.

### SoundManager.cs
Plays background music and sound effects.
- Requires two `AudioSource` components: one for music and one for SFX.
- Provides `StopMusic` to halt the current track.

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
2. **Ghost encounters** – pressing the interact key near a `GhostAI` checks the inventory for the ghost's `requiredItemId`. If found, `GhostAI` fires `onDefeated` events and hides the ghost.
3. **Room transitions** – `RoomManager` loads new scenes and applies ambience by calling the `SoundManager` and adjusting lighting.

## Prefab Setup
Prefab templates for these systems are provided in `Assets/Prefabs/`. Drop them into a scene and assign required references as outlined in [`Assets/Prefabs/README.md`](Assets/Prefabs/README.md).

## Usage
1. Create scenes for **Bedroom**, **Dark Hallway**, **Living Room**, **Bathroom**, and **Garden**.
2. Populate scenes with interactable objects and ghosts, assigning the appropriate scripts.
3. Create `Item` assets for Lamp, Spatula, Shoes, Insecticide, Stool, Toilet Brush, Garden Key, Jacket, Cough Medicine, Bucket, and Shovel.
4. Run **Tools > Build Prefabs** to generate prefabs for the core systems.

These scripts form a basic framework for the prototype—extend them further to complete the game.

