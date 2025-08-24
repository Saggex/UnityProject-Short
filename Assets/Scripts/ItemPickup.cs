using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component that allows the player to pick up a referenced Item via interaction.
/// </summary>
public class ItemPickup : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private string id;
    [SerializeField] private string[] requiredItemIds;
    [SerializeField] private bool consumeItem;
    [SerializeField] private UnityEvent onPickedUp;
    [SerializeField] private UnityEvent onFailed;
    [SerializeField] [TextArea] private string[] successResponses;
    [SerializeField] [TextArea] private string[] failedResponses;

    /// <summary>
    /// The item granted when picked up.
    /// </summary>
    public Item Item => item;

    private void Awake()
    {
        var key = GetId();
        if (DestroyState.IsDestroyed(key))
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Attempts to pick up the item using the player's inventory.
    /// </summary>
    public bool Interact()
    {
        var inventory = InventorySystem.Instance;
        var ui = UIManager.Instance;
        if (requiredItemIds != null && requiredItemIds.Length > 0)
        {
            foreach (var id in requiredItemIds)
            {
                if (inventory == null || !inventory.HasItem(id))
                {
                    onFailed?.Invoke();
                    ui?.ShowFlavourText(GetRandomResponse(failedResponses) ?? $"You need {string.Join(", ", requiredItemIds)}");
                    return false;
                }
            }
            if (consumeItem)
            {
                foreach (var id in requiredItemIds)
                {
                    inventory?.UseItem(id);
                }
                ui?.RefreshInventory(inventory);
            }
        }

        inventory?.AddItem(item);
        ui?.RefreshInventory(inventory);
        ui?.ShowFlavourText(GetRandomResponse(successResponses) ?? $"Picked up {item.DisplayName}");
        onPickedUp?.Invoke();
        SoundManager.Instance.PlaySFX(item.Sound);
        DestroyState.MarkDestroyed(GetId());
        Destroy(gameObject);
        return true;
    }

    private string GetRandomResponse(string[] responses)
    {
        if (responses == null || responses.Length == 0) return null;
        return responses[Random.Range(0, responses.Length)];
    }

    private string GetId()
    {
        return string.IsNullOrEmpty(id) ? item?.Id : id;
    }
}
