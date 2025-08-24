using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores collected items and provides usage checks.
/// </summary>
public class InventorySystem : PersistentSingleton<InventorySystem>
{
    private const string InventoryKey = "Inventory_Items";
    [SerializeField] public Item[] allItems;
    [SerializeField] public List<Item> Items;

    private void Awake()
    {
        base.Awake();
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
        Debug.Log("Adding Item " + item.name);
        if (item == null || Items.Contains(item))
        {
            Debug.Log("You already have " + item.name);
            return;
        }
        Items.Add(item);
        ItemAdded?.Invoke(item);
        SaveInventory();
    }

    /// <summary>
    /// Determines whether the player has a specific item.
    /// </summary>
    public bool HasItem(string id)
    {
        foreach(Item item in Items) { 
            if(item.Id == id)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Determines whether the player has a specific item.
    /// </summary>
    public bool HasItem(Item item)
    {
        return Items.Contains(item);
    }

    /// <summary>
    /// Removes an item without using it.
    /// </summary>
    public bool RemoveItem(string id)
    {
        if (!HasItem(id))
        {
            return false;
        }
        Item item = FindItem(id);
        Items.Remove(item);
        ItemRemoved?.Invoke(item);
        SaveInventory();
        return true;
    }

    /// <summary>
    /// Removes an item without using it.
    /// </summary>
    public bool RemoveItem(Item item)
    {
        if (!HasItem(item))
        {
            return false;
        }
        Items.Remove(item);
        ItemRemoved?.Invoke(item);
        SaveInventory();
        return true;
    }

    /// <summary>
    /// Attempts to use an item by id.
    /// </summary>
    public Item UseItem(string id)
    {
        Item item = FindItem(id);
        if (!HasItem(item))
        {
            return null;
        }
        Items.Remove(item);
        ItemRemoved?.Invoke(item);
        SaveInventory();
        return item;
    }

    /// <summary>
    /// Returns all items for UI display.
    /// </summary>
    public IEnumerable<Item> GetAllItems()
    {
        return Items;
    }

    private void LoadInventory()
    {
        Items.Clear();
        var saved = PlayerPrefs.GetString(InventoryKey, string.Empty);
        if (string.IsNullOrEmpty(saved)) return;
        var ids = saved.Split(',');
        foreach (var id in ids)
        {
            if (string.IsNullOrEmpty(id)) continue;
            var item = FindItem(id);
            if (item != null)
            {
                Items.Add(item);
            }
        }
    }

    private void SaveInventory()
    {
        List<string> idList = new List<string>();
        foreach(Item item in Items)
        {
            idList.Add(item.Id);
        }
        string ids = string.Join(",", idList);
        PlayerPrefs.SetString(InventoryKey, ids);
        PlayerPrefs.Save();
    }

    private Item FindItem(string id)
    {
        if (string.IsNullOrEmpty(id) || allItems == null) return null;
        foreach (var it in allItems)
        {
            if (it != null && it.Id == id)
            {
                return it;
            }
        }
        return null;
    }
}
