using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores collected items and provides usage checks.
/// </summary>
public class InventorySystem : PersistentSingleton<InventorySystem>
{
    private const string InventoryKey = "Inventory_Items";
    private readonly Dictionary<string, Item> items = new Dictionary<string, Item>();
    private static Dictionary<string, Item> resourceCache;

    private void Awake()
    {
        LoadInventory();
    }

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
            return;
        }
        items[item.Id] = item;
        ItemAdded?.Invoke(item);
        SaveInventory();
    }

    /// <summary>
    /// Determines whether the player has a specific item.
    /// </summary>
    public bool HasItem(string id)
    {
        return items.ContainsKey(id);
    }

    /// <summary>
    /// Removes an item without using it.
    /// </summary>
    public bool RemoveItem(string id)
    {
        if (!items.TryGetValue(id, out var item))
        {
            return false;
        }
        items.Remove(id);
        ItemRemoved?.Invoke(item);
        SaveInventory();
        return true;
    }

    /// <summary>
    /// Attempts to use an item by id.
    /// </summary>
    public Item UseItem(string id)
    {
        if (!items.TryGetValue(id, out var item))
        {
            return null;
        }
        items.Remove(id);
        ItemRemoved?.Invoke(item);
        SaveInventory();
        return item;
    }

    /// <summary>
    /// Returns all items for UI display.
    /// </summary>
    public IEnumerable<Item> GetAllItems()
    {
        return items.Values;
    }

    private void LoadInventory()
    {
        items.Clear();
        var saved = PlayerPrefs.GetString(InventoryKey, string.Empty);
        if (string.IsNullOrEmpty(saved)) return;
        var ids = saved.Split(',');
        foreach (var id in ids)
        {
            if (string.IsNullOrEmpty(id)) continue;
            var item = FindItem(id);
            if (item != null)
            {
                items[id] = item;
            }
        }
    }

    private void SaveInventory()
    {
        var ids = string.Join(",", items.Keys);
        PlayerPrefs.SetString(InventoryKey, ids);
        PlayerPrefs.Save();
    }

    private Item FindItem(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;
        if (resourceCache == null)
        {
            resourceCache = new Dictionary<string, Item>();
            foreach (var it in Resources.LoadAll<Item>(string.Empty))
            {
                if (!string.IsNullOrEmpty(it.Id))
                {
                    resourceCache[it.Id] = it;
                }
            }
        }
        resourceCache.TryGetValue(id, out var item);
        return item;
    }
}
