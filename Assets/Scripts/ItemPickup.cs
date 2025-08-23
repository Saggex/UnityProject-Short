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
    [SerializeField] [TextArea] private string[] successResponses;
    [SerializeField] [TextArea] private string[] failedResponses;

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
                ui?.ShowFlavourText(GetRandomResponse(failedResponses) ?? $"You need {requiredItemId}");
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
        ui?.ShowFlavourText(GetRandomResponse(successResponses) ?? $"Picked up {item.DisplayName}");
        onPickedUp?.Invoke();
        Destroy(gameObject);
        return true;
    }

    private string GetRandomResponse(string[] responses)
    {
        if (responses == null || responses.Length == 0) return null;
        return responses[Random.Range(0, responses.Length)];
    }
}
