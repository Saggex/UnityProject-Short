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
    [SerializeField] [TextArea] private string[] successResponses;
    [SerializeField] [TextArea] private string[] failedResponses;

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
                ui?.ShowFlavourText(GetRandomResponse(failedResponses) ?? $"You need {requiredItemId}");
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
        var success = GetRandomResponse(successResponses);
        if (!string.IsNullOrEmpty(success))
        {
            ui?.ShowFlavourText(success);
        }
        gameObject.SetActive(false);
        return true;
    }

    private string GetRandomResponse(string[] responses)
    {
        if (responses == null || responses.Length == 0) return null;
        return responses[Random.Range(0, responses.Length)];
    }
}
