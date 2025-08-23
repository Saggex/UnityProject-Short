using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores collected items and provides usage checks.
/// </summary>
public class InventorySystem : MonoBehaviour
{
    private readonly Dictionary<string, Item> items = new();

    /// <summary>
    /// Adds an item to the inventory.
    /// </summary>
    public void AddItem(Item item)
    {
        if (item == null || items.ContainsKey(item.Id)) return;
        items[item.Id] = item;
    }

    /// <summary>
    /// Determines whether the player has a specific item.
    /// </summary>
    public bool HasItem(string id) => items.ContainsKey(id);

    /// <summary>
    /// Attempts to use an item by id.
    /// </summary>
    public Item UseItem(string id)
    {
        if (!items.TryGetValue(id, out var item)) return null;
        // Placeholder for context checks.
        return item;
    }

    /// <summary>
    /// Returns all items for UI display.
    /// </summary>
    public IEnumerable<Item> GetAllItems() => items.Values;
}
