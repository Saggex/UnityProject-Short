using UnityEditor;
using UnityEngine;

/// <summary>
/// Adds a menu item to clear all saved PlayerPrefs.
/// </summary>
public static class ClearPlayerPrefs
{
    [MenuItem("Tools/Clear Player Prefs")]
    public static void Clear()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs cleared");
    }
}
