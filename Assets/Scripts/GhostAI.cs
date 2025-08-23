using UnityEngine;
<<<<<<< HEAD
using UnityEngine.Events;
=======
>>>>>>> main

/// <summary>
/// Simple ghost behavior that blocks progress until a required item is used.
/// </summary>
public class GhostAI : MonoBehaviour
{
    [SerializeField] private string requiredItemId;
    [SerializeField] private bool isDefeated;
<<<<<<< HEAD
    [SerializeField] private UnityEvent onDefeated;

    /// <summary>
    /// Item id required to clear the ghost.
    /// </summary>
    public string RequiredItemId => requiredItemId;
=======
>>>>>>> main

    /// <summary>
    /// Attempts to satisfy the ghost using the provided item.
    /// </summary>
    public bool TrySatisfy(Item item)
    {
        if (isDefeated || item == null) return false;
        if (item.Id != requiredItemId) return false;

        isDefeated = true;
<<<<<<< HEAD
        onDefeated?.Invoke();
        gameObject.SetActive(false);
=======
        // Placeholder for defeat logic (animations, removal, etc.).
>>>>>>> main
        return true;
    }
}
