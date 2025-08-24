using UnityEngine;

/// <summary>
/// Tracks whether interactable objects have been destroyed.
/// Uses PlayerPrefs for simple persistence across sessions.
/// </summary>
public static class DestroyState
{
    private static string GetKey(string id) => $"Destroyed_{id}";

    /// <summary>
    /// Returns true if the interactable with the given id was previously destroyed.
    /// </summary>
    public static bool IsDestroyed(string id)
    {
        if (string.IsNullOrEmpty(id)) return false;
        return PlayerPrefs.GetInt(GetKey(id), 0) == 1;
    }

    /// <summary>
    /// Marks the interactable with the given id as destroyed.
    /// </summary>
    public static void MarkDestroyed(string id)
    {
        if (string.IsNullOrEmpty(id)) return;
        PlayerPrefs.SetInt(GetKey(id), 1);
        PlayerPrefs.Save();
    }
}
