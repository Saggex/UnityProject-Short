using UnityEngine;

/// <summary>
/// Simple ghost behavior that blocks progress until a required item is used.
/// </summary>
public class GhostAI : MonoBehaviour
{
    [SerializeField] private string requiredItemId;
    [SerializeField] private bool isDefeated;

    /// <summary>
    /// Attempts to satisfy the ghost using the provided item.
    /// </summary>
    public bool TrySatisfy(Item item)
    {
        if (isDefeated || item == null) return false;
        if (item.Id != requiredItemId) return false;

        isDefeated = true;
        // Placeholder for defeat logic (animations, removal, etc.).
        return true;
    }
}
