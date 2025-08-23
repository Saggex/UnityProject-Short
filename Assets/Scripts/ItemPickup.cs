using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component that allows the player to pick up a referenced Item via interaction.
/// </summary>
public class ItemPickup : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private string requiredItemId;
    [SerializeField] private bool consumeItem;
    [SerializeField] private UnityEvent onPickedUp;
    [SerializeField] private UnityEvent onFailed;

    /// <summary>
    /// The item granted when picked up.
    /// </summary>
    public Item Item => item;

    /// <summary>
    /// Attempts to pick up the item using the player's inventory.
    /// </summary>
    public bool Interact(InventorySystem inventory, UIManager ui)
    {
        if (!string.IsNullOrEmpty(requiredItemId))
        {
            if (!inventory.HasItem(requiredItemId))
            {
                onFailed?.Invoke();
                ui?.ShowFlavourText($"You need {requiredItemId}");
                return false;
            }
            if (consumeItem)
            {
                inventory.UseItem(requiredItemId);
                ui?.RefreshInventory(inventory);
            }
        }

        inventory.AddItem(item);
        ui?.RefreshInventory(inventory);
        ui?.ShowFlavourText($"Picked up {item.DisplayName}");
        onPickedUp?.Invoke();
        Destroy(gameObject);
        return true;
    }
}
