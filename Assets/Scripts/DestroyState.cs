using UnityEngine;

/// <summary>
/// Tracks whether interactable objects have been destroyed.
/// Uses PlayerPrefs for simple persistence across sessions.
/// </summary>
public static class DestroyState
{
    private static string GetKey(string id) => $"Destroyed_{id}";
    private const string AllKey = "Destroyed_All";

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
        var list = PlayerPrefs.GetString(AllKey, string.Empty);
        if (!list.Contains($",{id},"))
        {
            list = $",{id}," + list;
            PlayerPrefs.SetString(AllKey, list);
        }
        PlayerPrefs.Save();
    }

    public static void ResetAll()
    {
        var list = PlayerPrefs.GetString(AllKey, string.Empty);
        if (!string.IsNullOrEmpty(list))
        {
            var ids = list.Split(',', System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var id in ids)
            {
                PlayerPrefs.DeleteKey(GetKey(id));
            }
        }
        PlayerPrefs.DeleteKey(AllKey);
        PlayerPrefs.Save();
    }
}
