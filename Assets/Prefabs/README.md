# Prefabs

This folder holds reusable templates for the game's interaction systems. Drop them into your scene and wire up their fields in the Inspector.

## Player.prefab
- Components: `Rigidbody2D`, `BoxCollider2D`, `PlayerController`, `ScaleWithDepth`.
- The collider should remain non-trigger so it can detect trigger colliders on interactables.

## ItemPickup.prefab
- Components: `ItemPickup`, `SpriteRenderer`, `BoxCollider2D` (set as trigger).
- Create an `Item` ScriptableObject and assign it to the **Item** field.
- Ensure the collider's *Is Trigger* box is checked so the player can detect the pickup.

## Ghost.prefab
- Contains only the `GhostAI` script. Add a 2D collider set to *Is Trigger* so the player can collide with the ghost.
- Fill in **Required Item Id** and optionally hook UnityEvents to **On Defeated**.

## InventorySystem.prefab
- Holds the `InventorySystem` component. A single instance persists across scenes and is accessed via `InventorySystem.Instance`.

## RoomManager.prefab
- Provides `RoomManager`. Populate the **Rooms** array with ambience settings and link a `SoundManager`.

## SoundManager.prefab
- Uses two `AudioSource` components for music and effects. Link the sources to the script fields and assign audio clips as needed.

## InventoryButton.prefab
- `InventoryButton` is a basic `Button` with an `Image` and `InventoryButton` script.

## ScriptableObjects/Items
- Contains example `Item` assets. Duplicate `SampleItem.asset` to create new items.
