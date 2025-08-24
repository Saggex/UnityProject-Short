using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Simple ghost behavior that blocks progress until a required item is used.
/// </summary>
public class GhostAI : MonoBehaviour
{
    [SerializeField] private string[] requiredItemIds;
    [SerializeField] private bool consumeItem = true;
    [SerializeField] private bool isDefeated;
    [SerializeField] private UnityEvent onDefeated;
    [SerializeField] private UnityEvent onFailed;
    [SerializeField] [TextArea] private string[] successResponses;
    [SerializeField] [TextArea] private string[] failedResponses;

    /// <summary>
    /// Item ids required to clear the ghost.
    /// </summary>
    public string[] RequiredItemIds => requiredItemIds;

    /// <summary>
    /// Attempts to interact with the ghost using the player's inventory.
    /// </summary>
    public bool Interact()
    {
        if (isDefeated) return false;
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
