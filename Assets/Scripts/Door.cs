using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents a doorway that transitions the player to another room.
/// </summary>
public class Door : MonoBehaviour
{
    [SerializeField] private string targetScene;
    [SerializeField] private string requiredItemId;
    [SerializeField] private bool consumeItem;
    [SerializeField] private UnityEvent onOpened;
    [SerializeField] private UnityEvent onFailed;
    [SerializeField] [TextArea] private string[] successResponses;
    [SerializeField] [TextArea] private string[] failedResponses;

    private RoomManager roomManager;

    private void Awake()
    {
        roomManager = FindObjectOfType<RoomManager>();
    }

    /// <summary>
    /// Attempts to open the door using the player's inventory.
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

        onOpened?.Invoke();
        var success = GetRandomResponse(successResponses);
        if (!string.IsNullOrEmpty(success))
        {
            ui?.ShowFlavourText(success);
        }

        if (roomManager != null && !string.IsNullOrEmpty(targetScene))
        {
            roomManager.LoadRoom(targetScene);
        }
        return true;
    }

    private string GetRandomResponse(string[] responses)
    {
        if (responses == null || responses.Length == 0) return null;
        return responses[Random.Range(0, responses.Length)];
    }
}
