using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents a doorway that transitions the player to another room.
/// </summary>
public class Door : MonoBehaviour
{
    [SerializeField] private string targetScene;
    [SerializeField] private string targetSpawnId;
    [SerializeField] private string[] requiredItemIds;
    [SerializeField] private bool consumeItem;
    [SerializeField] private UnityEvent onOpened;
    [SerializeField] private UnityEvent onFailed;
    [SerializeField] [TextArea] private string[] successResponses;
    [SerializeField] [TextArea] private string[] failedResponses;
    public AudioClip doorSoundSuccess;
    public AudioClip doorSoundFail;

    private RoomManager roomManager;
    private SoundManager soundManager;
    private bool triggered = false;

    private void Start()
    {
        roomManager = RoomManager.Instance;
        soundManager = SoundManager.Instance;
    }

    /// <summary>
    /// Attempts to open the door using the player's inventory.
    /// </summary>
    public bool Interact()
    {
        if (triggered)
            return false;
        var inventory = InventorySystem.Instance;
        var ui = UIManager.Instance;

        if (requiredItemIds != null && requiredItemIds.Length > 0)
        {

            foreach (var id in requiredItemIds)
            {
                if (inventory == null || !inventory.HasItem(id))
                {
                    onFailed?.Invoke();
                    if(doorSoundFail)
                    soundManager.PlaySFX(doorSoundFail);
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

        onOpened?.Invoke();
        triggered = true;
        if(doorSoundSuccess)
        soundManager.PlaySFX(doorSoundSuccess);
        var success = GetRandomResponse(successResponses);
        if (!string.IsNullOrEmpty(success))
        {
            ui?.ShowFlavourText(success);
        }

        if (roomManager != null && !string.IsNullOrEmpty(targetScene))
        {
            roomManager.LoadRoom(targetScene, targetSpawnId);
        }
        return true;
    }

    private string GetRandomResponse(string[] responses)
    {
        if (responses == null || responses.Length == 0) return null;
        return responses[Random.Range(0, responses.Length)];
    }
}
