using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores collected items and provides usage checks.
/// </summary>
public class InventorySystem : MonoBehaviour
{
    private readonly Dictionary<string, Item> items = new();

    /// <summary>
<<<<<<< HEAD
    /// Raised when an item is added to the inventory.
    /// </summary>
    public event System.Action<Item> ItemAdded;

    /// <summary>
    /// Raised when an item is removed from the inventory.
    /// </summary>
    public event System.Action<Item> ItemRemoved;

    /// <summary>
=======
>>>>>>> main
    /// Adds an item to the inventory.
    /// </summary>
    public void AddItem(Item item)
    {
        if (item == null || items.ContainsKey(item.Id)) return;
        items[item.Id] = item;
<<<<<<< HEAD
        ItemAdded?.Invoke(item);
=======
>>>>>>> main
    }

    /// <summary>
    /// Determines whether the player has a specific item.
    /// </summary>
    public bool HasItem(string id) => items.ContainsKey(id);

    /// <summary>
<<<<<<< HEAD
    /// Removes an item without using it.
    /// </summary>
    public bool RemoveItem(string id)
    {
        if (!items.TryGetValue(id, out var item)) return false;
        items.Remove(id);
        ItemRemoved?.Invoke(item);
        return true;
    }

    /// <summary>
=======
>>>>>>> main
    /// Attempts to use an item by id.
    /// </summary>
    public Item UseItem(string id)
    {
        if (!items.TryGetValue(id, out var item)) return null;
<<<<<<< HEAD
        items.Remove(id);
        ItemRemoved?.Invoke(item);
=======
        // Placeholder for context checks.
>>>>>>> main
        return item;
    }

    /// <summary>
    /// Returns all items for UI display.
    /// </summary>
    public IEnumerable<Item> GetAllItems() => items.Values;
}
