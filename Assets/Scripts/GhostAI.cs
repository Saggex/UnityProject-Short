using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Simple ghost behavior that blocks progress until a required item is used.
/// </summary>
public class GhostAI : MonoBehaviour
{
    [SerializeField] private string requiredItemId;
    [SerializeField] private bool consumeItem = true;
    [SerializeField] private bool isDefeated;
    [SerializeField] private UnityEvent onDefeated;
    [SerializeField] private UnityEvent onFailed;

    /// <summary>
    /// Item id required to clear the ghost.
    /// </summary>
    public string RequiredItemId => requiredItemId;

    /// <summary>
    /// Attempts to interact with the ghost using the player's inventory.
    /// </summary>
    public bool Interact(InventorySystem inventory, UIManager ui)
    {
        if (isDefeated) return false;
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

        isDefeated = true;
        onDefeated?.Invoke();
        gameObject.SetActive(false);
        return true;
    }
}
