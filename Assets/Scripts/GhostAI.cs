using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Simple ghost behavior that blocks progress until a required item is used.
/// </summary>
public class GhostAI : MonoBehaviour
{
    [SerializeField] private string requiredItemId;
    [SerializeField] private bool isDefeated;
    [SerializeField] private UnityEvent onDefeated;

    /// <summary>
    /// Item id required to clear the ghost.
    /// </summary>
    public string RequiredItemId => requiredItemId;

    /// <summary>
    /// Attempts to satisfy the ghost using the provided item.
    /// </summary>
    public bool TrySatisfy(Item item)
    {
        if (isDefeated || item == null) return false;
        if (item.Id != requiredItemId) return false;

        isDefeated = true;
        onDefeated?.Invoke();
        gameObject.SetActive(false);
        return true;
    }
}
