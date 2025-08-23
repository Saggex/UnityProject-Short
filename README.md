# UnityProject-Short

This repository contains a minimal Unity project with script stubs for a 1-bit style adventure prototype. Use these scripts as starting points and expand them to build gameplay.

## Scripts
All scripts are located in `Assets/Scripts/`. Attach them to appropriate GameObjects in your scenes and configure their fields in the Unity Inspector.

### PlayerController.cs
Handles player movement, tiptoeing, and interaction input.
- Attach to the player GameObject with a `Rigidbody2D`.
- Assign the `InventorySystem` reference.

### InventorySystem.cs
Stores collected items and provides methods for checking and using them.
- Attach to a persistent object such as a game manager.

### Item.cs
ScriptableObject defining collectible items with id, name, and description.
- Create new items via `Create > Game > Item` in the project window.

### PuzzleManager.cs
Controls riddle progression and room unlocking.
- Reference the `RoomManager` to trigger room changes when puzzles are solved.

### GhostAI.cs
Manages ghost behavior that blocks progress until a specific item is used.
- Set the required item id for each ghost.

### RoomManager.cs
Loads room scenes and applies atmosphere cues.
- Use `LoadRoom` to transition to another scene.

### SoundManager.cs
Plays background music and sound effects.
- Requires two `AudioSource` components: one for music and one for SFX.

### UIManager.cs
Displays inventory contents, flavour text, and interaction prompts.

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
5. Implement the gameplay details inside the placeholder methods provided in each script.
6. Run **Tools > Build Prefabs** to generate prefabs for the core systems.

These stubs are intended to accelerate setup; add your own logic to complete the game.
