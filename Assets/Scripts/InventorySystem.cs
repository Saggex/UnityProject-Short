using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores collected items and provides usage checks.
/// </summary>
public class InventorySystem : MonoBehaviour
{
    private readonly Dictionary<string, Item> items = new();

    /// <summary>
    /// Raised when an item is added to the inventory.
    /// </summary>
    public event System.Action<Item> ItemAdded;

    /// <summary>
    /// Raised when an item is removed from the inventory.
    /// </summary>
    public event System.Action<Item> ItemRemoved;

    /// <summary>
    /// Adds an item to the inventory.
    /// </summary>
    public void AddItem(Item item)
    {
        if (item == null || items.ContainsKey(item.Id))
        {
            Debug.Log($"[InventorySystem] AddItem ignored for {item?.DisplayName ?? "null"}");
            return;
        }
        items[item.Id] = item;
        Debug.Log($"[InventorySystem] Added {item.DisplayName}");
        ItemAdded?.Invoke(item);
    }

    /// <summary>
    /// Determines whether the player has a specific item.
    /// </summary>
    public bool HasItem(string id)
    {
        bool has = items.ContainsKey(id);
        Debug.Log($"[InventorySystem] HasItem {id} = {has}");
        return has;
    }

    /// <summary>
    /// Removes an item without using it.
    /// </summary>
    public bool RemoveItem(string id)
    {
        if (!items.TryGetValue(id, out var item))
        {
            Debug.Log($"[InventorySystem] RemoveItem failed for {id}");
            return false;
        }
        items.Remove(id);
        Debug.Log($"[InventorySystem] Removed {item.DisplayName}");
        ItemRemoved?.Invoke(item);
        return true;
    }

    /// <summary>
    /// Attempts to use an item by id.
    /// </summary>
    public Item UseItem(string id)
    {
        if (!items.TryGetValue(id, out var item))
        {
            Debug.Log($"[InventorySystem] UseItem failed for {id}");
            return null;
        }
        items.Remove(id);
        Debug.Log($"[InventorySystem] Used {item.DisplayName}");
        ItemRemoved?.Invoke(item);
        return item;
    }

    /// <summary>
    /// Returns all items for UI display.
    /// </summary>
    public IEnumerable<Item> GetAllItems()
    {
        Debug.Log($"[InventorySystem] GetAllItems count = {items.Count}");
        return items.Values;
    }
}
