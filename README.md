# UnityProject-Short

This repository contains a minimal Unity project with foundational scripts for a 1-bit style adventure prototype. These scripts provide basic gameplay behaviour for movement, inventory management, puzzles and more.

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
Simple component that exposes an `Item` which the player can collect when interacted with.

### PuzzleManager.cs
Controls riddle progression and room unlocking.
- Define puzzle rules in the inspector by pairing puzzle ids with required item ids and optional rooms to unlock.
- Reference the `RoomManager` to trigger room changes when puzzles are solved.

### GhostAI.cs
Manages ghost behavior that blocks progress until a specific item is used.
- Set the required item id for each ghost.
- Optionally assign a UnityEvent to `onDefeated` for custom reactions when the ghost is satisfied.

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
- Fill in the `inventoryText`, `flavourText`, and `prompt` fields with UI elements.

### QuestManager.cs (optional)
Tracks overall quest progression.

### PrefabBuilder.cs (Editor)
Located in `Assets/Editor/`, this editor utility generates placeholder prefabs for the core scripts.
- Open the Unity editor and select **Tools > Build Prefabs**.
- Prefabs will be created in `Assets/Prefabs/` with each script component attached.

## Usage
1. Create scenes for **Bedroom**, **Dark Hallway**, **Living Room**, **Bathroom**, and **Garden**.
2. Populate scenes with interactable objects and ghosts, assigning the appropriate scripts.
3. Create `Item` assets for Lamp, Spatula, Shoes, Insecticide, Stool, Toilet Brush, Garden Key, Jacket, Cough Medicine, Bucket, and Shovel.
4. Use `PuzzleManager` to define how items unlock progression and satisfy ghosts.
5. Run **Tools > Build Prefabs** to generate prefabs for the core systems.

These scripts form a basic framework for the prototype—extend them further to complete the game.
